using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Private_chat_SignalR.Models.ORM.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        //inverse propertynin ne işe yaradığına dair kaynak https://entityframework.net/inverse-property
        [InverseProperty("SenderUser")]
        public virtual ICollection<Message> Senders { get; set; }
        [InverseProperty("RecipientUser")]
        public virtual ICollection<Message> Recipients { get; set; }
        public virtual ICollection<ChatRoomUser> ChatRoomUsers { get; set; }


    }
}
