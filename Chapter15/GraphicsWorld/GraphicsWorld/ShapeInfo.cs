using Microsoft.Xna.Framework;

namespace GraphicsWorld
{
    struct ShapeInfo
    {
        public Matrix World;
        public int ModelIndex;
        public BoundingSphere Sphere;
        public bool Collected;
    }


}
