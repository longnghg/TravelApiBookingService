using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Context.Models.Travel;

namespace Travel.Context.Models
{
    [NotMapped]

    public class Voucher
    {
        public Guid IdVoucher { get; set; }
        public string Code { get; set; }
        
        public int Value { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        

    }
}
