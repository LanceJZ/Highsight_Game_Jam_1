using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Highsight_Game_Jam_1
{
    class Stars : PositionedObject
    {
        #region Fields
        GameLogic LogicRef;
        List<Cube> TheStars = new List<Cube>();
        Camera CameraRef;
        #endregion
        #region Properties

        #endregion
        #region Constructor
        public Stars(Game game, Camera camera, GameLogic gameLogic) : base(game)
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
            for (int i = 0; i < 50; i++)
            {
                TheStars.Add(new Cube(Game, CameraRef));
                TheStars.Last().Spawn(new Vector3(Helper.RandomMinMax(-110, 110),
                    Helper.RandomMinMax(-83, 83), -20));
                TheStars.Last().RotationVelocity = new Vector3(Helper.RandomMinMax(-3.1f, 3.1f),
                    Helper.RandomMinMax(-3.1f, 3.1f), Helper.RandomMinMax(-3.1f, 3.1f));
                TheStars.Last().ModelScale = new Vector3(Helper.RandomMinMax(0.15f, 0.5f));
                TheStars.Last().DefuseColor = new Vector3(Helper.RandomMinMax(0.1f, 1),
                    Helper.RandomMinMax(0.05f, 0.1f), Helper.RandomMinMax(0.1f, 1));
                TheStars.Last().EmissiveColor = new Vector3(Helper.RandomMinMax(0.5f, 1),
                    Helper.RandomMinMax(0.05f, 0.1f), Helper.RandomMinMax(0.5f, 1));
            }

            base.BeginRun();
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
        #endregion
    }
}
