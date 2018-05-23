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

        static int distanceBetweenColumns = -100;
        static int distanceBetweenRows = 50;

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
            GameService.Instance.GameWorld = world;

            return world;
        }

        /*
        * For all columns and rows, selects and creates worldgen entities for the world matrix
        */
        private void PopulateWorld(int nColumns, int nRows)
        {
            Random rnd;
            int weightTotal = GetWeightTotal();
            WorldgenEntities = WorldgenEntities.OrderBy(o => o.SelectionValue).ToList();
            GameService.Instance.EntityGameWorld = new Entity[nColumns, nRows];

            if(seed.Equals(""))
            {
                rnd = new Random();
            }
            else
            {
                rnd = new Random(seed.GetHashCode());
            }

            for(int column = 0; column < nColumns; column++)
            {
                for(int row = 0; row < nRows; row++)
                {
                    if(row == 0) // First row is always filled with blocks as the players start there
                    {
                        GameService.Instance.EntityGameWorld[column, row] = GameEntityFactory.NewBlock(new Vector3((column * distanceBetweenColumns), (0), (row * distanceBetweenRows)),
                            AssetManager.Instance.CreateTexture(Color.BlueViolet, gameManager.game.GraphicsDevice), GameEntityFactory.BLOCK);
                        world[column, row] = 1;
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
                    GameService.Instance.EntityGameWorld[column, row] = WorldgenEntities[i].RunWorldGenEntityCreator(gameManager, position);
                    world[column, row] = WorldgenEntities[i].Index;

                    if((WorldgenEntities[i].Index == -1) && (column == world.GetLength(0) - 1) && (IsRowTooOpen(row)))
                    {
                        FillOpenRow(column, row);
                        break;
                    }
                    break;
                }
            }
        }

        /*
         * Openess constraint which prevents any row from being completely void.
         * More advanced constraints may be developed later to increase difficulty.
         */
        private bool IsRowTooOpen(int row)
        {
            int openness = 0;

            for(int column = 0; column < world.GetLength(0); column++)
            {
                if(world[column, row] == -1)
                {
                    openness++;
                }
            }
            if(openness >= world.GetLength(0))
            {
                return true;
            }
            return false;
        }

        /* Turns a random tile on the row into a block to prevent large gaps in the level. */
        private void FillOpenRow(int currentColumn, int row)
        {
            Random rnd;

            if(seed.Equals(""))
            {
                rnd = new Random();
            }
            else
            {
                rnd = new Random(seed.GetHashCode());
            }

            int selectedColumn = rnd.Next(0, world.GetLength(0));
            GameService.Instance.EntityGameWorld[selectedColumn, row] = GameEntityFactory.NewBlock(new Vector3((selectedColumn * distanceBetweenColumns), (0), (row * distanceBetweenRows)),
                AssetManager.Instance.CreateTexture(Color.BlueViolet, gameManager.game.GraphicsDevice), GameEntityFactory.BLOCK);
            world[selectedColumn, row] = 1;
        }

        /* Returns the total weight of all Worldgen entity types. */
        private int GetWeightTotal()
        {
            int weightTotal = 0;

            for(int i = 0; i < WorldgenEntities.Count; i++)
            {
                weightTotal += WorldgenEntities[i].Weight;
            }
            return weightTotal;
        }

        /* 
         * Assigns the selection value (the normalized weight) of all worldgen entities
         * based on their weight modified by total weight and offset by the current selection value total.
         */
        private void SetSelectionValues()
        {
            int weightTotal = GetWeightTotal();
            float currentSelectionValueTotal = 0;

            for(int i = 0; i < WorldgenEntities.Count; i++)
            {
                WorldgenEntities[i].SelectionValue = (float)WorldgenEntities[i].Weight / weightTotal + currentSelectionValueTotal;
                currentSelectionValueTotal += WorldgenEntities[i].SelectionValue;
            }
        }

        internal static List<IWorldgenEntityDef> GetWorldgenEntityDefs()
        {
            List<IWorldgenEntityDef> worldgenEntities = new List<IWorldgenEntityDef>(){
                new BlockWorldgenDef(1),
                new VoidWorldgenDef(5)
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
