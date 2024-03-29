﻿using Game_Engine.Components;
using Game_Engine.Helpers;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thundercats.Factory
{
    /// <summary>
    /// WorldGenerator contains methods for generating an empty worldmatrix
    /// and populating the matrix 
    /// </summary>
    public class WorldGenerator
    {
        private string seed;
        public int[,] World { get; private set; }

        public WorldGenerator(string seed)
        {
            this.seed = seed;
        }

        internal int[,] GenerateWorld(int nLanes, int nRows)
        {
            World = new int[nLanes, nRows];

            PopulateWorld(World);

            return World;
        }

        private void PopulateWorld(int[,] world)
        {
            Random rnd = new Random(seed.GetHashCode());

            for(int i = 0; i < world.GetLength(0); i++)
            {
                for(int j = 0; j < world.GetLength(1); j++)
                {

                    if (j == world.GetLength(1) - 1)
                    {
                        world[i, j] = 2;
                    }
                    else
                    {
                        world[i, j] = rnd.Next(2);
                    }
                    if (j == 0)
                    {
                        world[i, j] = 1;
                    }
                }
                
            }

            // uggly, to check so no row is empty
            for (int row = 0; row < world.GetLength(1); row++)
            {
                if (world[0, row] == 0 && world[1, row] == 0 && world[2, row] == 0)
                    world[rnd.Next(2), row] = 1;
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


                Matrix translation = Matrix.CreateTranslation(transformComponent.Position);

                modelComponent.World = translation;
                //Console.WriteLine(modelComponent.World.Translation);
            }
        }
    }
}
