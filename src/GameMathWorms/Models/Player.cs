using GameMathWorms.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace GameMathWorms.Models
{
    public class Player
    {
        public Player(Image image, Label scoreText)
        {
            Image = image;
            ScoreText = scoreText;
        }

        public int Score { get; set; }
        public Label ScoreText { get; set; }

        public bool IsMovingLeft { get; set; } = false;
        public bool IsMovingRight { get; set; } = false;

        public Image Image { get; set; }

        public void RecalculateScore(Target target)
        {
            Score = target.Operation switch
            {
                TargetOperationEnum.Add => Score += target.Value,
                TargetOperationEnum.Subtract => Score -= target.Value,
                TargetOperationEnum.Multiply => Score *= target.Value,
                TargetOperationEnum.Divide => Score *= target.Value,
                TargetOperationEnum.None => Score,
                _ => Score
            };
        }
    }
}
