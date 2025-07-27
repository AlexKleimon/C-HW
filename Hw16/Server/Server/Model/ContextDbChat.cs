using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class ContextDbChat : DbContext
    {
        public virtual DbSet<User>? Users { get; set; }
        public virtual DbSet<MessageChat>? Messages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseNpgsql("Host=localhost;Username=postgres;Password=123456;Database=ChatMesUsers");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessageChat>(entity =>
            {
                entity.HasKey(x => x.Id).HasName("message_pkey");
                entity.ToTable("Messages");
                entity.Property(x => x.Id).HasColumnName("id");
                entity.Property(x => x.TextMessage).HasColumnName("text_message");
                entity.Property(x => x.FromUserId).HasColumnName("from_user_id");
                entity.Property(x => x.ToUserId).HasColumnName("to_user_id");

                entity.HasOne(x => x.FromUser).WithMany(x => x.FromMessages).
                HasForeignKey(x => x.FromUserId).HasConstraintName("messages_from_user_id_fkey");
                entity.HasOne(x => x.ToUser).WithMany(x => x.ToMessages)
                .HasForeignKey(x => x.ToUserId).HasConstraintName("messages_to_user_id_fkey");
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id).HasName("user_pkey");
                entity.ToTable("Users");
                entity.Property(x => x.Id).HasColumnName("id");
                entity.Property(x => x.Name).HasMaxLength(255).HasColumnName("name");
                entity.Property(x => x.Port).HasColumnName("port");
                entity.Property(x => x.IpAdress).HasColumnName("Ip");
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
