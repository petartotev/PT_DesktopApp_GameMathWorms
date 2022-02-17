using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace GameMathWorms.Models
{
    public class Player
    {
        public Player(Image image)
        {
            Image = image;
        }

        public int Score { get; set; }

        public bool IsMovingLeft { get; set; } = false;
        public bool IsMovingRight { get; set; } = false;

        public Image Image { get; set; }
    }
}
