using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thundercats.Service
{
    public class GameService
    {
        public int[,] GameWorld {get; set;}
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
