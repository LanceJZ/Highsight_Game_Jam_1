using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

public enum GameState
{
    Over,
    InPlay,
    HighScore,
    MainMenu
};

namespace Highsight_Game_Jam_1
{
    class GameLogic : GameComponent
    {
        Camera TheCamera;
        Player ThePlayer;
        Shield TheShield;
        Enemy TheEnemy;

        GameState GameMode = GameState.MainMenu;
        KeyboardState OldKeyState;

        public GameState CurrentMode { get => GameMode; }
        public Player PlayerRef { get => ThePlayer; }
        public Shield ShieldRef { get => TheShield; }
        public Enemy EnemyRef { get => TheEnemy; }

        public GameLogic(Game game, Camera camera) : base(game)
        {
            TheCamera = camera;
            TheEnemy = new Enemy(game, camera, this);
            TheShield = new Shield(game, camera, this);
            ThePlayer = new Player(game, camera, this);

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

            BeginRun();
        }

        public void BeginRun()
        {

        }

        public override void Update(GameTime gameTime)
        {
            Input();

            base.Update(gameTime);
        }

        void Input()
        {
            KeyboardState KBS = Keyboard.GetState();

            if (KBS != OldKeyState)
            {
            }


            OldKeyState = Keyboard.GetState();
        }
    }
}
