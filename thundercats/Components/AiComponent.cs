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
        public enum AiState {Losing,Winning,Even};
        public enum AiMove {Left = -1,Run = 0,Right = 1};
        public AiComponent(Entity id) : base(id)
        {

        }
    }
}
