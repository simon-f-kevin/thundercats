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
        public Dictionary<AiState, IAiState> aiStates;

        public AiState _currentAiState = AiState.Winning;
        public enum AiState {
            Winning,
            Losing
        };

        public AiStateManager() {

            aiStates = new Dictionary<AiState, IAiState>();
            aiStates.Add(AiState.Winning, new Winning(this));
            aiStates.Add(AiState.Losing, new Losing(this));

        }
        public void Update(GameTime gameTime) {

            aiStates[_currentAiState].Update(gameTime);
        }
    }
}
