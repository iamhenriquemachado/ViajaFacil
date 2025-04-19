using System.ComponentModel.DataAnnotations;

namespace ViajaFacil.Models {
    public class UserUpdateDTO {

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must be less than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^(?!.*\.\.)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9-]+(\.[a-zA-Z]{2,})+$", ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime LastModified; 

    }
}
