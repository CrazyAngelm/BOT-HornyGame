using BOT_HornyGame.DataBase;
using BOT_HornyGame.DataBase.Entities;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BOT_HornyGame.Modules
{
    [Group("item")]
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        private readonly RPGContext _context;

        public TestModule(IServiceProvider services)
        {
            _context = services.GetRequiredService<RPGContext>();
        }

        [Command("add")]
        [Summary("add item")]
        public async Task AddItem(string name) 
        {
            var item = await _context.Items.AddAsync( new ItemEntity {Name = name, Description= "Test1" } ).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            string str = string.Format("Item(Id:{0}, Name:{1}, Desc:{2}) added", item.Entity.Id, item.Entity.Name, item.Entity.Description);
            await Context.Channel.SendMessageAsync(str).ConfigureAwait(false);
        }

        [Command("list")]
        [Summary("list of item")]
        public async Task ListItems() 
        {
            string str = string.Empty;
            var items = await _context.Items.ToListAsync().ConfigureAwait(false);

            foreach (var item in items) 
            {
                str += string.Format("Id:{0}, Name:{1}, Desc:{2}\n",item.Id,item.Name,item.Description);
            }
            await Context.Channel.SendMessageAsync(str).ConfigureAwait(false);
        }
    }
}
