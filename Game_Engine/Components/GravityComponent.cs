using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    
    public class GravityComponent : Component
    {
        public bool IsFalling { get; set; } = false;
        public int MaxJump { get; set; }
        public int MinJump { get; set; }

        /// <summary>
        /// Default gravity is set to 9.82, but it is possible to set an unique
        /// gravity constant for each entity that has this component. Good feature
        /// for a potential game mode.
        /// </summary>
        public float Gravity { get; set; } = 9.82f;
        public float Mass { get; internal set; } = 10f;

        public GravityComponent(Entity id) : base(id)
        {
        }


    }
}

