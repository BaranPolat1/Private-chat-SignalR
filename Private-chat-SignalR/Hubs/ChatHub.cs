using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Private_chat_SignalR.Models.ORM.Context;
using Private_chat_SignalR.Models.ORM.Entity;

using System.Linq;
using System.Threading.Tasks;

namespace Private_chat_SignalR.Hubs
{
    [HubName("chatHub")]
    public class ChatHub:Hub
    {
        private ProjectContext db;
     
        public ChatHub(ProjectContext db )
        {
            this.db = db;
        }
       
        //Anlık olarak mesajlaşma metodum.Parametrelere ise sırasıyla view sayfasından: Online olan kullanıcının adı, Mesaj içeriğini,mesaj atılan kişinin ID'sini ve ortak olan ChatRoomun ID'sin gönderiyorum.
        public async Task Send(string user, string content, string recipientId, string chatRoomId)
        {
            var currentUser = db.Users.FirstOrDefault(x => x.UserName == user);

            //Bu metod bir grup oluşturma metodu. benden bir Connection Id ve grup adı istiyor.Grup adına ChatRoomId vermemin sebebi ise bu ID'nin sadece Online olan kullanıcıya ve mesaj atılan kullanıcıya özel olması.Bu Id'ye sahip olan kullanıcıları bu gruba ekliyor.(Controller kısmına bak)
           await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);

            //Burada gerekli database kayıt işlemlerini yapıyorum.
            Message message = new Message
            {
                Content = content,
                RecipientId = int.Parse(recipientId),
                SenderId = currentUser.Id,
                ChatRoomId = int.Parse(chatRoomId)

            };
            db.Messages.Add(message);
            db.SaveChanges();

            //Bu metod ise benim mesajı anlık olarak ileten metodum.Benden bir grup adı istiyor. Bunu istemesinin sebebi yollanan mesajı sadece o gruba dahil olan kullanıcılara göstermek.İşte her ChatRoomId'i sadece 2 kullanıcıya bu sebepten dolayı özel kıldık ve yukarıdaki Grup oluşturma metoduna bu sebepten dolayı ChatRoom Id'i verdik.Parametrelerde ise mesaj gönderidkten sonra View sayfasında göstermek istediğim elementler yer alıyor.
             await Clients.Group(chatRoomId).SendAsync("ReceiveMessage", user, content);
        }
    }
}
