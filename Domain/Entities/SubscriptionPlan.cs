using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class SubscriptionPlan :BaseEntity
    {
        public string Name { get; set; } = string.Empty;          // e.g., "Basic", "Premium"
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }                   // Plan validity in days
        public string Features { get; set; } = string.Empty;      // Description, JSON, or delimited text

        public ICollection<Subscription> Subscriptions { get; set; } 
    }
}
