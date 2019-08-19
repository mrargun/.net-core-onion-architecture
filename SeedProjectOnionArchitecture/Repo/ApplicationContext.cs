using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repo
{
    public class ApplicationContext : DbContext
    {

        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }



        public virtual DbSet<City> City { get; set; }

        public virtual DbSet<Town> Town { get; set; }

        //LAZY LOADING ENABLED
        //Connection string will be entered in GUI layer. (appsettings)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
            .UseLazyLoadingProxies();
        //{
        //    //if (!optionsBuilder.IsConfigured)
        //    //{
        //    //    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //    //}
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city", "DB_NAME");

                entity.HasIndex(e => e.IdCountry)
                    .HasName("FK_City_CountryID");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IdCountry)
                    .HasColumnName("id_country")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneCode)
                    .IsRequired()
                    .HasColumnName("phone_code")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.PlateNo)
                    .IsRequired()
                    .HasColumnName("plate_no")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("1");
            });


            modelBuilder.Entity<Town>(entity =>
            {
                entity.ToTable("town", "DB_NAME");

                entity.HasIndex(e => e.IdCity)
                    .HasName("FK_Town_CityID");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IdCity)
                    .HasColumnName("id_city")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("1");

                entity.HasOne(d => d.IdCityNavigation)
                    .WithMany(p => p.Town)
                    .HasForeignKey(d => d.IdCity)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Town_CityID");
            });
        }
    }
}