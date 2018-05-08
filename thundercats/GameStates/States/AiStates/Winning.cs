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
            //jag vill väl bara uppdatera AiSystem? så kör allt i AI?
            aiSystem.CheckNextRow(/*fuckit*/,TargetValue);
           // aiSystem.CheckNextRow(,TargetValue);
            
            throw new NotImplementedException();
        }
    }
}
