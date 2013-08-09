using DNT.Engine.Core.Cameras.Builtin;

namespace DNT.Engine.Core.Cameras
{
    public static class Camera
    {
        public static CameraConfig Default()
        {
            return new CameraConfig();
        }

        public static ChasingCameraConfig AttachedTo(IWorldObject chasedObject)
        {
            return new ChasingCameraConfig(chasedObject);
        }
    }
}