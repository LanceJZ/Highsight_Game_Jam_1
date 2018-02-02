using Microsoft.Xna.Framework;

namespace Highsight_Game_Jam_1
{
    public class Timer : GameComponent
    {
        private float TheSeconds;
        private float TheAmount;

        public float Seconds { get => TheSeconds; }
        public float TimeLeft { get => TheAmount - TheSeconds; }
        public bool Elapsed { get => (TheSeconds > TheAmount); }

        public float Amount
        {
            get => TheAmount;

            set
            {
                TheAmount = value;
                Reset();
            }
        }

        public Timer(Game game) : base(game)
        {
            Game.Components.Add(this);
        }

        public Timer(Game game, float amount) : base(game)
        {
            Amount = amount;
            Game.Components.Add(this);
        }

        public override void Initialize()
        {

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            TheSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Elapsed)
                Enabled = false;
        }

        public void Reset()
        {
            Enabled = true;
            TheSeconds = 0;
        }

        public void Reset(float time)
        {
            Amount = time;
        }
    }
}
