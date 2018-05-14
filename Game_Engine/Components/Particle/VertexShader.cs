using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components.Particle
{
    public class VertexShader
    {
        Vector4 Position;
        Vector2 TexCoord;
        // For shading
        Vector3 WorldNormal;
        Vector3 WorldPosition;
  
        Vector2 TextCoord;
        Vector3 Velocity;
        float Time;
       

     
        public void VertexShaderFunction(VertexShaderInput input)
        {
            VertexShaderOutput output;

            var worldPosition = mul(input.P)

            
            return output;
            
        }
    }
}
