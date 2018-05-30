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
        

        public LightComponent(Entity id, Vector3 DiffuseLightDirection, Vector4 DiffuseColor, float DiffuseIntensity, Vector4 AmbientColor, float AmbientIntensity, Vector4 SpecularColor, float SpecularIntensity) : base(id)
        {
            this.DiffuseLightDirection = DiffuseLightDirection;
            this.DiffuseColor = DiffuseColor;
            this.DiffuseIntensity = DiffuseIntensity;
            this.AmbientColor = AmbientColor;
            this.AmbientIntensity = AmbientIntensity;
            this.SpecularColor = SpecularColor;
            this.SpecularIntensity = SpecularIntensity;

        }
    }
}
