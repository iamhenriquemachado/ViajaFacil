namespace ViajaFacil.Models {
    public class Review {
        public required int Id { get; set; }
        public required int UserId { get; set; }
        public required int DestineId { get; set; }
        public required int Stars {  get; set; }
        public required string Comment { get; set; }
        public required DateTime Date { get; set; }

    }
}
