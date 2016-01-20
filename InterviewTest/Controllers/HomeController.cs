using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InterviewTest.Database;
using InterviewTest.Models;
using Microsoft.Ajax.Utilities;

namespace InterviewTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        private FileSystemDatabase GetDatabase() => new FileSystemDatabase();
        private static readonly Random _random = new Random();

        public ActionResult CreateTripsAndSellers()
        {
            var database = GetDatabase();

            for (int i = 0; i < 10; i++)
            {
                var host = EntityGenerator.GenerateHost();
                host.Id = i.ToString("00000");
                database.Save(host);
            }

            for (int i = 0; i < 10; i++)
            {
                var trip = EntityGenerator.GenerateTrip(_random.Next(0, 10).ToString("00000"));
                trip.Id = i.ToString("00000");
                database.Save(trip);
            }

            TempData["notification"] = "10 trips and 10 hosts created";

            return RedirectToAction("index");
        }
        
        public ActionResult Account()
        {
            var idCookie = HttpContext.Request.Cookies["id"];

            var viewModel = new AccountViewModel();
            

            if (idCookie?.Value != null)
            {
                var user = GetDatabase().Get<User>(idCookie.Value);
                viewModel.Name = user?.Name;
            }

            return View(viewModel);
        }

        public ActionResult SetAccountDetails(AccountViewModel viewModel)
        {
            var idCookie = HttpContext.Request.Cookies["id"];
            User user;
            if (idCookie?.Value != null)
            {
                user = GetDatabase().Get<User>(idCookie.Value);
            }
            else
            {
                user = new User();
            }

            user.Name = viewModel.Name;

            GetDatabase().Save(user);

            idCookie = idCookie ?? new HttpCookie("id");

            idCookie.Value = user.Id;

            HttpContext.Response.Cookies.Set(idCookie);

            return RedirectToAction("account");
        }
    }

    public class AccountViewModel
    {
        public string Name { get; set; }
    }

    public class User : IPersistable
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}