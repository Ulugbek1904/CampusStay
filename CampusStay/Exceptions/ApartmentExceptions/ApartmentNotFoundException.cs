using System;

namespace CampusStay.Exceptions.ApartmentExceptions
{
    public class ApartmentNotFoundException : Exception
    {
        public ApartmentNotFoundException(string message)
            : base(message) { }
    }
}
