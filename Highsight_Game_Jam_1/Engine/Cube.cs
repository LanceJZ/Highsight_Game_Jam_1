#region Using
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;
#endregion
namespace Highsight_Game_Jam_1
{
    class Cube : ModelEntity
    {
        #region Fields

        #endregion
        #region Properties

        #endregion
        #region Constructor
        public Cube(Game game, Camera camera) : base(game, camera)
        {
        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            LoadModel("Core/Cube");
            base.LoadContent();
        }

        public override void BeginRun()
        {
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
