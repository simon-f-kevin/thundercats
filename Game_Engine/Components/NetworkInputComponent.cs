using Game_Engine.Components;
using Game_Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class NetworkInputComponent : Component
    {
        public bool MoveLeft { get; set; } = false;
        public bool MoveRight { get; set; } = false;
        public bool Jump { get; set; } = false;
        public bool MoveForward { get; set; } = false;

        public NetworkInputComponent(Entity id) : base(id)
        {
        }

        public void Reset()
        {
            MoveLeft = false;
            MoveRight = false;
            Jump = false;
            MoveForward = false;
        }
    }
}
