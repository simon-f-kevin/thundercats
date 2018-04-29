using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using System.Collections.Generic;

namespace Game_Engine.Entities
{
    /* Methods for instantiating Entities with different sets of components. */
    public static class EntityFactory
    {
        public static Entity NewEntity()
        {
            return new Entity();
        }

        public static Entity NewEntity(string typeName)
        {
            return new Entity(typeName);
        }

        /* Adds all given components to a new Entity. */
        public static Entity NewEntityWithComponents(List<Component> components)
        {
            Entity entity = new Entity();

            for(int i = 0; i < components.Count; i++)
            {
                ComponentManager.Instance.AddComponentToEntity(entity, components[i]);
            }
            return new Entity();
        }
    }
}
