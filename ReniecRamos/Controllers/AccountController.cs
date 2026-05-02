using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ReniecRamos.Data;
using ReniecRamos.Models;
using ReniecRamos.Models.ViewModels;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
namespace ReniecRamos.Controllers
{

    public class AccountController : Controller
    {

        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Si ya está logueado, mandarlo directo a la lista de ciudadanos, NO a Home
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Citizens");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Username == model.Username);

                if (user != null)
                {  
                     string storedHash = System.Text.Encoding.UTF8.GetString(user.PasswordHash);
                    if (BCrypt.Net.BCrypt.Verify(model.Password, storedHash))
                    {
                        await RealizarSignIn(user);
                        return RedirectToAction("Index", "Citizens");
                    }
                }
                ModelState.AddModelError("", "Usuario o contraseña incorrecta");
            }
            return View(model);
        }

        // Método auxiliar para no repetir código de los Claims
        private async Task RealizarSignIn(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role.Name),
        new Claim("FullName", user.FullName)
    };

            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
            await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login", "Account");
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el usuario ya existe
                if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("", "El nombre de usuario ya está en uso.");
                    return View("Login");
                }

                // Hashear la contraseña
                string salt = BCrypt.Net.BCrypt.GenerateSalt(11);
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password, salt);

                var newUser = new User
                {
                    Username = model.Username,
                    PasswordHash = System.Text.Encoding.UTF8.GetBytes(hashedPassword),
                    FullName = model.FullName,
                    RoleId = model.RoleId,
                    IsActive = true
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

               
                return RedirectToAction("Login");
            }
            return View("Login");
        }
    }
}
