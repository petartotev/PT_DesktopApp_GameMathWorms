using System;
using System.Collections.Generic;
using System.Text;

namespace GameMathWorms.Constants
{
    public class GameConstants
    {
        public const int PlayerSpeed = 10;
        public const int GameSpeed = 12;

        public class Target
        {
            public const int MinPositionX = 50;
            public const int MaxPositionX = 750;

            public const int MinPositionY = 0;
            public const int MaxPositionY = 300;

            public const int ResetSeconds = 1;

            public const int OperationAddMaxValue = 100;
            public const int OperationSubtractMaxValue = 100;
            public const int OperationMultiplyMaxValue = 6;
            public const int OperationDivideMaxValue = 4;
        }
    }
}
