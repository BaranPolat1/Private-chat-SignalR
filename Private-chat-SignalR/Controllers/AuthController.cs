using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Private_chat_SignalR.Models.ORM.Context;
using Private_chat_SignalR.Models.ORM.Entity;
using Private_chat_SignalR.Models.VM;

namespace Private_chat_SignalR.Controllers
{
    public class AuthController : Controller
    {

        private ProjectContext db;
        public AuthController(ProjectContext db)
        {
            this.db = db;
        }
        //Kullanıcıları MSSQL Serverdan eklediğim için register metodu yazmadım.
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            var user = db.Users.FirstOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);
            if (user!=null)
            {
               
                HttpContext.Session.SetString("UserName", model.UserName);
                return View();
            }
           
            return View();
        }
    }
}
