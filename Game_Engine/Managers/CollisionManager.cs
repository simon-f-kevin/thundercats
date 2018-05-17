using Game_Engine.Components;
using Game_Engine.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

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

        /*
         * For all bounding box children of target collision component, check which ones interescts with the source collision component.
         */
        public List<dynamic> FindChildBoundingCollisions(CollisionComponent sourceCollisionComponent, CollisionComponent targetCollisionComponent)
        {
            List<dynamic> collidingChildren = new List<dynamic>();

            for(int i = 0; i < targetCollisionComponent.Children.Count; i++)
            {
                if(sourceCollisionComponent.BoundingShape.Intersects(targetCollisionComponent.Children[i]))
                {
                    collidingChildren.Add(targetCollisionComponent.Children[i]);
                }
            }
            return collidingChildren;
        }
    }
}
