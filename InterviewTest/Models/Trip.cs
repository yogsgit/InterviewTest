using InterviewTest.Database;

namespace InterviewTest.Models
{
    public class Trip : IPersistable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string HostId { get; set; }
        public string ImageUrl { get; set; }
        /// <summary>
        /// [YOGESH] - Added to maintain the count of times used - task 2 
        /// </summary>
        public int UseCount { get; set; }
    }

    public class Host : IPersistable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public string ImageUrl { get; set; }
        /// <summary>
        /// [YOGESH] - Added to maintain the count of times used - task 2 
        /// </summary>
        public int UseCount { get; set; }
    }

    /// <summary>
    /// [YOGESH] - Model for settings
    /// </summary>
    public class Settings : IPersistable
    {
        public string Id { get; set; }
        public string TripHostOrder { get; set; }
    }
}