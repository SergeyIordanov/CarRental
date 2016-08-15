using System.Collections.Generic;
using CarRental.BLL.DTO;

namespace CarRental.BLL.Interfaces
{
    public interface IRentService
    {
        #region Create

        void CreateReview(ReviewDTO review);

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

        #endregion

        void Dispose();
    }
}
