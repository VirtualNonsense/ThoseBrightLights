using Microsoft.Xna.Framework.Input;

namespace SE_Praktikum.Models
{
    public class Input
    {
        public  Keys Up { get; set; }

        public Keys Down { get; set; }

        public Keys Left { get; set; }

        public Keys Right { get; set; }
        
        public Keys TurnLeft { get; set; }
        
        public Keys TurnRight { get; set; }
        
        public Keys Shoot { get; set; }

        public Input(Keys up, Keys down, Keys left, Keys right, Keys turnLeft, Keys turnRight, Keys shoot = Keys.Space)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
            TurnLeft = turnLeft;
            TurnRight = turnRight;
            Shoot = shoot;
        }
    }
}