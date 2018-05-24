using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using thundercats.Actions;
using thundercats.GameStates;
using thundercats.GameStates.States.MenuStates;
using thundercats.Service;

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
                    ResolveCollision(gameTime, currentCollisionPair);
                }
            }
        }

        /* Runs collision actions based on entity type of the source component pair. */
        private void ResolveCollision(GameTime gameTime, Tuple<Entity, Entity> collisionPair)
        {
            Entity sourceEntity = collisionPair.Item1;
            Entity targetEntity = collisionPair.Item2;

            switch(sourceEntity.EntityTypeName)
            {
                case GameEntityFactory.REMOTE_PLAYER:
                    PlayerCollision(gameTime, collisionPair);
                    break;
                case GameEntityFactory.LOCAL_PLAYER:
                    PlayerCollision(gameTime, collisionPair);
                    break;
                case GameEntityFactory.AI_PLAYER:
                    PlayerCollision(gameTime, collisionPair);
                    break;
                default:
                    break;
            }       
		}

        private void PlayerCollision(GameTime gameTime, Tuple<Entity, Entity> collisionPair)
        {
            Entity sourceEntity = collisionPair.Item1;
            Entity targetEntity = collisionPair.Item2;

            switch (targetEntity.EntityTypeName)
            {
                case GameEntityFactory.GOAL:
                    HandleGoal(sourceEntity);
                    break;
                case GameEntityFactory.BLOCK:
                    KeepSourceOnTop(gameTime, sourceEntity, targetEntity);
                    break;
                case GameEntityFactory.REMOTE_PLAYER:
                    PushSourceAwayFromTarget(gameTime, sourceEntity, targetEntity);
                    break;
                case GameEntityFactory.LOCAL_PLAYER:
                    PushSourceAwayFromTarget(gameTime, sourceEntity, targetEntity);
                    break;
                case GameEntityFactory.OUTOFBOUNDS:
                    HandleOutOfBounds(sourceEntity);
                    break;
            }
        }
        private static void HandleOutOfBounds(Entity sourceEntity) {
            if (sourceEntity.EntityTypeName == GameEntityFactory.LOCAL_PLAYER)
            {
                GameService.Instance.gameManager.CurrentGameState = GameManager.GameState.GameOverScreen;
            }
        }
        private static void HandleGoal(Entity sourceEntity)
        {
            if (sourceEntity.EntityTypeName == GameEntityFactory.LOCAL_PLAYER)
            {
                GameService.Instance.gameManager.CurrentGameState = GameManager.GameState.VictoryScreen;
            }
            else
            {
                GameService.Instance.gameManager.CurrentGameState = GameManager.GameState.GameOverScreen;
            }
        }

        /*
         * Move the source entity away from the target which it has collided with.
         * TODO: Adjustment velocity is currently static and should be adjusted to be relative to how close the bounding volumes are at each axis.
         */
        private void KeepSourceOnTop(GameTime gameTime, Entity sourceEntity, Entity targetEntity)
        {
            CollisionComponent sourceCollisionComponent = ComponentManager.Instance.GetComponentOfEntity<CollisionComponent>(sourceEntity);
            CollisionComponent targetCollisionComponent = ComponentManager.Instance.GetComponentOfEntity<CollisionComponent>(targetEntity);
            GravityComponent sourceGravityComponent = ComponentManager.Instance.GetComponentOfEntity<GravityComponent>(sourceEntity);
            TransformComponent sourceTransformComponent = ComponentManager.Instance.GetComponentOfEntity<TransformComponent>(sourceEntity);

            if (sourceCollisionComponent.Center.Y < targetCollisionComponent.Center.Y)
            {
                CollisionActions.HandleCollisionFromBelow(sourceEntity);
            }
            else
            {
                CollisionActions.HandleCollisionFromAbove(gameTime, sourceEntity);
            }
            CheckSurfaceOfBlock(gameTime, sourceEntity, targetEntity, sourceCollisionComponent, targetCollisionComponent);
        }

        private void CheckSurfaceOfBlock(GameTime gameTime, Entity player, Entity block, CollisionComponent playerCollisionComponent, CollisionComponent blockCollisionComponent)
        {
            var playerBounding = (BoundingSphere)playerCollisionComponent.BoundingShape;
            var boxBounding = (BoundingBox)blockCollisionComponent.BoundingShape;
            var diff = boxBounding.Max.Y - (playerBounding.Center.Y - playerBounding.Radius);

            if (diff < 5 && diff > 0)
            {
                var gravityComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<GravityComponent>(player);
                if (!gravityComponent.HasJumped)
                {
                     CollisionActions.RunParticleSystem(player);
                }
                CollisionActions.HandleCollisionFromAbove(gameTime, player);
            }
            else
            {
                PushSourceAwayFromTarget(gameTime, player, block);
                CollisionActions.HandleCollisionFromBelow(player);
            }
        }

        private void PushSourceAwayFromTarget(GameTime gameTime, Entity sourceEntity, Entity targetEntity)
        {
            CollisionComponent sourceCollisionComponent = ComponentManager.Instance.GetComponentOfEntity<CollisionComponent>(sourceEntity);
            CollisionComponent targetCollisionComponent = ComponentManager.Instance.GetComponentOfEntity<CollisionComponent>(targetEntity);
            GravityComponent sourceGravityComponent = ComponentManager.Instance.GetComponentOfEntity<GravityComponent>(sourceEntity);
            TransformComponent sourceTransformComponent = ComponentManager.Instance.GetComponentOfEntity<TransformComponent>(sourceEntity);

            if (sourceCollisionComponent.Center.X < targetCollisionComponent.Center.X)
            {
                CollisionActions.AccelerateColliderRightwards(gameTime, sourceEntity);
            }
            else
            {
                CollisionActions.AccelerateColliderLeftwards(gameTime, sourceEntity);
            }
            if (sourceCollisionComponent.Center.Z < targetCollisionComponent.Center.Z)
            {
                CollisionActions.AccelerateColliderBackwards(gameTime, sourceEntity);
            }
            else
            {
                CollisionActions.AccelerateColliderForwards(gameTime, sourceEntity);
            }
        }
    }
}
