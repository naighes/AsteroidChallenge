using System;
using DNT.Engine.Core.Validation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.Engine.Core.Graphics.Builtin
{
    public class SkyGlobe : PrimitiveComponent<VertexPositionNormalTexture>, IWorldObject
    {
        public SkyGlobe(Scene scene, String assetName, Single size)
            : base(scene)
        {
            Verify.That(assetName).Named("assetName").IsNotNull();
            _assetName = assetName;

            _size = size;
            _world = Matrix.Identity;
            _cubeFaces = new Texture2D[6];
        }

        private TextureCube _textureCube;
        private readonly String _assetName;
        private readonly Single _size;

        private readonly Vector3[] _normals = new[]
                                                  {
                                                      Vector3.Backward,
                                                      Vector3.Forward,
                                                      Vector3.Right,
                                                      Vector3.Left,
                                                      Vector3.Up,
                                                      Vector3.Down
                                                  };

        private readonly Vector2[] _uvCoordinates = new[]
                                                        {
                                                            Vector2.One, Vector2.UnitY, Vector2.Zero, Vector2.UnitX,
                                                            Vector2.Zero, Vector2.UnitX, Vector2.One, Vector2.UnitY,
                                                            Vector2.Zero, Vector2.UnitX, Vector2.One, Vector2.UnitY,
                                                            Vector2.Zero, Vector2.UnitX, Vector2.One, Vector2.UnitY,
                                                            Vector2.UnitY, Vector2.Zero, Vector2.UnitX, Vector2.One,
                                                            Vector2.UnitY, Vector2.Zero, Vector2.UnitX, Vector2.One
                                                        };

        private readonly Texture2D[] _cubeFaces;
        private BasicEffect _basicEffect;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _world = Matrix.CreateTranslation(Scene.CurrentCamera.Position);
        }

        protected override void OnBeginDraw()
        {
            base.OnBeginDraw();

            Scene.GraphicsDevice.DepthStencilState = new DepthStencilState { DepthBufferEnable = false, DepthBufferWriteEnable = false };
            Scene.GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            Scene.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicClamp;
        }

        protected override void Render(Matrix view, Matrix projection)
        {
            base.Render(view, projection);

            SetEffectMatrices(_basicEffect, view, projection, _world);

            for (var i = 0; i < _cubeFaces.Length; i++)
            {
                _basicEffect.Texture = _cubeFaces[i];

                foreach (var pass in _basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    Scene.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, CurrentVertexCount, i*6, 2);
                }
            }

            Scene.GraphicsDevice.ResetRenderStates();
        }

        protected override void InitializeData()
        {
            var uvIndex = 0;

            foreach (var normal in _normals)
            {
                var side1 = new Vector3(normal.Y, normal.Z, normal.X);
                var side2 = Vector3.Cross(normal, side1);

                AddIndex(CurrentVertexCount);
                AddIndex(CurrentVertexCount + 1);
                AddIndex(CurrentVertexCount + 2);

                AddIndex(CurrentVertexCount);
                AddIndex(CurrentVertexCount + 2);
                AddIndex(CurrentVertexCount + 3);

                AddVertex(new VertexPositionNormalTexture((normal - side1 - side2) * _size / 2.0f, normal, _uvCoordinates[uvIndex++]));
                AddVertex(new VertexPositionNormalTexture((normal - side1 + side2) * _size / 2.0f, normal, _uvCoordinates[uvIndex++]));
                AddVertex(new VertexPositionNormalTexture((normal + side1 + side2) * _size / 2.0f, normal, _uvCoordinates[uvIndex++]));
                AddVertex(new VertexPositionNormalTexture((normal + side1 - side2) * _size / 2.0f, normal, _uvCoordinates[uvIndex++]));
            }
        }

        public override void Load()
        {
            base.Load();

            _textureCube = Scene.Content.Load<TextureCube>(_assetName);
            _basicEffect = new BasicEffect(Scene.GraphicsDevice) {LightingEnabled = false, TextureEnabled = true};

            BuildEachFaceTexture();
        }

        private void BuildEachFaceTexture()
        {
            var pixelArray = new Color[_textureCube.Size * _textureCube.Size];

            for (var i = 0; i < _cubeFaces.Length; i++)
            {
                _cubeFaces[i] = new Texture2D(Scene.GraphicsDevice,
                                              _textureCube.Size,
                                              _textureCube.Size,
                                              false,
                                              SurfaceFormat.Color);
                _textureCube.GetData((CubeMapFace)i, pixelArray);
                _cubeFaces[i].SetData(pixelArray);
            }
        }

        public Matrix World
        {
            get { return _world; }
        }
        private Matrix _world;
    }
}