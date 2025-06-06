﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViajaFacil.Data;
using ViajaFacil.Models.Users;

namespace ViajaFacil.Controllers.Users {

    [Route("api/users/[controller]")]
    [ApiController]
    public class ListUsersController : ControllerBase {

        private readonly AppDbContext _context;
        private readonly Helpers.Helpers _helpers;
        public ListUsersController(AppDbContext context, Helpers.Helpers helpers) {
            _context = context;
            _helpers = helpers; 
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers() {

            var userId = _helpers.GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized(new { message = "Unauthorized" });

            if (!await _helpers.IsAdminUser(userId.Value))
                return Unauthorized(new { message = "Only administrators are allowed" });



            // Retrieve users from the database
            var users = await _context.Users
                    .Select(u => new UserDtoModel {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email
                    }).ToListAsync();

            return Ok(users); 
        }

    }
}
