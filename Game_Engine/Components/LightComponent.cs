using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class LightComponent : Component
    {
        public Vector3 LightDirection { get; set; }


        public Matrix LightViewProjection { get; set; } 
        public Vector4 AmbientColor { get; set; }
        public float AmbientIntensity { get; set; }
        public Vector3 DiffuseLightDirection { get; set; }
        public Vector4 DiffuseColor { get; set; }
        public float DiffuseIntensity { get; set; }
        public Vector4 SpecularColor { get; set; }
        public float SpecularIntensity { get; set; }
        

        public LightComponent(Entity id) : base(id){}
    }
}
