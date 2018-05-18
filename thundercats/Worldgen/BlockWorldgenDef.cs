using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using thundercats.GameStates;

namespace thundercats.Worldgen
{
    internal class BlockWorldgenDef : IWorldgenEntityDef
    {
        public int Weight { get; }
        public float SelectionValue { get; set; }

        public BlockWorldgenDef(int weight)
        {
            Weight = weight;
            SelectionValue = 0f;
        }

        public void RunWorldGenEntityCreator(GameManager gameManager, Vector3 position)
        {
            GameEntityFactory.NewBlock(position, AssetManager.Instance.CreateTexture(Color.BlueViolet, gameManager.game.GraphicsDevice));
        }
    }
}
