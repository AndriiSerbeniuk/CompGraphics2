using System.Drawing;

namespace cglr2
{
    // A single 2D object, that visualy represent a sword hadle`s grip
    class Grip : PolygonalGraphicsObj
    {
        public static readonly PointF[] DefaultGrip = new PointF[]
        {
            new PointF(0, 0),
            new PointF(1f, 0),
            new PointF(1f, 3f),
            new PointF(0, 3f)
        };

        public static readonly PointF DefaultDockingPoint = new PointF(0.5f, 1f);
        public static readonly Color DefaultOutlineColor = Color.Black;
        public static readonly Color DefaultFillColor = Color.RosyBrown;

        public Grip() : base(DefaultGrip, DefaultDockingPoint, DefaultFillColor, DefaultOutlineColor)
        {
        }

        public Grip(PointF[] outline, PointF dockingPoint, Color fillcolor, Color outlinecolor) : base(outline, dockingPoint, fillcolor, outlinecolor)
        {
        }
    }
}