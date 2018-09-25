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


namespace PlayerControll
{
    public enum Action
    {
        Up,
        Down,
        Left,
        Right,
        Jump,
        Exit,
        None
    }
    
    public class InputManager
    {
        private Dictionary<Action, Keys> keyboardActionMapping = new Dictionary<Action, Keys>()
        {
            { Action.Up, Keys.Up },
            { Action.Down, Keys.Down },
            { Action.Left, Keys.Left },
            { Action.Right, Keys.Right },
            { Action.Jump, Keys.Space },
            { Action.Exit, Keys.Escape },
        };

        //Action mapping för gamepad
        private Dictionary<Action, Buttons> gamepadActionMapping = new Dictionary<Action, Buttons>()
        {
            { Action.Up, Buttons.LeftThumbstickUp },
            { Action.Down, Buttons.LeftThumbstickDown },
            { Action.Left, Buttons.LeftThumbstickLeft },
            { Action.Right, Buttons.LeftThumbstickRight },
            { Action.Jump, Buttons.A },
            { Action.Exit, Buttons.Back },
        };


        private bool isUsingKeyboard;

        private KeyboardState currKeyboardState;
        private KeyboardState prevKeyboardState;
        private GamePadState currGamePadState;
        private GamePadState prevGamePadState;

        public Keys Left { get; internal set; }
        public Keys Right { get; internal set; }

        public InputManager(bool _isUsingKeyboard = true)
        {
            isUsingKeyboard = _isUsingKeyboard;

            if (isUsingKeyboard)
            {
                currKeyboardState = Keyboard.GetState();
                prevKeyboardState = currKeyboardState;
            }
            else
            {
                currGamePadState = GamePad.GetState(PlayerIndex.One);
                prevGamePadState = currGamePadState;
            }
        }

        public void Update()
        {
            if (isUsingKeyboard)
            {
                prevKeyboardState = currKeyboardState;
                currKeyboardState = Keyboard.GetState();
            }
            else
            {
                prevGamePadState = currGamePadState;
                currGamePadState = GamePad.GetState(PlayerIndex.One);
            }
        }

        public bool IsPressed(Action action)
        {
            if (isUsingKeyboard)
            {
                return currKeyboardState.IsKeyDown(keyboardActionMapping[action]);
            }
            else
            {
                return currGamePadState.IsButtonDown(gamepadActionMapping[action]);
            }
        }

        public bool IsTriggered(Action action)
        {
            if (isUsingKeyboard)
            {
                return currKeyboardState.IsKeyDown(keyboardActionMapping[action]) && !prevKeyboardState.IsKeyDown(keyboardActionMapping[action]);
            }
            else
            {
                return currGamePadState.IsButtonDown(gamepadActionMapping[action]) && !prevGamePadState.IsButtonDown(gamepadActionMapping[action]);
            }
        }
    }
}
    

