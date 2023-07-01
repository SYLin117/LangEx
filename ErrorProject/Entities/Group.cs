using System.ComponentModel.DataAnnotations;

namespace ErrorProject.Entities
{
    public class Group
    {
        // for entity framework
        public Group()
        {
            
        }

        public Group(string name)
        {
            Name = name;
        }

        [Key]
        public string Name { get; set; }
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();
    }
}