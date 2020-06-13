using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Private_chat_SignalR.Models.ORM.Context;
using Private_chat_SignalR.Models.ORM.Entity;

namespace Private_chat_SignalR.Controllers
{
    public class MessageController : Controller
    {
       
        private readonly ProjectContext db;
        public MessageController( ProjectContext db)
        {
            
            this.db = db;
        }
        
        //Mesajlaşmak istediğim kullanıcının kullanıcı adını parametre ile metoda yolluyorum.
        public  IActionResult ShowChatRoom(string userName)
        {
            using(var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<ChatRoom> chatRooms = new List<ChatRoom>();
                    ChatRoom chat = null;

                    //Online olan kullanıcının tüm bilgilerine artık currentUser değişkenimden ulaşacağım.
                    var currentUser = db.Users.FirstOrDefault(x => x.UserName == HttpContext.Session.GetString("UserName"));


                    //Parametreden gelen isime sahip olan kullanıcya artık bu değişkenden ulaşacağım.
                    var user = db.Users.FirstOrDefault(x => x.UserName == userName);
                    
                    //Razor Page'da bu kullanıcının ID bilgisini tutmam gerek.
                    ViewBag.UserId = user.Id;

                    //ChatRoomUser tablosundan online olan kullanıcının kayıtlarını getirdim.
                    var myChatRoom = db.ChatRoomUsers.Where(x => x.UserId == currentUser.Id).ToList();

                    //Online olan kullanıcı, daha önce başka kullanıcılar ile mesajlaşmış mı yoksa mesajlaşmamış mı bunun kontrolünü yapıyorum.
                    if (myChatRoom.Count != 0)
                    {
                        //Eğer mesajlaşmışsa bu kullanıcının var olan ChatRoomlarını bu listeye aldım.
                        foreach (var item in myChatRoom)
                        {
                            chatRooms.AddRange(db.ChatRooms.Where(x => x.Id == item.ChatRoomId));
                        }

                        //Online olan kullanıcının ChatRoomlarının içinde dolaşıyorum.
                        foreach (var chatroom in chatRooms)
                        {
                            //Online olan kullanıcı daha önce parametreden gelen kullanıcı ile mesajlaşmış mı? bunun kontrolünü yapıyorum.
                            if (db.ChatRoomUsers.Any(x => x.ChatRoomId == chatroom.Id && x.UserId == user.Id))
                            {
                                //Eğer mesajlaşmışsa bu iki kullanıcının ortak olan ChatRoomunu getiriyorum(çünkü View kısmında bu ChatRoomu listeleyeceğim.)
                                var chatroomuser = myChatRoom.Where(x => x.ChatRoomId == chatroom.Id).FirstOrDefault();
                                //Ortak olan ChatRoomu bulduk.
                                chat = db.ChatRooms.FirstOrDefault(x => x.Id == chatroomuser.ChatRoomId);
                            }
                            //Eğer online olan kullanıcı daha önce hiç parametreden kullanıcı ile mesajlaşmamışsa,bu kullanıcı ile ortak bir ChatRoom yaratıyorum.
                            else
                            {
                                chat = new ChatRoom();
                                db.ChatRooms.Add(chat);
                                db.SaveChanges();
                                ChatRoomUser chatRoomUser = new ChatRoomUser();
                                chatRoomUser.ChatRoomId = chat.Id;
                                chatRoomUser.UserId = user.Id;
                                db.ChatRoomUsers.Add(chatRoomUser);
                                db.SaveChanges();
                                ChatRoomUser chatRoomUser2 = new ChatRoomUser();
                                chatRoomUser2.UserId = currentUser.Id;
                                chatRoomUser2.ChatRoomId = chat.Id;
                                db.ChatRoomUsers.Add(chatRoomUser2);
                                db.SaveChanges();
                                transaction.Commit();

                            }
                        }
                       
                    }
                    //Eğer online olan kullanıcı daha önce kimseyle mesajlaşmamış ise, parametreden gelen kullanıcı ile ortak bir ChatRoom yaratıyorum.
                    else
                    {
                        chat = new ChatRoom();
                        db.ChatRooms.Add(chat);
                        db.SaveChanges();
                        ChatRoomUser chatRoomUser = new ChatRoomUser();
                        chatRoomUser.ChatRoomId = chat.Id;
                        chatRoomUser.UserId = user.Id;
                        db.ChatRoomUsers.Add(chatRoomUser);
                        db.SaveChanges();
                        ChatRoomUser chatRoomUser2 = new ChatRoomUser();
                        chatRoomUser2.UserId = currentUser.Id;
                        chatRoomUser2.ChatRoomId = chat.Id;
                        db.ChatRoomUsers.Add(chatRoomUser2);
                        db.SaveChanges();
                        transaction.Commit();
                       
                    }
                    //ChatRoomu view sayfamda gösteriyorum.
                    return View(chat);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
              


            }



        }
       
    }
}
