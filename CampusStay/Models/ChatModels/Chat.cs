using System;

namespace CampusStay.Models.ChatModels
{
    public class Chat
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
