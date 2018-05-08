using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thundercats.GameStates.States.AiStates;

namespace thundercats.GameStates
{
    public class AiStateManager
    {
        private Dictionary<AiState, IAiState> aiStates;

        private AiState _currentAiState = AiState.Winning;
        public enum AiState {
            Winning,
            Losing
        };

        public void AiManager() {

            aiStates = new Dictionary<AiState, IAiState>();
            aiStates.Add(AiState.Winning, new Winning(this));
            aiStates.Add(AiState.Losing, new Losing(this));

        }
        public void Update(GameTime gameTime) {

            aiStates[_currentAiState].Update(gameTime);
        }
    }
}
