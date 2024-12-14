using System;

namespace CampusStay.Exceptions.UserExceptions
{
    public class UserNullException :Exception
    {
        public UserNullException(string message)
            : base(message) { }
    }
}
