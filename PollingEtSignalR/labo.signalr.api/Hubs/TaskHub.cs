using labo.signalr.api.Data;
using labo.signalr.api.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace labo.signalr.api.Hubs
{
    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }

    public class TaskHub : Hub
    {
        ApplicationDbContext _context;

        public TaskHub(ApplicationDbContext context)
        {
            _context = context;
        }
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            // TODO: Ajouter votre logique
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            await Clients.All.SendAsync("UserCount", UserHandler.ConnectedIds.Count);
            await Clients.Caller.SendAsync("TaskLIst", _context.UselessTasks.ToList());
        }

        public async Task AddTask(string titre)
        {
            UselessTask uselessTask = new UselessTask()
            {
                Completed = false,
                Text = titre
            };
            _context.UselessTasks.Add(uselessTask);
            _context.SaveChanges();
            await Clients.All.SendAsync("TaskLIst", _context.UselessTasks.ToList());
        }

        public async Task CompleteTask(int id)
        {
            var t = _context.UselessTasks.Single(t => t.Id == id);
            t.Completed = true;
            _context.SaveChanges();
            await Clients.All.SendAsync("TaskLIst", _context.UselessTasks.ToList());
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // TODO: Ajouter votre logique
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            await Clients.All.SendAsync("UserCount", UserHandler.ConnectedIds.Count);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
