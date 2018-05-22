using Game_Engine.Entities;
using Game_Engine.Components;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Game_Engine.Managers
{
    /*
     * Provides methods for adding/removing/retrieving all components in the game.
     * Component manager is made thread safe by the use of concurrent dictionaries.
     */
    public class ComponentManager
    {

        private ConcurrentDictionary<Type, ConcurrentDictionary<Entity, Component>> componentPairsAndTypesConcurrent;
        private Dictionary<Type, Dictionary<Entity, Component>> componentPairsAndTypes;


        #region Thread-safe singleton - use "ComponentManager.Instance" to access
        private static readonly Lazy<ComponentManager> lazy = new Lazy<ComponentManager>(() => new ComponentManager(), true);

        private ComponentManager()
        {
            componentPairsAndTypes = new Dictionary<Type, Dictionary<Entity, Component>>();
            componentPairsAndTypesConcurrent = new ConcurrentDictionary<Type, ConcurrentDictionary<Entity, Component>>();
        }

        //Creates an instance of the compmanager if one does not already exist
        public static ComponentManager Instance
        {
            get
            {
                return lazy.Value;
            }
        }
        #endregion

        /* Methods */

        /*
         * Returns the nestled Dictionary of all Components of the given type, with their attached Entities as keys, or an emtpy dictionary if not found.
         */

        public ConcurrentDictionary<Entity, Component> GetConcurrentDictionary<T>() where T : Component
        {
            ConcurrentDictionary<Entity, Component> compPairs;
            if(componentPairsAndTypesConcurrent.TryGetValue(typeof(T), out compPairs))
            {
                return compPairs;
            }
            return new ConcurrentDictionary<Entity, Component>();        
        }

        /// <summary>
        /// Returns a regular dictionary for when we do not want a concurrent dictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Dictionary<Entity, Component> GetDictionary<T>() where T: Component
        {
            Dictionary<Entity, Component> compPairs;
            if(componentPairsAndTypes.TryGetValue(typeof(T), out compPairs))
            {
                return compPairs;
            }
            return new Dictionary<Entity, Component>();
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
        
        public void AddComponentToEntity(Entity entity, Component component, Type componentType = null)
        {
            if(componentType == null)
            {
                componentType = component.GetType();
            }
            ConcurrentDictionary<Entity, Component> tempDictConcurrent;
            Dictionary<Entity, Component> tempDict;

            /* Check if any nested dictionary for the component type exists, if not create a new one. */
            if (!componentPairsAndTypesConcurrent.TryGetValue(componentType, out tempDictConcurrent))
            {
                tempDictConcurrent = new ConcurrentDictionary<Entity, Component>();
                componentPairsAndTypesConcurrent.TryAdd(componentType, tempDictConcurrent);
            }
            if (!componentPairsAndTypes.TryGetValue(componentType, out tempDict))
            {
                tempDict = new Dictionary<Entity, Component>();
                componentPairsAndTypes.Add(componentType, tempDict);
            }

            /* Check that the exact component instance does not already exist in the dictionary, if it does throw an error. */
            foreach (Component tempComponent in componentPairsAndTypesConcurrent[componentType].Values)
            {
                if (tempComponent.ComponentId.CompareTo(component.ComponentId) == 0)
                {
                    throw new Exception("Error: Attempted to add duplicate Component: " + component.ComponentId.ToString());
                }
            }

            foreach (Component tempComponent in componentPairsAndTypes[componentType].Values)
            {
                if (tempComponent.ComponentId.CompareTo(component.ComponentId) == 0)
                {
                    throw new Exception("Error: Attempted to add duplicate Component: " + component.ComponentId.ToString());
                }
            }
            /* Add the component to the entity. */
            componentPairsAndTypesConcurrent[componentType][entity] = component;
            componentPairsAndTypes[componentType][entity] = component;
        }

        /*
         * Returns the Component of type T connected to the Entity, or null if not found.
         */
        public T ConcurrentGetComponentOfEntity<T>(Entity entity) where T : Component
        {
            ConcurrentDictionary<Entity, Component> tempDict;
            if(componentPairsAndTypesConcurrent.TryGetValue(typeof(T), out tempDict))
            {
                Component component;
                if(tempDict.TryGetValue(entity, out component))
                {
                    return (T)component;
                }
            }
            return null;
        }

       
        public T GetComponentOfEntity<T>(Entity entity) where T : Component
        {
            Dictionary<Entity, Component> tempDict;
            if (componentPairsAndTypes.TryGetValue(typeof(T), out tempDict))
            {
                Component component;
                if (tempDict.TryGetValue(entity, out component))
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

            Dictionary<Entity, Component> tempDict;
            if(componentPairsAndTypes.TryGetValue(typeof(T), out tempDict))
            {
                Component comp;
                if(tempDict.TryGetValue(entity, out comp))
                {
                    componentPairsAndTypes[typeof(T)].Remove(entity);
                    componentPairsAndTypesConcurrent[typeof(T)].TryRemove(entity,out comp);
                    return true;
                }
            }
            return false;
        }
    }
}
