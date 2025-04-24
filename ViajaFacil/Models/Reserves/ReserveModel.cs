namespace ViajaFacil.Models.Reserves {
    public class ReserveModel {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DestinationId { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }
    }
}
