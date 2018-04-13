using Game_Engine.Entities;
using Microsoft.Xna.Framework;
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


        public Vector3 position;
        public Vector3 target;
        /*Properties*/
        public Matrix WorldMatrix { get; set; }
        public Matrix ViewMatrix { get; set; }
        public Matrix ProjectionMatrix { get; set; }
        public float FieldOfView { get; set; }
        public float AspectRatio { get; set; }
        public CameraComponent(Entity id) : base(id)
        {
            WorldMatrix = new Matrix();
            ViewMatrix = new Matrix();
            ProjectionMatrix = new Matrix();
        }
    }
}
