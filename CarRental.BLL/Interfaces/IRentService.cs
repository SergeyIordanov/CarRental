using System.Collections.Generic;
using CarRental.BLL.DTO;

namespace CarRental.BLL.Interfaces
{
    /// <summary>
    /// Interface for working with BLL from Presentation layer (WEB)
    /// </summary>
    public interface IRentService
    {
        #region Create

        void CreateReview(ReviewDTO review);

        void CreateOrder(OrderDTO order, int? carId);

        #endregion

        #region Update

        void UpdateCar(CarDTO car);

        void UpdateOrder(OrderDTO order);

        #endregion

        #region Delete

        void DeleteCar(int? id);

        void DeleteReview(int? id);

        void DeleteOrder(int? id);

        #endregion

        #region Get

        CarDTO GetCar(int? id);

        IEnumerable<CarDTO> GetCars();

        /// <summary>
        /// Filters cars by comparing 'searchString' arg with car's brand & model
        /// </summary>
        /// <param name="searchString">Search request</param>
        /// <returns>Filtred cars collection</returns>
        IEnumerable<CarDTO> GetCars(string searchString);

        /// <summary>
        /// Filters cars using searchModel
        /// </summary>
        /// <param name="filterModel">Model for filtering cars</param>
        /// <returns>Filtred cars collection</returns>
        IEnumerable<CarDTO> GetCars(FilterDTO filterModel);

        IEnumerable<ReviewDTO> GetReviews();

        OrderDTO GetOrder(int? id);

        IEnumerable<OrderDTO> GetOrders();

        IEnumerable<OrderDTO> GetOrders(string userId);

        #endregion

        void Dispose();
    }
}
