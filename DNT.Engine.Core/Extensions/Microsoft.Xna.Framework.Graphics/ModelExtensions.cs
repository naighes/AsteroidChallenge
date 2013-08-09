using System;
using System.Linq;

namespace Microsoft.Xna.Framework.Graphics
{
    public static class ModelExtensions
    {
        public static BoundingSphere GetBoundingSphere(this Model model)
        {
            return GetBoundingSphere(model, null);
        }

        public static BoundingSphere GetBoundingSphere(this Model model, Matrix[] transformations)
        {
            var result = new BoundingSphere();

            if (transformations.IsNull())
            {
                transformations = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(transformations);
            }

            foreach (var mesh in model.Meshes)
            {
                var additional = mesh.BoundingSphere.Transform(transformations[mesh.ParentBone.Index]);
                BoundingSphere.CreateMerged(ref result, ref additional, out result);
            }

            return result;
        }

        public static Single GetHighestMeshRadius(this Model model)
        {
            return model.Meshes.Max(m => m.BoundingSphere.Radius);
        }
    }
}