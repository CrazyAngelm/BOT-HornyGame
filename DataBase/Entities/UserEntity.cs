using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOT_HornyGame.DataBase.Entities
{
    interface IUserEntity : IEntity
    {
        long UserId { get; set; }
        int Money { get; set; }
    }
    public class UserEntity : Entity, IUserEntity
    {
        public long UserId { get; set; }
        public int Money { get; set; }
    }
}
