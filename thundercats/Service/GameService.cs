using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Entities;
using thundercats.Components;
using thundercats.GameStates;

namespace thundercats.Service
{
    public class GameService
    {
        public int[,] GameWorld {get; set;}
        public Entity[,] EntityGameWorld { get; set; }

        public static bool FreeParticleBuffer { get; set; } = false;
        public static bool CreateParticles { get; internal set; }

        private static GameService gameService;

        public GameManager gameManager { get; set; }
        private GameService() {

        }
        public static GameService Instance {
            get
            {
                if (gameService == null)
                {

                    gameService = new GameService();
                }
                return gameService;
            }
            set
            {
                gameService = value;
            }
        }
    }
}
