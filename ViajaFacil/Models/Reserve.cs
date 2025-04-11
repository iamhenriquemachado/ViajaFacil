namespace ViajaFacil.Models {
    public class Reserve {
        public required int ReserveId { get; set; }
        public required int UserId { get; set; }
        public required int DestineId { get; set; }
        public required DateTime DepartureDate { get; set; }
        public required DateTime ReturnDate { get; set; }
    }
}
