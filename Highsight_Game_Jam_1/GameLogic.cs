using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;
using Highsight_Game_Jam_1;

public enum GameState
{
    Over,
    InPlay,
    HighScore,
    MainMenu
};

namespace EngineTest
{
    class GameLogic : GameComponent
    {
        Camera TheCamera;
        List<Box> TheBoxs;
        Model BoxModel;
        Numbers ScoreDisplay;
        Words WordDisplay;
        float Rotation;

        GameState GameMode = GameState.MainMenu;
        KeyboardState OldKeyState;

        public GameState CurrentMode { get => GameMode; }

        public GameLogic(Game game, Camera camera) : base(game)
        {
            TheCamera = camera;
            TheBoxs = new List<Box>();
            ScoreDisplay = new Numbers(game);
            WordDisplay = new Words(game);

            // Screen resolution is 1200 X 900.
            // Y positive is Up.
            // X positive is right of window when camera is at rotation zero.
            // Z positive is towards the camera when at rotation zero.
            // Positive rotation rotates CCW. Zero has front facing X positive. Pi/2 on Y faces Z negative.
            game.Components.Add(this);
        }

        public override void Initialize()
        {

            base.Initialize();
            LoadContent();
        }

        public void LoadContent()
        {
            BoxModel = Helper.LoadModel("Core/Cube");

            BeginRun();
        }

        public void BeginRun()
        {

            for (int i = 0; i < 3; i++)
            {
                TheBoxs.Add(new Box(Game, TheCamera, BoxModel));
            }

            TheBoxs[0].ModelScale = new Vector3(20);
            TheBoxs[0].Position = new Vector3(0, 50, 0);
            TheBoxs[0].RotationVelocity = new Vector3(0, 0, 1);
            TheBoxs[0].DefuseColor = new Vector3(0.5f, 0.4f, 0.1f);
            TheBoxs[0].EmissiveColor = new Vector3(0.5f, 0.4f, 0.1f);
            TheBoxs[1].ModelScale = new Vector3(2);
            TheBoxs[1].Position = new Vector3(150, 0, 0);
            TheBoxs[1].RotationVelocity = new Vector3(0, 0, 2);
            TheBoxs[1].DefuseColor = new Vector3(0.2f, 0.2f, 0.6f);
            TheBoxs[1].AddAsChildOf(TheBoxs[0]);
            TheBoxs[2].ModelScale = new Vector3(1);
            TheBoxs[2].Position = new Vector3(30, 0, 0);
            TheBoxs[2].RotationVelocity = new Vector3(0, 0, 2);
            TheBoxs[2].AddAsChildOf(TheBoxs[1]);
            TheBoxs[2].DefuseColor = new Vector3(0.6f, 0.6f, 0.6f);
            //TheBoxs[3].ModelScale = new Vector3(3);
            //TheBoxs[3].Position = new Vector3(-10, 0, 0);
            //TheBoxs[3].AddAsChildOf(TheBoxs[1]);
            //TheBoxs[4].ModelScale = new Vector3(3);
            //TheBoxs[4].Position = new Vector3(0, -10, 0);
            //TheBoxs[4].AddAsChildOf(TheBoxs[1]);
            //TheBoxs[5].ModelScale = new Vector3(3);
            //TheBoxs[5].Position = new Vector3(0, 10, 0);
            //TheBoxs[5].AddAsChildOf(TheBoxs[1]);
            //TheBoxs[6].ModelScale = new Vector3(1);
            //TheBoxs[6].Position = new Vector3(5, 0, 0);
            //TheBoxs[6].AddAsChildOf(TheBoxs[2]);
            //TheBoxs[7].ModelScale = new Vector3(1);
            //TheBoxs[7].Position = new Vector3(-5, 0, 0);
            //TheBoxs[7].AddAsChildOf(TheBoxs[2]);
            //TheBoxs[8].ModelScale = new Vector3(1);
            //TheBoxs[8].Position = new Vector3(0, 5, 0);
            //TheBoxs[8].AddAsChildOf(TheBoxs[2]);
            //TheBoxs[9].ModelScale = new Vector3(1);
            //TheBoxs[9].Position = new Vector3(0, -5, 0);
            //TheBoxs[9].AddAsChildOf(TheBoxs[2]);

            ScoreDisplay.ProcessNumber(100, new Vector3(0, 200, 0), 1);
            WordDisplay.ProcessWords("This is a TEST", Vector3.Zero, 1);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState KBS = Keyboard.GetState();

            if (KBS != OldKeyState)
            {
                if (KBS.IsKeyDown(Keys.Space))
                {
                    TheBoxs[0].Enabled = !TheBoxs[0].Enabled;
                }
            }

            if (KBS.IsKeyDown(Keys.Left))
            {
                TheBoxs[0].PO.Velocity.X = -10;
            }
            else if (KBS.IsKeyDown(Keys.Right))
            {
                TheBoxs[0].PO.Velocity.X = 10;
            }
            else
            {
                TheBoxs[0].PO.Velocity.X = 0;
            }

            OldKeyState = Keyboard.GetState();

            Rotation += MathHelper.Pi * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }
    }
}
