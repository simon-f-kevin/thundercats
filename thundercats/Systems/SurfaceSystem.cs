using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thundercats.Components;

namespace thundercats.Systems
{


    public class SurfaceSystem : IUpdateableSystem
    {

        public SurfaceSystem()
        {
        }

        public void Update(GameTime gameTime)
        {
            var surfaceKeyValuePair = ComponentManager.Instance.GetConcurrentDictionary<SurfaceComponent>();
            var playerKeyValuePair = ComponentManager.Instance.GetConcurrentDictionary<PlayerComponent>();

            // Collision should be handled somewhere else? Else just get a flag if collided
            foreach (var player in playerKeyValuePair)
            {
                var playerBoundingSphereComponent = ComponentManager.Instance.GetComponentOfEntity<CollisionComponent>(player.Key);
                var playerFrictionComponent = ComponentManager.Instance.GetComponentOfEntity<FrictionComponent>(player.Key);
                // find a more effective way instead of comparing all surfaces if level is big.
                foreach (var surfaceComponent in surfaceKeyValuePair)
                {
                    // Check if it collides with this surface
                    var surfaceBoundingSphere = ComponentManager.Instance.GetComponentOfEntity<CollisionComponent>(surfaceComponent.Key);
                    var surface = surfaceComponent.Value as SurfaceComponent;

                    if (surfaceBoundingSphere.ComponentId != playerBoundingSphereComponent.ComponentId)
                    {
                        if (surfaceBoundingSphere.BoundingBox != null)
                        {
                            if (playerBoundingSphereComponent.BoundingBox != null)
                            {
                                if (surfaceBoundingSphere.BoundingBox.Intersects(playerBoundingSphereComponent.BoundingBox))
                                {
                                    CollisionManager.Instance.AddCollisionPair(player.Key, surfaceComponent.Key);
                                                  //Console.WriteLine(sourceBoundingSphereComponent.ComponentId.ToString() + " Intersects " + targetBoundingSphereComponent.ComponentId.ToString());
                                }
                            }
                            else if (playerBoundingSphereComponent.BoundingSphere != null)
                            {
                                if (surfaceBoundingSphere.BoundingBox.Intersects(playerBoundingSphereComponent.BoundingSphere))
                                {
                                    CollisionManager.Instance.AddCollisionPair(player.Key, surfaceComponent.Key);
                                                  //Console.WriteLine(sourceBoundingSphereComponent.ComponentId.ToString() + " Intersects " + targetBoundingSphereComponent.ComponentId.ToString());
                                }
                            }
                        }
                        else if (surfaceBoundingSphere.BoundingSphere != null)
                        {
                            if (playerBoundingSphereComponent.BoundingBox != null)
                            {
                                if (surfaceBoundingSphere.BoundingSphere.Intersects(playerBoundingSphereComponent.BoundingBox))
                                {
                                    CollisionManager.Instance.AddCollisionPair(player.Key, surfaceComponent.Key);
                                                  //Console.WriteLine(sourceBoundingSphereComponent.ComponentId.ToString() + " Intersects " + targetBoundingSphereComponent.ComponentId.ToString());
                                }
                            }
                            else if (playerBoundingSphereComponent.BoundingSphere != null)
                            {
                                if (surfaceBoundingSphere.BoundingSphere.Intersects(playerBoundingSphereComponent.BoundingSphere))
                                {
                                    CollisionManager.Instance.AddCollisionPair(player.Key, surfaceComponent.Key);
                                                  //Console.WriteLine(sourceBoundingSphereComponent.ComponentId.ToString() + " Intersects " + targetBoundingSphereComponent.ComponentId.ToString());
                                }
                            }
                        }
                    }

                    {
                        // Shouldn't it be players velocity we change rather?
                        playerFrictionComponent.Friction = (float)surface.SurfaceType;

                        // Continue to the next player
                        break;
                    }

                }
                // Change velocity
            }
        }
    }
}
