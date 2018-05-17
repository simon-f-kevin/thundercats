using Game_Engine.Components;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thundercats.Worldgen;

namespace thundercats.Factory
{
    /// <summary>
    /// WorldGenerator contains methods for generating an empty worldmatrix
    /// and populating the matrix 
    /// </summary>
    internal class WorldGenerator
    {
        private string seed;
        private float[,] world;
        private List<WorldgenEntityDef> WorldgenEntities { get; set;}

        internal WorldGenerator(string seed, List<WorldgenEntityDef> worldgenEntities)
        {
            this.seed = seed;
            WorldgenEntities = worldgenEntities;
            SetSelectionValues();
        }

        internal float[,] GenerateWorld(int nLanes, int nRows)
        {
            world = new float[nLanes, nRows];

            PopulateWorld(world);

            return world;
        }

        private void PopulateWorld(float[,] world)
        {
            Random rnd = new Random(seed.GetHashCode());
            int weightTotal = GetWeightTotal();
            WorldgenEntities = WorldgenEntities.OrderBy(o => o.SelectionValue).ToList();

            for(int i = 0; i < world.GetLength(0); i++)
            {
                for(int j = 0; j < world.GetLength(1); j++)
                {
                    world[i, j] = rnd.Next(2); //TODO: Disable when new generation is complete
                    CreateSelectedWorldEntity((float)rnd.NextDouble());
                }
            }
        }

        private void CreateSelectedWorldEntity(float selectedValue)
        {
            for(int i = 0; i < WorldgenEntities.Count; i++)
            {
                if(selectedValue < WorldgenEntities[i].SelectionValue)
                {
                    Console.WriteLine(selectedValue + ", " + WorldgenEntities[i].GetType());
                    WorldgenEntities[i].RunWorldGenEntityCreator();
                    break;
                }
            }
        }

        private int GetWeightTotal()
        {
            int weightTotal = 0;

            for(int i = 0; i < WorldgenEntities.Count; i++)
            {
                weightTotal += WorldgenEntities[i].Weight;
            }
            return weightTotal;
        }

        private void SetSelectionValues()
        {
            int weightTotal = GetWeightTotal();
            float prevSelectionValue = 0;

            for(int i = 0; i < WorldgenEntities.Count; i++)
            {
                WorldgenEntities[i].SelectionValue = (float)WorldgenEntities[i].Weight / weightTotal + prevSelectionValue;
                prevSelectionValue += WorldgenEntities[i].SelectionValue;
            }
        }

        internal static List<WorldgenEntityDef> GetWorldgenEntityDefs()
        {
            List<WorldgenEntityDef> worldgenEntities = new List<WorldgenEntityDef>(){
                new BlockWorldgenDef(3),
                new VoidWorldgenDef(2)
            };
            return worldgenEntities;
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
            }
        }
    }
}
