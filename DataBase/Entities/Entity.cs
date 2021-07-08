using System.ComponentModel.DataAnnotations;

namespace BOT_HornyGame.DataBase.Entities
{
    public interface IEntity
    {
        int Id { get; }
    }

    public abstract class Entity : IEntity
    {
        [Key]
        public int Id { get; private set; }
    }
}
