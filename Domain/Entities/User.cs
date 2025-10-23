using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace MyApp1.Domain.Entities
{
    public class User :BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Location { get; set; }
        //public bool IsMentor { get; set; } = false;
        public string Role { get; set; } = "Learner";  // Role for authorization
        public string? MentorStatus { get; set; } // None / Pending / Approved / Rejected
        public bool IsPremium { get; set; } = false;

        // Navigation properties
        public ICollection<UserSkill>? UserSkills { get; set; }
        public ICollection<Session>? SessionsAsMentor { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Group>? GroupsMentoring { get; set; }
        public ICollection<GroupMember>? GroupMemberships { get; set; }
        public ICollection<Message>? SentMessages { get; set; }
        public ICollection<Message>? ReceivedMessages { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<PostComment>? PostComments { get; set; }
        public ICollection<Connection>? Connections { get; set; }
        public ICollection<Rating>? RatingsGiven { get; set; }
        public ICollection<Rating>? RatingsReceived { get; set; }
        public ICollection<UserBadge>? UserBadges { get; set; }
        public ICollection<Subscription>? Subscriptions { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<Payment>? PaymentsAsLearner { get; set; }
        public ICollection<Payment>? PaymentsAsMentor { get; set; }
        public ICollection<Payout>? Payouts { get; set; }
    }
}
