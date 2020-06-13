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
            //Online olan kullanıcımızı currentUser değişkenine atadık.
            var currentUser = db.Users.FirstOrDefault(x => x.UserName == user);

            //Bu metod grup oluşturma metodu. Benden bir ConnectionId ve Grup adı istiyor. Bizim verdiğimiz isim ile bir grup oluşturacak.Grup adına online olan kullanıcı ile mesajlaşılan diğer kullanıcının ortak olan ChatRoom Id'sini verdik.
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

            //Bu metod oluşturduğumuz gruba mesaj yollama işlemini gerçekleştirecek olan metod. Bizden oluşturduğumuz grup adını istiyor.Biz ChatRoom ID bilgisiyle grup oluşturmuştuk ve o Id'yi bu metoda verdik.Artık yolladığımız tüm mesajlar oluşturduğumuz gruba dahil olan bu 2 kullanıcıya anlık olarak iletilecek."User" ve "Content" parametresi bizim Chat sayfamızda anlık olarak gösterilecek olan veriler.Senaryoya göre farklı parametreler de verilebilir.
             await Clients.Group(chatRoomId).SendAsync("ReceiveMessage", user, content);
        }
    }
}
