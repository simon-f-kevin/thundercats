using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using thundercats.Systems;

namespace thundercats.GameStates.States.AiStates
{
    public class Winning : IAiState
    {
        private AiStateManager aiStateManager;
        private AiSystem aiSystem;
        private int TargetValue { get; set; }
        public Winning(AiStateManager aiStateManager)
        {
            this.aiStateManager = aiStateManager;
            TargetValue = 1;
        }
        public void Update(GameTime gameTime)
        {
            SystemManager.Instance.Update(gameTime);
           // aiSystem.CheckNextRow(,TargetValue);
            
            throw new NotImplementedException();
        }
    }
}
