﻿using Microsoft.AspNetCore.Mvc;
using ViajaFacil.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ViajaFacil.Helpers;
using ViajaFacil.Models.Users;

namespace ViajaFacil.Controllers.Users {
    [Route("api/users/[controller]")]
    [ApiController]
    public class CreateUserController : ControllerBase {
        private readonly AppDbContext _context;
        private readonly Helpers.Helpers _helpers; 

        public CreateUserController(AppDbContext context, Helpers.Helpers helpers) {
            _context = context;
            _helpers = helpers; 
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel user) {
            
            // Validate the model 
            var modelError = this.GetModelStateErrorResponse();
            if (modelError != null)
                return modelError;

            // Check if the email is already in use
            var checkUserExists = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);
            if (checkUserExists != null) {
                return Conflict(new { error = "Email already in use.", StatusCode = 409 });
            }

            // Hashes the user password 
            var passwordHasher = new PasswordHasher<UserModel>();
            user.Password = passwordHasher.HashPassword(user, user.Password);

            // Save new user in the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Return the response with the created user
            return Ok(new { user = "User created successfully", user.Id, user.Name });
        }
    }
}
