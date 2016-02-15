using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InterviewTest.Database;
using InterviewTest.Extensions;
using InterviewTest.Models;

namespace InterviewTest.Controllers
{
    public class NewsletterController : Controller
    {
        public ActionResult Create(int count)
        {
            var db = GetDatabase();
            // [YOGESH] - Fetch the TripHostOrder setting from the DB.
            var tripHostOrder = db.GetAll<Settings>().Select(h => h.TripHostOrder).FirstOrDefault();
            // [YOGESH] - Count the number of Trips and Hosts for the newsletter.
            int countT = tripHostOrder.ToCharArray().Where(m => m.Equals('T')).Count();
            int countH = tripHostOrder.ToCharArray().Where(m => m.Equals('H')).Count();

            #region [YOGESH] - For tasks 1 & 2
            var hosts = db.GetAll<Host>().OrderBy(m => m.UseCount).ToList();
            var trips = db.GetAll<Trip>().OrderBy(m => m.UseCount).ToList();
            for (int i = 0; i < count; i++)
            {
                var newsletter = new Newsletter()
                {
                    HostIds = hosts.Select(h => h.Id).Take(countH).ToList(),
                    TripIds = trips.Select(t => t.Id).Take(countT).ToList(),
                    // [YOGESH] - Assign the TripHostOrder setting value to the TripHostOrder property of the Newsletter.
                    TripHostOrder = tripHostOrder
                };
                db.Save(newsletter);
                for (int j = 0; j < countH; j++)
                {
                    hosts[j].UseCount++;
                }
                hosts = hosts.OrderBy(m => m.UseCount).ToList();
                for (int j = 0; j < countT; j++)
                {
                    trips[j].UseCount++;
                }
                trips = trips.OrderBy(m => m.UseCount).ToList();
            }
            db.SaveAll(hosts);
            db.SaveAll(trips);
            #endregion

            #region [YOGESH] - For task 1 only
            //var hostIds = db.GetAll<Host>().Select(h => h.Id).ToArray();
            //var tripIds = db.GetAll<Trip>().Select(t => t.Id).ToArray();

            //for (int i = 0; i < count; i++)
            //{
            //    var newsletter = new Newsletter()
            //    {
            //        HostIds = Enumerable.Range(0, countH).Select(x => hostIds.GetRandom()).ToList(),
            //        TripIds = Enumerable.Range(0, countT).Select(x => tripIds.GetRandom()).ToList(),
            //        // [YOGESH] - Assign the TripHostOrder setting value to the TripHostOrder property of the Newsletter.
            //        TripHostOrder = tripHostOrder
            //    };

            //    db.Save(newsletter);
            //}
            #endregion

            TempData["notification"] = $"Created {count} newsletters";

            return RedirectToAction("list");
        }

        public ActionResult DeleteAll()
        {
            GetDatabase().DeleteAll<Newsletter>();

            TempData["notification"] = "All newsletters deleted";

            return RedirectToAction("list");
        }

        public ActionResult Display(string id)
        {
            var db = GetDatabase();

            var newsletter = db.Get<Newsletter>(id);
            // [YOGESH] - Get the TripHostOrder for the newsletter and convert to a char array.
            var tripHostOrder = newsletter.TripHostOrder;
            char[] tripHostChars = tripHostOrder.ToCharArray();

            // [YOGESH] - Add the Trip and Host in the required sequence to the NewsletterViewModel.
            var viewModel = new NewsletterViewModel();
            viewModel.Items = new List<object>();
            int countT = 0;
            int countH = 0;
            for (int i = 0; i < tripHostChars.Count(); i++)
            {
                if (tripHostChars[i] == 'T')
                    viewModel.Items.Add(Convert(db.Get<Trip>(newsletter.TripIds[countT++])));
                else
                    viewModel.Items.Add(Convert(db.Get<Host>(newsletter.HostIds[countH++])));
            }

            //var viewModel = new NewsletterViewModel
            //{
            //    Items = newsletter.TripIds.Select(tid => Convert(db.Get<Trip>(tid))).Cast<object>()
            //        .Union(newsletter.HostIds.Select(hid => Convert(db.Get<Host>(hid))))
            //        .ToList(),
            //};

            return View("Newsletter", viewModel);
        }

        public ActionResult Sample()
        {
            var viewModel = new NewsletterViewModel()
            {
                Items =
                {
                    Convert(EntityGenerator.GenerateHost()),
                    Convert(EntityGenerator.GenerateTrip(null), "Test host name"),
                    Convert(EntityGenerator.GenerateTrip(null), "Test host name"),
                    Convert(EntityGenerator.GenerateHost()),
                }
            };

            return View("Newsletter", viewModel);
        }

        public ActionResult List()
        {
            var viewModel = new NewsletterListViewModel
            {
                Newsletters = GetDatabase().GetAll<Newsletter>(),
            };

            return View(viewModel);
        }

        private NewsletterHostViewModel Convert(Host host) => new NewsletterHostViewModel
        {
            Name = host.Name,
            ImageUrl = host.ImageUrl,
            Job = host.Job,
        };

        private NewsletterTripViewModel Convert(Trip trip, string hostName = null) => new NewsletterTripViewModel
        {
            Name = trip.Name,
            Country = trip.Country,
            HostName = hostName ?? GetDatabase().Get<Host>(trip.HostId)?.Name,
            ImageUrl = trip.ImageUrl,
        };

        private FileSystemDatabase GetDatabase() => new FileSystemDatabase();

    }

    public class NewsletterViewModel
    {
        public List<object> Items { get; set; } = new List<object>();
    }

    public class NewsletterTripViewModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Country { get; set; }
        public string HostName { get; set; }
        public string ImageUrl { get; set; }
    }

    public class NewsletterHostViewModel
    {
        public string Name { get; set; }
        public string Job { get; set; }
        public string ImageUrl { get; set; }
    }

    public class NewsletterListViewModel
    {
        public List<Newsletter> Newsletters { get; set; }
    }
}