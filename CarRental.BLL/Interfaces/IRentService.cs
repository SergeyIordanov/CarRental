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

        /// <summary>
        /// Gets orders by special user
        /// </summary>
        /// <param name="userId">id of user who's orders you want to get</param>
        /// <returns>Orders of special user</returns>
        IEnumerable<OrderDTO> GetOrders(string userId);

        /// <summary>
        /// Filters cars by: 
        ///     - comparing 'searchCar' arg with car's brand & model
        ///     - comparing 'searchUser' arg with user's first & last name
        /// </summary>
        /// <param name="searchCar">car's brand and/or model</param>
        /// <param name="searchUser">user's first and/or last name</param>
        /// <returns>Filtred orders collection</returns>
        IEnumerable<OrderDTO> GetOrders(string searchCar, string searchUser);

        #endregion

        void Dispose();
    }
}
