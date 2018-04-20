using System;

namespace Game_Engine.Entities
{

    /*
     * Base class for all game objects.
     * Should only be instantiated through EntityFactory and should never be extended.
     */
    public sealed class Entity
    {
        public Guid EntityID { get; }

        internal Entity()
        {
            EntityID = Guid.NewGuid();
        }
    }
}
