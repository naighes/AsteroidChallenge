using Microsoft.Xna.Framework;

namespace DNT.Engine.Core.Data
{
    public class ModelData
    {
        public ModelData(BoundingBox boundingBox, Triangle[] triangles)
        {
            _triangles = triangles;
            _boundingBox = boundingBox;
        }

        public BoundingBox BoundingBox
        {
            get { return _boundingBox; }
        }
        private readonly BoundingBox _boundingBox;

        public Triangle[] Triangles
        {
            get { return _triangles; }
        }
        private readonly Triangle[] _triangles;
    }
}