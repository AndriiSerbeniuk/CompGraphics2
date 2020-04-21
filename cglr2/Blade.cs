using System.Drawing;

namespace cglr2
{
    class Blade : PolygonalGraphicsObj
    {
        public static readonly PointF[] DefaultBlade = new PointF[]
        {
            new PointF(0, 0),
            new PointF(1.5f, 0),
            new PointF(1.5f, 1.5f),
            new PointF(1.25f, 12.25f),
            new PointF(0.75f, 13.5f),
            new PointF(0.25f, 12.25f),
            new PointF(0, 1.5f)
        };

        public static readonly PointF DefaultDockingPoint = new PointF(0.25f, 4.5f);
        public static readonly Color DefaultOutlineColor = Color.Black;
        public static readonly Color DefaultFillColor = Color.LightGray;

        public Blade() : base(DefaultBlade, DefaultDockingPoint, DefaultFillColor, DefaultOutlineColor)
        {
        }

        public Blade(PointF[] outline, PointF dockingPoint, Color fillcolor, Color outlinecolor) : base(outline, dockingPoint, fillcolor, outlinecolor)
        {
        }

    }
}