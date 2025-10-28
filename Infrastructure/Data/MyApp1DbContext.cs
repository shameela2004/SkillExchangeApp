using Microsoft.EntityFrameworkCore;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Infrastructure.Data
{
    public class MyApp1DbContext :DbContext
    {
        public MyApp1DbContext(DbContextOptions<MyApp1DbContext> options):base(options) 
        { 
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<OtpVerification> OtpVerifications { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<GroupSession> GroupSessions { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<GroupMessage> GroupMessages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<UserReport> UserReports { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Payout> Payouts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);




            modelBuilder.Entity<Booking>()
    .Property(b => b.PaymentAmount)
    .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.CommissionAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.CommissionPercent)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.MentorAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payout>()
                .Property(po => po.AmountReleased)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payout>()
                .Property(po => po.CommissionAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Session>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18,2)");


            modelBuilder.Entity<SubscriptionPlan>()
                .Property(sp => sp.Price)
                .HasColumnType("decimal(18,2)");


            // ==============================
            // USER ↔ USER_SKILL
            // ==============================
            modelBuilder.Entity<UserSkill>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSkills)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserSkill>()
                .HasOne(us => us.Skill)
                .WithMany(s => s.UserSkills)
                .HasForeignKey(us => us.SkillId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserSkill>()
                .HasIndex(us => new { us.UserId, us.SkillId, us.Type })
                .IsUnique();

            // ==============================
            // USER ↔ SESSION ↔ SKILL
            // ==============================
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Mentor)
                .WithMany(u => u.SessionsAsMentor)
                .HasForeignKey(s => s.MentorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.Skill)
                .WithMany(sk => sk.Sessions)
                .HasForeignKey(s => s.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupSession>()
    .HasOne(gs => gs.Group)
    .WithMany(g => g.GroupSessions)
    .HasForeignKey(gs => gs.GroupId)
    .OnDelete(DeleteBehavior.Cascade);


            // ==============================
            // BOOKING ↔ SESSION ↔ USER ↔ PAYMENT
            // ==============================
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Session)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Learner)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.LearnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Payment)
                .WithOne(p => p.Booking)
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==============================
            // GROUP ↔ USER (Mentor)
            // ==============================
            modelBuilder.Entity<Group>()
                .HasOne(g => g.Skill)
                .WithMany(s => s.Groups)
                .HasForeignKey(g => g.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Group>()
                .HasOne(g => g.Mentor)
                .WithMany(u => u.GroupsMentoring)
                .HasForeignKey(g => g.MentorId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==============================
            // GROUP_MEMBER
            // ==============================
            modelBuilder.Entity<GroupMember>()
                .HasOne(gm => gm.Group)
                .WithMany(g => g.Members)
                .HasForeignKey(gm => gm.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupMember>()
                .HasOne(gm => gm.User)
                .WithMany(u => u.GroupMemberships)
                .HasForeignKey(gm => gm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==============================
            // GROUP_SESSION
            // ==============================
            modelBuilder.Entity<GroupSession>()
                .HasOne(gs => gs.Group)
                .WithMany(g => g.GroupSessions)
                .HasForeignKey(gs => gs.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            // ==============================
            // MESSAGE (Between Users or Linked to Session)
            // ==============================
            modelBuilder.Entity<Message>()
                .HasOne(m => m.FromUser)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.FromUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.ToUser)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Session)
                .WithMany(s => s.Messages)
                .HasForeignKey(m => m.SessionId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==============================
            // GROUP_MESSAGE
            // ==============================
            modelBuilder.Entity<GroupMessage>()
                .HasOne(gm => gm.Group)
                .WithMany(g => g.GroupMessages)
                .HasForeignKey(gm => gm.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupMessage>()
                .HasOne(gm => gm.FromUser)
                .WithMany()
                .HasForeignKey(gm => gm.FromUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==============================
            // POST & POST_COMMENT
            // ==============================
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostComment>()
                .HasOne(pc => pc.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(pc => pc.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostComment>()
                .HasOne(pc => pc.User)
                .WithMany(u => u.PostComments)
                .HasForeignKey(pc => pc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==============================
            // POST & POST_LIKE
            // ==============================
            modelBuilder.Entity<PostLike>()
     .HasOne(pl => pl.Post)
     .WithMany(p => p.Likes)
     .HasForeignKey(pl => pl.PostId)
     .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostLike>()
                .HasOne(pl => pl.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(pl => pl.UserId)
                .OnDelete(DeleteBehavior.Restrict);  


            // ==============================
            // CONNECTION (Self-referencing USER ↔ USER)
            // ==============================
            modelBuilder.Entity<Connection>()
                .HasOne(c => c.User)
                .WithMany(u => u.Connections)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Connection>()
                .HasOne(c => c.ConnectedUser)
                .WithMany()
                .HasForeignKey(c => c.ConnectedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Connection>()
                .HasIndex(c => new { c.UserId, c.ConnectedUserId })
                .IsUnique();

            // ==============================
            // RATING (Learner → Mentor)
            // ==============================
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Session)
                .WithMany(s => s.Ratings)
                .HasForeignKey(r => r.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.RatedByUser)
                .WithMany(u => u.RatingsGiven)
                .HasForeignKey(r => r.RatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.RatedToUser)
                .WithMany(u => u.RatingsReceived)
                .HasForeignKey(r => r.RatedToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Skill)
                .WithMany(s => s.Ratings)
                .HasForeignKey(r => r.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==============================
            // BADGE & USER_BADGE
            // ==============================
            modelBuilder.Entity<UserBadge>()
                .HasOne(ub => ub.User)
                .WithMany(u => u.UserBadges)
                .HasForeignKey(ub => ub.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserBadge>()
                .HasOne(ub => ub.Badge)
                .WithMany(b => b.UserBadges)
                .HasForeignKey(ub => ub.BadgeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserBadge>()
                .HasOne(ub => ub.Skill)
                .WithMany(s => s.UserBadges)
                .HasForeignKey(ub => ub.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==============================
            // SUBSCRIPTION & USER
            // ==============================
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            // ==============================
            // SUBSCRIPTION & SUBSCRIPTION_PLAN
            // ==============================

            modelBuilder.Entity<Subscription>()
           .HasOne(s => s.Plan)
           .WithMany(p => p.Subscriptions)
           .HasForeignKey(s => s.PlanId)
           .OnDelete(DeleteBehavior.Restrict);

            // ==============================
            // NOTIFICATION & USER
            // ==============================
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ==============================
            // PAYMENT (Learner → Mentor)
            // ==============================
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Learner)
                .WithMany(u => u.PaymentsAsLearner)
                .HasForeignKey(p => p.LearnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Mentor)
                .WithMany(u => u.PaymentsAsMentor)
                .HasForeignKey(p => p.MentorId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==============================
            // PAYOUT (Mentor + Admin)
            // ==============================
            modelBuilder.Entity<Payout>()
                .HasOne(po => po.Payment)
                .WithMany()
                .HasForeignKey(po => po.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payout>()
                .HasOne(po => po.Mentor)
                .WithMany(u => u.Payouts)
                .HasForeignKey(po => po.MentorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payout>()
                .HasOne(po => po.Admin)
                .WithMany()
                .HasForeignKey(po => po.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==============================
            // USER  ↔ USER REPORT
            // ==============================
            modelBuilder.Entity<UserReport>()
    .HasOne(ur => ur.ReportedUser)
    .WithMany()
    .HasForeignKey(ur => ur.ReportedUserId)
    .OnDelete(DeleteBehavior.Restrict);   

            modelBuilder.Entity<UserReport>()
                .HasOne(ur => ur.ReporterUser)
                .WithMany()
                .HasForeignKey(ur => ur.ReporterUserId)
                .OnDelete(DeleteBehavior.Cascade);


            // ==============================
            // USER ↔ OTP_VERIFICATION
            // ==============================
            modelBuilder.Entity<OtpVerification>()
                      .HasOne(o => o.User)
                       .WithMany(u => u.OtpVerifications) 
                      .HasForeignKey(o => o.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            // ==============================
            // USER ↔ MentorProfile
            // ==============================
            modelBuilder.Entity<User>()
        .HasOne(u => u.MentorProfile)
        .WithOne(mp => mp.User)
        .HasForeignKey<MentorProfile>(mp => mp.UserId)
        .OnDelete(DeleteBehavior.Cascade);
            // ==============================
            // MentorProfile ↔ MentorAvailability
            // ==============================
            modelBuilder.Entity<MentorAvailability>()
      .HasOne(ma => ma.MentorProfile)
      .WithMany(mp => mp.Availabilities)
      .HasForeignKey(ma => ma.MentorProfileId)
      .OnDelete(DeleteBehavior.Cascade);
        }
    }
    

}
