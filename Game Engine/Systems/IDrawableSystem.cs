using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Systems
{
    public interface IDrawableSystem
    {
        void Draw(GameTime gameTime);
    }
}
