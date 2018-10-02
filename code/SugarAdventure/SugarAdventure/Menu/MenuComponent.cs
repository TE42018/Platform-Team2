using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SugarAdventure
{
    public class MenuComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch _spriteBatch;
        SpriteFont _normalFont;
        SpriteFont _selectedFont;
        SpriteFont _activeFont;

        List<MenuChoice> _activeChoices;
        Color _backgroundColor;
        MouseState _previousMouseState;
        Texture2D _backgroundImage;
        private Texture2D _volumeGraph;

        private List<MenuChoice> _optionsChoices;
        private List<MenuChoice> _mainChoices;
        
        public MenuComponent(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            _mainChoices = new List<MenuChoice>();
            _mainChoices.Add(new MenuChoice() { Text = "START", Selected = true, ClickAction = MenuStartClicked });
            _mainChoices.Add(new MenuChoice() { Text = "SELECT LEVEL", ClickAction = MenuSelectClicked });
            _mainChoices.Add(new MenuChoice() { Text = "OPTIONS", ClickAction = MenuOptionsClicked });
            _mainChoices.Add(new MenuChoice() { Text = "QUIT", ClickAction = MenuQuitClicked });

            _optionsChoices = new List<MenuChoice>();
            _optionsChoices.Add(new MenuChoiceValue(50) { Text = "SFX SOUND", Selected = true, ClickAction = SfxSoundClicked });
            _optionsChoices.Add(new MenuChoiceValue(50) { Text = "MUSIC", ClickAction = MusicClicked, LeftAction = MenuMusicDown, RightAction = MenuMusicUp});
            _optionsChoices.Add(new MenuChoice() { Text = "BACK", ClickAction = BackOptionClicked });

            _activeChoices = _mainChoices;

            base.Initialize();
        }
        
        #region Menu Clicks

        private void MenuMusicUp()
        {
            MenuChoiceValue val = SelectedChoice as MenuChoiceValue;
            val.Value += 10;

            MediaPlayer.Volume = val.Value / 100.0f;
        }

        private void MenuMusicDown()
        {
            MenuChoiceValue val = SelectedChoice as MenuChoiceValue;
            val.Value -= 10;

            MediaPlayer.Volume = val.Value / 100.0f;
        }
        private void MenuStartClicked()
        {
            SugarGame.Instance.GSM.Push(new MainGameScreen());
            SugarGame.Instance.Components.Remove(this);
            SugarGame.entityManager.Reset();
            _backgroundColor = Color.Turquoise;
        }

        private void MenuSelectClicked()
        {
            _backgroundColor = Color.Teal;
        }

        private void MenuOptionsClicked()
        {
            _activeChoices = _optionsChoices;
        }

        private void MenuQuitClicked()
        {
            SugarGame.Instance.Exit();
        }

        private void SfxSoundClicked()
        {
            SelectedChoice.Active = true;
        }

        private void MusicClicked()
        {
            SelectedChoice.Active = true;
        }

        private void BackOptionClicked()
        {
            _activeChoices = _mainChoices;
        }

        #endregion

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _normalFont = Game.Content.Load<SpriteFont>("menuFontNormal");
            _selectedFont = Game.Content.Load<SpriteFont>("menuFontSelected");
            _activeFont = Game.Content.Load<SpriteFont>("menuFontActive");
            _backgroundImage = Game.Content.Load<Texture2D>("candybackground");
            //_volumeGraph = Game.Content.Load<Texture2D>("volumeGraph");

           InitializeMenu(_mainChoices);
           InitializeMenu(_optionsChoices);

            _previousMouseState = Mouse.GetState();
            base.LoadContent();
        }

        private void InitializeMenu(List<MenuChoice> list)
        {
            float startY = 0.2f * GraphicsDevice.Viewport.Height;

            foreach (var choice in list)
            {
                Vector2 size = _normalFont.MeasureString(choice.Text);
                choice.Y = startY;
                choice.X = GraphicsDevice.Viewport.Width / 2.0f - size.X / 2;
                choice.HitBox = new Rectangle((int)choice.X, (int)choice.Y, (int)size.X, (int)size.Y);
                startY += 70;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (SugarGame.inputManager.IsTriggered(Actions.Down))
                NextMenuChoice();
            if (SugarGame.inputManager.IsTriggered(Actions.Up))
                PreviousMenuChoice();
            if (SugarGame.inputManager.IsTriggered(Actions.Enter))
            {
                var selectedChoice = _activeChoices.First(c => c.Selected);
                selectedChoice.ClickAction?.Invoke();
            }
            if (SugarGame.inputManager.IsTriggered(Actions.Back))
            {
                if (SelectedChoice.Active)
                {
                    SelectedChoice.Active = false;
                    return;
                }

                if (_activeChoices == _optionsChoices)
                {
                    _activeChoices = _mainChoices;
                }
            }
            if (SugarGame.inputManager.IsTriggered(Actions.Left) && SelectedChoice.Active)
            {
                SelectedChoice.LeftAction?.Invoke();
            }
            if (SugarGame.inputManager.IsTriggered(Actions.Right) && SelectedChoice.Active)
            {
                SelectedChoice.RightAction?.Invoke();
            }

            var mouseState = Mouse.GetState();

            //... Komplettering #3
            foreach (var choice in _activeChoices)
            {
                if (choice.HitBox.Contains(mouseState.X, mouseState.Y))
                {
                    _activeChoices.ForEach(c => c.Selected = false);
                    choice.Selected = true;

                    if (_previousMouseState.LeftButton == ButtonState.Released
                        && mouseState.LeftButton == ButtonState.Pressed)
                        choice.ClickAction.Invoke();
                }
            }
            

            _previousMouseState = mouseState;

            base.Update(gameTime);
        }

        private MenuChoice SelectedChoice => _activeChoices.First(c => c.Selected);

        // ... Komplettering #4
        private void PreviousMenuChoice()
        {
            if(SelectedChoice.Active)
                return;
            
            int selectedIndex = _activeChoices.IndexOf(_activeChoices.First(c => c.Selected));
            _activeChoices[selectedIndex].Selected = false;
            selectedIndex--;
            if (selectedIndex < 0)
                selectedIndex = _activeChoices.Count - 1;
            _activeChoices[selectedIndex].Selected = true;
        }

        private void NextMenuChoice()
        {
            if (SelectedChoice.Active)
                return;

            int selectedIndex = _activeChoices.IndexOf(_activeChoices.First(c => c.Selected));
            _activeChoices[selectedIndex].Selected = false;
            selectedIndex++;
            if (selectedIndex >= _activeChoices.Count)
                selectedIndex = 0;
            _activeChoices[selectedIndex].Selected = true;
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_backgroundColor);
            _spriteBatch.Begin();

            //bakgrundsbild
            _spriteBatch.Draw(_backgroundImage, GraphicsDevice.Viewport.Bounds, Color.White);

            // ... Komplettering #5
            foreach (var choice in _activeChoices)
            {
                var spriteFont = choice.Selected ? _selectedFont : _normalFont;
                choice.Draw(_spriteBatch, choice.Active ? _activeFont : spriteFont);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}