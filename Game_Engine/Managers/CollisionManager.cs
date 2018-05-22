using Game_Engine.Entities;
using System;
using System.Collections.Concurrent;

namespace Game_Engine.Managers
{
    public class CollisionManager
    {
        public ConcurrentQueue<Tuple<Entity, Entity>> CurrentCollisionPairs {get;}


        #region Thread-safe singleton - use "CollisionManager.Instance" to access
        private static readonly Lazy<CollisionManager> lazy = new Lazy<CollisionManager>(() => new CollisionManager(), true);

        private CollisionManager()
        {
            CurrentCollisionPairs = new ConcurrentQueue<Tuple<Entity, Entity>>();
        }

        public static CollisionManager Instance
        {
            get
            {
                return lazy.Value;
            }
        }
        #endregion



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
