using Microsoft.EntityFrameworkCore;
using Private_chat_SignalR.Models.ORM.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Private_chat_SignalR.Models.ORM.Context
{
    public class ProjectContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseLazyLoadingProxies();
        }

        private static string GetConnectionString()
        {
            const string databaseName = "ExampleSignalR";

            return $"Server=localhost;" +
                   $"database={databaseName};" +
                   $"Trusted_Connection = True;" +
                   $"MultipleActiveResultSets = True;" +
                    $"pooling=true;";
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
         
            //Fluent API ile database ilişkisini kuruyorum.
            builder.Entity<ChatRoomUser>(b => b.HasOne<User>(navigationExpression: uf =>uf.User)
           .WithMany(navigationExpression: nf => nf.ChatRoomUsers)
           .HasForeignKey(nf => nf.UserId));

            builder.Entity<ChatRoomUser>(b => b.HasOne<ChatRoom>(navigationExpression: uf => uf.ChatRoom)
           .WithMany(navigationExpression: nf => nf.ChatRoomUsers)
           .HasForeignKey(nf => nf.ChatRoomId));

            builder.Entity<ChatRoomUser>(b => b.HasKey(x => new { x.UserId, x.ChatRoomId }));

            base.OnModelCreating(builder);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatRoomUser> ChatRoomUsers { get; set; }
    }
}
