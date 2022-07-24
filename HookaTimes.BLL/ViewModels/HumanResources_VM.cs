using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels
{
    public partial class BaseHumanResources_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public partial class StaffMember_VM : BaseHumanResources_VM
    {
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string RoleId { get; set; }
    }

    public partial class StaffList_VM : BaseHumanResources_VM
    {
      
    }

    public partial class HookaTimesTeamList_VM : BaseHumanResources_VM
    {
        public string Image { get; set; }
        
    }

    public partial class HookaTimesTeamMember_VM :BaseHumanResources_VM
    {
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public int ConcessionId { get; set; }
        public string Image { get; set; }
    }
}
