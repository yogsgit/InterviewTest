using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InterviewTest.Database;
using InterviewTest.Models;
using Microsoft.Ajax.Utilities;
using System.Text.RegularExpressions;
using System.Configuration;

namespace InterviewTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // [YOGESH] - Get the TripHostOrder setting form the DB.
            var database = GetDatabase();
            TripHostOrderViewModel tripHostOrderViewModel = new TripHostOrderViewModel();

            var tripHostOrder = database.GetAll<Settings>().Select(h => h.TripHostOrder).FirstOrDefault();
            // If the TripHostOrder is not set, get the default from te config file and save to the DB.
            if (string.IsNullOrEmpty(tripHostOrder))
            {
                Settings format = new Settings();
                format.TripHostOrder = DefaultTripHostOrder;
                format.Id = "NewsLetterFormat";
                database.Save(format);

                tripHostOrderViewModel.Order = DefaultTripHostOrder;
            }
            else
            {
                tripHostOrderViewModel.Order = tripHostOrder;
            }
            return View(tripHostOrderViewModel);
        }
        // [YOGESH] - Get the default TripHostOrder setting from the DB.
        private readonly string DefaultTripHostOrder = ConfigurationManager.AppSettings["DefaultTripHostOrder"] == null ? "TTHH" : ConfigurationManager.AppSettings["DefaultTripHostOrder"];
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

        /// <summary>
        /// [YOGESH] - Validates and saves the TripHostOrder value to the DB.
        /// </summary>
        /// <param name="model">TripHostOrderViewModel</param>
        /// <returns></returns>
        public ActionResult SetNewsletterFormat(TripHostOrderViewModel model)
        {
            var database = GetDatabase();
            string order = null;
            if (!string.IsNullOrEmpty(model.Order))
            {
                order = model.Order.ToUpper();
                var regex = new Regex("^[TH]+$");
                if (!regex.IsMatch(order))
                    order = null;
            }
            if (order == null)
                TempData["notification"] = "Please enter the trip-host order in the correct format.";
            else
            {
                var tripHostOrder = database.GetAll<Settings>().Where(m => m.Id == "NewsLetterFormat").SingleOrDefault();
                tripHostOrder.TripHostOrder = order;
                database.Save(tripHostOrder);
                TempData["notification"] = "Trip-host order for the newsletter has been updated.";
            }

            return RedirectToAction("index");
        }
    }

    /// <summary>
    /// [YOGESH] - View model for TripHostOrder setting.
    /// </summary>
    public class TripHostOrderViewModel
    {
        public string Order { get; set; }
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