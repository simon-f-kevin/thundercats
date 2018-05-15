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
                case GameEntityFactory.REMOTE_PLAYER:
                    PlayerCollision(collisionPair);
                    break;
                case GameEntityFactory.LOCAL_PLAYER:
                    PlayerCollision(collisionPair);
                    break;
                case GameEntityFactory.AI_PLAYER:
                    PlayerCollision(collisionPair);
                    break;
                default:
                    break;
            }       
		}

        private void PlayerCollision(Tuple<Entity, Entity> collisionPair)
        {
            Entity sourceEntity = collisionPair.Item1;
            Entity targetEntity = collisionPair.Item2;

            switch (targetEntity.EntityTypeName)
            {
                case GameEntityFactory.GOAL:
                    Debug.WriteLine("A winner is you");
                    break;
                case GameEntityFactory.BLOCK:
                    UpdateSourceCollider(sourceEntity, targetEntity);
                    break;
                case GameEntityFactory.REMOTE_PLAYER:
                    UpdateSourceCollider(sourceEntity, targetEntity);
                    break;
                case GameEntityFactory.LOCAL_PLAYER:
                    UpdateSourceCollider(sourceEntity, targetEntity);
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
            GravityComponent sourceGravityComponent = ComponentManager.Instance.GetComponentOfEntity<GravityComponent>(sourceEntity);
            TransformComponent sourceTransformComponent = ComponentManager.Instance.GetComponentOfEntity<TransformComponent>(sourceEntity);
            float diffX = sourceCollisionComponent.Center.X - targetCollisionComponent.Center.X;
            float diffZ = sourceCollisionComponent.Center.Z - targetCollisionComponent.Center.Z;
            //float diffY = sourceCollisionComponent.Center.Y - targetCollisionComponent.Center.Y;

            //if (sourceCollisionComponent.Center.X < targetCollisionComponent.Center.X)
            //{
            //    CollisionActions.AccelerateColliderRightwards(sourceEntity, diffX);
            //}
            //else
            //{
            //    CollisionActions.AccelerateColliderLeftwards(sourceEntity, diffX);
            //}
            if (sourceCollisionComponent.Center.Y < targetCollisionComponent.Center.Y)
            {
                CollisionActions.AccelerateColliderDownwards(sourceEntity);
            }
            else
            {
                //sourceGravityComponent.IsFalling = false;
                CollisionActions.AccelerateColliderUpwards(sourceEntity);
               //S sourceTransformComponent.Position.Y += 0.6f;
            }
            //if (sourceCollisionComponent.Center.Z < targetCollisionComponent.Center.Z)
            //{
            //    CollisionActions.AccelerateColliderBackwards(sourceEntity, diffZ);
            //}
            //else
            //{
            //    CollisionActions.AccelerateColliderForwards(sourceEntity, diffZ);
            //}
        }
    }
}
