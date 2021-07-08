using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOT_HornyGame.DataBase.Entities
{
    interface IItemEntity : IEntity
    {
        string Name { get; set; }
        string Description { get; set; }
    }

    public class ItemEntity : Entity, IItemEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
