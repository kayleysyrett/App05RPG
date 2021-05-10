﻿using App05MonoGame.Controllers;
using App05MonoGame.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace App05MonoGame
{
    /// <summary>
    /// This game creates a variety of sprites as an example.  
    /// There is no game to play yet. The spaceShip and the 
    /// asteroid can be used for a space shooting game, the player, 
    /// the coin and the enemy could be used for a pacman
    /// style game where the player moves around collecting
    /// random coins and the enemy tries to catch the player.
    /// </summary>
    /// <authors>
    /// Derek Peacock & Andrei Cruceru
    /// </authors>
    public class App05Game : Game
    {
        #region Constants

        public const int HD_Height = 1080;
        public const int HD_Width = 1920;

        #endregion

        #region Attribute

        private readonly GraphicsDeviceManager graphicsManager;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;

        private SpriteFont arialFont;
        private SpriteFont calibriFont;

        private Texture2D backgroundImage;
        private SoundEffect flameEffect;

        private PlayerSprite shipSprite;
        private Sprite asteroidSprite;

        private Button restartButton;

        private AsteroidController asteroidController;

        //should be in player 
        private int score;
        private int health;

        #endregion

        public App05Game()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Setup the game window size to 720P 1280 x 720 pixels
        /// Simple fixed playing area with no camera or scrolling
        /// </summary>
        protected override void Initialize()
        {
            graphicsManager.PreferredBackBufferWidth = HD_Width;
            graphicsManager.PreferredBackBufferHeight = HD_Height;

            graphicsManager.ApplyChanges();

            graphicsDevice = graphicsManager.GraphicsDevice;

            score = 0;
            health = 100;

            asteroidController = new AsteroidController();

            base.Initialize();
        }

        /// <summary>
        /// use Content to load your game images, fonts,
        /// music and sound effects
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundImage = Content.Load<Texture2D>(
                "backgrounds/spacebackground");

            // Load Music and SoundEffects

            SoundController.LoadContent(Content);
            //SoundController.PlaySong("Adventure");
            flameEffect = SoundController.GetSoundEffect("Flame");

            // Load Fonts

            arialFont = Content.Load<SpriteFont>("fonts/arial");
            calibriFont = Content.Load<SpriteFont>("fonts/calibri");

            restartButton = new Button(arialFont,
                Content.Load<Texture2D>("Controls/button-icon-png-200"))
            {
                Position = new Vector2(1750, 950),
                Text = "Quit",
                Scale = 0.6f
            };

            restartButton.click += RestartButton_click;

            // suitable for asteroids type game

            SetupSpaceShip();
            SetupAsteroid();

        }

        private void RestartButton_click(object sender, System.EventArgs e)
        {
            //TODO: do something when the button is clicked!
            
            Exit();
        }

        /// <summary>
        /// This is a single image sprite that rotates
        /// and move at a constant speed in a fixed direction
        /// </summary>
        private void SetupAsteroid()
        {

            asteroidController.LoadImages(Content);
            asteroidController.CreateAsteroid();

    }

        /// <summary>
        /// This is a Sprite that can be controlled by a
        /// player using Rotate Left = A, Rotate Right = D, 
        /// Forward = Space
        /// </summary>
        private void SetupSpaceShip()
        {
            Texture2D ship = Content.Load<Texture2D>(
               "Actors/GreenShip");

            shipSprite = new PlayerSprite(ship, 200, 500)
            {
                Direction = new Vector2(1, 0),
                Speed = 200,
                DirectionControl = DirectionControl.Rotational
            };
    }


        /// <summary>
        /// This is an enemy Sprite with four animations for the four
        /// directions, up, down, left and right.  Has no intelligence!
        /// </summary>

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();


            //maybe put a pause method in here 

            restartButton.Update(gameTime);

            // Update Asteroids

            shipSprite.Update(gameTime);
            asteroidController.Update(gameTime);
            asteroidController.HasCollided(shipSprite);

            // Update Chase Game

            base.Update(gameTime);
        }

        /// <summary>
        /// Called 60 frames/per second and Draw all the 
        /// sprites and other drawable images here
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LawnGreen);

            spriteBatch.Begin();


            spriteBatch.Draw(backgroundImage, Vector2.Zero, Color.White);

            restartButton.Draw(spriteBatch);

            // Draw asteroids game

            shipSprite.Draw(spriteBatch);
            asteroidController.Draw(spriteBatch);


            // Draw Chase game


            DrawGameStatus(spriteBatch);
            DrawGameFooter(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Display the name fo the game and the current score
        /// and health of the player at the top of the screen
        /// </summary>
        public void DrawGameStatus(SpriteBatch spriteBatch)
        {
            Vector2 topLeft = new Vector2(4, 4);
            string status = $"Score = {score:##0}";

            spriteBatch.DrawString(arialFont, status, topLeft, Color.White);

            string game = "Kayley asteroids";
            Vector2 gameSize = arialFont.MeasureString(game);
            Vector2 topCentre = new Vector2((HD_Width/2 - gameSize.X/2), 4);
            spriteBatch.DrawString(arialFont, game, topCentre, Color.White);

            string healthText = $"Health = {health}%";
            Vector2 healthSize = arialFont.MeasureString(healthText);
            Vector2 topRight = new Vector2(HD_Width - (healthSize.X + 4), 4);
            spriteBatch.DrawString(arialFont, healthText, topRight, Color.White);

        }

        /// <summary>
        /// Display the Module, the authors and the application name
        /// at the bottom of the screen
        /// </summary>
        public void DrawGameFooter(SpriteBatch spriteBatch)
        {
            int margin = 20;

            string names = "Kayley Syrett";
            string app = "App05: MonogGame";
            string module = "BNU CO453-2020";

            Vector2 namesSize = calibriFont.MeasureString(names);
            Vector2 appSize = calibriFont.MeasureString(app);

            Vector2 bottomCentre = new Vector2((HD_Width - namesSize.X)/ 2, HD_Height - margin);
            Vector2 bottomLeft = new Vector2(margin, HD_Height - margin);
            Vector2 bottomRight = new Vector2(HD_Width - appSize.X - margin, HD_Height - margin);

            spriteBatch.DrawString(calibriFont, names, bottomCentre, Color.Yellow);
            spriteBatch.DrawString(calibriFont, module, bottomLeft, Color.Yellow);
            spriteBatch.DrawString(calibriFont, app, bottomRight, Color.Yellow);

        }
    }
}
