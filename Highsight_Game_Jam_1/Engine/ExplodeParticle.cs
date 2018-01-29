using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;

namespace Highsight_Game_Jam_1
{
    class ExplodeParticle : ModelEntity
    {
        Timer LifeTimer;

        public ExplodeParticle(Game game, Camera camera, Model model) : base(game, camera, model)
        {
            LifeTimer = new Timer(game);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (LifeTimer.Elapsed)
                Enabled = false;

            base.Update(gameTime);
        }

        public void Spawn(Vector3 position, float velocity, float scaleRange, float maxLife)
        {
            base.Spawn(position);

            Velocity = ThePO.RandomVelocity(velocity);
            Scale = Helper.RandomMinMax(0.5f + scaleRange, 1.5f + scaleRange);
            LifeTimer.Reset(Helper.RandomMinMax(0.1f, maxLife));
        }
    }
}
