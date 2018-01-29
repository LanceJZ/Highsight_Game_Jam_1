#region Using
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Highsight_Game_Jam_1;
#endregion
namespace EngineTest
{
    public class Game1 : Game
    {
        GraphicsDeviceManager GDM;
        SpriteBatch SB;
        GameLogic TheGame;
        Camera TheCamera;

        public Game1()
        {
            GDM = new GraphicsDeviceManager(this);
            GDM.SynchronizeWithVerticalRetrace = true; //When true, 60FSP refresh rate locked.
            GDM.GraphicsProfile = GraphicsProfile.HiDef;
            GDM.PreferredBackBufferWidth = 1200;
            GDM.PreferredBackBufferHeight = 900;
            GDM.PreferMultiSampling = true; //Error in MonoGame 3.6 for DirectX, fixed in version 3.7.
            GDM.PreparingDeviceSettings += SetMultiSampling;
            GDM.ApplyChanges();
            GDM.GraphicsDevice.RasterizerState = new RasterizerState(); //Must be after Apply Changes.

            Content.RootDirectory = "Content";

            Helper.TheGame = this;
        }

        void SetMultiSampling(object sender, PreparingDeviceSettingsEventArgs eventArgs)
        {
            PresentationParameters PresentParm = eventArgs.GraphicsDeviceInformation.PresentationParameters;
            PresentParm.MultiSampleCount = 16;
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
            TheCamera = new Camera(this, new Vector3(-50, 50, 500), new Vector3(0, MathHelper.Pi, 0),
                GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);

            TheGame = new GameLogic(this, TheCamera);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SB = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
