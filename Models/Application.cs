using System;
using Microsoft.AspNetCore.Http;

namespace iqrasys.api.Models
{
    public class Application
    {
        public Guid Id { get; set; }
        public bool IsTrashed { get; set; }
        public DateTime AppliedDate { get; set; }
        public string  FilePath { get; set; }

        public Application()
        {
            IsTrashed = false;
            AppliedDate = DateTime.Now;
        }
    }
}