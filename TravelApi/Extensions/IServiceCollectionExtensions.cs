using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travel.Context.Models.Notification;
using Travel.Context.Models.Travel;
using Travel.Data.Interfaces;
using Travel.Data.Interfaces.INotify;
using Travel.Data.Repositories;
using Travel.Data.Repositories.NotifyRes;
using TravelApi.Helpers;
using TravelApi.Hubs.HubServices;

namespace TravelApi.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure DbContext with Scoped lifetime
            services.AddDbContext<NotificationContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("notifyTravelEntities"));
            }
            );

            services.AddDbContext<TravelContext>(options =>
            {
                options
                .UseSqlServer(configuration.GetConnectionString("bookingRoverEntities"));
            });
            return services;
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {

            services
            .AddScoped<IPayment, PaymentRes>();
           
            services
   .AddScoped<ITourBooking, TourBookingRes>();
            services
.AddScoped<IStatistic, StatisticRes>();

            services
          .AddScoped<IHubRepository, HubRepository>();
            services
            .AddScoped<INotification, NotificationRes>();
          
            services
           .AddScoped<IPayment, PaymentRes>();

            services
                 .AddScoped<IVnPay, VnpayRes>();
            services
                .AddScoped<ICache, MemoryCache>();
            return services;
        

        }

    }
}
