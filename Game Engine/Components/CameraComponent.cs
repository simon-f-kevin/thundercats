using Game_Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Game_Engine.Components
{
   public class CameraComponent : Component
    {
        //Matrix needed is
        //world
        //view
        //projection
        CameraComponent(Entity id) : base(id)
        {

        }
    }
}
