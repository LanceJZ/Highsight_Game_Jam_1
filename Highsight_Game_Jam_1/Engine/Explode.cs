using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Highsight_Game_Jam_1
{
    class Explode : GameComponent
    {
        List<ExplodeParticle> Particles;
        Model Cube;
        Camera TheCamera;
        Vector3 TheColor = Vector3.One;
        Vector3 TheEmissiveColor = Vector3.Zero;

        public Vector3 DefuseColor { set => TheColor = value; }
        public Vector3 EmissiveColor { set => TheEmissiveColor = value; }

        public Explode(Game game, Camera camera) : base(game)
        {
            Particles = new List<ExplodeParticle>();
            TheCamera = camera;

            game.Components.Add(this);
        }

        public override void Initialize()
        {
            TheColor = Vector3.One;
            base.Initialize();
            LoadContent();
            BeginRun();
        }

        public void LoadContent()
        {
            Cube = Helper.LoadModel("Core/Cube");
        }

        public void BeginRun()
        {

        }

        public override void Update(GameTime gameTime)
        {
            bool done = true;

            foreach (ExplodeParticle particle in Particles)
            {
                if (particle.Enabled)
                {
                    done = false;
                    break;
                }
            }

            if (done)
                Enabled = false;

            base.Update(gameTime);
        }

        public void Spawn(Vector3 position, float radius, int minCount, float speed,
            float scale, float life)
        {
            Enabled = true;
            int count = Helper.RandomMinMax(minCount, (int)(minCount + radius * 2));

            if (count > Particles.Count)
            {
                int more = count - Particles.Count;

                for (int i = 0; i < more; i++)
                {
                    Particles.Add(new ExplodeParticle(Game, TheCamera, Cube));
                    Particles.Last().DefuseColor = TheColor;
                    Particles.Last().EmissiveColor = TheEmissiveColor;
                }
            }

            foreach (ExplodeParticle particle in Particles)
            {
                position += new Vector3(Helper.RandomMinMax(-radius, radius),
                    Helper.RandomMinMax(-radius, radius), 0);

                particle.Spawn(position, speed, scale, life);
            }
        }

        public void Kill()
        {
            foreach (ExplodeParticle particle in Particles)
            {
                particle.Enabled = false;
                Enabled = false;
            }
        }
    }
}
