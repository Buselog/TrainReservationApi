using TrainReservationProject.Models;

namespace TrainReservationProject.Services
{
    public interface IReservationService
    {
        ReservationResponseDto MakeReservation(ReservationRequestDto reservationRequestDto);
    }
}
