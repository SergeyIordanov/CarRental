using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CarRental.Entities.General;
using NUnit.Framework;

namespace CarRental.Tests.DAL.Repositories
{
    public class TestData
    {
        public static IEnumerable CarsData
        {
            get
            {

                yield return new TestCaseData(new List<Car>
                    {
                        new Car { Id = 1, AirConditioning = true, AutomaticTransmission = false, Brand = "Renault", Class = "Standard",
                            ModelName = "Duster", PriceForDay = 45M, Seats = 5, Photo = null},
                        new Car { Id = 2, AirConditioning = true, AutomaticTransmission = true, Brand = "Renault", Class = "Premium",
                            ModelName = "Fluence", PriceForDay = 73.12M, Seats = 5, Photo = null },
                        new Car { Id = 3, AirConditioning = false, AutomaticTransmission = false, Brand = "Kia", Class = "Econom",
                            ModelName = "Rio", PriceForDay = 28.5M, Seats = 5, Photo = null },
                        new Car { Id = 4, AirConditioning = true, AutomaticTransmission = true, Brand = "Bmw", Class = "Premium",
                            ModelName = "X6", PriceForDay = 255M, Seats = 5, Photo = null },
                        new Car { Id = 5, AirConditioning = true, AutomaticTransmission = true, Brand = "Lamborghini", Class = "Sport",
                            ModelName = "Gallardo", PriceForDay = 1766M, Seats = 2, Photo = null }
                    }.AsQueryable());
            }
        }

        public static IEnumerable ReviewsData
        {
            get
            {

                yield return new TestCaseData(new List<Review>
                    {
                        new Review {Id = 1, UserId = null, PublishDate = DateTime.Now.AddHours(-8),
                            Text = "Car was exactly as expected. Pick-up and drop off simple & straightforward.\n\rI will use Car4Rent again." },
                        new Review { Id = 2, UserId = null, PublishDate = DateTime.Now.AddHours(-5),
                            Text = "A good solution for a holiday although the car we booked was out of stock and rather than take a smaller option we paid a little bit more for a bigger car. \n\rThe new car was very good" },
                        new Review { Id = 3, UserId = null, PublishDate = DateTime.Now.AddHours(-4),
                            Text = "Too expensive!!" },
                        new Review { Id = 4, UserId = null, PublishDate = DateTime.Now.AddHours(-2),
                            Text = "Quick and simple online booking process. Professional and friendly staff. \n\rGreat service, would definitely recommend." }
                    }.AsQueryable());
            }
        }

        public static IEnumerable OrdersData
        {
            get
            {

                yield return new TestCaseData(new List<Order>
                    {
                        new Order { Car = new Car(), Id = 1, UserId = "test", OrderStatus = Order.Status.Accepted, FirstName = "test1",
                            LastName = "test1", ToDate = DateTime.Now.AddDays(2), FromDate = DateTime.Now, PhoneNumber = "+380955555555",
                            PickUpAddress = "test", TotalPrice = 100, WithDriver = true},
                        new Order { Car = new Car(), Id = 2, UserId = "test", OrderStatus = Order.Status.Accepted, FirstName = "test2",
                            LastName = "test2", ToDate = DateTime.Now.AddDays(2), FromDate = DateTime.Now, PhoneNumber = "+380955555555",
                            PickUpAddress = "test", TotalPrice = 100, WithDriver = true},
                        new Order { Car = new Car(), Id = 3, UserId = "test", OrderStatus = Order.Status.Accepted, FirstName = "test3",
                            LastName = "test3", ToDate = DateTime.Now.AddDays(2), FromDate = DateTime.Now, PhoneNumber = "+380955555555",
                            PickUpAddress = "test", TotalPrice = 100, WithDriver = true}
                    }.AsQueryable());
            }
        }
    }
}
