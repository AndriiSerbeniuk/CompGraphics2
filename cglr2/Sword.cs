using System.Drawing;

namespace cglr2
{
    // A collection of 2D objects, that visualy represent a sword
    class Sword : PolygonalGrObjCollection
    {
        public static readonly T2DGraphicsObject[] DefaultSword = new T2DGraphicsObject[]
        {
            new Blade(),
            new Hilt()
        };

        public Sword() : base()
        {
            Elements = DefaultSword;
            Translate(DockingPoint.X, DockingPoint.Y);
        }

        public Sword(T2DGraphicsObject[] objElements, PointF dockingPoint, Color fillcolor, Color outlinecolor) : base(objElements, dockingPoint, fillcolor, outlinecolor)
        {
        }
    }
}