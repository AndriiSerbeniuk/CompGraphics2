using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace cglr2
{
    //Represents a base class for a 2D graphics object.
    abstract class T2DGraphicsObject
    {
        private Color fillColor;    //Color, used for the inside of the object
        private Color outlineColor; //Color, used for the outside of the object
        private bool enabled;       //Represents whether the object should be transformed or not
        protected int elementsCount;//The quantity of stored inside elements
        private PointF dockingPoint;//The point, that object should be docked to

        public Color FillColor { get => fillColor; set => fillColor = value; }
        public Color OutlineColor { get => outlineColor; set => outlineColor = value; }
        public bool Enabled { get => enabled; set => enabled = value; }
        public int ElementsCount { get => elementsCount; }

        public PointF DockingPoint { get => dockingPoint; set => dockingPoint = value; }

        //Returns stored in the object elements.
        //Input: index of the desired element.
        public abstract T2DGraphicsObject Element(int index);

        //Returns a list of normalised outlines of the elements of the object
        public abstract List<Point[]> GetNormalisedElementsOutline(double xMin, double xMax, double yMin, double yMax, Size DrawWindowSize);

        //Returns a list of outlines of the elements of the object
        public abstract List<PointF[]> GetElementsOutline();

        //Returns a list of copies of the object`s elements
        public abstract List<PolygonalGraphicsObj> GetElements();

        //Rotates the object by the "Angle" angle (in degrees) around the "RotationCentre" point.
        public abstract void Rotate(double Angle, PointF RotationCentre);

        //Translates the object by "HorisontalTrans" horizontaly and "VerticalTrans" verticaly.
        //Positive values translate roward, negative - backwards.
        public abstract void Translate(double HorisontalTrans, double VerticalTrans);

        //Scale the object relative to the "ScalingCentre" point by "HorisontalScale" horizontaly and "VerticalScale" verticaly.
        public abstract void Scale(double HorisontalScale, double VerticalScale, PointF ScalingCentre);

        //Reflects the object relative to the "RefletionCentre" point.
        //"ReflectXAxis" shows if the object should be reflected around the X axis, "ReflectYAxis" - around the Y axis.
        public abstract void Reflect(bool ReflectXAxis, bool ReflectYAxis, PointF RefletionCentre);
    }
}
