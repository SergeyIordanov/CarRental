namespace CarRental.Auth.BLL.Infrastructure
{
    /// <summary>
    /// Objects of this class are returned by IUserService methods for interaction with presentation layer 
    /// </summary>
    public class OperationDetails
    {
        public OperationDetails(bool succedeed, string message, string prop)
        {
            Succedeed = succedeed;
            Message = message;
            Property = prop;
        }
        public bool Succedeed { get; private set; }

        public string Message { get; private set; }

        public string Property { get; private set; }
    }
}
