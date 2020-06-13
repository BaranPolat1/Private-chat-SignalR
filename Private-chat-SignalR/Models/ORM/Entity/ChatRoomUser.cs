using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Private_chat_SignalR.Models.ORM.Entity
{
    public class ChatRoomUser 
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int ChatRoomId { get; set; }
        public virtual ChatRoom ChatRoom { get; set; }
    }
}
