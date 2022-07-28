using HookaTimes.BLL.ViewModels.Restaurant;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    public class ReservationsController : Controller
    {
        public static List<Reservation_VM> reservations = new List<Reservation_VM>();

        #region Sales List
        private void FillSales()
        {
            reservations.Add(new Reservation_VM
            {
                Id = 718208,
                Date = "27 June 2022",
                CustomerName = "Hasan Bdeir",
                NumberOfPpl = 4,
                Remarks = "Outdoor"
            });
            reservations.Add(new Reservation_VM
            {
                Id = 718207,
                Date = "26 June 2022",
                CustomerName = "Imad Al Riz",
                NumberOfPpl = 2,
                Remarks = "Outdoor"
            });
            reservations.Add(new Reservation_VM
            {
                Id = 718206,
                Date = "25 June 2022",
                CustomerName = "Mariam Meait",
                NumberOfPpl = 3,
                Remarks = "N/A"
            });
            reservations.Add(new Reservation_VM
            {
                Id = 718205,
                Date = "24 June 2022",
                CustomerName = "Badih Abou Hasan",
                NumberOfPpl = 4,
                Remarks = "N/A"
            });
            reservations.Add(new Reservation_VM
            {
                Id = 718204,
                Date = "24 June 2022",
                CustomerName = "Abir Mansour",
                NumberOfPpl = 2,
                Remarks = "Outdoor"
            });
            reservations.Add(new Reservation_VM
            {
                Id = 718203,
                Date = "23 June 2022",
                CustomerName = "Hasan Bdeir",
                NumberOfPpl = 5,
                Remarks = "Outdoor"
            });
        }
        #endregion

        public IActionResult ReservationHistory()
        {
            FillSales();
            List<ReservationsList_VM> reservationsList = reservations.Select(x => new ReservationsList_VM
            {
                Id = x.Id,
                Date = x.Date,
                CustomerName = x.CustomerName,
                NumberOfPpl = x.NumberOfPpl,
                Remarks = x.Remarks,
            }).ToList();

            return View("~/Areas/Restaurant/Views/Pages/Reservations/ReservationHistory.cshtml", reservationsList);
        }

        public IActionResult Requests()
        {
            FillSales();
            List<ReservationsList_VM> requestsList = reservations.Select(x => new ReservationsList_VM
            {
                Id = x.Id,
                Date = x.Date,
                CustomerName = x.CustomerName,
                NumberOfPpl = x.NumberOfPpl,
                Remarks = x.Remarks,
            }).ToList();

            return View("~/Areas/Restaurant/Views/Pages/Reservations/Requests.cshtml", requestsList);
        }

        //public IActionResult CreateSale()
        //{
        //    return View("~/Areas/DeliveryCenter/Views/Pages/Sales/CreateSale.cshtml");
        //}

    }
}
