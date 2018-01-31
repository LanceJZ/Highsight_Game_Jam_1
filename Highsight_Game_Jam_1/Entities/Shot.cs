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
    class Shot : Cube
    {
        #region Fields
        Shield ShieldRef;
        Timer LifeTimer;
        #endregion
        #region Properties

        #endregion
        #region Constructor
        public Shot(Game game, Camera camera, GameLogic gameLogic) : base(game, camera)
        {
            LifeTimer = new Timer(game);
            ShieldRef = gameLogic.ShieldRef;
        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {

            base.LoadContent();
        }

        public override void BeginRun()
        {
            base.BeginRun();

            Enabled = false;
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            if (LifeTimer.Elapsed)
                Enabled = false;

            if (PO.CheckPlayBorders(new Vector2(110, 83), Vector2.One))
                Enabled = false;

            ShieldRef.CheckColusion(this);

            base.Update(gameTime);
        }
        #endregion
        public override void Spawn(Vector3 position, Vector3 rotation, Vector3 velocity)
        {
            base.Spawn(position, rotation, velocity);
            LifeTimer.Reset(2.15f);
        }
    }
}
