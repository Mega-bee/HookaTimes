using HookaTimes.BLL.ViewModels;
using HookaTimes.DAL;
using HookaTimes.DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;
//using HookaTimes.DAL.Models;

namespace HookaTimes.BLL.Hubs
{
    public class NotificationHub : Hub<INotificationHub>
    {
        private static readonly Dictionary<string, string> UserIds = new Dictionary<string, string>();
        private readonly HookaDbContext _context;
        private readonly IUnitOfWork _uow;

        public NotificationHub(HookaDbContext context, IUnitOfWork uow)
        {
            _context = context;
            _uow = uow;
        }

        public void RegisterUser(string id)
        {

            UserIds.Add(Context.ConnectionId, id);
            UpdateUserList();
        }

        public override Task OnConnectedAsync()
        {
            //string userId = Context.User.Claims.Where(x => x.Type == "UID").FirstOrDefault().Value;
            //string userId = Context.UserIdentifier;
            //var role = _context.AspNetUsers.Where(x => x.Id == userId).Select(x => new { roleName = x.Roles }).FirstOrDefault();
            //if (!UserIds.ContainsKey(Context.ConnectionId))
            //{
            //    UserIds.Add(Context.ConnectionId, userId);
            //    UpdateUserList();
            //}

            ////UserIds.Add(userId, userId);
            //Groups.AddToGroupAsync(Context.ConnectionId, role.roleName);
            UpdateUserList();
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (UserIds.ContainsKey(Context.ConnectionId))
            {
                UserIds.Remove(Context.ConnectionId);
                UpdateUserList();
            }
            return base.OnDisconnectedAsync(exception);
        }

        private Task UpdateUserList()
        {
            //var usersList = _context.AccProfiles.Select(x => new
            //    Profile_VM
            //{
            //    Email = x.Email,
            //    Id = x.Id,
            //    Uid = x.UserId,
            //    Name = x.Name,
            //    Role = x.Role.RoleName,
            //    PhoneNumber = x.PhoneNumber
            //}).ToList();
            //var usersList = Users.Select(x => new Profile_VM
            //{
            //    Id = x.Value.Id,
            //    Name = x.Value.Name,
            //    Uid = x.Value.Uid,
            //    Role = x.Value.Role,
            //    Email = x.Value.Email,
            //}).ToList();
            var usersList = UserIds.Values.ToList();
            return Clients.All.UpdatedUserList(usersList);

        }

        public async Task SearchNearestBuddies(string longitude,string latitude)
        {
            HttpRequest request = Context.GetHttpContext().Request;
            List<HookaBuddy_VM> buddies = Array.Empty<HookaBuddy_VM>().ToList();
            DbGeography searchLocation = DbGeography.FromText(String.Format("POINT({0} {1})", longitude, latitude));
            buddies = await _uow.BuddyRepository.GetAll(x => x.IsDeleted == false && x.Longitude != null && x.Latitude != null).Select(x => new HookaBuddy_VM
            {

                About = x.About ?? "",
                Id = x.Id,
                IsAvailable = x.IsAvailable ?? false,
                Name = x.FirstName + " " + x.LastName,
                Image = $"{request.Scheme}://{request.Host}{x.Image}",
                Distance =  searchLocation.Distance(
                         DbGeography.FromText("POINT(" + x.Longitude + " " + x.Latitude + ")")),

            
            }).Where(x=> x.Distance >0 && x.Distance < 10000 ).ToListAsync();
            
            await Clients.Client(Context.ConnectionId).UpdateBuddiesMap(buddies);
        }
    }


}
