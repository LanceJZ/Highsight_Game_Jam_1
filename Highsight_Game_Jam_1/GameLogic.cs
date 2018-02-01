using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Highsight_Game_Jam_1
{
    enum State
    {
        Launched,
        Standby
    };

    enum GameState
    {
        Over,
        InPlay,
        HighScore,
        MainMenu
    };

    class GameLogic : GameComponent
    {
        Camera TheCamera;
        Player ThePlayer;
        Shield TheShield;
        Enemy TheEnemy;
        Destroyer TheDestroyer;
        NZone TheNZone;
        Stars TheStars;

        GameState GameMode = GameState.MainMenu;
        KeyboardState OldKeyState;

        public GameState CurrentMode { get => GameMode; }
        public Player PlayerRef { get => ThePlayer; }
        public Shield ShieldRef { get => TheShield; }
        public Enemy EnemyRef { get => TheEnemy; }
        public Destroyer DestroyerRef { get => TheDestroyer; }

        public GameLogic(Game game, Camera camera) : base(game)
        {
            TheCamera = camera;
            TheEnemy = new Enemy(game, camera, this);
            TheDestroyer = new Destroyer(game, camera, this);
            TheShield = new Shield(game, camera, this);
            TheNZone = new NZone(game, camera, this);
            ThePlayer = new Player(game, camera, this);
            TheStars = new Stars(game, camera, this);
            // Screen resolution is 1200 X 900.
            // Y positive is Up.
            // X positive is right of window when camera is at rotation zero.
            // Z positive is towards the camera when at rotation zero.
            // Positive rotation rotates CCW. Zero has front facing X positive. Pi/2 on Y faces Z negative.
            game.Components.Add(this);
        }

        public override void Initialize()
        {
            GameMode = GameState.InPlay;

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
