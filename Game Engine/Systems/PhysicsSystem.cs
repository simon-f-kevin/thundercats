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
        public async void Update(GameTime gameTime)
        {
            await Task.Run(() => RunGravity());
        }

        private void RunGravity()
        {
            var velocityComponents = ComponentManager.Instance.GetComponentDictionary<VelocityComponent>();
            foreach (var velocityComponentKeyValuePair in velocityComponents)
            {
                if (velocityComponentKeyValuePair.Value is VelocityComponent velocityComponent) velocityComponent.Velocity.Y -= 0.5f;
            }
        }
    }
}
