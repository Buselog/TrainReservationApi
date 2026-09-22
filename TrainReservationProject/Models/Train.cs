namespace TrainReservationProject.Models
{
    public class Train
    {
        public string Ad { get; set; }
        public List<Wagon> Vagonlar { get; set; } =  new();
    }
}

