using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel.Context.Models.Travel
{
    public class TravelContext : DbContext
    {
        public TravelContext()
        {
        }

        public TravelContext(DbContextOptions<TravelContext> options)
            : base(options)
        {
        }

        public DbSet<TourBooking> TourBookings { get; set; }
        public DbSet<TourBookingDetails> tourBookingDetails { get; set; }
        public DbSet<Payment> Payment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.IdPayment);
                entity.Property(e => e.IdPayment).HasMaxLength(50);
                entity.Property(e => e.NamePayment).HasMaxLength(100);
                entity.Property(e => e.Type).HasMaxLength(50);
            });


            modelBuilder.Entity<TourBooking>(entity =>
            {
                entity.HasKey(e => e.IdTourBooking);
                entity.Property(e => e.ScheduleId).HasMaxLength(50);
                entity.Property(e => e.UrlQR).HasMaxLength(100);
                entity.HasOne(e => e.Payment)
                .WithMany(e => e.TourBooking)
                .HasForeignKey(e => e.PaymentId);

                entity.HasOne(e => e.TourBookingDetails)
                .WithOne(e => e.TourBooking)
                .HasForeignKey<TourBookingDetails>(e => e.IdTourBookingDetails);


                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Address).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(14);
                entity.Property(e => e.Pincode).HasMaxLength(20);
                entity.Property(e => e.IdTourBooking).HasMaxLength(50);
                entity.Property(e => e.NameCustomer).HasMaxLength(100);
                entity.Property(e => e.NameContact).HasMaxLength(100);
                entity.Property(e => e.VoucherCode).HasMaxLength(10);
                entity.Property(e => e.ModifyBy).HasMaxLength(100);
                entity.Property(e => e.BookingNo).HasMaxLength(50);

                entity.Property(e => e.Email).IsRequired(true);
                entity.Property(e => e.Phone).IsRequired(true);
            });


            modelBuilder.Entity<TourBookingDetails>(entity =>
            {
                entity.HasKey(e => e.IdTourBookingDetails);
                entity.Property(e => e.IdTourBookingDetails).HasMaxLength(50);




                entity.Property(e => e.Note).HasMaxLength(255);


            });

        }

    }
}
