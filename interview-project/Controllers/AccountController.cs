﻿using interview_project.Database;
using interview_project.Database.Entities;
using interview_project.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace interview_project.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        private readonly PasswordService _passwordService;

        public AccountController(AppDbContext context, PasswordService passwordService)
        {
            _db = context;
            _passwordService = passwordService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, CancellationToken cancellationToken)
        {
            var user = await _db.AppUsers.FirstOrDefaultAsync(u => u.UserName == username, cancellationToken);
            if (user == null || !_passwordService.VerifyPassword(user.PasswordHash, password, user))
            {
                return Unauthorized("Invalid credentials");
            }

            var claims = new List<Claim> { new(ClaimTypes.Name, user.UserName) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var existingUser = await _db.AppUsers.FirstOrDefaultAsync(u => u.UserName == username);
            if (existingUser != null)
            {
                return BadRequest("Username already exists");
            }

            var user = new AppUser
            {
                UserName = username,
                PasswordHash = _passwordService.HashPassword(password, new AppUser())
            };

            _db.AppUsers.Add(user);
            await _db.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}