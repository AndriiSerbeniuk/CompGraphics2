using System.Collections.Generic;
using System.Drawing;

namespace cglr2
{
    //Represents a single 2D graphics object.
    class PolygonalGraphicsObj : T2DGraphicsObject
    {
        private PointF[] ObjectOutline;

        //An array of points, that, when all connected, represent the object`s outline
        public PointF[] Outline 
        { 
            get
            {
                PointF[] outlineCopy = new PointF[ObjectOutline.Length];
                ObjectOutline.CopyTo(outlineCopy, 0);
                return outlineCopy;
            }
            set => ObjectOutline = value;
        }


        public PolygonalGraphicsObj()
        {
            ObjectOutline = null;
            FillColor = Color.White;
            OutlineColor = Color.Black;
            Enabled = true;
            elementsCount = 1;  //Since this class represents a single graphics object - element count will always be 1.
            DockingPoint = new PointF(0, 0);
        }

        public PolygonalGraphicsObj(PointF[] outline, PointF dockingPoint, Color fillcolor, Color outlinecolor)
        {
            ObjectOutline = outline;
            FillColor = fillcolor;
            OutlineColor = outlinecolor;
            Enabled = true;
            elementsCount = 1;
            DockingPoint = dockingPoint;
            Translate(DockingPoint.X, DockingPoint.Y);
        }

        public override T2DGraphicsObject Element(int index)
        {
            return this;
        }

        public override List<PolygonalGraphicsObj> GetElements()
        {
            List<PolygonalGraphicsObj> Elem = new List<PolygonalGraphicsObj>(0);
            PolygonalGraphicsObj Copy = new PolygonalGraphicsObj(Outline, DockingPoint, FillColor, OutlineColor);
            Elem.Add(Copy);

            return Elem;
        }

        public override List<PointF[]> GetElementsOutline()
        {
            PointF[] outlineCopy = new PointF[ObjectOutline.Length];
            ObjectOutline.CopyTo(outlineCopy, 0);
            List<PointF[]> ElementsList = new List<PointF[]>(0);
            ElementsList.Add(outlineCopy);
            return ElementsList;
        }

        public override List<Point[]> GetNormalisedElementsOutline(double xMin, double xMax, double yMin, double yMax, Size DrawWindowSize)
        {
            List<Point[]> NormalisedList = new List<Point[]>(0);
            Point[] NormalisedPoints = new Point[ObjectOutline.Length];

            for (int i = 0; i < ObjectOutline.Length; i++)
            {
                NormalisedPoints[i].X = Normalisation.NormaliseX(ObjectOutline[i].X, xMin, xMax, DrawWindowSize.Width);
                NormalisedPoints[i].Y = Normalisation.NormaliseY(ObjectOutline[i].Y, yMin, yMax, DrawWindowSize.Height);
            }

            NormalisedList.Add(NormalisedPoints);

            return NormalisedList;
        }

        //Set value of existing outline point by the number of the point in the drawing sequence.
        //Returns true if the operation was successful, false if otherwise.
        public bool SetOutlineCoordinate(int drawsequencenumber, Point newvalue)
        {
            bool successful = false;
            if (drawsequencenumber >= 0 && drawsequencenumber < ObjectOutline.Length)
            {
                ObjectOutline[drawsequencenumber] = newvalue;
                successful = true;
            }
            return successful;
        }

        //Sets new value for all points with "oldvalue" value
        public void SetOutlineCoordinate(Point oldvalue, Point newvalue)
        {
            for (int i = 0; i < ObjectOutline.Length; i++)
                if (ObjectOutline[i] == oldvalue)
                    ObjectOutline[i] = newvalue;
        }

        //Adds a new point to the outline.
        //Input: drawsequencenumber - index of the new point;
        //Value - value of the point.
        public void AddOutlinePoint(int drawsequencenumber, Point Value)
        {
            if (drawsequencenumber < 0)
                drawsequencenumber = 0;
            else if (drawsequencenumber >= ObjectOutline.Length)
                drawsequencenumber = ObjectOutline.Length;

            PointF[] NewOutline = new PointF[ObjectOutline.Length + 1];
            for (int i = 0, j = 0; i < NewOutline.Length; i++)
            {
                if (i == drawsequencenumber)
                    NewOutline[i] = Value;
                else
                {
                    NewOutline[i] = ObjectOutline[j];
                    j++;
                }
                        
            }
            ObjectOutline = NewOutline;
        }

        public void RemoveOutlinePoint(int drawsequencenumber)
        {
            if (drawsequencenumber >=0 && drawsequencenumber < ObjectOutline.Length)
            {
                PointF[] NewOutline = new PointF[ObjectOutline.Length - 1];
                for (int i = 0, j = 0; i < NewOutline.Length; i++)
                {
                    if (i != drawsequencenumber)
                    {
                        NewOutline[j] = ObjectOutline[i];
                        j++;
                    }

                }
                ObjectOutline = NewOutline;
            }
        }

        public override void Rotate(double Angle, PointF RotationCentre)
        {
            if (Enabled)
            {
                ObjectOutline = Affinity.Rotate(ObjectOutline, Angle, RotationCentre);
            }
        }
        public override void Translate(double HorisontalTrans, double VerticalTrans)
        {
            if (Enabled)
                ObjectOutline = Affinity.Translate(ObjectOutline, HorisontalTrans, VerticalTrans);
        }

        public override void Scale(double HorisontalScale, double VerticalScale, PointF ScalingCentre)
        {
            if (Enabled)
                ObjectOutline = Affinity.Scale(ObjectOutline, HorisontalScale, VerticalScale, ScalingCentre);
        }

        public override void Reflect(bool ReflectXAxis, bool ReflectYAxis, PointF RefletionCentre)
        {
            if (Enabled)
                ObjectOutline = Affinity.Reflect(ObjectOutline, ReflectXAxis, ReflectYAxis, RefletionCentre);
        }
    }
}
