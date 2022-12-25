using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Context.Models;
using Travel.Data.Interfaces;
using Travel.Shared.ViewModels;
using Travel.Shared.ViewModels.Travel.TourBookingVM;
namespace Travel.Data.Interfaces
{
   public interface ITourBooking
    {
        string CheckBeforSave(JObject frmData, ref Notification _message, bool isUpdate = false);
        Task<Response> Gets();
        Task<Response> Create(CreateTourBookingViewModel input, string emailUser);
        Task<Response> TourBookingById(string idTourbooking);
        Response GetTourBookingFromDateToDate(DateTime? fromDate, DateTime? toDate);
        Task<Response> DoPayment(string idTourBooking,string idcustomer = null, string phonecustomer = null);
        // customer
        Task<Response> CancelBooking(string idTourBooking);
        Task<Response> RestoreBooking(string idTourBooking, string emailUser);
        Task<Response> TourBookingByBookingNo(string bookingNo);
        Response StatisticTourBooking();

        Response CheckCalled(string idTourBooking);

        Task<Response> SearchTourBooking(JObject frmData);

        //Response UpdateStatus(string pincode, string emailUser);
        Task<TourBooking> GetTourBookingByIdForPayPal (string idTourBooking);
        Task<TourBooking> GetTourBookingByIdForVnPay(string idTourBooking);

        Task<bool> UpdateTourBookingFinished();
        Task<bool> ChangePayment(string idTourBooking, int idPayment);
        Response CheckInBooking(string bookingNo);

        byte[] CreateByteQR(string qrCodeText);
        string AddImg(string qrCodeText, string idService);
        Response StatisticPaidNotCheckedin();

        Task<Response> CusSearchBookingNo(string bookingNo);
        Task<Response> UpdateStatus(string idTourBooking, int status, string emailUser);
        Task<List<TourBooking>> TourBookingByIdCustomer(Guid idCustomer);
        Task ChangeFeedBack(string idTourBooking);
        List<TourBooking> GetListBookingByIdSchedule(string schedule);
        Task <Response> DeleteBookingExpired();

        Task<Response> SendToBill(string pinCode, string nameCustomer, string tourId, string idTourBooking, long departTureday, long returnDate, string email);


        #region service call

        Task<Schedule> CallServiceGetSchedule(string idSchedule);
        Task<Tour> CallServiceGetTourByIdTour(string idTour);
        Task<Tour> CallServiceGetTourByIdSchedule(string idSchedule);
        Task<bool> CallServiceCheckEmptyCapacity(string idSchedule, int adult, int child, int baby);
        Task<List<string>> CallServiceGetListIdScheduleFinished();

        #endregion
    }
}
