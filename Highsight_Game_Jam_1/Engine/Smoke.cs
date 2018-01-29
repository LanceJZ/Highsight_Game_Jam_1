using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Highsight_Game_Jam_1
{
    class Smoke : GameComponent
    {
        Camera TheCamera;
        List<SmokeParticle> Particles;
        Model Cube;
        Vector3 Position;
        Vector3 TheColor = new Vector3(0.415f, 0.435f, 0.627f);
        float Radius;

        public Vector3 DefuseColor { set => TheColor = value; }

        public Smoke(Game game, Camera camera) : base(game)
        {
            TheCamera = camera;
            Particles = new List<SmokeParticle>();

            game.Components.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void LoadContent()
        {
            Cube = Helper.LoadModel("Core/Cube");
            Enabled = false;
        }

        public void BeginRun()
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (SmokeParticle particle in Particles)
            {
                if (!particle.Enabled)
                {
                    SpawnParticle(particle);
                }
            }

            base.Update(gameTime);
        }

        public void Spawn(Vector3 position, float radius, int minCount)
        {
            Enabled = true;
            Position = position;
            Radius = radius;
            int count = Helper.RandomMinMax(minCount, (int)(minCount + radius * 10));

            if (count > Particles.Count)
            {
                int more = count - Particles.Count;

                for (int i = 0; i < more; i++)
                {
                    Particles.Add(new SmokeParticle(Game, TheCamera, Cube));
                    Particles.Last().DefuseColor = TheColor;
                }
            }

            foreach (SmokeParticle particle in Particles)
            {
                SpawnParticle(particle);
            }
        }

        public void Kill()
        {
            foreach (SmokeParticle particle in Particles)
            {
                particle.Enabled = false;
                Enabled = false;
            }
        }

        void SpawnParticle(SmokeParticle particle)
        {
            Vector3 position = Position;
            position += new Vector3(Helper.RandomMinMax(-Radius, Radius),
                Helper.RandomMinMax(-Radius, Radius), 0);
            particle.Spawn(position);
        }
    }
}
