using System.Collections.Generic;
using System.Drawing;

namespace cglr2
{
    //Represents a collection of 2D graphics objects.
    class PolygonalGrObjCollection : T2DGraphicsObject
    {
        private T2DGraphicsObject[] elements;
        
        //An array of internal objects, this object consists of.
        //An elements may be a single elements, or another collection of elements.
        public T2DGraphicsObject[] Elements
            {
            get
            {
                T2DGraphicsObject[] elementsCopy = new T2DGraphicsObject[elements.Length];
                elements.CopyTo(elementsCopy, 0);
                return elementsCopy;
            }
            set => elements = value;
            }

        public PolygonalGrObjCollection()
        {
            elements = null;
            FillColor = Color.White;
            OutlineColor = Color.Black;
            Enabled = true;
            DockingPoint = new PointF(0, 0);
        }

        public PolygonalGrObjCollection(T2DGraphicsObject[] objElements, PointF dockingPoint, Color fillcolor, Color outlinecolor)
        {
            elements = objElements;
            FillColor = fillcolor;
            OutlineColor = outlinecolor;
            Enabled = true;
            elementsCount = 1;
            DockingPoint = dockingPoint;
            Translate(DockingPoint.X, DockingPoint.Y);
        }

        public override T2DGraphicsObject Element(int index)
        {
            T2DGraphicsObject retVal = null;
            if (index >= 0 && index < elements.Length)
                retVal = elements[index];
            return retVal;
        }

        //Changes value of one of the internal elements.
        public void SetElement(int index, PolygonalGraphicsObj Value)
        {
            if (index >= 0 && index < elements.Length)
                elements[index] = Value;
        }

        //Removes one of the internal elements.
        public void RemoveElement(int index)
        {
            if (index >= 0 && index < elements.Length)
            {
                T2DGraphicsObject[] NewElements = new T2DGraphicsObject[elements.Length - 1];
                for (int i = 0, j = 0; i < NewElements.Length; i++)
                {
                    if (i != index)
                    {
                        NewElements[j] = elements[i];
                        j++;
                    }

                }
                elements = NewElements;
            }
        }

        public override List<PolygonalGraphicsObj> GetElements()
        {
            List<PolygonalGraphicsObj> Elems = new List<PolygonalGraphicsObj>(0);

            foreach (T2DGraphicsObject obj in elements)
                Elems.AddRange(obj.GetElements());

            return Elems;
        }

        public override List<PointF[]> GetElementsOutline()
        {
            List<PointF[]> ElementsList = new List<PointF[]>(0);

            for (int i = 0; i < elements.Length; i++)
            {
                ElementsList.AddRange(elements[i].GetElementsOutline());
            }

            return ElementsList;
        }

        public override List<Point[]> GetNormalisedElementsOutline(double xMin, double xMax, double yMin, double yMax, Size DrawWindowSize)
        {
            List<Point[]> NormalisedList = new List<Point[]>(0);

            for (int i = 0; i < elements.Length; i++)
            {
                NormalisedList.AddRange(elements[i].GetNormalisedElementsOutline(xMin, xMax, yMin, yMax, DrawWindowSize));
            }

            return NormalisedList;
        }

        public override void Rotate(double Angle, PointF RotationCentre)
        {
            if (Enabled)
            {
                for (int i = 0; i < elements.Length; i++)
                    elements[i].Rotate(Angle, RotationCentre);
            }
        }
        public override void Translate(double HorisontalTrans, double VerticalTrans) 
        {
            if (Enabled)
            {
                for (int i = 0; i < elements.Length; i++)
                    elements[i].Translate(HorisontalTrans, VerticalTrans);
            }
        }

        public override void Scale(double HorisontalScale, double VerticalScale, PointF ScalingCentre)
        {
            if (Enabled)
            {
                for (int i = 0; i < elements.Length; i++)
                    elements[i].Scale(HorisontalScale, VerticalScale, ScalingCentre);
            }
        }

        public override void Reflect(bool ReflectXAxis, bool ReflectYAxis, PointF RefletionCentre)
        {
            if (Enabled)
            {
                for (int i = 0; i < elements.Length; i++)
                    elements[i].Reflect(ReflectXAxis, ReflectYAxis, RefletionCentre);
            }
        }
        
        //Returns the minimum X value out of all the internal objects.
        public float GetLeftBorder()
        {
            PointF[][] Elems = GetElementsOutline().ToArray();

            float LeftBorder = Coordinates.getMinX(Elems[0]), minX;

            for (int i = 1; i < Elems.GetLength(0); i++)
            {
                minX = Coordinates.getMinX(Elems[i]);
                if (minX < LeftBorder)
                    LeftBorder = minX;
            }

            return LeftBorder;
        }

        //Returns the maximum X value out of all the internal objects.
        public float GetRightBorder()
        {
            PointF[][] Elems = GetElementsOutline().ToArray();

            float RightBorder = Coordinates.getMaxX(Elems[0]), maxX;

            for (int i = 1; i < Elems.GetLength(0); i++)
            {
                maxX = Coordinates.getMaxX(Elems[i]);
                if (maxX > RightBorder)
                    RightBorder = maxX;
            }

            return RightBorder;
        }

        //Returns the minimum Y value out of all the internal objects.
        public float GetBottomBorder()
        {
            PointF[][] Elems = GetElementsOutline().ToArray();

            float BottomBorder = Coordinates.getMinY(Elems[0]), minY;

            for (int i = 1; i < Elems.GetLength(0); i++)
            {
                minY = Coordinates.getMinY(Elems[i]);
                if (minY < BottomBorder)
                    BottomBorder = minY;
            }

            return BottomBorder;
        }

        //Returns the maximum Y value out of all the internal objects.
        public float GetTopBorder()
        {
            PointF[][] Elems = GetElementsOutline().ToArray();

            float TopBorder = Coordinates.getMaxY(Elems[0]), maxY;

            for (int i = 1; i < Elems.GetLength(0); i++)
            {
                maxY = Coordinates.getMaxY(Elems[i]);
                if (maxY > TopBorder)
                    TopBorder = maxY;
            }

            return TopBorder;
        }

        
    }
}
