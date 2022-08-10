using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using HookaTimes.DAL.HookaTimesModels;

namespace HookaTimes.DAL.Data
{
    public partial class HookaDbContext : DbContext
    {
        public HookaDbContext()
        {
        }

        public HookaDbContext(DbContextOptions<HookaDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<BuddiesFilter> BuddiesFilters { get; set; }
        public virtual DbSet<BuddyProfile> BuddyProfiles { get; set; }
        public virtual DbSet<Cuisine> Cuisines { get; set; }
        public virtual DbSet<EmailOtp> EmailOtps { get; set; }
        public virtual DbSet<FavoriteUserPlace> FavoriteUserPlaces { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Invitation> Invitations { get; set; }
        public virtual DbSet<InvitationOption> InvitationOptions { get; set; }
        public virtual DbSet<InvitationStatus> InvitationStatuses { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<OfferType> OfferTypes { get; set; }
        public virtual DbSet<PhoneOtp> PhoneOtps { get; set; }
        public virtual DbSet<PlaceAlbum> PlaceAlbums { get; set; }
        public virtual DbSet<PlaceFilter> PlaceFilters { get; set; }
        public virtual DbSet<PlaceMenu> PlaceMenus { get; set; }
        public virtual DbSet<PlaceOffer> PlaceOffers { get; set; }
        public virtual DbSet<PlaceReview> PlaceReviews { get; set; }
        public virtual DbSet<PlacesProfile> PlacesProfiles { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=tiaragroup.database.windows.net;Initial Catalog=HookaTimes;User Id=adminall;Password=P@ssw0rd@123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");
                        });
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<BuddiesFilter>(entity =>
            {
                entity.Property(e => e.Title).IsFixedLength();
            });

            modelBuilder.Entity<BuddyProfile>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.BuddyProfiles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_BuddyProfile_AspNetUsers");
            });

            modelBuilder.Entity<FavoriteUserPlace>(entity =>
            {
                entity.HasOne(d => d.Buddy)
                    .WithMany(p => p.FavoriteUserPlaces)
                    .HasForeignKey(d => d.BuddyId)
                    .HasConstraintName("FK_FavoriteUserPlaces_BuddyProfile");

                entity.HasOne(d => d.PlaceProfile)
                    .WithMany(p => p.FavoriteUserPlaces)
                    .HasForeignKey(d => d.PlaceProfileId)
                    .HasConstraintName("FK_FavoriteUserPlaces_PlacesProfile");
            });

            modelBuilder.Entity<Invitation>(entity =>
            {
                entity.HasOne(d => d.FromBuddy)
                    .WithMany(p => p.InvitationFromBuddies)
                    .HasForeignKey(d => d.FromBuddyId)
                    .HasConstraintName("FK_Invitation_BuddyProfile");

                entity.HasOne(d => d.InvitationOption)
                    .WithMany(p => p.Invitations)
                    .HasForeignKey(d => d.InvitationOptionId)
                    .HasConstraintName("FK_Invitation_InvitationOption");

                entity.HasOne(d => d.InvitationStatus)
                    .WithMany(p => p.Invitations)
                    .HasForeignKey(d => d.InvitationStatusId)
                    .HasConstraintName("FK_Invitation_InvitationStatus");

                entity.HasOne(d => d.Place)
                    .WithMany(p => p.Invitations)
                    .HasForeignKey(d => d.PlaceId)
                    .HasConstraintName("FK_Invitation_PlacesProfile");

                entity.HasOne(d => d.ToBuddy)
                    .WithMany(p => p.InvitationToBuddies)
                    .HasForeignKey(d => d.ToBuddyId)
                    .HasConstraintName("FK_Invitation_BuddyProfile1");
            });

            modelBuilder.Entity<PlaceAlbum>(entity =>
            {
                entity.HasOne(d => d.PlaceProfile)
                    .WithMany(p => p.PlaceAlbums)
                    .HasForeignKey(d => d.PlaceProfileId)
                    .HasConstraintName("FK_PlacesAlbums_PlacesProfile");
            });

            modelBuilder.Entity<PlaceMenu>(entity =>
            {
                entity.HasOne(d => d.PlaceProfile)
                    .WithMany(p => p.PlaceMenus)
                    .HasForeignKey(d => d.PlaceProfileId)
                    .HasConstraintName("FK_PlacesMenus_PlacesProfile");
            });

            modelBuilder.Entity<PlaceOffer>(entity =>
            {
                entity.HasOne(d => d.PlaceProfile)
                    .WithMany(p => p.PlaceOffers)
                    .HasForeignKey(d => d.PlaceProfileId)
                    .HasConstraintName("FK_PlaceOffer_PlacesProfile");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.PlaceOffers)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_PlaceOffer_OfferType");
            });

            modelBuilder.Entity<PlaceReview>(entity =>
            {
                entity.HasOne(d => d.Buddy)
                    .WithMany(p => p.PlaceReviews)
                    .HasForeignKey(d => d.BuddyId)
                    .HasConstraintName("FK_PlaceReview_BuddyProfile");

                entity.HasOne(d => d.PlaceProfile)
                    .WithMany(p => p.PlaceReviews)
                    .HasForeignKey(d => d.PlaceProfileId)
                    .HasConstraintName("FK_PlacesReviews_PlacesProfile");
            });

            modelBuilder.Entity<PlacesProfile>(entity =>
            {
                entity.HasOne(d => d.Cuisine)
                    .WithMany(p => p.PlacesProfiles)
                    .HasForeignKey(d => d.CuisineId)
                    .HasConstraintName("FK_PlacesProfile_Cuisine");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.PlacesProfiles)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_PlacesProfile_Location");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PlacesProfiles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_PlacesProfile_AspNetUsers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
