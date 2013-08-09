using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace DNT.Engine.Core.Data
{
    public class ModelDataTypeReader : ContentTypeReader<ModelData>
    {
        protected override ModelData Read(ContentReader input, ModelData existingInstance)
        {
            return new ModelData(input.ReadObject<BoundingBox>(),
                                 input.ReadObject<Triangle[]>());
        }
    }
}