using System;
using System.Collections.Generic;
using InterviewTest.Database;

namespace InterviewTest.Models
{
    public class Newsletter : IPersistable
    {
        public Newsletter()
        {
            CreatedAt = DateTime.Now;
        }
        public string Id { get; set; }
        public List<string> TripIds { get; set; }
        public List<string> HostIds { get; set; }
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// [YOGESH] - TripHostOrder will save the trip-host order setting when a newsletter was generated.
        /// </summary>
        public string TripHostOrder { get; set; }
    }
}