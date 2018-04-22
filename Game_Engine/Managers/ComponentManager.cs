using Game_Engine.Entities;
using Game_Engine.Components;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Game_Engine.Managers
{
    /*
     * Provides methods for adding/removing/retrieving all components in the game.
     */
    public class ComponentManager
    {
        private ConcurrentDictionary<Type, ConcurrentDictionary<Entity, Component>> _components;
        

        private static ComponentManager instance;

        /* Constructors */

        private ComponentManager()
        {
            _components = new ConcurrentDictionary<Type, ConcurrentDictionary<Entity, Component>>();
        }

        /* Properties */

        //Creates an instance of the compmanager if one does not already exist
        public static ComponentManager Instance
        {
            get {
                if(instance == null)
                {
                    instance = new ComponentManager();
                }
                return instance;
            }
        }

        /* Methods */

        /*
         * Returns the nestled Dictionary of all Components of the given type, with their attached Entities as keys.
         */
        public ConcurrentDictionary<Entity, Component> GetComponentDictionary<T>() where T : Component
        {
            ConcurrentDictionary<Entity, Component> compDictionary;
            if(_components.TryGetValue(typeof(T), out compDictionary))
            {
                return compDictionary;
            }
            compDictionary = new ConcurrentDictionary<Entity, Component>();
            _components.TryAdd(typeof(T), compDictionary);
            return compDictionary;
        }

        /*
         * Returns component of type T attached to given Entity in the provided Dictionary, or null if not found.
         */
        public T GetComponentFromDict<T>(Entity entity, ConcurrentDictionary<Entity, Component> dictionary) where T : Component
        {
            Component outValue;
            if(dictionary.TryGetValue(entity, out outValue))
            {
                return (T)outValue;
            }
            return null;
        }

        /*
         * Adds the given Component to the Entity by adding it to the static dictionary of components.
         * The Entity object reference itself is used as the key of the dictionary.
         * This works because a component should never be added to two different Entities.
         */
        public void AddComponentToEntity(Entity entity, Component component)
        {
            ConcurrentDictionary<Entity, Component> tempDict;

            /* Check if any nested dictionary for the component type exists, if not create a new one. */
            if(!_components.TryGetValue(component.GetType(), out tempDict))
            {
                tempDict = new ConcurrentDictionary<Entity, Component>();
                _components.TryAdd(component.GetType(), tempDict);
            }

            /* Check that the exact component instance does not already exist in the dictionary, if it does throw an error. */
            foreach(Component tempComponent in _components[component.GetType()].Values)
            {
                if(tempComponent.ComponentId.CompareTo(component.ComponentId) == 0)
                {
                    throw new Exception("Error: Attempted to add duplicate Component: " + component.ComponentId.ToString());
                }
            }
            /* Add the component to the entity. */
            _components[component.GetType()][entity] = component;
        }

        /*
         * Returns the Component of type T connected to the Entity, or null if not found.
         */
        public T GetComponentOfEntity<T>(Entity entity) where T : Component
        {
            ConcurrentDictionary<Entity, Component> tempDict;
            if(_components.TryGetValue(typeof(T), out tempDict))
            {
                Component component;
                if(tempDict.TryGetValue(entity, out component))
                {
                    return (T)component;
                }
            }
            return null;
        }

        /*
         * Removes Component of selected type from an Entity and returns true if succesful.
         */
        public bool RemoveComponentFromEntity<T>(Entity entity) where T : Component
        {
            Component componentValue;
            ConcurrentDictionary<Entity, Component> tempDict;
            if(_components.TryGetValue(typeof(T), out tempDict))
            {
                Component comp;
                if(tempDict.TryGetValue(entity, out comp))
                {
                    _components[typeof(T)].TryRemove(entity,out componentValue);
                    return true;
                }
            }
            return false;
        }
    }
}
