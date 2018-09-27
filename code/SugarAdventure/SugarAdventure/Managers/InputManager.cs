using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;


namespace SugarAdventure
{
    public enum Actions
    {
        Up,
        Down,
        Left,
        Right,
        Jump,
        Enter,
        Activate,
        Back,
        None
    }

    public class InputManager
    {
        private Dictionary<Actions, Keys> keyboardActionMapping = new Dictionary<Actions, Keys>()
        {
            { Actions.Up, Keys.Up },
            { Actions.Down, Keys.Down },
            { Actions.Left, Keys.Left },
            { Actions.Right, Keys.Right },
            { Actions.Jump, Keys.Space },
            { Actions.Enter, Keys.Enter },
            { Actions.Back, Keys.Escape },
            { Actions.Activate, Keys.E },
        };
        
        private Dictionary<Actions, Buttons> gamepadActionMapping = new Dictionary<Actions, Buttons>()
        {
            { Actions.Up, Buttons.LeftThumbstickUp },
            { Actions.Down, Buttons.LeftThumbstickDown },
            { Actions.Left, Buttons.LeftThumbstickLeft },
            { Actions.Right, Buttons.LeftThumbstickRight },
            { Actions.Jump, Buttons.A },
            { Actions.Enter, Buttons.A },
            { Actions.Activate, Buttons.Y },
            { Actions.Back, Buttons.B },
        };

        private KeyboardState currKeyboardState;
        private KeyboardState prevKeyboardState;
        private GamePadState currGamePadState;
        private GamePadState prevGamePadState;

        public InputManager()
        {
            currKeyboardState = Keyboard.GetState();
            prevKeyboardState = currKeyboardState;
            
            currGamePadState = GamePad.GetState(PlayerIndex.One);
            prevGamePadState = currGamePadState;
        }

        public void Update()
        {
            prevKeyboardState = currKeyboardState;
            currKeyboardState = Keyboard.GetState();
           
            prevGamePadState = currGamePadState;
            currGamePadState = GamePad.GetState(PlayerIndex.One);
           
        }

        public bool IsKeyPressed(Keys key)
        {
            return currKeyboardState.IsKeyDown(key);
        }
        public bool IsKeyTriggered(Keys key)
        {
            return currKeyboardState.IsKeyDown(key) && !prevKeyboardState.IsKeyDown(key);
        }

        public bool IsButtonPressed(Buttons button)
        {
            return currGamePadState.IsButtonDown(button);
        }

        public bool IsPressed(Actions action)
        {
            return currKeyboardState.IsKeyDown(keyboardActionMapping[action]) || currGamePadState.IsButtonDown(gamepadActionMapping[action]);
        }

        public bool IsTriggered(Actions action)
        {
            return currKeyboardState.IsKeyDown(keyboardActionMapping[action]) && !prevKeyboardState.IsKeyDown(keyboardActionMapping[action]) ||
                   currGamePadState.IsButtonDown(gamepadActionMapping[action]) && !prevGamePadState.IsButtonDown(gamepadActionMapping[action]);
        }
    }
}


