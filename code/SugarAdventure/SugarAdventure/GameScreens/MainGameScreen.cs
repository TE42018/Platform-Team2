using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAdventure
{
    class MainGameScreen : GameScreen
    {
        Level level;
        Player player;
        Camera cam;

        private Texture2D background;
        private Texture2D hud_heartFull;
        private Texture2D hud_coins;
        private Texture2D hud_x;
        private Texture2D hud_keyBlue_Disabled;
        private Texture2D hud_keyGreen_Disabled;
        private Texture2D hud_keyRed_Disabled;
        private Texture2D hud_keyBlue_Enabled;
        private Texture2D hud_keyGreen_Enabled;
        private Texture2D hud_keyRed_Enabled;
        private Texture2D hud_heartHalf;
        private Texture2D hud_heartEmpty;
        private Texture2D heartHalf;
        private Texture2D[] hud_numbers;
        private Texture2D paused;
        private Song backgroundMusic;
        private bool isPaused;


        public MainGameScreen()
        {
            if (cam == null)
                cam = new Camera(Vector2.Zero, SugarGame.graphics.GraphicsDevice.Viewport.Width, SugarGame.graphics.GraphicsDevice.Viewport.Height, 0.6f);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent(GraphicsDevice pGraphicsDevice)
        {
            level = SugarGame.levelManager.LoadLevel(LevelNumber.level1);

            Camera.SetBoundingLevel(level);

            player = new Player(level, 4);
            player.LoadContent();
            player.SetBoundingLevel(level);

            paused = SugarGame.contentManager.Load<Texture2D>("pause");

            hud_heartFull = SugarGame.contentManager.Load<Texture2D>("hud_heartFull");
            hud_heartHalf = SugarGame.contentManager.Load<Texture2D>("hud_heartHalf");
            hud_heartEmpty = SugarGame.contentManager.Load<Texture2D>("hud_heartEmpty");

            hud_coins = SugarGame.contentManager.Load<Texture2D>("hud_coins");
            hud_x = SugarGame.contentManager.Load<Texture2D>("hud_x");
            hud_numbers = new Texture2D[10];
            for (int i = 0; i < hud_numbers.Length; i++)
            {
                hud_numbers[i] = SugarGame.contentManager.Load<Texture2D>($"hud_{i}");
            }

            hud_keyBlue_Disabled = SugarGame.contentManager.Load<Texture2D>("hud_keyBlue_Disabled");
            hud_keyGreen_Disabled = SugarGame.contentManager.Load<Texture2D>("hud_keyGreen_Disabled");
            hud_keyRed_Disabled = SugarGame.contentManager.Load<Texture2D>("hud_keyRed_Disabled");

            hud_keyBlue_Enabled = SugarGame.contentManager.Load<Texture2D>("hud_keyBlue");
            hud_keyGreen_Enabled = SugarGame.contentManager.Load<Texture2D>("hud_keyGreen");
            hud_keyRed_Enabled = SugarGame.contentManager.Load<Texture2D>("hud_keyRed");

            backgroundMusic = SugarGame.contentManager.Load<Song>("backMusic");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.15f;

            background = SugarGame.contentManager.Load<Texture2D>("bakgrund");

            base.LoadContent(pGraphicsDevice);
        }

        private void DrawHearts(SpriteBatch spriteBatch, Vector2 pos, int health, int maxHealth)
        {
            int heartCount = maxHealth / 2;
            int fullHearts = health / 2;
            int halfHearts = (int)Math.Ceiling((float)health % 2);
            int emptyHearts = heartCount - fullHearts - halfHearts;

            int offset = 0;

            for (int i = 0; i < fullHearts; i++)
            {
                spriteBatch.Draw(hud_heartFull, pos + new Vector2(hud_heartFull.Width * offset, 0), Color.White);
                offset++;
            }

            for (int i = 0; i < halfHearts; i++)
            {
                spriteBatch.Draw(hud_heartHalf, pos + new Vector2(hud_heartHalf.Width * offset, 0), Color.White);
                offset++;
            }

            for (int i = 0; i < emptyHearts; i++)
            {
                spriteBatch.Draw(hud_heartEmpty, pos + new Vector2(hud_heartEmpty.Width * offset, 0), Color.White);
                offset++;
            }
        }

        private void DrawCoins(SpriteBatch spriteBatch, Vector2 pos, int amount)
        {
            string amountString = amount.ToString();
            int coinsWidth = hud_coins.Width + hud_x.Width + (amountString.Length * hud_numbers[0].Width);
            int coinsHeight = hud_coins.Height;
            Vector2 drawPos = Vector2.Clamp(pos, Vector2.Zero, pos - new Vector2(coinsWidth, coinsHeight));

            spriteBatch.Draw(hud_coins, drawPos, Color.White);
            spriteBatch.Draw(hud_x, drawPos + new Vector2(hud_coins.Width, hud_coins.Height / 2 - hud_x.Height / 2), Color.White);

            for (int i = 0; i < amountString.Length; i++)
            {
                int digit = (int)char.GetNumericValue(amountString[i]);
                spriteBatch.Draw(hud_numbers[digit], drawPos + new Vector2(hud_coins.Width + hud_x.Width + hud_numbers[0].Width * i, hud_coins.Height / 2 - hud_numbers[0].Height / 2), Color.White);
            }
        }

        private void DrawKeys(SpriteBatch pSpriteBatch, Vector2 position)
        {
            if (player.HasItem("key_red"))
                pSpriteBatch.Draw(hud_keyRed_Enabled, new Rectangle((int)position.X, (int)position.Y, 32, 32), Color.White);
            else
                pSpriteBatch.Draw(hud_keyRed_Disabled, new Rectangle((int)position.X, (int)position.Y, 32, 32), Color.White);

            if (player.HasItem("key_green"))
                pSpriteBatch.Draw(hud_keyGreen_Enabled, new Rectangle((int)position.X + 30, (int)position.Y, 32, 32), Color.White);
            else
                pSpriteBatch.Draw(hud_keyGreen_Disabled, new Rectangle((int)position.X + 30, (int)position.Y, 32, 32), Color.White);

            if (player.HasItem("key_blue"))
                pSpriteBatch.Draw(hud_keyBlue_Enabled, new Rectangle((int)position.X + 60, (int)position.Y, 32, 32), Color.White);
            else
                pSpriteBatch.Draw(hud_keyBlue_Disabled, new Rectangle((int)position.X + 60, (int)position.Y, 32, 32), Color.White);
        }

        public override void Update(GameTime pGameTime)
        {

            if (SugarGame.inputManager.IsKeyTriggered(Keys.Escape))
            {
                //SugarGame.Instance.Components.Add(new MenuComponent(SugarGame.Instance));
                isPaused = !isPaused;

            }
            if (isPaused)
            {
                MediaPlayer.Volume = 0.05f;
                return;
            }
            MediaPlayer.Volume = 0.15f;

            if (SugarGame.inputManager.IsKeyPressed(Keys.Escape))
            {
                //quit = true;
                //SugarGame.Instance.Components.Add(new MenuComponent(SugarGame.Instance));
            }
            if (SugarGame.inputManager.IsPressed(Actions.Up))
            {
                player.ClimbUp();
            }
            else if (SugarGame.inputManager.IsPressed(Actions.Down))
            {
                player.ClimbDown();
            }
            if (!SugarGame.inputManager.IsPressed(Actions.Up) && !SugarGame.inputManager.IsPressed(Actions.Down))
            {
                //player.StopClimbing();
            }
            if (SugarGame.inputManager.IsPressed(Actions.Left))
            {
                player.MoveLeft();
            }
            else if (SugarGame.inputManager.IsPressed(Actions.Right))
            {
                player.MoveRight();
            }
            if (!SugarGame.inputManager.IsPressed(Actions.Right) && !SugarGame.inputManager.IsPressed(Actions.Left))
            {
                player.Stop();
            }

            if (SugarGame.inputManager.IsTriggered(Actions.Activate))
            {
                player.CurrentAction = Actions.Activate;
            }
            if (SugarGame.inputManager.IsTriggered(Actions.Jump))
            {
                player.Jump();
            }
            if (SugarGame.inputManager.IsKeyPressed(Keys.R))
            {
                player.Reset();
            }
            if (SugarGame.inputManager.IsKeyTriggered(Keys.F))
            {
                SugarGame.graphics.PreferredBackBufferWidth = 1920;
                SugarGame.graphics.PreferredBackBufferHeight = 1080;
                SugarGame.graphics.ApplyChanges();
                SugarGame.graphics.ToggleFullScreen();
                cam.UpdateViewport(SugarGame.Instance.GraphicsDevice.Viewport.Bounds);
            }
            player.Update(pGameTime);
            SugarGame.entityManager.Update(player, pGameTime);
            cam.SetTarget(player.Hitbox);
            cam.Update(pGameTime);
            base.Update(pGameTime);
        }

        public override void Draw(SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, cam.ViewMatrix);

            if (player != null)
            {
                pSpriteBatch.Draw(background, cam.Pos, Color.White);
                player.Level.Draw(pSpriteBatch);
                player.Draw(pSpriteBatch);
                SugarGame.entityManager.Draw(pSpriteBatch);
            }
            base.Draw(pSpriteBatch);
            pSpriteBatch.End();

            pSpriteBatch.Begin();

            DrawHearts(pSpriteBatch, Vector2.One, player.Health, player.MaxHealth);
            DrawCoins(pSpriteBatch, new Vector2(SugarGame.graphics.PreferredBackBufferWidth, 0), player.Money);
            DrawKeys(pSpriteBatch, new Vector2(0, hud_heartFull.Height));

            pSpriteBatch.End();

            pSpriteBatch.Begin(blendState: BlendState.AlphaBlend);

            if (isPaused)
            {
                Texture2D solid = new Texture2D(SugarGame.graphics.GraphicsDevice, 1, 1);
                Color[] colorData = { Color.White };
                solid.SetData(colorData);
                var x = pSpriteBatch.GraphicsDevice.Viewport.Width / 2 - paused.Width / 2;
                pSpriteBatch.Draw(solid, new Rectangle(0, 0, SugarGame.graphics.PreferredBackBufferWidth, SugarGame.graphics.PreferredBackBufferHeight), Color.Black * 0.4f);
                pSpriteBatch.Draw(paused, new Rectangle(x, 50, 180, 180), Color.White);

            }



            pSpriteBatch.End();
        }
    }
}
