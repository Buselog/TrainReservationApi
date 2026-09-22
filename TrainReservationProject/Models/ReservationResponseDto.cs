namespace TrainReservationProject.Models
{
    public class ReservationResponseDto
    {
        public bool RezervasyonYapilabilir { get; set; }
        public List<PlacementDetail> YerlesimAyrinti { get; set; } = new();

    }

    public class PlacementDetail
    {
        public string VagonAdi { get; set; } = string.Empty;
        public int KisiSayisi { get; set; }
    }
}

