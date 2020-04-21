using System.Drawing;

namespace cglr2
{
    // A single 2D object, that visualy represent a sword`s pommel
    class Pommel : PolygonalGraphicsObj
    {
        public static readonly PointF[] DefaultPommel = new PointF[]
        {
            new PointF(1f, 0),
            new PointF(2f, 0.5f),
            new PointF(1.5f, 1f),
            new PointF(0.5f, 1f),
            new PointF(0, 0.5f),
        };

        public static readonly PointF DefaultDockingPoint = new PointF(0, 0);
        public static readonly Color DefaultOutlineColor = Color.Black;
        public static readonly Color DefaultFillColor = Color.DarkGray;

        public Pommel() : base(DefaultPommel, DefaultDockingPoint, DefaultFillColor, DefaultOutlineColor)
        {
        }

        public Pommel(PointF[] outline, PointF dockingPoint, Color fillcolor, Color outlinecolor) : base(outline, dockingPoint, fillcolor, outlinecolor)
        {
        }
    }
}