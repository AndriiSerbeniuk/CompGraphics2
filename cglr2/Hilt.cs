using System.Drawing;

namespace cglr2
{
    // A collection of 2D objects, that visualy represent a sword hilt
    class Hilt : PolygonalGrObjCollection
    {
        public static readonly PolygonalGraphicsObj[] DefaultHilt = new PolygonalGraphicsObj[]
        {
            new Guard(),
            new Grip(),
            new Pommel()
        };

        public Hilt() : base()
        {
            Elements = DefaultHilt;
            Translate(DockingPoint.X, DockingPoint.Y);
        }

        public Hilt(T2DGraphicsObject[] objElements, PointF dockingPoint, Color fillcolor, Color outlinecolor) : base(objElements, dockingPoint, fillcolor, outlinecolor)
        {
        }

    }
}