namespace ViajaFacil.Models.Reserves {
    public class ReserveModel {
        public int Id { get; set; }
        public required int UserId { get; set; }
        public required int DestinationId { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
        public required string Status { get; set; }
    }
}
