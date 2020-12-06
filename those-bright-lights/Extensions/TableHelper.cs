namespace SE_Praktikum.Extensions
{
    public static class TableHelper
    {
        public static TablePosition GetTablePos(int x, int y, uint xMax, uint yMax)
        {
            
            
            
            // Top left
            if (x == 0 && y == 0)
            {
                if (x == xMax && y == yMax)
                {
                    return TablePosition.SingleTile;
                }
                if (x == xMax)
                {
                    return TablePosition.SingleColumnTop;
                }
                if (y == yMax)
                {
                    return TablePosition.SingleRowLeft;
                }
                return TablePosition.TopLeft;
            }
                    
            // Top right
            if (x == xMax && y == 0)
            {
                if (y == yMax)
                {
                    return TablePosition.SingleRowRight;
                }
                return TablePosition.TopRight;
            }
                    
            // Bottom left
            if (x == 0 && y == yMax)
            {
                // only one column
                if (x == xMax)
                {
                    return TablePosition.SingleColumnBottom;
                }
                return TablePosition.BottomLeft;
            }
                    
            // Bottom right
            if (x == xMax && y == yMax)
            {
                return TablePosition.BottomRight;
            }
                    
            // Top
            if (y == 0)
            {
                // only one line
                if (y == yMax)
                {
                    return TablePosition.SingleRowMiddle;
                }
                return TablePosition.Top;
            }
                    
            // Bottom
            if (y == yMax)
            {
                return TablePosition.Bottom;
            }

            // Left
            if (x == 0)
            {
                // only one column
                if (x == xMax)
                {
                    return TablePosition.SingleColumnMiddle;
                }
                return TablePosition.Left;
            }
                    
            // Right
            if (x == xMax)
            {
                return TablePosition.Right;
            }

            return TablePosition.Middle;
        }
        public enum TablePosition
        {
            TopLeft,
            TopRight,
            Top,
            Right,
            BottomRight,
            Bottom,
            BottomLeft,
            Left,
            Middle,
            SingleColumnTop,
            SingleColumnMiddle,
            SingleColumnBottom,
            SingleRowLeft,
            SingleRowMiddle,
            SingleRowRight,
            SingleTile
            
        }
    }
}