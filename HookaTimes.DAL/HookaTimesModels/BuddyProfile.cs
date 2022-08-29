using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.DAL.HookaTimesModels
{
    [Table("BuddyProfile")]
    public partial class BuddyProfile
    {
        public BuddyProfile()
        {
            BuddyProfileAddresses = new HashSet<BuddyProfileAddress>();
            BuddyProfileEducations = new HashSet<BuddyProfileEducation>();
            BuddyProfileExperiences = new HashSet<BuddyProfileExperience>();
            Carts = new HashSet<Cart>();
            FavoriteUserPlaces = new HashSet<FavoriteUserPlace>();
            InvitationFromBuddies = new HashSet<Invitation>();
            InvitationToBuddies = new HashSet<Invitation>();
            Notifications = new HashSet<Notification>();
            Orders = new HashSet<Order>();
            PlaceReviews = new HashSet<PlaceReview>();
            Wishlists = new HashSet<Wishlist>();
        }

        [Key]
        public int Id { get; set; }
        public double? Rating { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal? Height { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Weight { get; set; }
        public int? BodyType { get; set; }
        public int? Eyes { get; set; }
        public int? Hair { get; set; }
        [StringLength(255)]
        public string Profession { get; set; }
        [StringLength(255)]
        public string Interests { get; set; }
        [StringLength(1023)]
        public string Hobbies { get; set; }
        public string About { get; set; }
        [StringLength(255)]
        public string FirstName { get; set; }
        [StringLength(255)]
        public string LastName { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [StringLength(511)]
        public string Image { get; set; }
        public int? GenderId { get; set; }
        public bool? IsAvailable { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateOfBirth { get; set; }
        public int? MaritalStatus { get; set; }
        [StringLength(255)]
        public string SocialMediaLink1 { get; set; }
        [StringLength(255)]
        public string SocialMediaLink2 { get; set; }
        [StringLength(255)]
        public string SocialMediaLink3 { get; set; }
        [StringLength(450)]
        public string Latitude { get; set; }
        [StringLength(450)]
        public string Longitude { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("BuddyProfiles")]
        public virtual AspNetUser User { get; set; }
        [InverseProperty("BuddyProfile")]
        public virtual ICollection<BuddyProfileAddress> BuddyProfileAddresses { get; set; }
        [InverseProperty("BuddyProfile")]
        public virtual ICollection<BuddyProfileEducation> BuddyProfileEducations { get; set; }
        [InverseProperty("BuddyProfile")]
        public virtual ICollection<BuddyProfileExperience> BuddyProfileExperiences { get; set; }
        [InverseProperty("Buddy")]
        public virtual ICollection<Cart> Carts { get; set; }
        [InverseProperty("Buddy")]
        public virtual ICollection<FavoriteUserPlace> FavoriteUserPlaces { get; set; }
        [InverseProperty("FromBuddy")]
        public virtual ICollection<Invitation> InvitationFromBuddies { get; set; }
        [InverseProperty("ToBuddy")]
        public virtual ICollection<Invitation> InvitationToBuddies { get; set; }
        [InverseProperty("Buddy")]
        public virtual ICollection<Notification> Notifications { get; set; }
        [InverseProperty("Buddy")]
        public virtual ICollection<Order> Orders { get; set; }
        [InverseProperty("Buddy")]
        public virtual ICollection<PlaceReview> PlaceReviews { get; set; }
        [InverseProperty("Buddy")]
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}
