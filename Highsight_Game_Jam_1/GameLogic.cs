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
        #region Fields
        Camera TheCamera;
        Player ThePlayer;
        Shield TheShield;
        Enemy TheEnemy;
        Destroyer TheDestroyer;
        NZone TheNZone;
        Stars TheStars;
        Display TheDisplay;

        GameState GameMode = GameState.MainMenu;
        KeyboardState OldKeyState;

        int TheScore;
        int PlayerLifes;
        #endregion
        #region Properties
        public GameState CurrentMode { get => GameMode; }
        public Player PlayerRef { get => ThePlayer; }
        public Shield ShieldRef { get => TheShield; }
        public Enemy EnemyRef { get => TheEnemy; }
        public Destroyer DestroyerRef { get => TheDestroyer; }
        public NZone NzoneRef { get => TheNZone; }
        #endregion
        public GameLogic(Game game, Camera camera) : base(game)
        {
            TheCamera = camera;
            TheDisplay = new Display(game, camera, this);
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
            base.Initialize();
        }

        public void LoadContent()
        {

        }

        public void BeginRun()
        {
            TheDisplay.SetupWords();
            EndGame();
        }

        public override void Update(GameTime gameTime)
        {
            Input();

            base.Update(gameTime);
        }

        public void NewGame()
        {
            TheDisplay.SetPlayerLifes(PlayerLifes = 3);
            AddPoints(TheScore = 0);
            GameMode = GameState.InPlay;
            ThePlayer.Reset();
            TheEnemy.Reset();
            TheShield.Reset();
            TheDestroyer.Reset();
            TheDisplay.ShowGameEnd(false);
        }

        public void EndGame()
        {
            GameMode = GameState.Over;
            ThePlayer.EndGame();
            TheEnemy.EndGame();
            TheDestroyer.Enabled = false;
            TheDisplay.ShowGameEnd(true);
        }

        public void AddPoints(int points)
        {
            TheScore += points;
            TheDisplay.Score(TheScore);
        }

        public void AddLife()
        {
            PlayerLifes++;
            TheDisplay.SetPlayerLifes(PlayerLifes);
        }

        public void LoseLife()
        {
            PlayerLifes--;
            TheDisplay.SetPlayerLifes(PlayerLifes);

            if (PlayerLifes <= 0)
            {
                EndGame();
                return;
            }

            ThePlayer.Explode();
        }

        void Input()
        {
            KeyboardState KBS = Keyboard.GetState();

            if (KBS != OldKeyState)
            {
                switch(GameMode)
                {
                    case GameState.Over:
                        if (KBS.IsKeyDown(Keys.Enter))
                        {
                            NewGame();
                        }
                        break;
                }
            }


            OldKeyState = Keyboard.GetState();
        }
    }
}
