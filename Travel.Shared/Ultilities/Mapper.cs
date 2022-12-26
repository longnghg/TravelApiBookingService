using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Context.Models;
using Travel.Shared.ViewModels.Travel;
using Travel.Shared.Ultilities;
using Travel.Shared.ViewModels;
using Travel.Shared.ViewModels.Travel.TourBookingVM;
using static Travel.Shared.Ultilities.Enums;
using Travel.Context.Models.Notification;
using Travel.Shared.ViewModels.Notify.CommentVM;

namespace Travel.Shared.Ultilities
{
    public static class Mapper
    {
        private static IMapper _mapper;
        public static void RegisterMappings()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {



                // create tourbooking details

                cfg.CreateMap<CreateBookingDetailViewModel, TourBookingDetails>()
                   .ForMember(dto => dto.IsCalled, opt => opt.MapFrom(src => false))
                   .ForMember(dto => dto.Baby, opt => opt.MapFrom(src => src.Baby))
                   .ForMember(dto => dto.Child, opt => opt.MapFrom(src => src.Child))
                   .ForMember(dto => dto.Adult, opt => opt.MapFrom(src => src.Adult == 0 ? 1 : src.Adult))
                   .ForMember(dto => dto.Status, opt => opt.MapFrom(src => src.Status))
                   .ForMember(dto => dto.HotelId, opt => opt.MapFrom(src => src.HotelId))
                   .ForMember(dto => dto.RestaurantId, opt => opt.MapFrom(src => src.RestaurantId))
                   .ForMember(dto => dto.PlaceId, opt => opt.MapFrom(src => src.PlaceId))
                   ;
                // create tourbooking
                cfg.CreateMap<CreateTourBookingViewModel, TourBooking>()
                   .ForMember(dto => dto.IdTourBooking, opt => opt.MapFrom(src => $"TRB-{Ultility.ConvertDatetimeToUnixTimeStampMiliSecond(DateTime.Now)}"))
                   .ForMember(dto => dto.PaymentId, opt => opt.MapFrom(src => src.PaymentId))
                   .ForMember(dto => dto.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                   .ForMember(dto => dto.TotalPricePromotion, opt => opt.MapFrom(src => src.TotalPricePromotion))
                   .ForMember(dto => dto.ScheduleId, opt => opt.MapFrom(src => src.ScheduleId))
                    .ForMember(dto => dto.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                   .ForMember(dto => dto.NameCustomer, opt => opt.MapFrom(src => src.NameCustomer))
                   .ForMember(dto => dto.Address, opt => opt.MapFrom(src => src.Address))
                   .ForMember(dto => dto.Email, opt => opt.MapFrom(src => src.Email))
                   .ForMember(dto => dto.Phone, opt => opt.MapFrom(src => src.Phone))
                   .ForMember(dto => dto.NameContact, opt => opt.MapFrom(src => src.NameContact))
                   .ForMember(dto => dto.DateBooking, opt => opt.MapFrom(src => Ultility.ConvertDatetimeToUnixTimeStampMiliSecond(DateTime.Now)))
                   .ForMember(dto => dto.LastDate, opt => opt.MapFrom(src => Ultility.ConvertDatetimeToUnixTimeStampMiliSecond(DateTime.Now.AddMinutes(4))))
                   .ForMember(dto => dto.Vat, opt => opt.MapFrom(src => src.Vat))
                   .ForMember(dto => dto.Pincode, opt => opt.MapFrom(src => src.Pincode))
                   .ForMember(dto => dto.VoucherCode, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.VoucherCode) ? src.VoucherCode : ""))
                   .ForMember(dto => dto.BookingNo, opt => opt.MapFrom(src => ""))
                      .ForMember(dto => dto.ValuePromotion, opt => opt.MapFrom(src => src.ValuePromotion == null ? 0 : src.ValuePromotion))
                   .ForMember(dto => dto.IsCalled, opt => opt.MapFrom(src => false))
                   .ForMember(dto => dto.Deposit, opt => opt.MapFrom(src => 0))
                   .ForMember(dto => dto.RemainPrice, opt => opt.MapFrom(src => 0))
                   .ForMember(dto => dto.Status, opt => opt.MapFrom(src => Enums.StatusBooking.Paying))
                   .ForMember(dto => dto.ModifyBy, opt => opt.MapFrom(src => ""))
                   .ForMember(dto => dto.ModifyDate, opt => opt.MapFrom(src => 0))
                                  .ForMember(dto => dto.CheckIn, opt => opt.MapFrom(src => 0))
                   .ForMember(dto => dto.CheckOut, opt => opt.MapFrom(src => 0));

                // view tourbooking
                cfg.CreateMap<TourBooking, TourBookingViewModel>()
                    .ForMember(dto => dto.IdTourBooking, opt => opt.MapFrom(src => src.IdTourBooking))
                    .ForMember(dto => dto.NameCustomer, opt => opt.MapFrom(src => src.NameCustomer))
                    .ForMember(dto => dto.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                    .ForMember(dto => dto.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                     .ForMember(dto => dto.TotalPricePromotion, opt => opt.MapFrom(src => src.TotalPricePromotion))
                    .ForMember(dto => dto.Address, opt => opt.MapFrom(src => src.Address))
                    .ForMember(dto => dto.Email, opt => opt.MapFrom(src => src.Email))
                    .ForMember(dto => dto.Phone, opt => opt.MapFrom(src => src.Phone))
                    .ForMember(dto => dto.NameContact, opt => opt.MapFrom(src => src.NameContact))
                    .ForMember(dto => dto.DateBooking, opt => opt.MapFrom(src => src.DateBooking))
                    .ForMember(dto => dto.LastDate, opt => opt.MapFrom(src => src.LastDate))
                    .ForMember(dto => dto.Vat, opt => opt.MapFrom(src => src.Vat))
                    .ForMember(dto => dto.Pincode, opt => opt.MapFrom(src => src.Pincode))
                    .ForMember(dto => dto.VoucherCode, opt => opt.MapFrom(src => src.VoucherCode))
                    .ForMember(dto => dto.BookingNo, opt => opt.MapFrom(src => src.BookingNo))
                    .ForMember(dto => dto.IsCalled, opt => opt.MapFrom(src => src.IsCalled))
                    .ForMember(dto => dto.Deposit, opt => opt.MapFrom(src => src.Deposit))
                    .ForMember(dto => dto.RemainPrice, opt => opt.MapFrom(src => src.RemainPrice))
                    .ForMember(dto => dto.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                    .ForMember(dto => dto.ModifyBy, opt => opt.MapFrom(src => src.ModifyBy))
                    .ForMember(dto => dto.ModifyDate, opt => opt.MapFrom(src => src.ModifyDate))
                    .ForMember(dto => dto.ModifyDate, opt => opt.MapFrom(src => src.ModifyDate))
                    .ForMember(dto => dto.PaymentId, opt => opt.MapFrom(src => src.PaymentId))
                     .ForMember(dto => dto.Status, opt => opt.MapFrom(src => src.Status))
                     .ForMember(dto => dto.ToPlace, opt => opt.MapFrom(src => src.Schedule.Tour.ToPlace))
                     .ForMember(dto => dto.Tour, opt => opt.MapFrom(src => src.Schedule.Tour))
                    //.ForMember(dto => dto.Note, opt => opt.MapFrom(src => src.TourbookingDetails.Note))



                    ;


                cfg.CreateMap<Payment, PaymentViewModel>()
                           .ForMember(dto => dto.IdPayment, otp => otp.MapFrom(src => src.IdPayment))
                           .ForMember(dto => dto.NamePayment, otp => otp.MapFrom(src => src.NamePayment))
                           .ForMember(dto => dto.Type, otp => otp.MapFrom(src => src.Type));


                cfg.CreateMap<CreatePaymentViewModel, Payment>()
                           .ForMember(dto => dto.IdPayment, otp => otp.MapFrom(src => src.IdPayment))
                           .ForMember(dto => dto.NamePayment, otp => otp.MapFrom(src => src.NamePayment))
                           .ForMember(dto => dto.Type, otp => otp.MapFrom(src => src.Type));

                cfg.CreateMap<UpdatePaymentViewModel, Payment>()
                           .ForMember(dto => dto.IdPayment, otp => otp.MapFrom(src => src.IdPayment))
                           .ForMember(dto => dto.NamePayment, otp => otp.MapFrom(src => src.NamePayment))
                           .ForMember(dto => dto.Type, otp => otp.MapFrom(src => src.Type));

            
            });
            _mapper = mapperConfiguration.CreateMapper();
        }
        public static PaymentViewModel MapPayment(Payment data)
        {
            return _mapper.Map<Payment, PaymentViewModel>(data);
        }
        public static List<PaymentViewModel> MapPayment(List<Payment> data)
        {
            return _mapper.Map<List<Payment>, List<PaymentViewModel>>(data);
        }
        public static Payment MapCreatePayment(CreatePaymentViewModel data)
        {
            return _mapper.Map<CreatePaymentViewModel, Payment>(data);
        }
  
        public static List<TourBookingViewModel> MapTourBooking(List<TourBooking> data)
        {
            return _mapper.Map<List<TourBooking>, List<TourBookingViewModel>>(data);
        }

        // create restaurant

        // create tourbooking
        public static TourBooking MapCreateTourBooking(CreateTourBookingViewModel data)
        {
            return _mapper.Map<CreateTourBookingViewModel, TourBooking>(data);
        }
        // create tourbookingDetail
        public static TourBookingDetails MapCreateTourBookingDetail(CreateBookingDetailViewModel data)
        {
            return _mapper.Map<CreateBookingDetailViewModel, TourBookingDetails>(data);
        }

    }
}
