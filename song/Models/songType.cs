using Entity.Interfaces;
namespace SONG.Models
{
    public class songType : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string singer { get; set; }
    }
} 