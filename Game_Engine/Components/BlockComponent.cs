using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Entities;

namespace Game_Engine.Components
{
    /// <summary>
    /// All blocks must have a block component so that we can move them to their positions after creating them
    /// </summary>
    public class BlockComponent : Component
    {
        public BlockComponent(Entity id) : base(id)
        {
        }
    }
}
