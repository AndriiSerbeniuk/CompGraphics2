using System.Drawing;

namespace cglr2
{
    // A single 2D object, that visualy represent a sword`s guard
    class Guard : PolygonalGraphicsObj
    {
        public static readonly PointF[] DefaultGuard = new PointF[]
        {
            new PointF(0, 0.25f),
            new PointF(2f, 0),
            new PointF(3f, 0),
            new PointF(5f, 0.25f),
            new PointF(5f, 0.5f),
            new PointF(0, 0.5f)
        };

        public static readonly PointF DefaultDockingPoint = new PointF(-1.5f, 4f);
        public static readonly Color DefaultOutlineColor = Color.Black;
        public static readonly Color DefaultFillColor = Color.DarkGray;

        public Guard() : base(DefaultGuard, DefaultDockingPoint, DefaultFillColor, DefaultOutlineColor)
        {
        }

        public Guard(PointF[] outline, PointF dockingPoint, Color fillcolor, Color outlinecolor) : base(outline, dockingPoint, fillcolor, outlinecolor)
        {
        }
    }
}