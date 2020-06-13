using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Private_chat_SignalR.Models;
using Private_chat_SignalR.Models.ORM.Context;

namespace Private_chat_SignalR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjectContext db;
        public HomeController( ProjectContext db)
        {
            this.db = db;
        }
        //
        public IActionResult Index()
        {
            return View(db.Users.ToList());
        }

        
    }
}
