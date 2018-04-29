using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

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

        private void ResolveCollision(Tuple<Entity, Entity> collisionPair)
        {
            Entity sourceEntity = collisionPair.Item1;
            Entity targetEntity = collisionPair.Item2;

            switch(sourceEntity.EntityTypeName)
            {
                case "local_player":
                    UpdateSourceCollider(sourceEntity, targetEntity);
                    //Console.WriteLine("local_player collision at sides: " + collisionSides.Item1 + " " + collisionSides.Item2 + " " + collisionSides.Item3);
                    break;
                case "default":
                    break;
            }       
		}

        private void UpdateSourceCollider(Entity sourceEntity, Entity targetEntity)
        {
            BoundingSphereComponent sourceBoundingSphereComponent = ComponentManager.Instance.GetComponentOfEntity<BoundingSphereComponent>(sourceEntity);
            BoundingSphereComponent targetBoundingSphereComponent = ComponentManager.Instance.GetComponentOfEntity<BoundingSphereComponent>(targetEntity);
            VelocityComponent sourceVelocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(sourceEntity);

            if(sourceBoundingSphereComponent.BoundingSphere.Center.X < targetBoundingSphereComponent.BoundingSphere.Center.X)
            {
                sourceVelocityComponent.Velocity.X -= 0.1f;
            }
            else
            {
                sourceVelocityComponent.Velocity.X += 0.1f;
            }
            if(sourceBoundingSphereComponent.BoundingSphere.Center.Y < targetBoundingSphereComponent.BoundingSphere.Center.Y)
            {
                //sourceVelocityComponent.Velocity.Y -= 0.1f;
            }
            else
            {
                //sourceVelocityComponent.Velocity.Y += 0.1f;
            }
            if(sourceBoundingSphereComponent.BoundingSphere.Center.Z < targetBoundingSphereComponent.BoundingSphere.Center.Z)
            {
                sourceVelocityComponent.Velocity.Z -= 0.1f;
            }
            else
            {
                sourceVelocityComponent.Velocity.Z += 0.1f;
            }
        }
    }
}
