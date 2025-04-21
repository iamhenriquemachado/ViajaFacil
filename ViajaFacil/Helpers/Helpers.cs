using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViajaFacil.Data;
using ViajaFacil.Models;

namespace ViajaFacil.Helpers {
    public class Helpers {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Helpers(AppDbContext context, IHttpContextAccessor httpContextAccessor) {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // Retrieves the authenticated user's ID from claims
        public int? GetUserIdFromClaims() {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userId, out var userIdClaim) ? userIdClaim : null;
        }

        // Checks if the user with the given ID is an administrator
        public async Task<bool> IsAdminUser(int id) {
            var user = await _context.Users.FindAsync(id);
            return user != null && user.IsAdmin;
        }

        // Retrieves a user by their ID
        public async Task<User?> GetUserById(int id) {
            return await _context.Users.FindAsync(id);
        }

        // Checks if the provided email is already used by another user
        public async Task<bool> EmailExistsForAnotherUser(string email, int currentUserId) {
            var normalizedEmail = email.Trim().ToLowerInvariant();
            return await _context.Users.AnyAsync(u => u.Email == normalizedEmail && u.Id != currentUserId);
        }
    }

 public static class ControllerExtensions {
        // Returns a ModelState validation error response, if any
        public static IActionResult? GetModelStateErrorResponse(this ControllerBase controller) {
            if (controller.ModelState.IsValid) return null;

            var errors = controller.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => new {
                    Field = e.Key,
                    Errors = e.Value.Errors.Select(err => err.ErrorMessage)
                });

            return controller.BadRequest(new {
                message = "Invalid request or user not found",
                errors
            });
        }

        // Checks if the incoming object is null and returns a BadRequest if true
        public static IActionResult? GetNullDataResponse<T>(this ControllerBase controller, T? data) {
            if (data == null) {
                return controller.BadRequest(new { message = "No data provided" });
            }

            return null;
        }
    }
}
