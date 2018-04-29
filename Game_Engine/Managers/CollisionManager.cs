using Game_Engine.Components;
using Game_Engine.Entities;
using System;
using System.Collections.Concurrent;

namespace Game_Engine.Managers
{
    public class CollisionManager
    {
        public Queue<Tuple<Entity, Entity>> CurrentCollisionPairs {get;}

        private static CollisionManager instance;

        private CollisionManager()
        {
            CurrentCollisionPairs = new Queue<Tuple<Entity, Entity>>();
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

        public void AddCollisionPair(Entity source, Entity target)
        {
            CurrentCollisionPairs.Enqueue(new Tuple<Entity, Entity>(source, target));
        }

        public Component RemoveCollisionPair()
        {
            return CollisionPairs.Dequeue();
        }
    }
}
