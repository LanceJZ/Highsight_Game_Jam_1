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
            Speed = 10;

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

            Reset();
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            FollowPlayer();
            CollideWithPlayer();

            base.Update(gameTime);
        }
        #endregion
        public void Reset()
        {
            DefuseColor = new Vector3(0.6f, 0.1f, 0.4f);
            Spawn(LogicRef.EnemyRef.Position);
        }

        void FollowPlayer()
        {
            Velocity = PO.VelocityFromVectorsZ(LogicRef.PlayerRef.Position, Speed);
        }

        void CollideWithPlayer()
        {
            if (LogicRef.PlayerRef.Enabled)
            {
                if (Sphere.Intersects(LogicRef.PlayerRef.Sphere))
                {
                    if (!LogicRef.NzoneRef.CheckCollide(Sphere))
                    {
                        Enabled = false;
                        Reset();
                        LogicRef.LoseLife();
                    }
                }
            }
        }

    }
}
