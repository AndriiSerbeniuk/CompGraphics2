namespace cglr2
{
    static class Normalisation
    {
        //Normalises an array of X values on the "x1Val" - "x2Val" interval.
        public static int[] NormaliseXArray(float[] Array, double x1Val, double x2Val, int DrawWindowWidth)
        {
            int[] NormalisedArray = new int[Array.Length];

            for (int i = 0; i < Array.Length; i++)
            {
                NormalisedArray[i] = NormaliseX(Array[i], x1Val, x2Val, DrawWindowWidth);
            }

            return NormalisedArray;
        }

        //Normalises an array of Y values on the "yMinVal" - "yMaxVal" interval.
        public static int[] NormaliseYArray(float[] Array, double yMinVal, double yMaxVal, int DrawWindowHeight)
        {
            int[] NormalisedArray = new int[Array.Length];

            for (int i = 0; i < Array.Length; i++)
            {
                NormalisedArray[i] = NormaliseY(Array[i], yMinVal, yMaxVal, DrawWindowHeight);
            }

            return NormalisedArray;
        }

        //Normalises X point on the "x1Val" - "x2Val" interval.
        public static int NormaliseX(double xVal, double x1Val, double x2Val, int DrawWindowWidth)
        {
            return (x1Val == x2Val) ? -1 : (int)(DrawWindowWidth * (xVal - x1Val) / (x2Val - x1Val));
        }

        //Normalises Y point on the "yMinVal" - "yMaxVal" interval.
        public static int NormaliseY(double yVal, double yMinVal, double yMaxVal, int DrawWindowHeight)
        {
            return (yMaxVal == yMinVal) ? -1 : (int)(DrawWindowHeight * (yMaxVal - yVal) / (yMaxVal - yMinVal));
        }

        //Calculates real X coordinate based on screen dimentions and screen coordinate
        public static double DenormaliseX(int ScreenXCoord, int ScreenWidth, double LeftBorder, double RightBorder)
        {
            return ScreenXCoord * (RightBorder - LeftBorder) / ScreenWidth + LeftBorder;
        }

        //Calculates real Y coordinate based on screen dimentions and screen coordinate
        public static double DenormaliseY(int ScreenYCoord, int ScreenHeight, double BottomBorder, double TopBorder)
        {
            return TopBorder - ScreenYCoord * (TopBorder - BottomBorder) / ScreenHeight;
        }
    }
}