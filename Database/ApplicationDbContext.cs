using Microsoft.EntityFrameworkCore;
using TeamChat.Models;

namespace TeamChat.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Profile> profiles { get; set; }
        public DbSet<Server> servers { get; set; }
        public DbSet<Member> members { get; set; }
        public DbSet<Channel> channels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Profile
            modelBuilder
                .Entity<Profile>()
                .HasMany(p => p.servers)
                .WithOne(p => p.profile)
                .HasForeignKey(p => p.profileId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Profile>()
                .HasMany(p => p.members)
                .WithOne(p => p.profile)
                .HasForeignKey(p => p.profileId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Profile>()
                .HasMany(p => p.channels)
                .WithOne(p => p.profile)
                .HasForeignKey(p => p.profileId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Server
            modelBuilder
                .Entity<Server>()
                .HasOne(s => s.profile)
                .WithMany(s => s.servers)
                .HasForeignKey(s => s.profileId)
                .IsRequired(false);

            modelBuilder
                .Entity<Server>()
                .HasMany(p => p.members)
                .WithOne(p => p.server)
                .HasForeignKey(p => p.serverId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Server>()
                .HasMany(p => p.channels)
                .WithOne(p => p.server)
                .HasForeignKey(p => p.serverId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Member
            modelBuilder
                .Entity<Member>()
                .HasOne(m => m.profile)
                .WithMany(m => m.members)
                .HasForeignKey(m => m.profileId)
                .IsRequired(false);

            modelBuilder
                .Entity<Member>()
                .HasOne(m => m.server)
                .WithMany(m => m.members)
                .HasForeignKey(m => m.serverId)
                .IsRequired(false);

            modelBuilder
                .Entity<Member>()
                .HasMany(m => m.messages)
                .WithOne(m => m.member)
                .HasForeignKey(m => m.memberId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Member>()
                .HasMany(m => m.directMessages)
                .WithOne(m => m.member)
                .HasForeignKey(m => m.memberId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Member>()
                .HasMany(m => m.conversationsInitialted)
                .WithOne(m => m.memberOne)
                .HasForeignKey(m => m.memberOneId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Member>()
                .HasMany(m => m.conversationsReceived)
                .WithOne(m => m.memberTwo)
                .HasForeignKey(m => m.memberTwoId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion


            #region Channel

            modelBuilder
                .Entity<Channel>()
                .HasOne(c => c.profile)
                .WithMany(c => c.channels)
                .HasForeignKey(c => c.profileId)
                .IsRequired(false);

            modelBuilder
                .Entity<Channel>()
                .HasOne(c => c.server)
                .WithMany(c => c.channels)
                .HasForeignKey(c => c.serverId)
                .IsRequired(false);

            modelBuilder
                .Entity<Channel>()
                .HasMany(c => c.messages)
                .WithOne(c => c.channel)
                .HasForeignKey(c => c.channelId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Message

            modelBuilder
                .Entity<Message>()
                .HasOne(s => s.member)
                .WithMany(s => s.messages)
                .HasForeignKey(s => s.memberId)
                .IsRequired(false);

            modelBuilder
                .Entity<Message>()
                .HasOne(s => s.channel)
                .WithMany(s => s.messages)
                .HasForeignKey(s => s.channelId)
                .IsRequired(false);

            #endregion

            #region DirectMessage

            modelBuilder
                .Entity<DirectMessage>()
                .HasOne(s => s.member)
                .WithMany(s => s.directMessages)
                .HasForeignKey(s => s.memberId)
                .IsRequired(false);

            modelBuilder
                .Entity<DirectMessage>()
                .HasOne(s => s.conversation)
                .WithMany(s => s.directMessages)
                .HasForeignKey(s => s.conversationId)
                .IsRequired(false);

            #endregion

            #region Conversation

            modelBuilder
                .Entity<Conversation>()
                .HasOne(s => s.memberOne)
                .WithMany(s => s.conversationsInitialted)
                .HasForeignKey(s => s.memberOneId)
                .IsRequired(false);

            modelBuilder
                .Entity<Conversation>()
                .HasOne(s => s.memberTwo)
                .WithMany(s => s.conversationsReceived)
                .HasForeignKey(s => s.memberTwoId)
                .IsRequired(false);

            modelBuilder
                .Entity<Conversation>()
                .HasMany(c => c.directMessages)
                .WithOne(c => c.conversation)
                .HasForeignKey(c => c.conversationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion


            modelBuilder
                .Entity<Profile>()
                .HasData(
                    new Profile
                    {
                        id = Guid.NewGuid().ToString(),
                        name = "John",
                        email = "john@gmail.com",
                        imageUrl = "my image url",
                        createdAt = DateTime.UtcNow,
                    }
                );
        }
    }
}
