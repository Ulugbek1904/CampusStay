using System;

namespace CampusStay.Models.ChatModels
{
    public class AdminChat
    {
        public Guid Id { get; set; }
        public Guid AdminId { get; set; }
        public Guid ChatId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
