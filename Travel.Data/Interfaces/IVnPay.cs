using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Shared.ViewModels;
namespace Travel.Data.Interfaces
{
    public interface IVnPay
    {
        Task<string> CreatePaymentUrl(string idTourBooking,string idCustomer, HttpContext context);
         Task<PaymentResponse> PaymentExecute(IQueryCollection collections,string idTourBooking,string idcustomer = null);
    }
}
