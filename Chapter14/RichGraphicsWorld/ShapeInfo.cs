using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RichGraphicsWorld
{
    struct ShapeInfo
    {
        public Matrix World;
        public int ModelIndex;
        public BoundingSphere Sphere;
        public bool Collected;
    }
}
