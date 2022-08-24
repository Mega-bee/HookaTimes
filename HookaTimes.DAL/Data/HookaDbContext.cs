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
        public virtual DbSet<BuddyProfileAddress> BuddyProfileAddresses { get; set; }
        public virtual DbSet<BuddyProfileEducation> BuddyProfileEducations { get; set; }
        public virtual DbSet<BuddyProfileExperience> BuddyProfileExperiences { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<ContactInfo> ContactInfos { get; set; }
        public virtual DbSet<ContactU> ContactUs { get; set; }
        public virtual DbSet<Cuisine> Cuisines { get; set; }
        public virtual DbSet<EmailOtp> EmailOtps { get; set; }
        public virtual DbSet<FavoriteUserPlace> FavoriteUserPlaces { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Invitation> Invitations { get; set; }
        public virtual DbSet<InvitationOption> InvitationOptions { get; set; }
        public virtual DbSet<InvitationStatus> InvitationStatuses { get; set; }
        public virtual DbSet<JobVacancy> JobVacancies { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<OfferType> OfferTypes { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<PartnerRequest> PartnerRequests { get; set; }
        public virtual DbSet<PhoneOtp> PhoneOtps { get; set; }
        public virtual DbSet<PlaceAlbum> PlaceAlbums { get; set; }
        public virtual DbSet<PlaceFilter> PlaceFilters { get; set; }
        public virtual DbSet<PlaceMenu> PlaceMenus { get; set; }
        public virtual DbSet<PlaceOffer> PlaceOffers { get; set; }
        public virtual DbSet<PlaceReview> PlaceReviews { get; set; }
        public virtual DbSet<PlacesProfile> PlacesProfiles { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<VirtualCart> VirtualCarts { get; set; }
        public virtual DbSet<VirtualWishlist> VirtualWishlists { get; set; }
        public virtual DbSet<Wishlist> Wishlists { get; set; }

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

            modelBuilder.Entity<BuddyProfileAddress>(entity =>
            {
                entity.HasOne(d => d.BuddyProfile)
                    .WithMany(p => p.BuddyProfileAddresses)
                    .HasForeignKey(d => d.BuddyProfileId)
                    .HasConstraintName("FK_BuddyProfileAddress_BuddyProfile");
            });

            modelBuilder.Entity<BuddyProfileEducation>(entity =>
            {
                entity.HasOne(d => d.BuddyProfile)
                    .WithMany(p => p.BuddyProfileEducations)
                    .HasForeignKey(d => d.BuddyProfileId)
                    .HasConstraintName("FK_BuddyProfileEducation_BuddyProfile");
            });

            modelBuilder.Entity<BuddyProfileExperience>(entity =>
            {
                entity.HasOne(d => d.BuddyProfile)
                    .WithMany(p => p.BuddyProfileExperiences)
                    .HasForeignKey(d => d.BuddyProfileId)
                    .HasConstraintName("FK_BuddyProfileExperience_BuddyProfile");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasOne(d => d.Buddy)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.BuddyId)
                    .HasConstraintName("FK_Cart_BuddyProfile");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Cart_Product");
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

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Buddy)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.BuddyId)
                    .HasConstraintName("FK_Notification_BuddyProfile");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Notification_Order");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_Order_BuddyProfileAddress");

                entity.HasOne(d => d.Buddy)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.BuddyId)
                    .HasConstraintName("FK_Order_BuddyProfile");

                entity.HasOne(d => d.OrderStatus)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderStatusId)
                    .HasConstraintName("FK_Order_OrderStatus");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderItem_Order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_OrderItem_Product");
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

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK_Product_ProductCategory");
            });

            modelBuilder.Entity<VirtualCart>(entity =>
            {
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.VirtualCarts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_VirtualCart_Product");
            });

            modelBuilder.Entity<VirtualWishlist>(entity =>
            {
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.VirtualWishlists)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_VirtualWishlist_Product");
            });

            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.HasOne(d => d.Buddy)
                    .WithMany(p => p.Wishlists)
                    .HasForeignKey(d => d.BuddyId)
                    .HasConstraintName("FK_Wishlist_BuddyProfile");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Wishlists)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Wishlist_Product");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
