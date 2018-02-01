using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Highsight_Game_Jam_1
{
    class NZone : PositionedObject
    {
        #region Fields
        GameLogic LogicRef;
        Camera CameraRef;
        List<Cube> Lines = new List<Cube>();

        #endregion
        #region Properties

        #endregion
        #region Constructor
        public NZone(Game game, Camera camera, GameLogic gameLogic) : base(game)
        {
            LogicRef = gameLogic;
            CameraRef = camera;
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

        }

        public override void BeginRun()
        {
            MakeZone();

            base.BeginRun();
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
        #endregion
        void MakeZone()
        {
            for (int i = 0; i < 40; i++)
            {
                Lines.Add(new Cube(Game, CameraRef));
                Lines.Last().Spawn(new Vector3(-50, (i * 4) - 78, 2));
                Lines.Last().ModelScale = new Vector3(30, 1, 1);
                Lines.Last().DefuseColor = new Vector3(Helper.RandomMinMax(0.1f, 1),
                    Helper.RandomMinMax(0.05f, 0.25f), Helper.RandomMinMax(0.1f, 1));
            }
        }
    }
}
