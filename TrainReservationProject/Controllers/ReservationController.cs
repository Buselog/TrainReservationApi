using Microsoft.AspNetCore.Mvc;
using TrainReservationProject.Models;
using TrainReservationProject.Services;

namespace TrainReservationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        public IActionResult Train(ReservationRequestDto requestDto)
        {
            var result = _reservationService.MakeReservation(requestDto);
            return Ok(result);
        }
    }
}
