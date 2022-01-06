using System;

namespace iqrasys.api.Models
{
    public class QuickRequest
    {
        public Guid Id { get; set; }
        public DateTime RequestDate { get; set; }
        public bool IsTrashed { get; set; }
        public string Phone { get; set; }
        public bool IsSeen { get; set; }
    
        public QuickRequest()
        {
            IsTrashed = false;
            RequestDate = DateTime.Now;
            IsSeen = false;
        }
    }
}