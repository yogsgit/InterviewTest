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
    }

    public class Host : IPersistable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public string ImageUrl { get; set; }
    }
}