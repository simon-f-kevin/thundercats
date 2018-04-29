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
        public String EntityTypeName { get; }

        internal Entity()
        {
            EntityID = Guid.NewGuid();
            EntityTypeName = "default";
        }

        internal Entity(string typeName)
        {
            EntityID = Guid.NewGuid();
            EntityTypeName = typeName;
        }
    }
}
