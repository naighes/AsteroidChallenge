using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.Engine.Core.Graphics
{
    public class Material
    {
        internal Texture2D Texture;
        public Single SpecularPower;
        public Vector3 SpecularColor;
        public Vector3 EmissiveColor;
        public Vector3 DiffuseColor;
    }
}