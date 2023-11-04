using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexRunner.Entities;

namespace TexRunner.System
{
    public class InputController
    {
        private Trex _trex;
        private KeyboardState _previousKeyboardState;
        public InputController(Trex trex)
        {
            _trex = trex;
        }
        public void ProcessControls(GameTime gameTime) 
        { 
            KeyboardState keyboardState=Keyboard.GetState(); 
            if(!_previousKeyboardState.IsKeyDown(Keys.Up)&&keyboardState.IsKeyDown(Keys.Up))
            {
                if(_trex.State!=TrexState.Jumping)
                { 
                    _trex.BeginJump(); 
                }
            }
            else if (!keyboardState.IsKeyDown(Keys.Up) && _trex.State == TrexState.Jumping)
            {
                _trex.CancelJump();
            }
            else if(keyboardState.IsKeyDown(Keys.Down))
            {
                _trex.Duck();
            }
            _previousKeyboardState = keyboardState;
        }
    }
}
