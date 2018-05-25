using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;

namespace Game_Engine.Systems.Physics
{
    public class CollisionDetectionSystem : IUpdateableSystem
    {
        private bool compareAllModels;

        public CollisionDetectionSystem(bool compareAllModels = false)
        {
            this.compareAllModels = compareAllModels;
        }

        /// <summary>
        /// Checks intersections for all CollisionComponents if compareAllModels is set to true
        /// Otherwise we only compare players to all other models and reducing number of comparisons. 
        /// </summary>
        public void Update(GameTime gameTime)
        {
            ConcurrentDictionary<Entity, Component> collisionComponentPairs 
                = ComponentManager.Instance.GetConcurrentDictionary<CollisionComponent>();

            var playerComponents = ComponentManager.Instance.GetConcurrentDictionary<PlayerComponent>();
            if (compareAllModels)
            {
                Parallel.ForEach(collisionComponentPairs, sourceCollisionPair =>
                {
                    Entity sourceEntity = sourceCollisionPair.Key;
                    var sourceCollisionComponent = sourceCollisionPair.Value as CollisionComponent;

                    foreach (var targetCollisionComponentPair in collisionComponentPairs)
                    {
                        Entity targetEntity = targetCollisionComponentPair.Key;
                        CollisionComponent targetCollisionComponent = targetCollisionComponentPair.Value as CollisionComponent;
                        if (sourceCollisionComponent.ComponentId != targetCollisionComponent.ComponentId &&
                        sourceCollisionComponent.BoundingVolume.Intersects(targetCollisionComponent.BoundingVolume))
                        {
                            CollisionManager.Instance.AddCollisionPair(sourceEntity, targetEntity);
                        }
                    }
                });
            }
            else
            {
                Parallel.ForEach(playerComponents, playerComponentPair =>
                {
                    Entity sourceEntity = playerComponentPair.Key;
                    var sourceCollisionComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<CollisionComponent>(playerComponentPair.Key);

                    foreach (var targetCollisionComponentPair in collisionComponentPairs)
                    {
                        Entity targetEntity = targetCollisionComponentPair.Key;
                        CollisionComponent targetCollisionComponent = targetCollisionComponentPair.Value as CollisionComponent;
                        if (sourceCollisionComponent.ComponentId != targetCollisionComponent.ComponentId &&
                        sourceCollisionComponent.BoundingVolume.Intersects(targetCollisionComponent.BoundingVolume))
                        {
                            CollisionManager.Instance.AddCollisionPair(sourceEntity, targetEntity);
                        }
                    }
                });
            }
        }
    }
}
