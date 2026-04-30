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

        // 1. LISTADO DE CIUDADANOS
        public async Task<IActionResult> Index()
        {
            var citizens = await _context.Citizens
                .Include(c => c.DocumentType)
                .Include(c => c.CivilStatus)
                .Include(c => c.ResidencePlace)
                .ToListAsync();
            return View(citizens);
        }

        // 2. VISTA PARA CREAR (GET)
        public IActionResult Create()
        {
            // Llenamos los SelectList para los combos de la vista
            CargarCombos();
            return View();
        }

        // 3. PROCESAR CREACIÓN (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CitizenCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Manejo de la Imagen
                string nombreImagen = "default.png"; // Imagen por defecto
                if (model.PhotoFile != null)
                {
                    nombreImagen = await _fileStorage.SaveFileAsync(model.PhotoFile, "citizens");
                }

                // Mapeo del ViewModel a la Entidad
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

        // Método auxiliar para no repetir carga de combos
        private void CargarCombos()
        {
            ViewBag.DocumentTypes = new SelectList(_context.DocumentTypes, "DocumentTypeId", "Description");
            ViewBag.CivilStatus = new SelectList(_context.CivilStatuses, "CivilStatusId", "Description");

            // Para Ubigeos, mostramos "Departamento - Provincia - Distrito"
            var ubigeos = _context.Ubigeos.Select(u => new
            {
                Id = u.UbigeoId,
                NombreCompleto = $"{u.Department} - {u.Province} - {u.District}"
            }).ToList();

            ViewBag.Ubigeos = new SelectList(ubigeos, "Id", "NombreCompleto");
        }

        // GET: Citizens/Details/5
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

        // GET: Citizens/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var citizen = await _context.Citizens.FindAsync(id);
            if (citizen == null) return NotFound();

            var vm = new CitizenCreateViewModel
            {
                // Estos son los que enviaremos como HIDDEN
                Gender = citizen.Gender,
                BirthUbigeoId = citizen.BirthUbigeoId,
                BirthDate = citizen.BirthDate, // También lo incluimos

                // Los que el usuario sí puede ver/editar
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
        /* [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Edit(int id, CitizenCreateViewModel model)
         {
             // 1. Buscamos el ciudadano real en la base de datos
             var citizenDb = await _context.Citizens.FindAsync(id);
             if (citizenDb == null) return NotFound();

             // 2. Quitamos la validación de ImagePath del ModelState 
             // porque el ViewModel no lo tiene y eso causa que IsValid sea false
             ModelState.Remove("ImagePath");

             if (ModelState.IsValid)
             {
                 try
                 {
                     // 3. Lógica de la foto: ¿Subió una nueva?
                     if (model.PhotoFile != null && model.PhotoFile.Length > 0)
                     {
                         // Guardamos el nuevo archivo y actualizamos la propiedad ImagePath
                         citizenDb.ImagePath = await _fileStorage.SaveFileAsync(model.PhotoFile, "citizens");
                     }
                     // Si model.PhotoFile es nulo, citizenDb.ImagePath conserva su valor anterior automáticamente

                     // 4. Actualizamos el resto de campos manualmente
                     citizenDb.DocumentNumber = model.DocumentNumber;
                     citizenDb.FirstName = model.FirstName;
                     citizenDb.FirstLastName = model.FirstLastName;
                     citizenDb.SecondLastName = model.SecondLastName;
                     citizenDb.CivilStatusId = model.CivilStatusId;
                     citizenDb.CurrentAddress = model.CurrentAddress;
                     citizenDb.CurrentUbigeoId = model.CurrentUbigeoId;
                     citizenDb.BirthDate = model.BirthDate;
                     citizenDb.Gender = model.Gender;

                     _context.Update(citizenDb);
                     await _context.SaveChangesAsync();
                     return RedirectToAction(nameof(Index));
                 }
                 catch (Exception ex)
                 {
                     ModelState.AddModelError("", "Error al guardar: " + ex.Message);
                 }
             }

             // Si algo falló, recargamos combos y volvemos a la vista
             ViewBag.CurrentPhoto = citizenDb.ImagePath; // Importante para la previsualización
             CargarCombos();
             return View(model);
         } */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CitizenCreateViewModel model)
        {
            // 1. Buscar el ciudadano original
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
                    // 3. Procesar Nueva Foto (Solo si el usuario seleccionó una)
                    if (model.PhotoFile != null && model.PhotoFile.Length > 0)
                    {
                        citizenDb.ImagePath = await _fileStorage.SaveFileAsync(model.PhotoFile, "citizens");
                    }

                    // 4. Actualizar datos básicos
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

                    // 5. REDIRECCIÓN: Si todo sale bien, te manda al Index
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al guardar: " + ex.Message);
                }
            }

            // --- SI LLEGAMOS AQUÍ, ES PORQUE ALGO FALLÓ ---
            // Recargamos los datos para que la vista no se rompa
            ViewBag.CurrentPhoto = citizenDb.ImagePath;
            CargarCombos();
            return View(model);
        }
    }
}