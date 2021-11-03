using System;

namespace Data
{
    public class EntityBase
    {
        public Guid Id { get; set; }
        public int CreationTime { get; set; } //depends on the time in the system
        public DateTime RealCreationTime { get; set; }
    }
}
