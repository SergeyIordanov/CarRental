namespace CarRental.Auth.BLL.Interfaces
{
    /// <summary>
    /// Using for realisation of abstract factory pattern
    /// </summary>
    public interface IServiceCreator
    {
        /// <summary>
        /// Creates an instance of IUserService
        /// </summary>
        /// <param name="connection">Connection string</param>
        /// <returns>Instance of implementation of IUserService interface</returns>
        IUserService CreateUserService(string connection);
    }
}
