using System.Collections.Generic;
using System.Linq;
using DNT.Engine.Core.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace DNT.Engine.ContentPipeline
{
    [ContentProcessor(DisplayName = "Triangle model mesh processor")]
    public class TriangleModelMeshProcessor : ModelProcessor
    {
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            // I have a triangle array for each mesh.
            var trianglesPerGeometry = new List<Triangle[]>();
            AddModelMeshVertexArrayToList(input, trianglesPerGeometry);

            var model = base.Process(input, context);
            var i = 0;

            foreach (var mesh in model.Meshes)
            {
                var modelMeshTriangles = new List<Triangle>();

                // The point is that a mesh may be formed by more than one part.
#pragma warning disable 168
                foreach (var part in mesh.MeshParts)
#pragma warning restore 168
                    modelMeshTriangles.AddRange(trianglesPerGeometry[i++]);

                mesh.Tag = modelMeshTriangles.ToArray();
            }

            var triangles = trianglesPerGeometry.SelectMany(k => k);
            model.Tag = new ModelData(BoundingBox.CreateFromPoints(triangles.SelectMany(t => t.Points)),
                                      triangles.ToArray());
            return model;
        }

        private void AddModelMeshVertexArrayToList(NodeContent node, ICollection<Triangle[]> modelTriangles)
        {
            // Iterating through child nodes.
            foreach (var child in node.Children)
                AddModelMeshVertexArrayToList(child, modelTriangles);

            // Check if node is a mesh.
            var mesh = node as MeshContent;

            if (mesh == null)
                return;

            var triangles = new List<Triangle>();

            foreach (var geometry in mesh.Geometry)
            {
                // Calculating the number of triangles for current geometry.
                var count = geometry.Indices.Count / 3;

                var transform = Matrix.Identity;//mesh.Transform;
                // Add 3 vertices to the list for each triangle.
                for (var i = 0; i < count; i++)
                    triangles.Add(new Triangle(Vector3.Transform(geometry.Vertices.Positions[geometry.Indices[i * 3]], transform),
                                               Vector3.Transform(geometry.Vertices.Positions[geometry.Indices[i * 3 + 1]], transform),
                                               Vector3.Transform(geometry.Vertices.Positions[geometry.Indices[i * 3 + 2]], transform),
                                               mesh.Name));
            }

            // Add the mesh's triangles to the list.
            modelTriangles.Add(triangles.ToArray());
        }
    }
}