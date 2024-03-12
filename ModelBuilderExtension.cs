using lista10.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lista10
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    Name = "Food"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Electronics"
                },
                new Category()
                {
                    Id = 3,
                    Name = "Clothes"
                },
                new Category()
                {
                    Id = 4,
                    Name = "Other"
                });

            modelBuilder.Entity<Order>().HasData(
                new Order()
                {
                    Id = 1,
                    FirstName = "Łukasz",
                    LastName = "Wasilewski",
                    Email = "lukaszwasilewski22@gmail.com",
                    Phone = "725744993",
                    Street = "Kolberga",
                    HomeNumber = "1B",
                    LocalNumber = "22",
                    ZipCode = "58-506",
                    City = "Jelenia Góra",
                    Points = 0,
                    DeliveryMethod = "Kurier",
                    PaymentMethod = "Blik",
                    IsConfirmed = true,
                    Price = 2137M,
                   });

            modelBuilder.Entity<Article>().HasData(
                new Article()
                {
                    Id = 1,
                    Name = "Piwo Żywiec 500ml",
                    Price = "4,49",
                    ExpiryDate = new DateTime(2024, 10, 10),
                    IsPromo = true,
                    CategoryId = 1,
                    ImagePath = "zywiec.png"
                },
                new Article()
                {
                    Id = 2,
                    Name = "Draże Korsarze",
                    Price = "2,99",
                    ExpiryDate = new DateTime(2024, 4, 2),
                    IsPromo = false,
                    CategoryId = 1,
                    ImagePath = "korsarze.jpg"
                }); ; ; ;
        }
    }
}
