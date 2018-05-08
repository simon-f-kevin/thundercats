using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace thundercats.GameStates.States.AiStates
{
    public class Losing : IAiState
    {
        private AiStateManager aiStateManager;
        public Losing(AiStateManager aiStateManager) {
            this.aiStateManager = aiStateManager;

        }
        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
