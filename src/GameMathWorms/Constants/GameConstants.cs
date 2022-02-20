namespace GameMathWorms.Constants
{
    public class GameConstants
    {
        public class Game
        {
            public const int FinalGoal = 10;
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
            public const int ResetSeconds = 3;
            public const int SpeedFalling = 3;

            public class Operation
            {
                public const int AddMinValue = 1;
                public const int AddMaxValue = Game.FinalGoal / 3;
                public const int SubtractMinValue = 1;
                public const int SubtractMaxValue = Game.FinalGoal / 3;
                public const int MultiplyMinValue = 2;
                public const int MultiplyMaxValue = 4;
                public const int DivideMinValue = 2;
                public const int DivideMaxValue = 3;
            }
        }
    }
}
