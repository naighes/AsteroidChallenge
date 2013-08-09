namespace DNT.Engine.Core.CollisionsDetection
{
    public interface ICollidable : IWorldObject
    {
        void CheckCollision(ICollidable target);
    }
}