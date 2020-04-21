namespace cglr2
{
    //A structure, capable of storing and performing operations on matrixes.
    class TMatrix
    {
        private float[,] values;
        private int rowCount, columnCount;
        public int RowCount { get => rowCount; }
        public int ColumnCount { get => columnCount; }

        public float GetValue(int row, int column)
        {
            float RetVal = 0;
            if (row >= 0 && row < rowCount && column >= 0 && column < columnCount)
                RetVal = values[row, column];
            return RetVal;
        }

        public void SetValue(int row, int column, float value)
        {
            if (row >= 0 && row < rowCount && column >= 0 && column < columnCount)
                values[row, column] = value;
        }

        //==========================================================================
        public TMatrix()
        {
            rowCount = columnCount = 1;
            values = new float[1, 1] { { 0f } };
        }

        public TMatrix(float[,] _matrix)
        {
            values = _matrix;
            rowCount = _matrix.GetLength(0);
            columnCount = _matrix.GetLength(1);
        }

        // Multiplies matrix, represented by this object by the "MulMatrix" matrix.
        public TMatrix Multiply(TMatrix MulMatrix)
        {
            TMatrix ResMatrix = null;

            if (MulMatrix != null && MulMatrix.values != null && values != null && columnCount == MulMatrix.rowCount)
            {
                ResMatrix = new TMatrix(new float[rowCount, MulMatrix.columnCount]);
                for (int r = 0; r < rowCount; r++)
                    for (int c = 0; c < MulMatrix.columnCount; c++)
                        ResMatrix.values[r, c] = 0;
                
                for (int row = 0; row < rowCount; row++)        //For each row
                    for (int MulCol = 0; MulCol < MulMatrix.columnCount; MulCol++)  //Iterate through each column of second matrix                                           
                        for (int MulRow = 0; MulRow < MulMatrix.rowCount; MulRow++) //And throuhg each element of that column                    
                            ResMatrix.values[row, MulCol] += values[row, MulRow] * MulMatrix.values[MulRow, MulCol];                                                 
            }

            return ResMatrix;
        }
    }
}
