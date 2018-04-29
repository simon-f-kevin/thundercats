using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
            Queue<Tuple<Entity, Entity>> collisionPairs = collisionManager.CurrentCollisionPairs;
            Tuple<Entity, Entity> currentCollisionPair;

            for(int i = 0; i < collisionPairs.Count; i++)
            {
                currentCollisionPair = collisionManager.RemoveCollisionPair();
                ResolveCollision(currentCollisionPair);
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
                    UpdateSourceCollider(sourceEntity, targetEntity);
                    break;
                case "default":
                    break;
            }       
		}

        /*
         * Move the source entity away from the target which it has collided with.
         * TODO: Adjustment velocity is currently static and should be adjusted to be relative to how close the spheres are at each axis.
         */
        private void UpdateSourceCollider(Entity sourceEntity, Entity targetEntity)
        {
            BoundingSphereComponent sourceBoundingSphereComponent = ComponentManager.Instance.GetComponentOfEntity<BoundingSphereComponent>(sourceEntity);
            BoundingSphereComponent targetBoundingSphereComponent = ComponentManager.Instance.GetComponentOfEntity<BoundingSphereComponent>(targetEntity);

            if(sourceBoundingSphereComponent.BoundingSphere.Center.X < targetBoundingSphereComponent.BoundingSphere.Center.X)
            {
                CollisionActions.AccelerateColliderRightwards(sourceEntity);
            }
            else
            {
                CollisionActions.AccelerateColliderLeftwards(sourceEntity);
            }
            if(sourceBoundingSphereComponent.BoundingSphere.Center.Y < targetBoundingSphereComponent.BoundingSphere.Center.Y)
            {
                CollisionActions.AccelerateColliderDownwards(sourceEntity);
            }
            else
            {
                CollisionActions.AccelerateColliderUpwards(sourceEntity);
            }
            if(sourceBoundingSphereComponent.BoundingSphere.Center.Z < targetBoundingSphereComponent.BoundingSphere.Center.Z)
            {
                CollisionActions.AccelerateColliderBackwards(sourceEntity);
            }
            else
            {
                CollisionActions.AccelerateColliderForwards(sourceEntity);
            }
        }
    }
}
