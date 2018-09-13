using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Meny
{
    public class MenuComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch _spriteBatch;
        SpriteFont _normalFont;
        SpriteFont _selectedFont;

        List<MenuChoice> _choices;
        Color _backgroundColor;
        MouseState _previousMouseState;
        Texture2D _backgroundImage;

        private SubMenu _mainMenu;
        private SubMenu _activeMenu;
        private SubMenu _optionsMenu;
        private Texture2D _volumeGraph;
        private int volume;

        private List<SubMenu> _optionsChoices;
        
        public MenuComponent(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            _choices = new List<MenuChoice>();
            _choices.Add(new MenuChoice() { Text = "START", Selected = true, ClickAction = MenuStartClicked });
            _choices.Add(new MenuChoice() { Text = "SELECT LEVEL", ClickAction = MenuSelectClicked });
            _choices.Add(new MenuChoice() { Text = "OPTIONS", ClickAction = MenuOptionsClicked });
            _choices.Add(new MenuChoice() { Text = "QUIT", ClickAction = MenuQuitClicked });

            base.Initialize();
            _optionsChoices = new List<SubMenu>();
            _optionsChoices.Add(new SubMenu() { Text = "SFX SOUND", ClickAction = SfxSoundClicked, LeftAction = SfxVolumeOptionLeft, RightAction = SfxVolumeOptionRight });
            _optionsChoices.Add(new SubMenu() { Text = "MUSIC", ClickAction = MusicClicked });
            _optionsChoices.Add(new SubMenu() { Text = "BACK", ClickAction = BackOptionClicked });

            base.Initialize();
        }

        // ... Komplettering #1
        #region Menu Clicks

        private void MenuStartClicked()
        {
            _backgroundColor = Color.Turquoise;
        }

        private void MenuSelectClicked()
        {
            _backgroundColor = Color.Teal;
        }

        private void MenuOptionsClicked()
        {
            _backgroundColor = Color.AliceBlue;
        }

        private void MenuQuitClicked()
        {
            this.Game.Exit();
        }

        private void SfxSoundClicked()
        {
            _backgroundColor = Color.AntiqueWhite;
        }

        private void MusicClicked()
        {
            _backgroundColor = Color.Azure;
        }

        private void BackOptionClicked()
        {
            _backgroundColor = Color.Beige;
        }

        #endregion

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _normalFont = Game.Content.Load<SpriteFont>("menuFontNormal");
            _selectedFont = Game.Content.Load<SpriteFont>("menuFontSelected");
            _backgroundImage = Game.Content.Load<Texture2D>("candybackground");
            _volumeGraph = Game.Content.Load<Texture2D>("volumeGraph");

            //... Komplettering #2
            float startY = 0.2f * GraphicsDevice.Viewport.Height;

            foreach (var choice in _choices)
            {
                Vector2 size = _normalFont.MeasureString(choice.Text);
                choice.Y = startY;
                choice.X = GraphicsDevice.Viewport.Width / 2.0f - size.X / 2;
                choice.HitBox = new Rectangle((int)choice.X, (int)choice.Y, (int)size.X, (int)size.Y);
                startY += 70;
            }

            _previousMouseState = Mouse.GetState();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (KeyboardComponent.KeyPressed(Keys.Down))
                NextMenuChoice();
            if (KeyboardComponent.KeyPressed(Keys.Up))
                PreviousMenuChoice();
            if (KeyboardComponent.KeyPressed(Keys.Enter))
            {
                var selectedChoice = _choices.First(c => c.Selected);
                selectedChoice.ClickAction.Invoke();
            }
            if (KeyboardComponent.KeyPressed(Keys.Back))
            {
               
            }
            if (KeyboardComponent.KeyPressed(Keys.Left))
            {
                var selectedChoice = _activeMenu.Choices.First(c => c.Selected);
                if (selectedChoice.Activated)
                {
                    selectedChoice.LeftAction.Invoke();
                }
            }
            if (KeyboardComponent.KeyPressed(Keys.Right))
            {
                var selectedChoice = _activeMenu.Choices.First(c => c.Selected);
                if (selectedChoice.Activated)
                {
                    selectedChoice.RightAction.Invoke();
                }
            }

            var mouseState = Mouse.GetState();

            //... Komplettering #3
            foreach (var choice in _choices)
            {
                if (choice.HitBox.Contains(mouseState.X, mouseState.Y))
                {
                    _choices.ForEach(c => c.Selected = false);
                    choice.Selected = true;

                    if (_previousMouseState.LeftButton == ButtonState.Released
                        && mouseState.LeftButton == ButtonState.Pressed)
                        choice.ClickAction.Invoke();
                }
            }
            foreach (var choice in _optionsChoices)
            {
                if (choice.HitBox.Contains(mouseState.X, mouseState.Y))
                {
                    _choices.ForEach(c => c.Selected = false);
                    choice.Selected = true;

                    if (_previousMouseState.LeftButton == ButtonState.Released
                        && mouseState.LeftButton == ButtonState.Pressed)
                        choice.ClickAction.Invoke();
                }
            }

            _previousMouseState = mouseState;

            base.Update(gameTime);
        }

        // ... Komplettering #4
        private void PreviousMenuChoice()
        {
            int selectedIndex = _choices.IndexOf(_choices.First(c => c.Selected));
            _choices[selectedIndex].Selected = false;
            selectedIndex--;
            if (selectedIndex < 0)
                selectedIndex = _choices.Count - 1;
            _choices[selectedIndex].Selected = true;
        }

        private void NextMenuChoice()
        {
            int selectedIndex = _choices.IndexOf(_choices.First(c => c.Selected));
            _choices[selectedIndex].Selected = false;
            selectedIndex++;
            if (selectedIndex >= _choices.Count)
                selectedIndex = 0;
            _choices[selectedIndex].Selected = true;
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_backgroundColor);
            _spriteBatch.Begin();

            //bakgrundsbild
            _spriteBatch.Draw(_backgroundImage, GraphicsDevice.Viewport.Bounds, Color.White);

            _spriteBatch.Draw(choice.Image, new Vector2(0, 0), new Rectangle(0, 0, 512 / 5 * volume, _volumeGraph.Height), Color.White);

            // ... Komplettering #5
            foreach (var choice in _choices)
            {
                _spriteBatch.DrawString(choice.Selected ? _selectedFont : _normalFont,
                    choice.Text, new Vector2(choice.X, choice.Y), Color.White);
            }
            // ... Komplettering #5
            foreach (var choice in _optionsChoices)
            {
                _spriteBatch.DrawString(choice.Selected ? _selectedFont : _normalFont,
                    choice.Text, new Vector2(choice.X, choice.Y), Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}