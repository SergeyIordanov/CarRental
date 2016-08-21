using System;

namespace CarRental.BLL.Infrastructure
{
    /// <summary>
    /// Exception that can be used for data validation at the presentation layer
    /// </summary>
    public class ValidationException : Exception
    {
        public string Property { get; protected set; }

        public ValidationException(string message, string prop) : base(message)
        {
            Property = prop;
        }
    }
}
