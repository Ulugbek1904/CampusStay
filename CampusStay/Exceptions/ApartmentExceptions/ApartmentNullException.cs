using System;

namespace CampusStay.Exceptions.ApartmentExceptions
{
    public class ApartmentNullException : Exception
    {
        public ApartmentNullException(string message)
            : base(message) { }
    }
}
