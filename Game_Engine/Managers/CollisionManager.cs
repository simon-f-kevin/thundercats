using Game_Engine.Entities;
using System;
using System.Collections.Concurrent;

namespace Game_Engine.Managers
{
    public class CollisionManager
    {
        public ConcurrentQueue<Tuple<Entity, Entity>> CurrentCollisionPairs {get;}

        private static CollisionManager instance;

        private CollisionManager()
        {
            CurrentCollisionPairs = new ConcurrentQueue<Tuple<Entity, Entity>>();
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

        public Tuple<Entity, Entity> RemoveCollisionPair()
        {
            Tuple<Entity, Entity> result;
            CurrentCollisionPairs.TryDequeue(out result);
            return result;
        }
    }
}
