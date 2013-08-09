using System;
using System.Collections.Generic;
using System.Linq;
using DNT.Engine.Core.Data;
using DNT.Engine.Core.Validation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.Engine.Core.Graphics
{
    public abstract class SceneModel : DrawableComponent<Model>, IWorldObject, IBoundable
    {
        protected SceneModel(Scene scene, String assetName)
            : base(scene)
        {
            Verify.That(assetName).Named("assetName").IsNotNullOrWhiteSpace();
            _assetName = assetName;

            _materialsMap = new Dictionary<Int32, Material>();
            _parts = new Dictionary<Int32, Int32>();
            _textures = new Dictionary<Int32, Texture2D>();
        }

        protected Triangle[] Triangles
        {
            get
            {
                if (Content.Tag.IsNull())
                    return null;

                if (!(Content.Tag is ModelData))
                    return null;

                return ((ModelData)Content.Tag).Triangles;
            }
        }

        public Boolean HasAtLeastOneTriangleIntersectsBoundingSphere(BoundingSphere boundingSphere)
        {
            foreach (var mesh in Content.Meshes)
            {
                var meshTriangles = mesh.Tag as Triangle[];

                if (meshTriangles.IsNull())
                    return false;

                var world = GetWorldForMesh(mesh);

                // Check if a mesh triangle has one vertex inside bounding sphere.
                foreach (var triangle in meshTriangles)
                    if (triangle.HasAtLeastOnPointInsideBoundingSphere(boundingSphere, world))
                        return true;
            }

            return false;
        }

        private readonly String _assetName;

        protected Matrix[] ModelTransformations
        {
            get { return _modelTransformations; }
        }
        private Matrix[] _modelTransformations;

        protected override Model Content
        {
            get { return _content; }
        }
        private Model _content;

        private readonly IDictionary<Int32, Material> _materialsMap;

        public void SetMaterial(Int32 textureIndex, Material material)
        {
            if (_materialsMap.ContainsKey(textureIndex))
                _materialsMap[textureIndex] = material;
            else
                _materialsMap.Add(textureIndex, material);
        }

        protected override void Render(Matrix view, Matrix projection)
        {
            if (IsOutsideFrustum)
                return;

            var i = 0;

            foreach (var mesh in _content.Meshes)
            {
                var world = GetWorldForMesh(mesh);

                foreach (var part in mesh.MeshParts)
                {
                    var effect = GetCustomEffect() ?? part.Effect;
                    ApplyMaterial(effect, GetMaterial(i++));
                    SetupEffect(effect, view, projection, world);

                    RenderPart(part, effect);
                }
            }
        }

        private void RenderPart(ModelMeshPart part, Effect effect)
        {
            Scene.GraphicsDevice.SetVertexBuffer(part.VertexBuffer, part.VertexOffset);
            Scene.GraphicsDevice.Indices = part.IndexBuffer;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Scene.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList,
                                                           0,
                                                           0,
                                                           part.NumVertices,
                                                           part.StartIndex,
                                                           part.PrimitiveCount);
            }
        }

        protected Material GetMaterial(Int32 meshPartIndex)
        {
            if (!_parts.ContainsKey(meshPartIndex))
                return null;

            var textureIndex = _parts[meshPartIndex];

            if (!_textures.ContainsKey(textureIndex))
                return null;

            var material = _materialsMap.ContainsKey(textureIndex) ? _materialsMap[textureIndex] : null;

            if (material.IsNotNull())
                material.Texture = _textures[textureIndex];

            return material;
        }

        protected virtual void ApplyMaterial(Effect effect, Material material)
        {
            if (material.IsNull())
                return;

            Scene.EffectConfigurer.ApplyMaterial(effect, material);
        }

        protected Matrix GetWorldForMesh(ModelMesh mesh)
        {
            return ModelTransformations[mesh.ParentBone.Index] * World;
        }

        protected abstract Effect GetCustomEffect();

        protected virtual void SetupEffect(Effect effect, Matrix view, Matrix projection, Matrix world)
        {
            var e = effect as IEffectMatrices;

            if (effect.IsNotNull())
                SetEffectMatrices(e, view, projection, world);
        }

        protected Boolean IsOutsideFrustum
        {
            get { return Scene.CurrentCamera.BoundingFrustum.Contains(BoundingSphere) == ContainmentType.Disjoint; }
        }

        public virtual BoundingSphere BoundingSphere
        {
            get { return _content.GetBoundingSphere(_modelTransformations).Transform(World); }
        }

        public override void Load()
        {
            base.Load();

            _content = Scene.Content.Load<Model>(_assetName);

            if (_content.IsNull())
                throw new InvalidOperationException(String.Format("Cannot find a model for asset name '{0}'.",
                                                                  _assetName));

            _modelTransformations = new Matrix[_content.Bones.Count];
            _content.CopyAbsoluteBoneTransformsTo(_modelTransformations);
            ExtractTextures(_content);
        }

        private void ExtractTextures(Model model)
        {
            var i = 0;
            var k = 0;

            foreach (var mesh in model.Meshes)
                foreach (var part in mesh.MeshParts)
                {
                    Texture2D texture;
                    part.Effect.TryToExtractTexture(out texture);

                    if (texture.IsNotNull())
                    {
                        if (!_textures.Any(t => t.Value == texture))
                        {
                            _textures.Add(k, texture);
                            _parts.Add(i, k);
                            k++;
                        }
                        else
                            _parts.Add(i, _textures.Single(t => t.Value == texture).Key);

                        if (!_materialsMap.ContainsKey(i))
                            _materialsMap.Add(i, new Material());
                    }

                    i++;
                }
        }

        private readonly IDictionary<Int32, Int32> _parts;
        private readonly IDictionary<Int32, Texture2D> _textures;

        public abstract Matrix World { get; }
    }
}