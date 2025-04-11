namespace ViajaFacil.Models {
    public class Destination {
        public required int DestinationId { get; set; }
        public required string Name { get; set; }
        public required string Country { get; set; }
        public required string Description { get; set; }
        public required double Price { get; set; }
        public required string ImageUrl { get; set; }

    }
}
