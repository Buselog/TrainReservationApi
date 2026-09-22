using TrainReservationProject.Models;

namespace TrainReservationProject.Services
{
    public class ReservationService : IReservationService
    {
        private const decimal MaxOccupancyRate = 0.70m;

        public ReservationResponseDto MakeReservation(ReservationRequestDto request)
        {
            if (request == null || request.RezervasyonYapilacakKisiSayisi <= 0 || request.Tren?.Vagonlar == null || request.Tren.Vagonlar.Count == 0)
            {
                return new ReservationResponseDto { 
                    RezervasyonYapilabilir = false,
                    YerlesimAyrinti = new List<PlacementDetail>()
                };
            }

            if (request.KisilerFarkliVagonlaraYerlestirilebilir)
            {
                return ProcessDistributedReservation(request);
            }

            return ProcessSingleWagonReservation(request);
        }

        private ReservationResponseDto ProcessSingleWagonReservation(ReservationRequestDto request)
        {
            foreach (var wagon in request.Tren.Vagonlar)
            {
                int availableSeats = CalculateAvailableSeats(wagon);

                if (availableSeats >= request.RezervasyonYapilacakKisiSayisi)
                {
                    return new ReservationResponseDto
                    {
                        RezervasyonYapilabilir = true,
                        YerlesimAyrinti = new List<PlacementDetail>
                        {
                            new PlacementDetail
                            {
                                VagonAdi = wagon.Ad,
                                KisiSayisi = request.RezervasyonYapilacakKisiSayisi
                            }
                        }
                    };
                }
            }

            return new ReservationResponseDto
            {
                RezervasyonYapilabilir = false,
                YerlesimAyrinti = new List<PlacementDetail>()
            };
        }

        private ReservationResponseDto ProcessDistributedReservation(ReservationRequestDto request)
        {
            var placements = new List<PlacementDetail>();
            int remainingPeople = request.RezervasyonYapilacakKisiSayisi;

            foreach (var wagon in request.Tren.Vagonlar)
            {
                if (remainingPeople == 0)
                    break;

                int availableSeats = CalculateAvailableSeats(wagon);

                if (availableSeats > 0)
                {
                    int placeCount = remainingPeople > availableSeats ? availableSeats : remainingPeople;

                    placements.Add(new PlacementDetail
                    {
                        VagonAdi = wagon.Ad,
                        KisiSayisi = placeCount
                    });

                    remainingPeople -= placeCount;
                }
            }

            if (remainingPeople == 0)
            {
                return new ReservationResponseDto
                {
                    RezervasyonYapilabilir = true,
                    YerlesimAyrinti = placements
                };
            }

            return new ReservationResponseDto
            {
                RezervasyonYapilabilir = false,
                YerlesimAyrinti = new List<PlacementDetail>()

            };
        }

        private int CalculateAvailableSeats(Wagon wagon)
        {
            int maxCapacity = (int)Math.Floor(wagon.Kapasite * MaxOccupancyRate);
            int available = maxCapacity - wagon.DoluKoltukAdet;

            return available > 0 ? available : 0;
        }

    }
}
