using System;

namespace iqrasys.api.Models
{
    public class Request
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Text { get; set; }
        public Solution Solution { get; set; }
        public Guid SolutionId { get; set; }
        public bool IsTrashed { get; set; }
        public DateTime RequestDate { get; set; }

        public Request()
        {
            RequestDate = DateTime.Now;
            IsTrashed = false;
        }

    }
}