using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;

namespace Highsight_Game_Jam_1
{
    class SmokeParticle : ModelEntity
    {
        Timer LifeTimer;

        public SmokeParticle(Game game, Camera camera, Model model) : base(game, camera, model)
        {
            LifeTimer = new Timer(game);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void BeginRun()
        {
            base.BeginRun();
        }

        public override void Update(GameTime gameTime)
        {
            if (LifeTimer.Elapsed)
                Enabled = false;

            base.Update(gameTime);
        }

        public void Spawn(Vector3 position, float size, float drift, float speed, float life)
        {
            Scale = Helper.RandomMinMax(size / 2, size);
            LifeTimer.Reset(Helper.RandomMinMax(life / 10, life));
            Vector3 velocity;
            velocity.Y = Helper.RandomMinMax(speed / 10, speed);
            velocity.X = Helper.RandomMinMax(-drift, drift);
            velocity.Z = Helper.RandomMinMax(-drift, drift);
            base.Spawn(position, velocity);
        }
    }
}
