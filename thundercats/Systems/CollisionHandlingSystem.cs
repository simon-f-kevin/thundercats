using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using thundercats.Actions;

namespace thundercats.Systems
{
    class CollisionHandlingSystem : IUpdateableSystem
    {
        private CollisionManager collisionManager = CollisionManager.Instance;

        /*
        * Gets all collisions from engine and resolves them according to collision rules
        */
        public void Update(GameTime gameTime)
        {
            ConcurrentQueue<Tuple<Entity, Entity>> collisionPairs = collisionManager.CurrentCollisionPairs;
            Tuple<Entity, Entity> currentCollisionPair;

            for(int i = 0; i < collisionPairs.Count; i++)
            {
                currentCollisionPair = collisionManager.RemoveCollisionPair();
                if(currentCollisionPair != null)
                {
                    ResolveCollision(currentCollisionPair);
                }
            }
        }

        /* Runs collision actions based on entity type of the source component pair. */
        private void ResolveCollision(Tuple<Entity, Entity> collisionPair)
        {
            Entity sourceEntity = collisionPair.Item1;
            Entity targetEntity = collisionPair.Item2;

            switch(sourceEntity.EntityTypeName)
            {
                case "local_player":
                    if(targetEntity.EntityTypeName == "Goal")
                    {
                        Debug.WriteLine("A winner is you");
                    }
                    else UpdateSourceCollider(sourceEntity, targetEntity);
                    break;
                case "default":
                    break;
            }       
		}

        /*
         * Move the source entity away from the target which it has collided with.
         * TODO: Adjustment velocity is currently static and should be adjusted to be relative to how close the bounding volumes are at each axis.
         */
        private void UpdateSourceCollider(Entity sourceEntity, Entity targetEntity)
        {
            CollisionComponent sourceCollisionComponent = ComponentManager.Instance.GetComponentOfEntity<CollisionComponent>(sourceEntity);
            CollisionComponent targetCollisionComponent = ComponentManager.Instance.GetComponentOfEntity<CollisionComponent>(targetEntity);
    
            if(sourceCollisionComponent.Center.X <= targetCollisionComponent.Center.X)
            {
                CollisionActions.AccelerateColliderRightwards(sourceEntity);
            }
            else if (sourceCollisionComponent.Center.X > targetCollisionComponent.Center.X)

            {
                CollisionActions.AccelerateColliderLeftwards(sourceEntity);
            }
            if(sourceCollisionComponent.Center.Y <= targetCollisionComponent.Center.Y)
            {
                CollisionActions.AccelerateColliderDownwards(sourceEntity);
            }
            else if (sourceCollisionComponent.Center.Y > targetCollisionComponent.Center.Y)

            {
                CollisionActions.AccelerateColliderUpwards(sourceEntity);
            }
            if(sourceCollisionComponent.Center.Z <= targetCollisionComponent.Center.Z)
            {
                CollisionActions.AccelerateColliderBackwards(sourceEntity);
            }
            else if (sourceCollisionComponent.Center.Z > targetCollisionComponent.Center.Z)

            {
                CollisionActions.AccelerateColliderForwards(sourceEntity);
            }
        }
    }
}
