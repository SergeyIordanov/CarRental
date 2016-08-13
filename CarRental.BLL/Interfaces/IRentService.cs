using System.Collections.Generic;
using CarRental.BLL.DTO;
using CarRental.BLL.Models;

namespace CarRental.BLL.Interfaces
{
    public interface IRentService
    {
        CarDTO GetCar(int? id);

        IEnumerable<CarDTO> GetCars();

        /// <summary>
        /// Filters cars using searchModel
        /// </summary>
        /// <param name="searchModel">Model for filtering cars</param>
        /// <returns>Filtred cars collection</returns>
        IEnumerable<CarDTO> GetCars(SearchCarModel searchModel);

        void Dispose();
    }
}
