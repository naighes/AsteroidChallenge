using System;
using DNT.Engine.Core.Data;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace DNT.Engine.ContentPipeline
{
    [ContentTypeWriter]
    public class ModelDataTypeWriter : ContentTypeWriter<ModelData>
    {
        public override String GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(ModelDataTypeReader).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, ModelData value)
        {
            output.WriteObject(value.BoundingBox);
            output.WriteObject(value.Triangles);
        }
    }
}