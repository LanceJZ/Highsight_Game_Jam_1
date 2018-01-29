using Microsoft.Xna.Framework;

namespace Highsight_Game_Jam_1
{
    public class Timer : GameComponent
    {
        private float TheSeconds;
        private float TheAmount;

        public float Seconds
        {
            get { return TheSeconds; }
        }

        public float Amount
        {
            get => TheAmount;

            set
            {
                TheAmount = value;
                Reset();
            }
        }

        public bool Elapsed
        {
            get
            {
                return (TheSeconds > TheAmount);
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

            if (!Elapsed)
                TheSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Reset()
        {
            Enabled = true;
            TheSeconds = 0;
        }

        public void Reset(float time)
        {
            TheAmount = time;
            Reset();
        }
    }
}
