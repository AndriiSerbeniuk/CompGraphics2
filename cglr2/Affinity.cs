using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace cglr2
{
    // A library of methods, utilised in afine transformations of a graphics object.
    // An object here is represented as an array of points, that form the object`s outline.
    static class Affinity
    {
        //Returns a TMatrix for rotaion of an object by the "angle" angle
        public static TMatrix GetRotationMatrix(double angle)
        {
            double AngleInRadian = angle * Math.PI / 180;
            float AngCos = (float)Math.Cos(AngleInRadian);
            float AngSin = (float)Math.Sin(AngleInRadian);
            return new TMatrix(new float[,]
            {
                {AngCos,    -AngSin,    0f },
                {AngSin,    AngCos,     0f},
                {0f,        0f,         1f }
            }
            );
        }

        //Returns a TMatrix for translation of an object.
        //Input: double HorizontalTrans - horizontal translation distance;
        //double VerticalTrans - vertical translation distance.
        public static TMatrix GetTranslationMatrix(double HorizontalTrans, double VerticalTrans)
        {
            return new TMatrix(new float[,]
            {
                {1f,    0f,    (float)HorizontalTrans },
                {0f,    1f,    (float)VerticalTrans },
                {0f,    0f,    1f }
            }
            );
        }

        //Returns a TMatrix for scaling of an object relative to the (0;0) point.
        //Input: double HorisontalScaling - horizontal scaling value;
        //double VerticalTrans - vertical scaling value.
        public static TMatrix GetScalingMatrix(double HorisontalScaling, double VerticalScaling)
        {
            return new TMatrix(new float[,]
            {
                {(float)HorisontalScaling,    0f,    0f },
                {0f,    (float)VerticalScaling,    0f },
                {0f,    0f,    1f }
            }
            );
        }

        //Returns a 3 rows by 1 column TMatrix.
        //Output matrix values: 
        //[0][0] - X coordinate;
        //[0][1] - Y coordinate;
        //[0][3] - a value of 1, required solely for the calculations.
        public static TMatrix FormCoordsMatrix(float x, float y)
        {
            return new TMatrix(new float[,] { { x }, { y }, { 1 } });
        }

        /* Rotates the object.
         * Input:
         * PointF[] obj - the object`s outline, represented as an array of points;
         * double angle - the rotation angle in degrees;
         * PointF RotationCentre - a point the rotation will be performed around.
         * Output: a PointF array that contains the rotated object`s outline.
         */
        public static PointF[] Rotate(PointF[] obj, double angle, PointF RotationCentre)
        {
            PointF[] RotatedObj = new PointF[obj.Length];
            obj.CopyTo(RotatedObj, 0);

            RotatedObj = Translate(RotatedObj, -RotationCentre.X, -RotationCentre.Y);

            TMatrix CurPoint;
            for (int i = 0; i < RotatedObj.Length; i++) 
            {
                CurPoint = FormCoordsMatrix(RotatedObj[i].X, RotatedObj[i].Y);
                CurPoint = GetRotationMatrix(angle).Multiply(CurPoint);
                RotatedObj[i].X = CurPoint.GetValue(0, 0);
                RotatedObj[i].Y = CurPoint.GetValue(1, 0);
            }

            RotatedObj = Translate(RotatedObj, RotationCentre.X, RotationCentre.Y);
            return RotatedObj;
        }

        /// <summary>
        /// Translates a 2D object, represented as an array of points that form it`s outline.
        /// </summary>
        /// <param name="obj"> The object to transform represented as an array of points that form it`s outline. </param>
        /// <param name="HorisontalTrans"> The horizontal translation distance. </param>
        /// <param name="VerticalTrans">  The vertical translation distance.</param>
        /// <returns> A PointF[] with outline points of the transformed copy of the object. </returns>
        public static PointF[] Translate(PointF[] obj, double HorisontalTrans, double VerticalTrans)
        {
            PointF[] TranslatedtedObj = new PointF[obj.Length];
            TMatrix CurPoint;
            for (int i = 0; i < TranslatedtedObj.Length; i++)
            {
                CurPoint = FormCoordsMatrix(obj[i].X, obj[i].Y);
                CurPoint = GetTranslationMatrix(HorisontalTrans, VerticalTrans).Multiply(CurPoint);
                TranslatedtedObj[i].X = CurPoint.GetValue(0, 0);
                TranslatedtedObj[i].Y = CurPoint.GetValue(1, 0);
            }

            return TranslatedtedObj;
        }

        /// <summary>
        /// Scales a 2D object relative to a point. 
        /// The object is represented as an array of points that form it`s outline.
        /// </summary>
        /// <param name="obj"> The object to transform, represented as an array of points that form it`s outline. </param>
        /// <param name="HorisontalScaling"> Value of horizontal scaling. </param>
        /// <param name="VerticalScaling"> Value of vertical scaling. </param>
        /// <param name="ScalingCentre"> The point that scaling is relative to. </param>
        /// <returns> A PointF[] with outline points of the transformed copy of the object. </returns>
        public static PointF[] Scale(PointF[] obj, double HorisontalScaling, double VerticalScaling, PointF ScalingCentre)
        {
            PointF[] ScaledtedObj = new PointF[obj.Length];
            obj.CopyTo(ScaledtedObj, 0);

            ScaledtedObj = Translate(ScaledtedObj, -ScalingCentre.X, -ScalingCentre.Y);

            TMatrix CurPoint;
            for (int i = 0; i < ScaledtedObj.Length; i++)
            {
                CurPoint = FormCoordsMatrix(ScaledtedObj[i].X, ScaledtedObj[i].Y);
                CurPoint = GetScalingMatrix(HorisontalScaling, VerticalScaling).Multiply(CurPoint);
                ScaledtedObj[i].X = CurPoint.GetValue(0, 0);
                ScaledtedObj[i].Y = CurPoint.GetValue(1, 0);
            }

            ScaledtedObj = Translate(ScaledtedObj, ScalingCentre.X, ScalingCentre.Y);

            return ScaledtedObj;
        }

        /// <summary>
        /// Reflects a 2D object relative to a point.
        /// The object is represented as an array of points that form it`s outline.
        /// </summary>
        /// <param name="obj"> The object to transform, represented as an array of points that form it`s outline. </param>
        /// <param name="ReflectXAxis"> A flag, that tells whether the object should be reflected around the X axis. </param>
        /// <param name="ReflectYAxis"> A flag, that tells whether the object should be reflected around the Y axis. </param>
        /// <param name="ReflectionCentre"> A point that the object should be reflected relative to. </param>
        /// <returns> A PointF[] with outline points of the transformed copy of the object. </returns>
        public static PointF[] Reflect(PointF[] obj, bool ReflectXAxis, bool ReflectYAxis, PointF ReflectionCentre)
        {
            PointF[] ReflectedObj = new PointF[obj.Length];
            obj.CopyTo(ReflectedObj, 0);

            ReflectedObj = Translate(ReflectedObj, -ReflectionCentre.X, -ReflectionCentre.Y);

            int xReflection = (ReflectXAxis) ? -1 : 1;
            int YReflection = (ReflectYAxis) ? -1 : 1;

            TMatrix CurPoint;
            for (int i = 0; i < ReflectedObj.Length; i++)
            {
                CurPoint = FormCoordsMatrix(ReflectedObj[i].X, ReflectedObj[i].Y);
                CurPoint = GetScalingMatrix(xReflection, YReflection).Multiply(CurPoint);
                ReflectedObj[i].X = CurPoint.GetValue(0, 0);
                ReflectedObj[i].Y = CurPoint.GetValue(1, 0);
            }

            ReflectedObj = Translate(ReflectedObj, ReflectionCentre.X, ReflectionCentre.Y);

            return ReflectedObj;
        }
    }
}
