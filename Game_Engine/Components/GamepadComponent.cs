using Game_Engine.Entities;

namespace Game_Engine.Components
{
    public class GamePadComponent : Component
    {
        public int Index { get; set;}

        public GamePadComponent(Entity id, int index) : base(id)
        {
            Index = index;
        }
    }
}
