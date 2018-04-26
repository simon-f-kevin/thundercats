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
                var playerBoundingSphere = ComponentManager.Instance.GetComponentOfEntity<BoundingSphereComponent>(player.Key);
                var playerFrictionConstant = ComponentManager.Instance.GetComponentOfEntity<FrictionComponent>(player.Key);
                // find a more effective way instead of comparing all surfaces if level is big.
                foreach (var surface in surfaceKeyValuePair)
                {
                    // Check if it collides with this surface
                    var surfaceBoundingSphere = ComponentManager.Instance.GetComponentOfEntity<BoundingSphereComponent>(surface.Key);
                    var surfaceTest = ComponentManager.Instance.GetComponentOfEntity<SurfaceComponent>(surface.Key);
                    if (surfaceBoundingSphere.BoundingSphere.Intersects(playerBoundingSphere.BoundingSphere))
                    {

                        // Change player velocity with surface values
                        playerFrictionConstant.Friction = (float)surfaceTest.SurfaceType;

                        // Continue to the next player
                        break;
                    }

                }
                // Change velocity
            }
        }
    }
}
