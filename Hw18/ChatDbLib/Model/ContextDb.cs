using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatDbLib.Model
{
    public class ContextDb : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<MessageDb> Messages { get; set; }

        public ContextDb() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine)
                 .UseLazyLoadingProxies()
                 .UseNpgsql("Host = localhost; Username = postgres; Password = 123456; Database = ChatDbLib;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessageDb>(e =>
            {
                e.ToTable("messages");
                e.HasKey(x => x.Id).HasName("messages_pkey");
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.Text).HasColumnName("text");
                e.Property(x => x.ToUserId).HasColumnName("to_user_id");
                e.Property(x => x.FromUserId).HasColumnName("from_user_id");


                e.HasOne(x => x.FromUser)
                .WithMany(x => x.FromMessages)
                .HasForeignKey(x => x.FromUserId)
                .HasConstraintName("messages_from_user_id_fkey");

                e.HasOne(x => x.ToUser)
                .WithMany(x => x.ToMessages)
                .HasForeignKey(x => x.ToUserId);
            });

            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("Users");
                e.HasKey(x => x.Id).HasName("users_pkey");

                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.Name).HasMaxLength(250).HasColumnName("name");

            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
