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
        public void Update(GameTime gameTime)
        {
            var gravityComponents = ComponentManager.Instance.GetComponentDictionary<GravityComponent>();
            foreach (var gravityComponent in gravityComponents)
            {
                var gravity = gravityComponent.Value as GravityComponent;
                var transform = ComponentManager.Instance.GetComponentOfEntity<TransformComponent>(gravityComponent.Key);
  
                Gravity(transform, gravity);
            }
        }
        private void Gravity(TransformComponent transform, GravityComponent gravity)
        {
            if(transform.position.Y != 0)
            {
                transform.position.Y += gravity.Gravity;
            }
        }
       
    }
}
