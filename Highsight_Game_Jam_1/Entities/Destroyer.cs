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
    class Destroyer : ModelEntity
    {
        #region Fields
        GameLogic LogicRef;
        float Speed;
        #endregion
        #region Properties

        #endregion
        #region Constructor
        public Destroyer(Game game, Camera camera, GameLogic gameLogic) : base(game, camera)
        {
            LogicRef = gameLogic;

        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {
            Speed = 5;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            LoadModel("Destroyer");

            base.LoadContent();
        }

        public override void BeginRun()
        {
            base.BeginRun();

            Spawn(LogicRef.EnemyRef.Position);
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            FollowPlayer();

            base.Update(gameTime);
        }
        #endregion
        void FollowPlayer()
        {
            Velocity = PO.VelocityFromVectorsZ(LogicRef.PlayerRef.Position, Speed);
        }
    }
}
