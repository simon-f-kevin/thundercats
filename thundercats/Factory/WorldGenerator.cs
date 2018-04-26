using Game_Engine.Components;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thundercats.Factory
{
    public class WorldGenerator
    {
        private string seed;
        private int[,] world;

        public WorldGenerator(string seed)
        {
            this.seed = seed;
        }

        internal int[,] GenerateWorld(int nLanes, int nRows)
        {
            world = new int[nRows, nLanes];

            PopulateWorld(world);

            return world;
        }

        private void PopulateWorld(int[,] world)
        {
            Random rnd = new Random(seed.GetHashCode());

            for(int i = 0; i < world.GetLength(0); i++)
            {
                for(int j = 0; j < world.GetLength(1); j++)
                {
                    world[i, j] = rnd.Next(2);
                }
                
            }
        }

        /// <summary>
        /// Changes the world matrix of each block model so that they are moved in the actual world space
        /// </summary>
        internal void MoveBlocks()
        {
            var blockComponentsKeyValuePairs = ComponentManager.Instance.GetDictionary<BlockComponent>();
            foreach(var blockComponentKvP in blockComponentsKeyValuePairs)
            {
                var entity = blockComponentKvP.Key;
                ModelComponent modelComponent = ComponentManager.Instance.GetComponentOfEntity<ModelComponent>(entity);
                TransformComponent transformComponent = ComponentManager.Instance.GetComponentOfEntity<TransformComponent>(entity);

                
                Matrix translation = Matrix.CreateTranslation(transformComponent.Position)
                        * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(transformComponent.Position);

                modelComponent.World *= translation;
            }
        }
    }
}
