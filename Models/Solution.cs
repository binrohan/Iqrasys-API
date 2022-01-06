using System;

namespace iqrasys.api.Models
{
    public class Solution
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? Order { get; set; }
        public bool IsTrashed { get; set; }

        public Solution()
        {
            IsTrashed = false;
        }
    }
}