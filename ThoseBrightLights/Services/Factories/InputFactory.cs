using System;
using Microsoft.Xna.Framework.Input;
using ThoseBrightLights.Models;

namespace ThoseBrightLights.Services.Factories
{
    public class InputFactory
    {
        public InputFactory()
        {
            
        }

        public Input GetInstance()
        {
            var i = new Input(Keys.W,Keys.S, Keys.A, Keys.D, Keys.Q, Keys.E, Keys.Space);
            return i;
        }

    }
}