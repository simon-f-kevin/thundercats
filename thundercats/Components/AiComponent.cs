using Game_Engine.Components;
using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thundercats.GameStates;
using static thundercats.Systems.AiSystem;

namespace thundercats.Components
{
    public class AiComponent : Component
    {
        public AiState CurrentState { get; set; }
        public Point MatrixPosition { get; set; }
            
        public AiComponent(Entity id) : base(id)
        {
            CurrentState = AiState.Winning;
        }
    }
}
