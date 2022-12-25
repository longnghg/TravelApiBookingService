using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using PrUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Travel.Context.Models;
using Travel.Data.Interfaces;
using Travel.Shared.Ultilities;
using Travel.Shared.ViewModels;
using Travel.Shared.ViewModels.Travel;
using Travel.Shared.ViewModels.Travel.TourBookingVM;
using TravelApi.Hubs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace TravelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourBookingController : Controller
    {
        private ITourBooking _tourbooking;

        private Notification message;
        private Response res;
        public TourBookingController(ITourBooking tourbooking)
        {
            _tourbooking = tourbooking;
            res = new Response();
        }


        [NonAction]
        private Claim GetEmailUserLogin()
        {
            return (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
        }

        [HttpGet]
        [Authorize]
        [Route("do-payment")]
        public async Task<object> DoPayment(string idTourBooking,string customerId = null, string phoneCus = null)
        {
            res =  await _tourbooking.DoPayment(idTourBooking, customerId, phoneCus);
            await GoogleCalendarHelper.CreateGoogleCalendar(request)

            return Ok(res);
        }

        [HttpGet]
        [Authorize]
        [Route("list-tourbooking-by-date")]
        public object GetTourBookingFromDateToDate(DateTime? fromDateInput, DateTime? toDateInput)
        {
            res = _tourbooking.GetTourBookingFromDateToDate(fromDateInput, toDateInput);
            return Ok(res);
        }
        [HttpGet]
        [Authorize]
        [Route("list-tourbooking")]
        public async Task<object> getsTourBooking()
        {
            res = await _tourbooking.Gets();
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        [Route("create-tourbooking")]
        public async Task<object> Create([FromBody] JObject frmData)
        {
            message = null;
            var result = _tourbooking.CheckBeforSave(frmData, ref message, false);
            if (message == null)
            {
                var createObj = JsonSerializer.Deserialize<CreateTourBookingViewModel>(result);
                int adult = createObj.BookingDetails.Adult;
                int child = createObj.BookingDetails.Child;
                int baby = createObj.BookingDetails.Baby;

                var checkEmpty = await _tourbooking.CallServiceCheckEmptyCapacity(createObj.ScheduleId, adult, child, baby);
                if(checkEmpty)
                {
                    await _tourbooking.CallServiceUpdateCapacity(createObj.ScheduleId, adult, child, baby);
                    //var emailUser = GetEmailUserLogin().Value;
                    var emailUser = createObj.Email;
                    res = await _tourbooking.Create(createObj, emailUser);
                }
                else
                {
                    return Ok(checkEmpty);
                }
            }
            else
            {
                res.Notification = message;
            }
            return Ok(res);
        }
        [HttpGet]
        [Authorize]
        [Route("tour-booking-by-id")]
        public async Task<object> TourBookingById(string idTourBooking)
        {
            res = await _tourbooking.TourBookingById(idTourBooking);
            return Ok(res);
        }
        [HttpGet]
        [Authorize]
        [Route("payment-changed")]
        public async Task<object> ChangePayment(string idTourBooking,int idPayment)
        {
            var result =  await _tourbooking.ChangePayment(idTourBooking, idPayment);
            return Ok(result);
        }
        

        [HttpGet]
        [Authorize]
        [Route("tour-booking-by-booking-no")]
        public async Task<object> TourBookingByBookingNo(string bookingNo)
        {
            res = await _tourbooking.TourBookingByBookingNo(bookingNo);
            return Ok(res);
        }


        [HttpPut]
        [Authorize]
        [Route("cancel-booking")]
        public async Task<object> CancelBooking(string idTourBooking)
        {
            res = await _tourbooking.CancelBooking(idTourBooking);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        [Route("restore-booking")]
        public async Task<object> RestoreBooking(string idTourBooking)
        {
            var emailUser = GetEmailUserLogin().Value;
            res = await _tourbooking.RestoreBooking(idTourBooking, emailUser);
            
            return Ok(res);
        }
        [HttpGet]
        [Authorize]
        [Route("statistic-tourbooking")]
        public object StatisticTourBooking()
        {
            res = _tourbooking.StatisticTourBooking();
            return Ok(res);
        }
        [HttpGet]
        [Authorize]
        [Route("check-called")]
        public object CheckCalled(string idTourBooking)
        {
            res = _tourbooking.CheckCalled(idTourBooking);
            return Ok(res);
        }
        [HttpPost]
        [Route("search-TourBooking")]
        public async Task<object> SearchTourBooking([FromBody] JObject frmData)
        {
            res = await _tourbooking.SearchTourBooking(frmData);
            return Ok(res);
        }

        //[HttpPut]
        //[Authorize]
        //[Route("update-tourBooking-status")]
        //public object UpdateStatus(string pincode)
        //{
           
        //    var emailUser = GetEmailUserLogin().Value;
        //    res = _tourbooking.UpdateStatus(pincode, emailUser);
        //    return Ok(res);
        //}

        [HttpGet]
        [Authorize]
        [Route("send-bill")]
        public async Task<object> SendToBill(string pinCode, string nameCustomer, string tourId, string idTourBooking, long departTureday, long returnDate, string email)
        {
            res = await _tourbooking.SendToBill( pinCode ,nameCustomer,  tourId,  idTourBooking,  departTureday,  returnDate,  email);
            return Ok(res);
        }


        [HttpPut]
        [Authorize]
        [Route("adm-update-tourBooking-status")]
        public async Task<object> UpdateStatus(string idTourBooking, int status)
        {

            var emailUser = GetEmailUserLogin().Value;
            res = await _tourbooking.UpdateStatus(idTourBooking, status, emailUser);
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        [Route("update-tourBooking-finished")]
        public async Task<object> UpdateTourBookingFinished()
        {
            var flag = await _tourbooking.UpdateTourBookingFinished();
            if (flag)
            {
                return Ok(new
                {
                    Notification = new {
                        Type = Enums.TypeCRUD.Success,
                        Messenge = "Thành công"
                    }
                });
            }
            else
            {
                return BadRequest(new
                {
                    Status = -1,
                    Message = "Thất bại"
                });
            }
        }



        [HttpPut]
        [Authorize]
        [Route("check-in-booking")]
        public object CheckinBooking(string bookingNo)
        {
            res = _tourbooking.CheckInBooking(bookingNo);
            return Ok(res);
        }
        [HttpGet]
        [Authorize]
        [Route("list-statistic-paid-not-checkedin")]
        public object StatisticPaidNotCheckedin()
        {
            res = _tourbooking.StatisticPaidNotCheckedin();
            return Ok(res);
        }

        [HttpGet]
        [Authorize]
        [Route("cus-search-bookingNo")]
        public async Task<object> CusSearchBookingNo(string bookingNo)
        {
            res = await _tourbooking.CusSearchBookingNo(bookingNo);
            return Ok(res);
        }
        #region service call

      
         [HttpGet]
        [Authorize]
        [Route("list-tour-booking-by-id-customer-s")]
        public async Task<object> TourBookingByIdCustomer(Guid idCustomer)
        {
            return   await _tourbooking.TourBookingByIdCustomer(idCustomer);
        }

        [HttpGet]
        [Authorize]
        [Route("delete-booking-s")]
        public async Task DeleteTourBookingImme(string idSchedule)
        {
             await _tourbooking.DeleteTourBookingImme(idSchedule);
        }

        [HttpGet]
        [Authorize]
        [Route("change-feedback-s")]
        public async Task ChangeFeedBack(string idTourBooking)
        {
             await _tourbooking.ChangeFeedBack(idTourBooking);
        }



        [HttpGet]
        [Authorize]
        [Route("list-tour-booking-by-idschedule-s")]
        public object ListTourBookingByIdSchedule(string idSchedule)
        {
            return _tourbooking.GetListBookingByIdSchedule(idSchedule);
        }

        [HttpPost]
        [Authorize]
        [Route("delete-booking-expired")]
        public async Task DeleteBookingExpiredController()
        {
            await _tourbooking.DeleteBookingExpired();
        }
        #endregion


    }
}
