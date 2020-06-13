using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Private_chat_SignalR.Models.ORM.Entity
{
    public class ChatRoom
    {
        public int Id { get; set; }
        public virtual ICollection<ChatRoomUser> ChatRoomUsers { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
