using System.Threading.Tasks;

namespace CampusStay.Brokers.Email
{
    public interface IEmailBroker
    {
        public Task SendAccessTokenByEmailAsync(string email, string token);
        public string GetEmailFromToken(string token);
        public Task CompleteRegistrationForEmailAsync(string email);
        public Task SendEmailAsync(string to, string subject, string body);
    }
}