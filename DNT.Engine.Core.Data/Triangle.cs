using Microsoft.Xna.Framework;
using System;

namespace DNT.Engine.Core.Data
{
    public class Triangle
    {
        public Triangle(Vector3 p0, Vector3 p1, Vector3 p2, String meshName)
        {
            _points = new Vector3[3];
            _points[0] = p0;
            _points[1] = p1;
            _points[2] = p2;
            _meshName = meshName;
        }

        public Vector3[] Points
        {
            get { return _points; }
        }
        private readonly Vector3[] _points;

        public Vector3 Point0
        {
            get { return _points[0]; }
        }

        public Vector3 Point1
        {
            get { return _points[1]; }
        }

        public Vector3 Point2
        {
            get { return _points[2]; }
        }

        public String MeshName
        {
            get { return _meshName; }
        }
        private readonly String _meshName;
    }
}