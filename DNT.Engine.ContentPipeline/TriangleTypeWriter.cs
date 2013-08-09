using System;
using DNT.Engine.Core.Data;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace DNT.Engine.ContentPipeline
{
    [ContentTypeWriter]
    public class TriangleTypeWriter : ContentTypeWriter<Triangle>
    {
        public override String GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(TriangleTypeReader).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, Triangle value)
        {
            output.WriteObject(value.Point0);
            output.WriteObject(value.Point1);
            output.WriteObject(value.Point2);
            output.Write(value.MeshName);
        }
    }
}