using Game_Engine.Components;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Systems
{
    public class PhysicsSystem : IUpdateableSystem
    {
        KeyboardState newState = Keyboard.GetState();
        
        public async void Update(GameTime gameTime)
        {
            await Task.Run(() => RunGravity());
        }

        private  void RunGravity()
        {
            var gravityComponents = ComponentManager.Instance.GetComponentDictionary<GravityComponent>();
            foreach (var gravityComponent in gravityComponents)
            {
                var gravity = gravityComponent.Value as GravityComponent;
                var velocity = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(gravityComponent.Key);

                velocity.Velocity.Y -= 0.5f;
            }
        }
    }
}
