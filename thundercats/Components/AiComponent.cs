using Game_Engine.Components;
using Game_Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thundercats.GameStates;
using static thundercats.GameStates.AiStateManager;
using static thundercats.Systems.AiSystem;

namespace thundercats.Components
{
    public class AiComponent : Component
    {
        public AiState CurrentState { get; set; }
        public enum AiMove {Left,Run,Right};
        public AiMove CurrentMove { get; set; }
        public AiComponent(Entity id) : base(id)
        {
            CurrentState = AiState.Winning;
        }
    }
}
