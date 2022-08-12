using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels
{
    public partial class SendInvitation_VM
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public int OptionId { get; set; }
        public string Description { get; set; }
        public int ToBuddyId { get; set; }
        public int PlaceId { get; set; }
    }

    public partial class SentInvitation_VM
    {
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
        public string Image { get; set; }
        public float Rating { get; set; }
        public int BuddiesCount { get; set; }
    }

    public partial class Invitation_VM
    {
        public int Id { get; set; }
        public string BuddyName { get; set; }
        public string BuddyImage { get; set; }
        public float BuddyRating { get; set; }
        public int InvitationStatusId { get; set; }
        public string InvitationStatus { get; set; }
        public string Description { get; set; }
    }

    public partial class InvitationOption_VM
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public partial class PlaceInvitation_VM
    {
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
        public string PlaceImage { get; set; }
        public string PlaceLocation { get; set; }
        public float PlaceRating { get; set; }
        public List<Invitation_VM> Buddies { get; set; }

    }
}
