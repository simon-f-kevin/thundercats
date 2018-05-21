using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Entities;
using thundercats.Components;

namespace thundercats.Service
{
    public class GameService
    {
        public int[,] GameWorld {get; set;}
        public Entity[,] EntityGameWorld { get; set; }

        public static bool FreeParticleBuffer { get; set; } = false;
        public static bool CreateParticles { get; internal set; }

        private static GameService gameService;
        private GameService() {

        }
        public static GameService Instance() {

            if (gameService == null) {

                gameService = new GameService();
            }
            return gameService;
        }
    }
}
