using Game_Engine.Components;
using Game_Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thundercats.Components
{
    public class AiComponent : Component
    {
        public enum AiState { Losing, Winning, Even };
        public enum AiMove {Left,Run,Right};
        public AiState CurrentState { get; set; }
        public AiComponent(Entity id) : base(id)
        {
            CurrentState = AiState.Even;
        }
    }
}
