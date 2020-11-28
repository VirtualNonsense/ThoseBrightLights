using System;
using Microsoft.Xna.Framework.Input;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class InputFactory
    {
        public InputFactory()
        {
            
        }

        public Input GetInstance()
        {
            var i = new Input(Keys.W,Keys.S, Keys.A, Keys.D, Keys.Q, Keys.E);
            //TODO Andi du weißt scho was hier zu tun is
            return i;
        }

    }
}