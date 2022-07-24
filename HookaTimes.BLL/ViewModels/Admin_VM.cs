using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels
{
    public partial class ViewUser_VM
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string RoleId { get; set; }
    }

    public partial class UserList_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public DateTime DateOfBirth { get; set; }
        //public string Email { get; set; }
        //public string Password { get; set; }
        //public string Role { get; set; }
        //public string RoleId { get; set; }
    }
}
