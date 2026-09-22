namespace TrainReservationProject.Models
{
    public class ReservationRequestDto
    {
        public Train Tren { get; set; } = new();
        public int RezervasyonYapilacakKisiSayisi { get; set; }
        public bool KisilerFarkliVagonlaraYerlestirilebilir { get; set; }
    }
}

