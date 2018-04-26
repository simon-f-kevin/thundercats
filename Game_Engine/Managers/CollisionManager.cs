using Game_Engine.Components;
using Game_Engine.Entities;
using System;
using System.Collections.Concurrent;

namespace Game_Engine.Managers
{
    public class CollisionManager
    {
        public ConcurrentQueue<Tuple<Component, Component>> CurrentCollisionPairs {get;}

        private static CollisionManager instance;

        private CollisionManager()
        {
            CurrentCollisionPairs = new ConcurrentQueue<Tuple<Component, Component>>();
        }

        public static CollisionManager Instance
        {
            get {
                if(instance == null)
                {
                    instance = new CollisionManager();
                }
                return instance;
            }
        }

        public void AddCollisionPair(Component sourceComponent, Component targetComponent)
        {
            CurrentCollisionPairs.Enqueue(new Tuple<Component, Component>(sourceComponent, targetComponent));
        }

        public void RemoveCollisionPair()
        {
            Tuple<Component, Component> tempCollisionPair;
            CurrentCollisionPairs.TryDequeue(out tempCollisionPair);
        }
    }
}
