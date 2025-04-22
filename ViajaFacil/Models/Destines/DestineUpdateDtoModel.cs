using System.ComponentModel.DataAnnotations;

namespace ViajaFacil.Models.Destines {
    public class DestineUpdateDtoModel {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
    }
}
