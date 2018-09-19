using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAdventure
{
    public enum Action
    {
        Up,
        Down,
        Left,
        Right,
        Jump,
        ZoomIn,
        ZoomOut,
        Fullscreen,
        Exit
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
            { Action.ZoomIn, Keys.OemPlus },
            { Action.ZoomOut, Keys.OemMinus },
            { Action.Fullscreen, Keys.F },
        };

        private bool isUsingKeyboard;

        private KeyboardState currKeyboardState;
        private KeyboardState prevKeyboardState;

        public InputManager(bool _isUsingKeyboard = true)
        {
            isUsingKeyboard = _isUsingKeyboard;
            currKeyboardState = Keyboard.GetState();
            prevKeyboardState = currKeyboardState;
        }

        public void Update()
        {
            prevKeyboardState = currKeyboardState;
            currKeyboardState = Keyboard.GetState();
        }

        public bool IsKeyPressed(Keys key)
        {
            return currKeyboardState.IsKeyDown(key);
        }

        public bool IsPressed(Action action)
        {
            return currKeyboardState.IsKeyDown(keyboardActionMapping[action]);
        }

        public bool IsTriggered(Action action)
        {
            return currKeyboardState.IsKeyDown(keyboardActionMapping[action]) && !prevKeyboardState.IsKeyDown(keyboardActionMapping[action]);
        }
    }
}
