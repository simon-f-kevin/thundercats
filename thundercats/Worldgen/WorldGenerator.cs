using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thundercats.GameStates;
using thundercats.Service;
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
        private int[,] world;
        private GameManager gameManager;
        private Viewport viewport;

        private List<IWorldgenEntityDef> WorldgenEntities { get; set;}

        internal WorldGenerator(string seed, List<IWorldgenEntityDef> worldgenEntities, GameManager gameManager, Viewport viewport)
        {
            this.seed = seed;
            WorldgenEntities = worldgenEntities;
            this.gameManager = gameManager;
            this.viewport = viewport;
            SetSelectionValues();
        }

        /*
        * Creates a new world matrix and populates it with randomly selected worldgen entities
        */
        internal int[,] GenerateWorld(int nColumns, int nRows)
        {
            world = new int[nColumns, nRows];

            PopulateWorld(nColumns, nRows);
            GameService.Instance().GameWorld = world;

            return world;
        }

        /*
        * For all columns and rows, selects and creates worldgen entities for the world matrix
        */
        private void PopulateWorld(int nColumns, int nRows)
        {
            int distanceBetweenColumns = 100;
            int distanceBetweenRows = 50;
            Random rnd = new Random(seed.GetHashCode());
            int weightTotal = GetWeightTotal();
            WorldgenEntities = WorldgenEntities.OrderBy(o => o.SelectionValue).ToList();
            GameService.Instance().EntityGameWorld = new Entity[nColumns, nRows];

            for(int column = 0; column < nColumns; column++)
            {
                for(int row = 0; row < nRows; row++)
                {
                    //world[column, row] = rnd.Next(2); //TODO: Disable when new generation is complete
                    if(row == 0) // First row is always filled with blocks as the players start there
                    {
                        GameEntityFactory.NewBlock(new Vector3((column * distanceBetweenColumns), (0), (row * distanceBetweenRows)),
                            AssetManager.Instance.CreateTexture(Color.BlueViolet, gameManager.game.GraphicsDevice), GameEntityFactory.BLOCK);
                    }
                    else
                    {
                        CreateSelectedWorldEntity((float)rnd.NextDouble(), column, row, new Vector3((column * distanceBetweenColumns), (0), (row * distanceBetweenRows)));
                    }
                }
            }
        }

        /*
        * Creates a game entity at the specified position and matrix index. The entity is selected based on the selection value which picks an entity in a pre-sorted list
        */
        private void CreateSelectedWorldEntity(float selectedValue, int column, int row, Vector3 position)
        {
            for(int i = 0; i < WorldgenEntities.Count; i++)
            {
                if(selectedValue < WorldgenEntities[i].SelectionValue)
                {
                    GameService.Instance().EntityGameWorld[column, row] = WorldgenEntities[i].RunWorldGenEntityCreator(gameManager, position);
					world[column, row] = WorldgenEntities[i].Index;
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

        internal static List<IWorldgenEntityDef> GetWorldgenEntityDefs()
        {
            List<IWorldgenEntityDef> worldgenEntities = new List<IWorldgenEntityDef>(){
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
