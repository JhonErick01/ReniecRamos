using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReniecRamos.Data;
using ReniecRamos.Infraestructure;
using ReniecRamos.Models;
using ReniecRamos.Models.ViewModels;

namespace ReniecRamos.Controllers
{
    [Authorize] //
    public class CitizensController : Controller
    {
        private readonly AppDbContext _context;
        private readonly FileStorage _fileStorage;

        public CitizensController(AppDbContext context, FileStorage fileStorage)
        {
            _context = context;
            _fileStorage = fileStorage;
        }

       
        public async Task<IActionResult> Index(string searchQuery)
        {
            var query = _context.Citizens
                .Include(c => c.DocumentType)
                .Include(c => c.CivilStatus)
                .Include(c => c.ResidencePlace)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(c => c.DocumentNumber.Contains(searchQuery));
                ViewBag.SearchQuery = searchQuery;
            }

            var citizens = await query.ToListAsync();
            return View(citizens);
        }

        public IActionResult Create()
        {
            CargarCombos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CitizenCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string nombreImagen = "default.png"; 
                if (model.PhotoFile != null)
                {
                    nombreImagen = await _fileStorage.SaveFileAsync(model.PhotoFile, "citizens");
                }

                var citizen = new Citizen
                {
                    DocumentNumber = model.DocumentNumber,
                    FirstName = model.FirstName,
                    FirstLastName = model.FirstLastName,
                    SecondLastName = model.SecondLastName,
                    BirthDate = model.BirthDate,
                    Gender = model.Gender,
                    DocumentTypeId = model.DocumentTypeId,
                    CivilStatusId = model.CivilStatusId,
                    BirthUbigeoId = model.BirthUbigeoId,
                    CurrentUbigeoId = model.CurrentUbigeoId,
                    CurrentAddress = model.CurrentAddress,
                    ImagePath = nombreImagen,
                    IssueDate = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddYears(8),
                    IsActive = true
                };

                _context.Citizens.Add(citizen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CargarCombos();
            return View(model);
        }

        private void CargarCombos()
        {
            ViewBag.DocumentTypes = new SelectList(_context.DocumentTypes, "DocumentTypeId", "Description");
            ViewBag.CivilStatus = new SelectList(_context.CivilStatuses, "CivilStatusId", "Description");

            var ubigeos = _context.Ubigeos.Select(u => new
            {
                Id = u.UbigeoId,
                NombreCompleto = $"{u.Department} - {u.Province} - {u.District}"
            }).ToList();

            ViewBag.Ubigeos = new SelectList(ubigeos, "Id", "NombreCompleto");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var citizen = await _context.Citizens
                .Include(c => c.DocumentType)
                .Include(c => c.CivilStatus)
                .Include(c => c.BirthPlace)
                .Include(c => c.ResidencePlace)
                .FirstOrDefaultAsync(m => m.CitizenId == id);

            if (citizen == null) return NotFound();

            return View(citizen);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var citizen = await _context.Citizens.FindAsync(id);
            if (citizen == null) return NotFound();

            var vm = new CitizenCreateViewModel
            {
                Gender = citizen.Gender,
                BirthUbigeoId = citizen.BirthUbigeoId,
                BirthDate = citizen.BirthDate, 

                DocumentNumber = citizen.DocumentNumber,
                FirstName = citizen.FirstName,
                FirstLastName = citizen.FirstLastName,
                SecondLastName = citizen.SecondLastName,
                CivilStatusId = citizen.CivilStatusId,
                CurrentUbigeoId = citizen.CurrentUbigeoId,
                CurrentAddress = citizen.CurrentAddress,
                DocumentTypeId = citizen.DocumentTypeId
            };

            ViewBag.CurrentPhoto = citizen.ImagePath;
            CargarCombos();
            return View(vm);
        }
      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CitizenCreateViewModel model)
        {
            var citizenDb = await _context.Citizens.FindAsync(id);
            if (citizenDb == null) return NotFound();

            // 2. IMPORTANTE: Limpiar validaciones que no pertenecen al ViewModel
            // Esto evita que el sistema se bloquee por campos como 'DocumentType' o 'ImagePath'
            ModelState.Remove("DocumentType");
            ModelState.Remove("CivilStatus");
            ModelState.Remove("BirthPlace");
            ModelState.Remove("ResidencePlace");
            ModelState.Remove("ImagePath");
            

            if (ModelState.IsValid)
            {
                
                try
                {
                    if (model.PhotoFile != null && model.PhotoFile.Length > 0)
                    {
                        citizenDb.ImagePath = await _fileStorage.SaveFileAsync(model.PhotoFile, "citizens");
                    }

                    citizenDb.DocumentNumber = model.DocumentNumber;
                    citizenDb.FirstName = model.FirstName;
                    citizenDb.FirstLastName = model.FirstLastName;
                    citizenDb.SecondLastName = model.SecondLastName;
                    citizenDb.BirthDate = model.BirthDate;
                    citizenDb.Gender = model.Gender;
                    citizenDb.CivilStatusId = model.CivilStatusId;
                    citizenDb.CurrentAddress = model.CurrentAddress;
                    citizenDb.CurrentUbigeoId = model.CurrentUbigeoId;

                    _context.Update(citizenDb);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al guardar: " + ex.Message);
                }
            }

            
            ViewBag.CurrentPhoto = citizenDb.ImagePath;
            CargarCombos();
            return View(model);
        }
    }
}