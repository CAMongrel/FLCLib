using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FLCLib;
using FLCLib.Metro;

namespace FLCPlayer.Metro
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        FLCLib.Metro.FLCPlayer player;
        Texture2D texture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            texture = null;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected async override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            var localStorage = Windows.ApplicationModel.Package.Current.InstalledLocation;
            localStorage = await localStorage.GetFolderAsync("Assets");
            var resultFile = await localStorage.GetFileAsync("Test.flc");

            player = new FLCLib.Metro.FLCPlayer(GraphicsDevice);
            player.OnFrameUpdated += player_OnFrameUpdated;
            player.OnPlaybackFinished += player_OnPlaybackFinished;
            await player.Open(resultFile);
            player.ShouldLoop = false;
        }

        void player_OnPlaybackFinished(FLCFile file)
        {
            Exit();
        }

        void player_OnFrameUpdated(Texture2D setTexture, FLCFile file)
        {
            texture = setTexture;
            counter++;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            base.Update(gameTime);

            if (player.IsPlaying == false)
                player.Play();
        }

        int counter = 0;

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (texture != null)
            {
                float scale = this.Window.ClientBounds.Width / texture.Width;

                Rectangle rect = new Rectangle(0, 0, (int)(texture.Width * scale), (int)(texture.Height * scale));
                rect.Y = this.Window.ClientBounds.Height / 2 - rect.Height / 2;

                _spriteBatch.Begin();
                _spriteBatch.Draw(texture, rect, Color.White);
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
