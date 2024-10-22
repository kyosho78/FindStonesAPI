using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FindStonesAPI.Models
{
    public partial class FindStoneDBContext : DbContext
    {
        public FindStoneDBContext()
        {
        }

        public FindStoneDBContext(DbContextOptions<FindStoneDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Item> Items { get; set; } = null!;
        public virtual DbSet<ItemHistory> ItemHistories { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserFoundItem> UserFoundItems { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("SECRET");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.Clue)
                    .HasColumnType("text")
                    .HasColumnName("clue");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatorId).HasColumnName("creator_id");

                entity.Property(e => e.FinderId).HasColumnName("finder_id");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("image_url");

                entity.Property(e => e.IsMissing)
                    .HasColumnName("is_missing")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("item_name");

                entity.Property(e => e.LastSeenLocation)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("last_seen_location");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.MissingSince)
                    .HasColumnType("datetime")
                    .HasColumnName("missing_since");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.ItemCreators)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK__Items__creator_i__4222D4EF");

                entity.HasOne(d => d.Finder)
                    .WithMany(p => p.ItemFinders)
                    .HasForeignKey(d => d.FinderId)
                    .HasConstraintName("FK__Items__finder_id__4316F928");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__Items__location___412EB0B6");
            });

            modelBuilder.Entity<ItemHistory>(entity =>
            {
                entity.HasKey(e => e.HistoryId)
                    .HasName("PK__ItemHist__096AA2E9CE4674DD");

                entity.ToTable("ItemHistory");

                entity.Property(e => e.HistoryId).HasColumnName("history_id");

                entity.Property(e => e.ChangeType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("change_type");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.NewValue)
                    .HasColumnType("text")
                    .HasColumnName("new_value");

                entity.Property(e => e.PreviousValue)
                    .HasColumnType("text")
                    .HasColumnName("previous_value");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemHistories)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK__ItemHisto__item___5165187F");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.ItemHistories)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__ItemHisto__updat__52593CB8");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.Latitude)
                    .HasColumnType("decimal(10, 8)")
                    .HasColumnName("latitude");

                entity.Property(e => e.Longitude)
                    .HasColumnType("decimal(11, 8)")
                    .HasColumnName("longitude");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Locations__user___3C69FB99");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.NotificationId).HasColumnName("notification_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsRead)
                    .HasColumnName("is_read")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.Message)
                    .HasColumnType("text")
                    .HasColumnName("message");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK__Notificat__item___4D94879B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Notificat__user___4CA06362");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__AB6E61641B0435DC")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password_hash");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<UserFoundItem>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ItemId })
                    .HasName("PK__User_Fou__7C9E17F298699108");

                entity.ToTable("User_Found_Items");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.FoundAt)
                    .HasColumnType("datetime")
                    .HasColumnName("found_at")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.UserFoundItems)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User_Foun__item___47DBAE45");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserFoundItems)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User_Foun__user___46E78A0C");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
