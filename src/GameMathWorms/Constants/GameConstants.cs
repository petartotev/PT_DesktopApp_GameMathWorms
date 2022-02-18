﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GameMathWorms.Constants
{
    public class GameConstants
    {

        public class Game
        {
            public const int Speed = 12;
            public const int FinalGoalMaxLimit = 301;
        }

        public class Canvas
        {
            public const int HeightMin = 0;
            public const int HeightMax = 450;
            public const int WidthMin = 0;
            public const int WidthMax = 800;
        }

        public class Player
        {
            public const int Speed = 10;
        }

        public class Target
        {
            public const int MinPositionX = 50;
            public const int MaxPositionX = 750;

            public const int MinPositionY = 35;
            public const int MaxPositionY = 300;

            public const int ResetSeconds = 2;

            public const int SpeedFalling = 3;

            public const int OperationAddMaxValue = 100;
            public const int OperationSubtractMaxValue = 100;
            public const int OperationMultiplyMaxValue = 6;
            public const int OperationDivideMaxValue = 4;
        }
    }
}
