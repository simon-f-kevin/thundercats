using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Engine.Components
{
    public class TextureComponent : Component
    {
        public Texture2D Texture { get; set; }

        public TextureComponent(Entity id) : base(id)
        {
        }
    }
}
