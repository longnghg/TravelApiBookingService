using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PrUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using Travel.Context.Models;
using Travel.Context.Models.Travel;
using Travel.Data.Interfaces;
using Travel.Shared.Ultilities;
using Travel.Shared.ViewModels;
using Travel.Shared.ViewModels.Travel.TourBookingVM;
using Microsoft.Extensions.Configuration;
using Travel.Shared.SpeedSMSAPI;
using System.IO;
using QRCoder;
using Travel.Data.Interfaces.INotify;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Travel.Data.Repositories
{
    public class TourBookingRes : ITourBooking
    {
        private readonly TravelContext _db;
        private readonly string keySecurity;
        private readonly IConfiguration _config;
        private Notification message;
        private readonly ICache _cache;
        private INotification _notification;
        public TourBookingRes(TravelContext db, ICache cache,
            IConfiguration config,
            INotification notification)
        {
            _db = db;
            _config = config;
            keySecurity = _config["keySecurity"];
            message = new Notification();
            _cache = cache;
            _notification = notification;
        }
        private void UpdateDatabase<T>(T input)
        {
            _db.Entry(input).State = EntityState.Modified;
        }
        private void DeleteDatabase<T>(T input)
        {
            _db.Entry(input).State = EntityState.Deleted;
        }
        private void CreateDatabase<T>(T input)
        {
            _db.Entry(input).State = EntityState.Added;
        }
        private async Task SaveChangeAsync()
        {
            await _db.SaveChangesAsync();
        }
        private void SaveChange()
        {
            _db.SaveChanges();
        }

        public string CheckBeforSave(JObject frmData, ref Notification _message, bool isUpdate)
        {
            try
            {
                var idTourBooking = PrCommon.GetString("idTourBooking", frmData);
                if (String.IsNullOrEmpty(idTourBooking))
                {
                    //   payment.IdPayment = idPay;
                }
                var customerId = Guid.Empty;
                var stringIdCustomer = PrCommon.GetString("customerId", frmData);
                if (!String.IsNullOrEmpty(stringIdCustomer))
                {
                    customerId = Guid.Parse(stringIdCustomer);
                }

                var baby = PrCommon.GetString("baby", frmData);
                if (String.IsNullOrEmpty(baby))
                {
                    //   payment.IdPayment = idPay;
                }

                var child = PrCommon.GetString("child", frmData);
                if (String.IsNullOrEmpty(child))
                {
                    //   payment.IdPayment = idPay;
                }

                var adult = PrCommon.GetString("adult", frmData);
                if (String.IsNullOrEmpty(adult))
                {
                    //   payment.IdPayment = idPay;
                }

                var status = PrCommon.GetString("status", frmData);
                if (String.IsNullOrEmpty(status))
                {
                    //   payment.IdPayment = idPay;
                }
                var hotelId = PrCommon.GetString("hotelId", frmData);
                if (String.IsNullOrEmpty(hotelId))
                {
                    //   payment.IdPayment = idPay;
                }
                var restaurantId = PrCommon.GetString("restaurantId", frmData);
                if (String.IsNullOrEmpty(restaurantId))
                {
                    //   payment.IdPayment = idPay;
                }
                var placeId = PrCommon.GetString("placeId", frmData);
                if (String.IsNullOrEmpty(placeId))
                {
                    //   payment.IdPayment = idPay;
                }

                var scheduleId = PrCommon.GetString("scheduleId", frmData);
                if (String.IsNullOrEmpty(scheduleId))
                {
                    //   payment.IdPayment = idPay;
                }
                var paymentId = PrCommon.GetString("paymentId", frmData);
                if (String.IsNullOrEmpty(paymentId))
                {
                    //   payment.IdPayment = idPay;
                }
                var nameCustomer = PrCommon.GetString("nameCustomer", frmData);
                if (String.IsNullOrEmpty(nameCustomer))
                {
                    // payment.IdPayment = namePay;
                }
                var address = PrCommon.GetString("address", frmData);
                if (String.IsNullOrEmpty(address))
                {
                    // payment.IdPayment = type;
                }
                var email = PrCommon.GetString("email", frmData);
                if (String.IsNullOrEmpty(email))
                { }
                var phone = PrCommon.GetString("phone", frmData);
                if (String.IsNullOrEmpty(phone))
                { }

                var nameContact = PrCommon.GetString("nameContact", frmData);
                if (String.IsNullOrEmpty(nameContact))
                { }
                var vat = PrCommon.GetString("vat", frmData);
                if (String.IsNullOrEmpty(vat))
                { }
                var pincode = PrCommon.GetString("pincode", frmData);
                if (String.IsNullOrEmpty(pincode))
                { }
                var voucherCode = PrCommon.GetString("voucherCode", frmData);
                if (String.IsNullOrEmpty(voucherCode))
                {
                }
                var totalPrice = PrCommon.GetString("totalPrice", frmData);
                if (String.IsNullOrEmpty(totalPrice))
                {
                    totalPrice = "0";
                }
                var totalPricePromotion = PrCommon.GetString("totalPricePromotion", frmData);
                if (String.IsNullOrEmpty(totalPricePromotion))
                {
                    totalPricePromotion = "0";
                }
                var valuePromotion = PrCommon.GetString("valuePromotion", frmData);
                if (isUpdate)
                {
                    CreateTourBookingViewModel updateObj = new CreateTourBookingViewModel();
                    updateObj.IdTourBooking = idTourBooking;
                    updateObj.NameCustomer = nameCustomer;
                    updateObj.Address = address;
                    updateObj.Email = email;
                    updateObj.Phone = phone;
                    updateObj.NameContact = nameContact;
                    updateObj.Vat = Convert.ToInt16(vat);
                    updateObj.Pincode = pincode;
                    return JsonSerializer.Serialize(updateObj);
                }
                CreateBookingDetailViewModel createDetailObj = new CreateBookingDetailViewModel();
                createDetailObj.Baby = Convert.ToInt16(baby);
                createDetailObj.Child = Convert.ToInt16(child);
                createDetailObj.Adult = Convert.ToInt16(adult);
                createDetailObj.Status = (Enums.StatusBooking)(Convert.ToInt16(status));
                createDetailObj.HotelId = Guid.Parse(hotelId);
                createDetailObj.RestaurantId = Guid.Parse(restaurantId);
                createDetailObj.PlaceId = Guid.Parse(placeId);

                CreateTourBookingViewModel createObj = new CreateTourBookingViewModel();
                createObj.ScheduleId = scheduleId;
                createObj.PaymentId = Convert.ToInt16(paymentId);
                createObj.NameCustomer = nameCustomer;
                createObj.Address = address;
                createObj.Email = email;
                createObj.Phone = phone;
                createObj.NameContact = nameContact;
                createObj.Vat = Convert.ToInt16(vat);
                createObj.TotalPrice = float.Parse(totalPrice);
                createObj.TotalPricePromotion = float.Parse(totalPricePromotion);
                createObj.Pincode = $"PIN{Ultility.ConvertDatetimeToUnixTimeStampMiliSecond(DateTime.Now)}";
                createObj.BookingDetails = createDetailObj;
                createObj.CustomerId = customerId;
                createObj.VoucherCode = voucherCode;
                if (!string.IsNullOrEmpty(valuePromotion))
                {
                    createObj.ValuePromotion = Convert.ToInt16(valuePromotion);

                }
                return JsonSerializer.Serialize(createObj);
            }
            catch (Exception e)
            {
                _message = Ultility.Responses("Có lỗi xảy ra !", Enums.TypeCRUD.Error.ToString(), description: e.Message).Notification;
                return null;
            }
        }
        public async Task<bool> CallServiceCheckIsScheduleInPromotion(string idSchedule)
        {
            bool isCorrect = false;
            using (var client = new HttpClient())
            {
                var urlService = _config["UrlService"].ToString();
                client.BaseAddress = new Uri($"{urlService}");
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync($"api/schedule/schedule-in-promotion?idSchedule={idSchedule}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string data = await response.Content.ReadAsStringAsync();
                    isCorrect = JsonSerializer.Deserialize<bool>(data,options);
                    return isCorrect;
                }

            }
            return isCorrect;
        }

        public async Task<Voucher> CallServiceCheckIsVoucherValid(string code, string customerId)
        {

            using (var client = new HttpClient())
            {
                var urlService = _config["UrlService"].ToString();
                client.BaseAddress = new Uri($"{urlService}");
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response = await client.GetAsync($"api/voucher/check-vourcher-by-code?code={code}&customerId={customerId}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string data = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Voucher>(data,options);
                }

            }
            return null;
        }
        public async Task<Schedule> CallServiceGetSchedule(string idSchedule)
        {

            using (var client = new HttpClient())
            {
                var urlService = _config["UrlService"].ToString();
                client.BaseAddress = new Uri($"{urlService}");
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response = await client.GetAsync($"api/schedule/schedule-s?idSchedule={idSchedule}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string data = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Schedule>(data,options);
                }

            }
            return null;
        }
        public async Task<Guid> CallServiceGetCustomerIdByPhone(string phone)
        {

            using (var client = new HttpClient())
            {
                var urlService = _config["UrlService"].ToString();
                client.BaseAddress = new Uri($"{urlService}");
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response = await client.GetAsync($"api/customer/customer-by-phone-s?phone={phone}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string data = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Guid>(data, options);
                }

            }
            return Guid.Empty;
        }
        public async Task<bool> CallServiceCheckEmptyCapacity(string idSchedule, int adult, int child, int baby)
        {
            bool isCorrect = false;
            using (var client = new HttpClient())
            {
                var urlService = _config["UrlService"].ToString();
                client.BaseAddress = new Uri($"{urlService}");
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response = await client.GetAsync($"api/schedule/check-empty-capacity?idSchedule={idSchedule}&adult={adult}&child={child}&baby={baby}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string data = await response.Content.ReadAsStringAsync();
                    isCorrect = JsonSerializer.Deserialize<bool>(data, options);
                    return isCorrect;
                }

            }
            return isCorrect;
        }
        public async Task<Tour> CallServiceGetTourByIdTour(string idTour)
        {
            using (var client = new HttpClient())
            {
                var urlService = _config["UrlService"].ToString();
                client.BaseAddress = new Uri($"{urlService}");
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response = await client.GetAsync($"api/tour/tour-s?idTour={idTour}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string data = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Tour>(data, options);
                }

            }
            return null;
        }
        public async Task CallServiceDeleteVoucher(Guid idVoucher)
        {

            using (var client = new HttpClient())
            {
                var urlService = _config["UrlService"].ToString();
                client.BaseAddress = new Uri($"{urlService}");
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response = await client.DeleteAsync($"api/voucher/vourcher-customer?idVoucher={idVoucher}");

            }
        }
        public async Task<List<string>> CallServiceGetListIdScheduleFinished()
        {
            List<string> lsIdSchedule = new();
            using (var client = new HttpClient())
            {
                var urlService = _config["UrlService"].ToString();
                client.BaseAddress = new Uri($"{urlService}");
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync($"api/schedule/list-id-finished-schedule");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string data = await response.Content.ReadAsStringAsync();
                    lsIdSchedule = JsonSerializer.Deserialize<List<string>>(data,options);
                    return lsIdSchedule;
                }

            }
            return lsIdSchedule;
        }
        public async Task<bool> CallServiceUpdateScoreToCustomer(Guid idCustomer, int point)
        {
            bool isCorrect = false;
            using (var client = new HttpClient())
            {
                var urlService = _config["UrlService"].ToString();
                client.BaseAddress = new Uri($"{urlService}");
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync($"api/customer/update-score?idCustomer={idCustomer}&point={point}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string data = await response.Content.ReadAsStringAsync();
                    isCorrect = JsonSerializer.Deserialize<bool>(data,options);
                    return isCorrect;
                }

            }
            return isCorrect;
        }

        public async Task<Tour> CallServiceGetTourByIdSchedule(string idSchedule) //CallServiceGetTourByIdSchedule
        {

            using (var client = new HttpClient())
            {
                var urlService = _config["UrlService"].ToString();
                client.BaseAddress = new Uri($"{urlService}");
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response = await client.GetAsync($"api/tour/tour-by-idschedule-s?idSchedule={idSchedule}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string data = await response.Content.ReadAsStringAsync();
                    var tour = JsonSerializer.Deserialize<Tour>(data, options);
                    return tour;
                }
                return null;
            }
        }
        public async Task CallServiceUpdateCapacity(string idSchedule, int quantityAdult, int quantityChild, int quantityBaby)
        {

            using (var client = new HttpClient())
            {
                var urlService = _config["UrlService"].ToString();
                client.BaseAddress = new Uri($"{urlService}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.DeleteAsync($"api/schedule/update-quantity-s?idSchedule={idSchedule}&quantityAdult={quantityAdult}&quantityChild={quantityChild}&quantityBaby={quantityBaby}");
            }
        }
        public async Task<Response> Create(CreateTourBookingViewModel input, string emailUser)
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                Voucher vourcher = new();
                // nếu có sài vourcher thì coi còn thời hạn hay ko
                if (!string.IsNullOrEmpty(input.VoucherCode))
                {
                    var isTourInPromotion = await CallServiceCheckIsScheduleInPromotion(input.ScheduleId);
                    if (isTourInPromotion)
                        return Ultility.Responses("Không thể áp dụng voucher cho tour đang có khuyến mãi !", Enums.TypeCRUD.Error.ToString());
                    var unixDateTimeNow = Ultility.ConvertDatetimeToUnixTimeStampMiliSecond(DateTime.Now);
                    vourcher = await CallServiceCheckIsVoucherValid(input.VoucherCode, input.CustomerId.ToString());

                    if (vourcher == null)
                    {
                        return Ultility.Responses("Vourcher không tồn tại hoặc hết hạn !", Enums.TypeCRUD.Error.ToString());
                    }

                    var valueVourcher = vourcher.Value;
                    input.TotalPrice = input.TotalPrice - (input.TotalPrice * (valueVourcher / 100));
                    await CallServiceDeleteVoucher(vourcher.IdVoucher);
                }

                TourBooking tourbooking = Mapper.MapCreateTourBooking(input);
                TourBookingDetails tourBookingDetail = Mapper.MapCreateTourBookingDetail(input.BookingDetails);

                #region check price
                var schedule = await CallServiceGetSchedule(input.ScheduleId);
                float priceSchedule = 0;
                var adult = input.BookingDetails.Adult;
                var child = input.BookingDetails.Child;
                var baby = input.BookingDetails.Baby;
                // có km
                if (schedule.IsHoliday)
                {
                    priceSchedule = schedule.FinalPriceHoliday;
                    priceSchedule = (adult * schedule.FinalPriceHoliday) + (child * schedule.PriceChildHoliday);
                }
                else
                {
                    priceSchedule = schedule.FinalPrice;
                    priceSchedule = (adult * schedule.FinalPrice) + (child * schedule.PriceChild);
                }
                var pricePromotion = (priceSchedule * (float)schedule.ValuePromotion) / 100;
                var totalPrice = Math.Round(priceSchedule - pricePromotion);
                // tính giá cho tất cả hành kháhc
                double totalPriceInput = 0;
                if (vourcher.Code != null) // có áp dụng vourcher hợp lệ
                {
                    var valueVourcher = vourcher.Value;
                    /*totalPrice = totalPrice - (totalPrice * (valueVourcher / 100));*/ // áp dụng giảm giá của vourcher

                    decimal price = (100m - valueVourcher) / 100m;
                    totalPrice = totalPrice * (double)price;
                    float totalPriceVoucher = (float)totalPrice;

                    totalPrice = Math.Round(totalPriceVoucher);
                    totalPriceInput = Math.Round(input.TotalPrice); // đã qua tính vourcher
                    //if (totalPrice != totalPriceInput) // giá ko giống nhau
                    //{
                    //    return Ultility.Responses("Hệ thống xảy ra lỗi, vui lòng thử lại !", Enums.TypeCRUD.Warning.ToString());
                    //}
                }

                #endregion
                await transaction.CreateSavepointAsync("BeforeSave");

                tourbooking.TourBookingDetails = tourBookingDetail;
                #region create qr
                string qrCodeText = _config["Bill"]+ "bill/" + tourbooking.IdTourBooking; // cần truyền gì bỏ vào
                string urlQR = AddImg(qrCodeText, tourbooking.IdTourBooking);
                tourbooking.UrlQR = urlQR;
                #endregion
                CreateDatabase<TourBooking>(tourbooking);
                CreateDatabase<TourBookingDetails>(tourBookingDetail);
                await SaveChangeAsync();
                _cache.Remove("schedule");
                _cache.Remove("scheduleflashsale");
                // cập nhật số lượng
                int quantityAdult = tourbooking.TourBookingDetails.Adult;
                int quantityChild = tourbooking.TourBookingDetails.Child;
                int quantityBaby = tourbooking.TourBookingDetails.Baby;
                await CallServiceUpdateCapacity(input.ScheduleId, quantityAdult, quantityChild, quantityBaby);



                transaction.Commit();
                transaction.Dispose();

                #region sms
                //Gửi sms
                //SpeedSMSAPI api = new SpeedSMSAPI("eHTE2iExhWKHCRk4OvTVT2gFHuPl4wDd");
                //String[] phones = new String[] { tourbooking.Phone };
                //String str = "Lụm";
                //String response = api.sendSMS(phones, str, 5, "d675521d17749e04");
                #endregion



                #region send mail
                var subjectOTP = _config["OTPSubject"];
                var emailSend = _config["emailSend"];
                var keySecurity = _config["keySecurity"];
                var linkbill = _config["Bill"];
                var stringHtml = Ultility.getHtmlBookingSuccess(tourbooking.Pincode, tourbooking.NameCustomer, tourbooking.Phone, tourbooking.TotalPrice.ToString(), urlQR, "Chưa thanh toán");

                Ultility.sendEmail(stringHtml, tourbooking.Email, "THÔNG BÁO ĐẶT TOUR", emailSend, keySecurity);
                #endregion

                #region send notifi
                var tour = await CallServiceGetTourByIdSchedule(input.ScheduleId);
                var nameTour = tour.NameTour;
                await _notification.CreateNotification(input.CustomerId.Value, Convert.ToInt16(Enums.TypeNotification.TourBooking), nameTour, new int[] { Convert.ToInt16(Enums.TitleRole.TourBookingManager) }, "");
                #endregion
                // làm tới đây rồi
                return Ultility.Responses("Đặt tour thành công !", Enums.TypeCRUD.Success.ToString(), tourbooking.IdTourBooking);


            }
            catch (Exception e)
            {
                transaction.RollbackToSavepoint("BeforeSave");
                return Ultility.Responses("Có lỗi xảy ra !", Enums.TypeCRUD.Error.ToString(), description: e.Message);
                ;
            }
        }
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            return ms.ToArray();
        }
        public byte[] CreateByteQR(string qrCodeText)
        {

            QRCodeGenerator _qrCode = new QRCodeGenerator();
            QRCodeData _qrCodeData = _qrCode.CreateQrCode(qrCodeText, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            System.Drawing.Image qrCodeImage = qrCode.GetGraphic(20);
            var bytes = ImageToByteArray(qrCodeImage);
            return bytes;
        }
        public Response StatisticPaidNotCheckedin()
        {
            try
            {
                var list = (from x in _db.TourBookings.AsNoTracking()
                            where x.Status == (int)Enums.StatusBooking.Paid &&
                                  x.CheckIn == 0
                            select x).ToList();

                var res = Ultility.Responses("", Enums.TypeCRUD.Success.ToString(), list);
                res.TotalResult = list.Count;
                return res;
            }
            catch (Exception e)
            {
                return Ultility.Responses("Có lỗi xảy ra !", Enums.TypeCRUD.Error.ToString(), description: e.Message);
            }
        }
        public string AddImg(string qrCodeText, string idService)
        {
            try
            {
                string urlQR = "";
                var bytes = CreateByteQR(qrCodeText);
                Travel.Context.Models.Image img = new Travel.Context.Models.Image();
                using (Stream stream = new MemoryStream(bytes))
                {
                    urlQR = Ultility.UploadQR(stream, idService, ref message);

                }
                return urlQR;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Response> Gets()
        {
            try
            {
                var ListTourBooking = await (from x in _db.TourBookings.AsNoTracking()
                                             orderby x.DateBooking descending
                                             select new TourBooking
                                             {
                                                 IdTourBooking = x.IdTourBooking,
                                                 LastDate = x.LastDate,
                                                 NameCustomer = x.NameCustomer,
                                                 NameContact = x.NameContact,
                                                 Pincode = x.Pincode,
                                                 Email = x.Email,
                                                 Phone = x.Phone,
                                                 ScheduleId = x.ScheduleId,
                                                 Status = x.Status,
                                                 Address = x.Address,
                                                 AdditionalPrice = x.AdditionalPrice,
                                                 BookingNo = x.BookingNo,
                                                 IsCalled = x.IsCalled,
                                                 DateBooking = x.DateBooking,
                                                 TotalPrice = x.TotalPrice,
                                                 TotalPricePromotion = x.TotalPricePromotion,
                                                 VoucherCode = x.VoucherCode,
                                                 ValuePromotion = x.ValuePromotion,
                                                 Payment = (from p in _db.Payment.AsNoTracking()
                                                            where p.IdPayment == x.PaymentId
                                                            select p).FirstOrDefault(),
                                                 TourBookingDetails = (from tbd in _db.tourBookingDetails.AsNoTracking()
                                                                       where tbd.IdTourBookingDetails == x.IdTourBooking
                                                                       select tbd).FirstOrDefault(),
                                             }).ToListAsync();
                foreach (var item in ListTourBooking)
                {
                    item.Schedule = await CallServiceGetSchedule(item.ScheduleId);
                }
                return Ultility.Responses("", Enums.TypeCRUD.Success.ToString(), ListTourBooking);

            }
            catch (Exception e)
            {
                return Ultility.Responses("Có lỗi xảy ra !", Enums.TypeCRUD.Error.ToString(), description: e.Message);
            }
        }

        public Response GetTourBookingFromDateToDate(DateTime? fromDateInput, DateTime? toDateInput)
        {

            try
            {
                // khai báo
                long fromDate = 0;
                long toDate = 0;

                // gán dữ liệu
                if (fromDateInput != null)
                {
                    fromDate = Ultility.ConvertDatetimeToUnixTimeStampMiliSecond(fromDateInput.Value); // nếu ko bị null thì gán dữ liệu vào
                }
                if (toDateInput != null)
                {
                    toDate = Ultility.ConvertDatetimeToUnixTimeStampMiliSecond(toDateInput.Value); // nếu ko bị null thì gán dữ liệu vào
                }
                else
                {
                    toDate = long.MaxValue; // nếu toDate ko gán thì cho nó dữ liệu max, để ngày nào nó cũng lấy 
                }

                var list = (from x in _db.TourBookings
                            where x.DateBooking >= fromDate
                            && x.DateBooking <= toDate
                            select x).OrderByDescending(x => x.DateBooking).ToList();
                var result = Mapper.MapTourBooking(list);
                return Ultility.Responses("", Enums.TypeCRUD.Success.ToString(), result);
            }
            catch (Exception e)
            {
                return Ultility.Responses("Có lỗi xảy ra !", Enums.TypeCRUD.Error.ToString(), description: e.Message);
            }
        }
        public async Task<Response> TourBookingById(string idTourbooking)
        {
            try
            {
                var tourbooking = await (from x in _db.TourBookings.AsNoTracking()
                                         where x.IdTourBooking == idTourbooking
                                         orderby x.DateBooking descending
                                         select new TourBooking
                                         {

                                             IdTourBooking = x.IdTourBooking,
                                             LastDate = x.LastDate,
                                             NameCustomer = x.NameCustomer,
                                             NameContact = x.NameContact,
                                             Pincode = x.Pincode,
                                             ScheduleId = x.ScheduleId,
                                             Email = x.Email,
                                             Phone = x.Phone,
                                             Status = x.Status,
                                             Address = x.Address,
                                             AdditionalPrice = x.AdditionalPrice,
                                             Deposit = x.Deposit,
                                             RemainPrice = x.RemainPrice,
                                             BookingNo = x.BookingNo,
                                             DateBooking = x.DateBooking,
                                             TotalPrice = x.TotalPrice,
                                             TotalPricePromotion = x.TotalPricePromotion,
                                             VoucherCode = x.VoucherCode,
                                             ValuePromotion = x.ValuePromotion,
                                             Payment = (from p in _db.Payment.AsNoTracking()
                                                        where p.IdPayment == x.PaymentId
                                                        select p).FirstOrDefault(),
                                             TourBookingDetails = (from tbd in _db.tourBookingDetails
                                                                   where tbd.IdTourBookingDetails == x.IdTourBooking
                                                                   select tbd).FirstOrDefault()
                                         }).FirstOrDefaultAsync();
                tourbooking.Schedule = await CallServiceGetSchedule(tourbooking.ScheduleId);
                return Ultility.Responses("", Enums.TypeCRUD.Success.ToString(), tourbooking);

            }
            catch (Exception e)
            {
                return Ultility.Responses("Có lỗi xảy ra !", Enums.TypeCRUD.Error.ToString(), description: e.Message);
            }
        }
        public async Task<Response> DoPayment(string idTourBooking,string customerid, string phone) // for admin if customer payment
        {
            try
            {
                Guid idCustomer = Guid.Empty;
                var tourbooking = await (from tb in _db.TourBookings.AsNoTracking()
                                         where tb.IdTourBooking == idTourBooking
                                         && tb.Status == (int)Enums.StatusBooking.Paying
                                         select tb).FirstOrDefaultAsync();
                



               
                if (tourbooking != null)
                {
                    // kiểm tra xem có không, nếu chưa có id thì gán lại cho nó
                    if (tourbooking.CustomerId == Guid.Empty)
                    {
                        if (!string.IsNullOrEmpty(customerid))
                        {
                             idCustomer = Guid.Parse(customerid);
                        }
                        // gọi service lấy ra customer 
                        if (!string.IsNullOrEmpty(phone))
                        {
                            idCustomer = await CallServiceGetCustomerIdByPhone(phone);
                        }
                        tourbooking.CustomerId = idCustomer;
                    }

                    var bookingNo = $"{tourbooking.IdTourBooking}NO";
                    tourbooking.Deposit = tourbooking.TotalPrice;
                    tourbooking.Status = (int)Enums.StatusBooking.Paid;
                    tourbooking.BookingNo = bookingNo;
                    UpdateDatabase<TourBooking>(tourbooking);
                    await SaveChangeAsync();
                    #region sendMail

                    var emailSend = _config["emailSend"];
                    var keySecurity = _config["keySecurity"];
                    var stringHtml = Ultility.getHtmlBookingTicket($"{bookingNo} <br> Vui lòng ghi nhớ mã BookingNo này", "Thanh toán thành công", "BookingNo");

                    Ultility.sendEmail(stringHtml, tourbooking.Email, "Thanh toán dịch vụ", emailSend, keySecurity);
                    #endregion

                    return Ultility.Responses("Thanh toán thành công !", Enums.TypeCRUD.Success.ToString());

                }
                else
                {
                    return Ultility.Responses("Không tìm thấy dữ liệu !", Enums.TypeCRUD.Warning.ToString(), null);

                }
            }
            catch (Exception e)
            {
                return Ultility.Responses("Có lỗi xảy ra !", Enums.TypeCRUD.Error.ToString(), description: e.Message);
            }
        }

        public async Task<Response> CancelBooking(string idTourBooking)
        {
            try
            {
                var tourbooking = await (from tb in _db.TourBookings.AsNoTracking()
                                         where tb.IdTourBooking == idTourBooking
                                         && tb.Status == (int)Enums.StatusBooking.Paying
                                         select tb).FirstOrDefaultAsync();
                if (tourbooking != null)
                {
                    tourbooking.Status = (int)Enums.StatusBooking.Cancel;
                    UpdateDatabase<TourBooking>(tourbooking);
                    #region sendMail

                    var emailSend = _config["emailSend"];
                    var keySecurity = _config["keySecurity"];

                    var stringHtml = Ultility.getHtmlBookingCancel(tourbooking.IdTourBooking, tourbooking.Email);

                    Ultility.sendEmail(stringHtml, tourbooking.Email, "Thanh toán dịch vụ", emailSend, keySecurity);
                    #endregion
                    SaveChange();

                    return Ultility.Responses("Đã hủy booking !", Enums.TypeCRUD.Success.ToString());
                }
                else
                {
                    return Ultility.Responses("Hủy booking thất bại !", Enums.TypeCRUD.Error.ToString());
                }

            }
            catch (Exception e)
            {
                return Ultility.Responses("Có lỗi xảy ra !", Enums.TypeCRUD.Error.ToString(), description: e.Message);
            }
        }
        public async Task<Response> RestoreBooking(string idTourBooking, string emailUser)
        {
            try
            {
                var tourbooking = await (from tb in _db.TourBookings.AsNoTracking()
                                         where tb.IdTourBooking == idTourBooking
                                         && tb.Status == (int)Enums.StatusBooking.Cancel
                                         select tb).FirstOrDefaultAsync();
                if (tourbooking != null)
                {
                    tourbooking.Status = (int)Enums.StatusBooking.Paying;
                    UpdateDatabase<TourBooking>(tourbooking);
                    SaveChange();


                    return Ultility.Responses("Đã hủy booking !", Enums.TypeCRUD.Success.ToString());


                }
                else
                {
                    return Ultility.Responses("Hủy booking thất bại !", Enums.TypeCRUD.Error.ToString());
                }

            }
            catch (Exception e)
            {
                return Ultility.Responses("Có lỗi xảy ra !", Enums.TypeCRUD.Error.ToString(), description: e.Message);
            }
        }

        public async Task<Response> TourBookingByBookingNo(string bookingNo)
        {
            try
            {
                var tourbooking = await (from x in _db.TourBookings.AsNoTracking()
                                         where x.BookingNo == bookingNo
                                         select new TourBooking
                                         {
                                             LastDate = x.LastDate,
                                             NameCustomer = x.NameCustomer,
                                             NameContact = x.NameContact,
                                             Pincode = x.Pincode,
                                             Email = x.Email,
                                             Phone = x.Phone,
                                             ScheduleId = x.ScheduleId,
                                             Address = x.Address,
                                             AdditionalPrice = x.AdditionalPrice,
                                             BookingNo = x.BookingNo,
                                             DateBooking = x.DateBooking,
                                             TotalPrice = x.TotalPrice,
                                             TotalPricePromotion = x.TotalPricePromotion,
                                             VoucherCode = x.VoucherCode,
                                             ValuePromotion = x.ValuePromotion,
                                             Payment = (from p in _db.Payment.AsNoTracking()
                                                        where p.IdPayment == x.PaymentId
                                                        select p).FirstOrDefault(),
                                             TourBookingDetails = (from tbd in _db.tourBookingDetails.AsNoTracking()
                                                                   where tbd.IdTourBookingDetails == x.IdTourBooking
                                                                   select tbd).FirstOrDefault(),
                                         }).FirstOrDefaultAsync();
                tourbooking.Schedule = await CallServiceGetSchedule(tourbooking.ScheduleId);
                return Ultility.Responses("", Enums.TypeCRUD.Success.ToString(), tourbooking);

            }
            catch (Exception e)
            {
                return Ultility.Responses("Có lỗi xảy ra !", Enums.TypeCRUD.Error.ToString(), description: e.Message);
            }
        }

        public Response StatisticTourBooking()
        {
            try
            {  // Đã đặt tour nhưng chưa thanh toán
                var lsTourBookingPaying = (from x in _db.TourBookings.AsNoTracking()
                                           where x.Status == (int)Enums.StatusBooking.Paying
                                           select x).Count();
                // tour đã thanh toán hết  
                var lsTourBookingPaid = (from x in _db.TourBookings.AsNoTracking()
                                         where x.Status == (int)Enums.StatusBooking.Paid
                                         select x).Count();
                // tourr đã hủy
                var lsTourBookingCancel = (from x in _db.TourBookings.AsNoTracking()
                                           where x.Status == (int)Enums.StatusBooking.Cancel
                                           select x).Count();
                var ab = String.Format("tourPaying: {0} && tourPaid: {1} && tourCancel: {2}", lsTourBookingPaying, lsTourBookingPaid, lsTourBookingCancel);
                return Ultility.Responses("", Enums.TypeCRUD.Success.ToString(), ab);
            }
            catch (Exception e)
            {
                return Ultility.Responses("Có lỗi xảy ra !", Enums.TypeCRUD.Error.ToString(), description: e.Message);


            }
        }

        public Response CheckCalled(string idTourBooking)
        {
            try
            {
                var tourbooking = (from tb in _db.TourBookings.AsNoTracking()
                                   where tb.IdTourBooking == idTourBooking
                                   select tb).FirstOrDefault();
                if (tourbooking != null)
                {
                    tourbooking.IsCalled = true;
                    UpdateDatabase<TourBooking>(tourbooking);
                    SaveChange();

                    //#region sendMail

                    //var emailSend = _config["emailSend"];
                    //var keySecurity = _config["keySecurity"];
                    //var stringHtml = Ultility.getHtml($"{bookingNo} <br> Vui lòng ghi nhớ mã BookingNo này", "Thanh toán thành công", "BookingNo");

                    //Ultility.sendEmail(stringHtml, tourbooking.Email, "Thanh toán dịch vụ", emailSend, keySecurity);
                    //#endregion
                    return Ultility.Responses("Đã gọi !", Enums.TypeCRUD.Success.ToString());
                }
                else
                {
                    return Ultility.Responses("Gọi thất bại !", Enums.TypeCRUD.Error.ToString());
                }

            }
            catch (Exception e)
            {
                return Ultility.Responses("Có lỗi xảy ra !", Enums.TypeCRUD.Error.ToString(), description: e.Message);
            }
        }

        private void UpdateDatabase(TourBooking tourbooking)
        {
            _db.Entry(tourbooking).State = EntityState.Modified;
            _db.SaveChanges();
        }
        private async Task UpdateDatabaseAsync(TourBooking tourbooking)
        {
            _db.Entry(tourbooking).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
        private void DeleteDatabase(TourBooking tourbooking)
        {
            _db.Entry(tourbooking).State = EntityState.Deleted;
            _db.SaveChanges();
        }
        private void CreateDatabase(TourBooking tourbooking)
        {
            _db.TourBookings.Add(tourbooking);
            _db.SaveChanges();
        }

        public async Task<Response> SearchTourBooking(JObject frmData)
        {
            try
            {
                var totalResult = 0;
                Keywords keywords = new Keywords();
                var pageSize = PrCommon.GetString("pageSize", frmData) == null ? 10 : Convert.ToInt16(PrCommon.GetString("pageSize", frmData));
                var pageIndex = PrCommon.GetString("pageIndex", frmData) == null ? 1 : Convert.ToInt16(PrCommon.GetString("pageIndex", frmData));
                var kwId = PrCommon.GetString("IdTourBooking", frmData).Trim();
                if (!String.IsNullOrEmpty(kwId))
                {
                    keywords.KwId = kwId.Trim().ToLower();
                }
                else
                {
                    keywords.KwId = "";

                }
                var kwPincode = PrCommon.GetString("Pincode", frmData).Trim();
                if (!String.IsNullOrEmpty(kwPincode))
                {
                    keywords.KwPincode = kwPincode.Trim().ToLower();
                }
                else
                {
                    keywords.KwPincode = "";

                }

                var kwBookingNo = PrCommon.GetString("BookingNo", frmData).Trim();
                if (!String.IsNullOrEmpty(kwBookingNo))
                {
                    keywords.KwBookingNo = kwBookingNo.Trim().ToLower();
                }
                else
                {
                    keywords.KwBookingNo = "";

                }

                //var kwEmail = PrCommon.GetString("Email", frmData).Trim();
                //if (!String.IsNullOrEmpty(kwEmail))
                //{
                //    keywords.KwEmail = kwEmail.Trim().ToLower();
                //}
                //else
                //{
                //    keywords.KwEmail = "";

                //}
                var kwPhone = PrCommon.GetString("phone", frmData).Trim();
                if (!String.IsNullOrEmpty(kwPhone))
                {
                    keywords.KwPhone = kwPhone.Trim().ToLower();
                }
                else
                {
                    keywords.KwPhone = "";

                }

                var fromDate = PrCommon.GetString("dateBookingFrom", frmData);
                if (!String.IsNullOrEmpty(fromDate))
                {
                    keywords.KwFromDate = Ultility.ConvertDatetimeToUnixTimeStampMiliSecond(DateTime.Parse(fromDate));
                }
                else
                {
                    keywords.KwFromDate = 0;
                }

                var toDate = PrCommon.GetString("dateBookingTo", frmData);
                if (!String.IsNullOrEmpty(toDate))
                {
                    keywords.KwToDate = Ultility.ConvertDatetimeToUnixTimeStampMiliSecond(DateTime.Parse(toDate).AddDays(1).AddSeconds(-1));
                }
                else
                {
                    keywords.KwToDate = 0;
                }

                //var kwDate = PrCommon.GetString("DateBooking", frmData).Trim();
                //if (!String.IsNullOrEmpty(kwDate))
                //{
                //    keywords.KwDate = long.Parse(kwDate);
                //}
                //else
                //{

                //    keywords.KwDate = 0;
                //}
                var kwIsCall = PrCommon.GetString("IsCalled", frmData);

                if (!String.IsNullOrEmpty(kwIsCall))
                {
                    keywords.kwIsCalled = Boolean.Parse(kwIsCall);
                }

                var status = PrCommon.GetString("status", frmData);
                keywords.KwStatusList = PrCommon.getListInt(status, ',', false);
                var listTourBooking = new List<TourBooking>();
                #region filter
                var queryListTourBooking = (from x in _db.TourBookings.AsNoTracking()
                                            where
                                                            x.IdTourBooking.ToLower().Contains(keywords.KwId) &&
                                                            x.Pincode.ToLower().Contains(keywords.KwPincode) &&
                                                            x.BookingNo.ToLower().Contains(keywords.KwBookingNo) &&
                                                            x.Phone.ToLower().Contains(keywords.KwPhone)

                                            select x);

                if (keywords.KwFromDate != 0 && keywords.KwToDate != 0)
                {
                    queryListTourBooking = from x in queryListTourBooking
                                           where x.DateBooking >= keywords.KwFromDate
                                           && x.DateBooking <= keywords.KwToDate
                                           select x;
                }
                else if (keywords.KwFromDate == 0 && keywords.KwToDate == 0)
                {
                }
                else
                {
                    if (keywords.KwFromDate == 0)
                    {
                        queryListTourBooking = from x in queryListTourBooking
                                               where x.DateBooking <= keywords.KwToDate
                                               select x;
                    }
                    else
                    {
                        queryListTourBooking = from x in queryListTourBooking
                                               where x.DateBooking >= keywords.KwFromDate
                                               select x;
                    }
                }

                if (!string.IsNullOrEmpty(kwIsCall))
                {
                    queryListTourBooking = from x in queryListTourBooking
                                           where x.IsCalled == keywords.kwIsCalled
                                           select x;
                }

                if (keywords.KwStatusList.Count > 0)
                {
                    queryListTourBooking = from x in queryListTourBooking
                                           where keywords.KwStatusList.Contains(x.Status)
                                           select x;
                }
                var kwPayment = PrCommon.GetString("Payment", frmData);
                keywords.KwPayment = PrCommon.getListInt(kwPayment, ',', false);
                if (keywords.KwPayment.Count > 0)
                {
                    queryListTourBooking = from x in queryListTourBooking
                                           where keywords.KwPayment.Contains(x.PaymentId)
                                           select x;
                }
                #endregion
                var kwToPlace = PrCommon.GetString("ToPlace", frmData);

                totalResult = queryListTourBooking.Count();
                queryListTourBooking = (from x in queryListTourBooking
                                        orderby x.DateBooking descending
                                        select new TourBooking
                                        {
                                            DateBooking = x.DateBooking,
                                            Deposit = x.Deposit,
                                            TourBookingDetails = (from tbkd in _db.tourBookingDetails.AsNoTracking()
                                                                  where tbkd.IdTourBookingDetails == x.IdTourBooking
                                                                  select tbkd).FirstOrDefault(),
                                            ScheduleId = x.ScheduleId,
                                            LastDate = x.LastDate,
                                            ModifyDate = x.ModifyDate,
                                            BookingNo = x.BookingNo,
                                            AdditionalPrice = x.AdditionalPrice,
                                            Address = x.Address,
                                            CheckIn = x.CheckIn,
                                            CheckOut = x.CheckOut,
                                            CustomerId = x.CustomerId,
                                            Email = x.Email,
                                            IdTourBooking = x.IdTourBooking,
                                            IsCalled = x.IsCalled,
                                            ModifyBy = x.ModifyBy,
                                            NameContact = x.NameContact,
                                            NameCustomer = x.NameCustomer,
                                            Payment = x.Payment,
                                            PaymentId = x.PaymentId,
                                            Phone = x.Phone,
                                            Pincode = x.Pincode,
                                            RemainPrice = x.RemainPrice,
                                            Status = x.Status,
                                            TotalPrice = x.TotalPrice,
                                            TotalPricePromotion = x.TotalPricePromotion,
                                            UrlQR = x.UrlQR,
                                            ValuePromotion = x.ValuePromotion,
                                            Vat = x.Vat,
                                            VoucherCode = x.VoucherCode
                                        });
                 listTourBooking = queryListTourBooking.ToList();
                foreach (var item in listTourBooking)
                {
                    item.Schedule = await CallServiceGetSchedule(item.ScheduleId);
                }
                if (!string.IsNullOrEmpty(kwToPlace))
                {
                    listTourBooking = (from x in listTourBooking
                                       where x.Schedule.Tour.ToPlace == kwToPlace
                                            select x).ToList();
                }

                listTourBooking = listTourBooking.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                var result = Mapper.MapTourBooking(listTourBooking);
                var res = Ultility.Responses("", Enums.TypeCRUD.Success.ToString(), result);
                res.TotalResult = totalResult;
                if (result.Count == 0)
                {
                    res = Ultility.Responses("Không có dữ liệu trả về !", Enums.TypeCRUD.Warning.ToString(), null);
                }

                return res;
            }
            catch (Exception e)
            {
                return Ultility.Responses("Có lỗi xảy ra !", Enums.TypeCRUD.Error.ToString(), description: e.Message);
            }
        }

        //public Response UpdateStatus(string pincode, string emailUser)
        //{
        //    try
        //    {
        //        var tourBooking = (from x in _db.TourBookings.AsNoTracking()
        //                    where x.Pincode == pincode
        //                    select x).FirstOrDefault();

        //        if (tourBooking != null )
        //        {
        //            if(tourBooking.Status == 1)
        //            {

        //                tourBooking.Status = (int)Enums.StatusBooking.Paid;
        //                UpdateDatabase(tourBooking);
        //            }
        //            else
        //            {
        //                return Ultility.Responses($"Không tìm thấy !", Enums.TypeCRUD.Warning.ToString());
        //            }
        //        }
        //        else
        //        {
        //            return Ultility.Responses($"Không tìm thấy !", Enums.TypeCRUD.Warning.ToString());
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return Ultility.Responses("Có lỗi xảy ra !", Enums.TypeCRUD.Error.ToString(), description: e.Message);

        //    }
        //}


        public async Task<Response> UpdateStatus(string idTourBooking, int status, string emailUser)
        {
            try
            {
                var tourBooking = (from x in _db.TourBookings.AsNoTracking()
                                   where x.IdTourBooking == idTourBooking
                                   select x).FirstOrDefault();

                if (tourBooking != null)
                {
                    tourBooking.Status = status;
                    await UpdateDatabaseAsync(tourBooking);
                    return Ultility.Responses($"Cập nhật trạng thái thành công !", Enums.TypeCRUD.Success.ToString());

                }
                else
                {
                    return Ultility.Responses($"Không tìm thấy !", Enums.TypeCRUD.Warning.ToString());
                }
            }
            catch (Exception e)
            {
                return Ultility.Responses("Có lỗi xảy ra !", Enums.TypeCRUD.Error.ToString(), description: e.Message);

            }
        }

        public async Task<TourBooking> GetTourBookingByIdForPayPal(string idTourBooking)
        {
            try
            {
                return await _db.TourBookings.FindAsync(idTourBooking);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<TourBooking> GetTourBookingByIdForVnPay(string idTourBooking)
        {
            try
            {
                return await _db.TourBookings.FindAsync(idTourBooking);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> UpdateTourBookingFinished()
        {
            try
            {

                List<TourBooking> lsTourbookingFinished = new();
                var lsIdScheduleFinished = await CallServiceGetListIdScheduleFinished();
                if (lsIdScheduleFinished.Count() > 0)
                {
                    #region get tourbooking by idScheduleFinished

                    foreach (var item in lsIdScheduleFinished)
                    {
                        var lsBooking = await (from tbk in _db.TourBookings.AsNoTracking()
                                               where tbk.Status == (int)Enums.StatusBooking.Paid
                                               && tbk.ScheduleId == item
                                               select tbk).ToListAsync();
                        lsTourbookingFinished.AddRange(lsBooking);
                    }
                    #endregion
                    foreach (var item in lsTourbookingFinished)
                    {
                        var point = (item.TotalPrice + item.TotalPricePromotion) / 100000;
                        var pointAdd = (int)Math.Round(point);
                        var idCustomer = item.CustomerId;

                        var isAddedPointSuccess = await CallServiceUpdateScoreToCustomer(idCustomer, pointAdd);
                        if (!isAddedPointSuccess)
                        {
                            return false;
                        }

                        item.Status = (int)Enums.StatusBooking.Finished;
                        item.CheckOut = Ultility.ConvertDatetimeToUnixTimeStampMiliSecond(DateTime.Now);
                        UpdateDatabase(item);
                    }
                    await SaveChangeAsync();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Response CheckInBooking(string bookingNo)
        {
            try
            {
                var tourBooking = (from x in _db.TourBookings.AsNoTracking()
                                   where x.BookingNo == bookingNo
                                   select x).FirstOrDefault();
                if (tourBooking != null)
                {
                    tourBooking.CheckIn = Ultility.ConvertDatetimeToUnixTimeStampMiliSecond(DateTime.Now);
                    UpdateDatabase(tourBooking);
                }
                else
                {
                    return Ultility.Responses($"Không tìm thấy !", Enums.TypeCRUD.Warning.ToString());
                }

                return Ultility.Responses("Check in thành công !", Enums.TypeCRUD.Success.ToString());
            }
            catch (Exception e)
            {
                return Ultility.Responses("Có lỗi xảy ra !", Enums.TypeCRUD.Error.ToString(), description: e.Message);
            }
        }

        public async Task<Response> CusSearchBookingNo(string bookingNo)
        {
            try
            {
                var tourBooking = (from x in _db.TourBookings.AsNoTracking()
                                   where x.BookingNo == bookingNo
                                   select new TourBooking
                                   {
                                       IdTourBooking = x.IdTourBooking,
                                       LastDate = x.LastDate,
                                       NameCustomer = x.NameCustomer,
                                       NameContact = x.NameContact,
                                       Pincode = x.Pincode,
                                       ScheduleId = x.ScheduleId,
                                       Email = x.Email,
                                       Phone = x.Phone,
                                       Status = x.Status,
                                       Address = x.Address,
                                       AdditionalPrice = x.AdditionalPrice,
                                       BookingNo = x.BookingNo,
                                       DateBooking = x.DateBooking,
                                       TotalPrice = x.TotalPrice,
                                       TotalPricePromotion = x.TotalPricePromotion,
                                       VoucherCode = x.VoucherCode,
                                       ValuePromotion = x.ValuePromotion,
                                       Payment = (from p in _db.Payment.AsNoTracking()
                                                  where p.IdPayment == x.PaymentId
                                                  select p).FirstOrDefault(),
                                       TourBookingDetails = (from tbd in _db.tourBookingDetails
                                                             where tbd.IdTourBookingDetails == x.IdTourBooking
                                                             select tbd).FirstOrDefault(),
                                   }).FirstOrDefault();
                
                if (tourBooking != null)
                {
                    tourBooking.Schedule = await CallServiceGetSchedule(tourBooking.ScheduleId);
                    return Ultility.Responses("", Enums.TypeCRUD.Success.ToString(), tourBooking);
                }
                else
                {
                    return Ultility.Responses($"Số BookingNo: {bookingNo} Không tìm thấy !", Enums.TypeCRUD.Warning.ToString());
                }
            }
            catch (Exception e)
            {
                return Ultility.Responses("Có lỗi xảy ra !", Enums.TypeCRUD.Error.ToString(), description: e.Message);
            }
        }

        public async Task<bool> ChangePayment(string idTourBooking, int idPayment)
        {
            try
            {

                var tourBooking = (from x in _db.TourBookings.AsNoTracking()
                                   where x.IdTourBooking == idTourBooking
                                   select x).FirstOrDefault();

                tourBooking.PaymentId = idPayment;
                UpdateDatabase(tourBooking);
                await SaveChangeAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<TourBooking>> TourBookingByIdCustomer(Guid idCustomer)
        {

            var tourBooking = await _db.TourBookings.Where(x => x.CustomerId == idCustomer).Include(x => x.TourBookingDetails).AsNoTracking().ToListAsync();
            return tourBooking;
        }

        public async Task ChangeFeedBack(string idTourBooking)
        {
            var tourBooking = await (from x in _db.TourBookings.AsNoTracking()
                                     where x.IdTourBooking == idTourBooking
                                     select x).FirstOrDefaultAsync();

            if (tourBooking != null)
            {
                tourBooking.IsSendFeedBack = true;
                UpdateDatabase(tourBooking);
                await SaveChangeAsync();
            }
        }

        public List<TourBooking> GetListBookingByIdSchedule(string idschedule)
        {
            var tourBooking = (from x in _db.TourBookings.AsNoTracking()
                                    where x.ScheduleId == idschedule
                                    && (x.Status == (int)Enums.StatusBooking.Paying || x.Status == (int)Enums.StatusBooking.Paid)
                                    select x).ToList();
            return tourBooking;
        }
    }
}
