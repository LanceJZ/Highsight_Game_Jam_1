using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Highsight_Game_Jam_1
{
    class Display : PositionedObject
    {
        #region Fields
        GameLogic LogicRef;
        Camera CameraRef;
        Numbers ScoreNumbers;
        Letters[] GameOverWords = new Letters[2];
        List<ModelEntity> PlayerLifes = new List<ModelEntity>();
        Model PlayerModel;
        #endregion
        #region Properties

        #endregion
        #region Constructor
        public Display(Game game, Camera camera, GameLogic gameLogic) : base(game)
        {
            LogicRef = gameLogic;
            CameraRef = camera;
            ScoreNumbers = new Numbers(game);

            for (int i = 0; i < 2; i++)
                GameOverWords[i] = new Letters(game);
        }
        #endregion
        #region Initialize-Load-Begin
        public override void Initialize()
        {
            LoadContent();
            base.Initialize();

        }

        public void LoadContent()
        {
            PlayerModel = Helper.LoadModel("Player");
        }

        public override void BeginRun()
        {
            ScoreNumbers.Setup(new Vector3(-400, 420, 0), 2);

            for (int i = 0; i < 2; i++)
            {
                GameOverWords[i].Setup(new Vector3(-150 - (i * 240), 60 + (i * -50), 0), 3);
            } //390

            base.BeginRun();
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
        #endregion
        public void SetupWords()
        {
            GameOverWords[0].SetWords("Game Over");
            GameOverWords[1].SetWords("Press Enter for new game");
        }

        public void ShowGameEnd(bool show)
        {
            for (int i = 0; i < 2; i++)
            {
                GameOverWords[i].ShowWords(show);
            }
        }

        public void Score(int score)
        {
            ScoreNumbers.SetNumber(score);
        }

        public void SetPlayerLifes(int lifes)
        {
            foreach(ModelEntity life in PlayerLifes)
            {
                life.Enabled = false;
            }

            int newlifes = lifes - PlayerLifes.Count;

            float posY = 74;

            if (newlifes > 0)
            {
                for (int i = 0; i < newlifes; i++)
                {
                    PlayerLifes.Add(new ModelEntity(Game, CameraRef, PlayerModel));
                    PlayerLifes.Last().ModelScale = new Vector3(0.33f);
                    PlayerLifes.Last().PO.Position.Y = posY;
                    PlayerLifes.Last().PO.Position.Z = 15;
                    PlayerLifes.Last().PO.Rotation.Z = MathHelper.PiOver2;
                }
            }

            float posXstart = -100;

            for (int i = 0; i < lifes; i++)
            {
                PlayerLifes[i].Enabled = true;
                PlayerLifes[i].PO.Position.X = (i * 4) + posXstart;
                PlayerLifes[i].MatrixUpdate();
            }
        }
    }
}
