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
        Timer FPSTimer;
        float FPSFrames = 0;
        bool NotFirstFrame;

        public Game1()
        {
            GDM = new GraphicsDeviceManager(this);
            GDM.SynchronizeWithVerticalRetrace = false; //When true, 60FSP refresh rate locked.
            GDM.GraphicsProfile = GraphicsProfile.HiDef;
            GDM.PreferredBackBufferWidth = 1200;
            GDM.PreferredBackBufferHeight = 900;
            GDM.PreferMultiSampling = true; //Error in MonoGame 3.6 for DirectX, fixed in version 3.7.
            GDM.PreparingDeviceSettings += SetMultiSampling;
            GDM.ApplyChanges();
            GDM.GraphicsDevice.RasterizerState = new RasterizerState(); //Must be after Apply Changes.
            IsFixedTimeStep = true; //When true, 60FSP refresh rate locked.
            Content.RootDirectory = "Content";
            Helper.Initialize(this, GDM, GraphicsDevice);
            FPSTimer = new Timer(this, 1);
        }

        void SetMultiSampling(object sender, PreparingDeviceSettingsEventArgs eventArgs)
        {
            PresentationParameters PresentParm = eventArgs.GraphicsDeviceInformation.PresentationParameters;
            PresentParm.MultiSampleCount = 4;
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
            TheCamera = new Camera(this, new Vector3(0, 0, 200), new Vector3(0, MathHelper.Pi, 0),
                GraphicsDevice.Viewport.AspectRatio, 1, 300f);

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

            base.Update(gameTime);

            FPSFrames++;

            if (FPSTimer.Elapsed)
            {
                FPSTimer.Reset();
                System.Diagnostics.Debug.WriteLine("FPS " + FPSFrames.ToString());
                FPSFrames = 0;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateBlue);

            if (NotFirstFrame)
            {
                base.Draw(gameTime);
            }
            else
            {
                NotFirstFrame = true;
            }
        }
    }
}
