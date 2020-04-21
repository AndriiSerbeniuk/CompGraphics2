using System.Collections.Generic;
using System.Drawing;

namespace cglr2
{
    static class Coordinates
    {
        

        /*Evenly distributes X values on the X axis.
         * Input: "TakeValsFrom" - function points on the interval, "offset" - offset for markers distribution.
         * Output: double[] array with values of distributed X values.
         */
        public static float[] SpreadXMarkers(float xMin, float xMax, float Offset)
        {
            List<float> LMarkers = new List<float>();

            float curX;
            if (xMin < 0 && xMax > 0)   //when Y axis is visible
            {
                curX = -Offset;
                while (curX > xMin)
                {
                    LMarkers.Add(curX);
                    curX -= Offset;
                }
                curX = Offset;
                while (curX < xMax)
                {
                    LMarkers.Add(curX);
                    curX += Offset;
                }
            }
            else 
            {
                curX = xMin;
                while (curX<xMax)
                {
                    LMarkers.Add(curX);
                    curX += Offset;
                }
            }

            float[] Markers = LMarkers.ToArray();
           
            return Markers;
        }

        /*Evenly distributes Y values on the Y axis.
         * Input: "TakeValsFrom" - function points on the interval, "offset" - offset for markers distribution.
         * Output: double[] array with values of distributed Y values.
         */
        public static float[] SpreadYMarkers(float yMin, float yMax, float Offset)
        {
            List<float> LMarkers = new List<float>();

            float curY;
            if (yMin < 0 && yMax > 0)   //when Y axis is visible
            {
                curY = -Offset;
                while (curY > yMin)
                {
                    LMarkers.Add(curY);
                    curY -= Offset;
                }
                curY = Offset;
                while (curY < yMax)
                {
                    LMarkers.Add(curY);
                    curY += Offset;
                }
            }
            else
            {
                curY = yMin;
                while (curY < yMax)
                {
                    LMarkers.Add(curY);
                    curY += Offset;
                }
            }

            float[] Markers = LMarkers.ToArray();

            return Markers;
        }

        // Return minimum Y value in the "checkIn" array
        public static float getMinY(PointF[] checkIn)
        {
            float minY = checkIn[0].Y;

            for (int i = 0; i < checkIn.Length; i++) 
                if (minY > checkIn[i].Y)
                    minY = checkIn[i].Y;
            return minY;
        }

        // Return maximum Y value in the "checkIn" array
        public static float getMaxY(PointF[] checkIn)
        {
            float maxY = checkIn[0].Y;

            for (int i = 0; i < checkIn.Length; i++)
                if (maxY < checkIn[i].Y)
                    maxY = checkIn[i].Y;
            return maxY;
        }

        // Return minimum X value in the "checkIn" array
        public static float getMinX(PointF[] checkIn)
        {
            float minX = checkIn[0].X;

            for (int i = 0; i < checkIn.Length; i++)
                if (minX > checkIn[i].X)
                    minX = checkIn[i].X;
            return minX;
        }

        // Return maximum X value in the "checkIn" array
        public static float getMaxX(PointF[] checkIn)
        {
            float maxX = checkIn[0].X;

            for (int i = 0; i < checkIn.Length; i++)
                if (maxX < checkIn[i].X)
                    maxX = checkIn[i].X;
            return maxX;
        }
    }
}