#region Using
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Highsight_Game_Jam_1
{
    public class PositionedObject : GameComponent
    {
        #region Fields
        public PositionedObject ParentPO;
        public List<PositionedObject> ChildrenPOs;
        public List<PositionedObject> ParentPOs;
        // These are fields because XYZ are fields of Vector3, a struct,
        // so they do not get data binned as a property.
        public Vector3 Position = Vector3.Zero;
        public Vector3 Acceleration = Vector3.Zero;
        public Vector3 Velocity = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;
        public Vector3 WorldPosition = Vector3.Zero;
        public Vector3 WorldRotation = Vector3.Zero;
        public Vector3 RotationVelocity = Vector3.Zero;
        public Vector3 RotationAcceleration = Vector3.Zero;
        Vector2 TheHeightWidth;
        float TheElapsedGameTime;
        float TheScalePercent = 1;
        float TheGameScale = 1;
        float TheRadius = 0;
        bool TheHit;
        bool TheExplosionActive;
        bool IsPaused;
        bool IsMoveable = true;
        bool IsActiveDependent;
        bool IsDirectConnected;
        bool IsParent;
        bool IsChild;
        bool InDebugMode;
        #endregion
        #region Properties
        public float ElapsedGameTime { get => TheElapsedGameTime; }
        /// <summary>
        /// Scale by percent of original. If base of sprite, used to enlarge sprite.
        /// </summary>
        public float Scale { get => TheScalePercent; set => TheScalePercent = value; }
        /// <summary>
        /// Used for circle collusion. Sets radius of circle.
        /// </summary>
        public float Radius { get => TheRadius; set => TheRadius = value; }
        /// <summary>
        /// Enabled means this class is a parent, and has at least one child.
        /// </summary>
        public bool Parent { get => IsParent; set => IsParent = value; }
        /// <summary>
        /// Enabled means this class is a child to a parent.
        /// </summary>
        public bool Child { get => IsChild; set => IsChild = value; }
        /// <summary>
        /// Enabled tells class hit by another class.
        /// </summary>
        public bool Hit { get => TheHit; set => TheHit = value; }
        /// <summary>
        /// Enabled tells class an explosion is active.
        /// </summary>
        public bool ExplosionActive { get => TheExplosionActive; set => TheExplosionActive = value; }
        /// <summary>
        /// Enabled pauses class update.
        /// </summary>
        public bool Pause { get => IsPaused; set => IsPaused = value; }
        /// <summary>
        /// Enabled will move using velocity and acceleration.
        /// </summary>
        public bool Moveable { get => IsMoveable; set => IsMoveable = value; }
        /// <summary>
        /// Enabled causes the class to update. If base of Sprite, enables sprite to be drawn.
        /// </summary>
        public new bool Enabled
        {
            get => base.Enabled;

            set
            {
                base.Enabled = value;

                foreach (PositionedObject child in ChildrenPOs)
                {
                    if (child.ActiveDependent)
                        child.Enabled = value;
                }
            }
        }
        /// <summary>
        /// Enabled the active bool will mirror that of the parent.
        /// </summary>
        public bool ActiveDependent { get => IsActiveDependent; set => IsActiveDependent = value; }
        /// <summary>
        /// Enabled the position and rotation will always be the same as the parent.
        /// </summary>
        public bool DirectConnection { get => IsDirectConnected; set => IsDirectConnected = value; }
        /// <summary>
        /// Gets or sets the GameModel's AABB
        /// </summary>
        public bool Debug { set => InDebugMode = value; }

        public Vector2 WidthHeight { get => TheHeightWidth; set => TheHeightWidth = value; }

        public float GameScale { get => TheGameScale; set => TheGameScale = value; }

        public Rectangle BoundingBox
        {
            get => new Rectangle((int)Position.X, (int)Position.Y, (int)WidthHeight.X, (int)WidthHeight.Y);
        }
        #endregion
        #region Constructor
        /// <summary>
        /// This is the constructor that gets the Positioned Object ready for use and adds it to the Drawable Components list.
        /// </summary>
        /// <param name="game">The game class</param>
        public PositionedObject(Game game) : base(game)
        {
            ChildrenPOs = new List<PositionedObject>();
            ParentPOs = new List<PositionedObject>();
            Game.Components.Add(this);
        }
        #endregion
        #region Public Methods
        public override void Initialize()
        {
            base.Initialize();
            BeginRun();
        }

        public virtual void BeginRun() { }
        /// <summary>
        /// Allows the game component to be updated.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (IsMoveable)
            {
                base.Update(gameTime);

                TheElapsedGameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Velocity += Acceleration * TheElapsedGameTime;
                Position += Velocity * TheElapsedGameTime;
                RotationVelocity += RotationAcceleration * TheElapsedGameTime;
                Rotation += RotationVelocity * TheElapsedGameTime;

                Rotation = WrapAngle(Rotation);
            }

            if (IsChild)
            {
                if (DirectConnection)
                {
                    Position = ParentPO.Position;
                    Rotation = ParentPO.Rotation;
                }
                else
                {
                    WorldPosition = Vector3.Zero;
                    WorldRotation = Vector3.Zero;

                    foreach (PositionedObject po in ParentPOs)
                    {
                        WorldPosition += po.Position;
                        WorldRotation += po.Position;
                    }
                }

            }
            else
            {
                WorldPosition = Position;
                WorldRotation = Rotation;
            }

            base.Update(gameTime);
        }

        Vector3 WrapAngle(Vector3 angle)
        {
            if (angle.X < 0)
                angle.X += MathHelper.TwoPi;

            if (angle.Y < 0)
                angle.Y += MathHelper.TwoPi;

            if (angle.Z < 0)
                angle.Z += MathHelper.TwoPi;

            if (angle.X > MathHelper.TwoPi)
                angle.X -= MathHelper.TwoPi;

            if (angle.Y > MathHelper.TwoPi)
                angle.Y -= MathHelper.TwoPi;

            if (angle.Z > MathHelper.TwoPi)
                angle.Z -= MathHelper.TwoPi;

            return angle;
        }
        /// <summary>
        /// Add PO class or base PO class from AModel or Sprite as child of this class.
        /// Make sure all the parents of the parent are added before the children.
        /// </summary>
        /// <param name="parent">The parent to this class.</param>
        /// <param name="activeDependent">If this class is active when the parent is.</param>
        /// <param name="directConnection">Bind Position and Rotation to child.</param>
        public virtual void AddAsChildOf(PositionedObject parent, bool activeDependent,
            bool directConnection)
        {
            if (ParentPO != null)
                return;

            ActiveDependent = activeDependent;
            DirectConnection = directConnection;
            Child = true;
            ParentPO = parent;
            ParentPO.Parent = true;
            ParentPO.ChildrenPOs.Add(this);
            ParentPOs.Add(parent);

            for (int i = 0; i < ParentPOs.Count; i++)
            {
                if (ParentPOs[i].ParentPO != null && ParentPOs[i].ParentPO != parent)
                {
                    ParentPOs.Add(ParentPOs[i].ParentPO);
                }
            }
        }
        /// <summary>
        /// Adds child that is not directly connect.
        /// </summary>
        /// <param name="parrent">The parent to this class.</param>
        /// <param name="activeDependent">If this class is active when the parent is.</param>
        public virtual void AddAsChildOf(PositionedObject parrent, bool activeDependent)
        {
            AddAsChildOf(parrent, activeDependent, false);
        }
        /// <summary>
        /// Adds child that is active dependent and not directly connected.
        /// </summary>
        /// <param name="parrent">The Parent to this class.</param>
        public virtual void AddAsChildOf(PositionedObject parrent)
        {
            AddAsChildOf(parrent, true, false);
        }

        public void Remove()
        {
            Game.Components.Remove(this);
        }
        /// <summary>
        /// Circle collusion detection. Target circle will be compared to this class's.
        /// Will return true of they intersect. Only for use with 2D Z plane.
        /// </summary>
        /// <param name="Target">Position of target.</param>
        /// <param name="TargetRadius">Radius of target.</param>
        /// <returns></returns>
        public bool CirclesIntersect(Vector3 Target, float TargetRadius)
        {
            float distanceX = Target.X - Position.X;
            float distanceY = Target.Y - Position.Y;
            float radius = Radius + TargetRadius;

            if ((distanceX * distanceX) + (distanceY * distanceY) < radius * radius)
                return true;

            return false;
        }
        /// <summary>
        /// Circle collusion detection. Target circle will be compared to this class's.
        /// Will return true of they intersect. Only for use with 2D Z plane.
        /// </summary>
        /// <param name="Target">Target Positioned Object.</param>
        /// <returns></returns>
        public bool CirclesIntersect(PositionedObject Target)
        {
            float distanceX = Target.Position.X - Position.X;
            float distanceY = Target.Position.Y - Position.Y;
            float radius = Radius + Target.Radius;

            if ((distanceX * distanceX) + (distanceY * distanceY) < radius * radius)
                return true;

            return false;
        }
        /// <summary>
        /// Returns Vector3 direction of travel from origin to target. Y is ignored.
        /// </summary>
        /// <param name="origin">Vector3 of origin</param>
        /// <param name="target">Vector3 of target</param>
        /// <param name="magnitude">float of speed of travel</param>
        /// <returns>Vector3</returns>
        public Vector3 VelocityFromVectorsY(Vector3 origin, Vector3 target, float magnitude)
        {
            return VelocityFromAngleY(AngleFromVectorsY(origin, target), magnitude);
        }
        /// <summary>
        /// Returns Vector3 direction of travel to target. Y is ignored.
        /// </summary>
        /// <param name="target">Vector3 of target</param>
        /// <param name="magnitude">float of speed of travel</param>
        /// <returns>Vector3</returns>
        public Vector3 VelocityFromVectorsY(Vector3 target, float magnitude)
        {
            return VelocityFromAngleY(AngleFromVectorsY(target), magnitude);
        }
        /// <summary>
        /// Returns a float of the angle in radians derived from two Vector3 passed into it,
        /// using only the X and Z.
        /// </summary>
        /// <param name="origin">Vector3 of origin</param>
        /// <param name="target">Vector3 of target</param>
        /// <returns>Float</returns>
        /// <summary>
        /// Returns Vector3 direction of travel from origin to target. Z is ignored.
        /// </summary>
        /// <param name="origin">Vector3 of origin</param>
        /// <param name="target">Vector3 of target</param>
        /// <param name="magnitude">float of speed of travel</param>
        /// <returns>Vector3</returns>
        public Vector3 VelocityFromVectorsZ(Vector3 origin, Vector3 target, float magnitude)
        {
            return VelocityFromAngleZ(AngleFromVectorsZ(origin, target), magnitude);
        }
        /// <summary>
        /// Returns Vector3 direction of travel to target. Z is ignored.
        /// </summary>
        /// <param name="target">Vector3 of target</param>
        /// <param name="magnitude">float of speed of travel</param>
        /// <returns>Vector3</returns>
        public Vector3 VelocityFromVectorsZ(Vector3 target, float magnitude)
        {
            return VelocityFromAngleZ(AngleFromVectorsZ(target), magnitude);
        }
        /// <summary>
        /// Returns a float of the angle in radians derived from two Vector3 passed into it,
        /// using only the X and Z.
        /// </summary>
        /// <param name="origin">Vector3 of origin</param>
        /// <param name="target">Vector3 of target</param>
        /// <returns>Float</returns>
        public float AngleFromVectorsY(Vector3 origin, Vector3 target)
        {
            return (float)(Math.Atan2(-target.Z - -origin.Z, target.X - origin.X));
        }
        /// <summary>
        /// Returns a float of the angle in radians derived from two Vector3 passed into it,
        /// using only the X and Z.
        /// </summary>
        /// <param name="target">Vector3 of target</param>
        /// <returns>Float</returns>
        /// <summary>
        /// Returns a float of the angle in radians to target, using only the X and Y.
        /// </summary>
        /// <param name="target">Vector3 of target</param>
        /// <returns>Float</returns>
        public float AngleFromVectorsY(Vector3 target)
        {
            return (float)(Math.Atan2(-target.Z - -Position.Z, target.X - Position.X));
        }
        /// <summary>
        /// Returns a float of the angle in radians to target, using only the X and Y.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public float AngleFromVectorsZ(Vector3 origin, Vector3 target)
        {
            return (float)(Math.Atan2(target.Y - origin.Y, target.X - origin.X));
        }
        /// <summary>
        /// Returns a float of the angle in radians to target, using only the X and Y.
        /// </summary>
        /// <param name="target">Vector3 of target</param>
        /// <returns>Float</returns>
        public float AngleFromVectorsZ(Vector3 target)
        {
            return (float)(Math.Atan2(target.Y - Position.Y, target.X - Position.X));
        }

        public float RandomRadian()
        {
            return Helper.RandomMinMax(0, MathHelper.TwoPi);
        }

        public Vector3 RandomVelocity(float speed)
        {
            float ang = RandomRadian();
            float amt = Helper.RandomMinMax(speed * 0.15f, speed);
            return VelocityFromAngleZ(ang, amt);
        }

        public Vector3 RandomVelocity(float speed, float radianDirection)
        {
            float amt = Helper.RandomMinMax(speed * 0.15f, speed);
            return VelocityFromAngleZ(radianDirection, amt);
        }
        /// <summary>
        /// Returns a velocity with Z as the ground plane. Y as up.
        /// X as the Y, Y as the Z.
        /// </summary>
        /// <param name="angle">Angel as Vector2 Y and Z of object.</param>
        /// <param name="magnitude">How fast</param>
        /// <returns>Vector3 velocity</returns>
        public Vector3 VelocityFromAngle(Vector2 angle, float magnitude)
        {
            return new Vector3((float)Math.Cos(angle.X) * magnitude,
                (float)Math.Sin(angle.Y) * magnitude,
                -(float)(Math.Sin(angle.X) * magnitude));
        }
        /// <summary>
        /// Returns a Vector3 direction of travel from angle and magnitude.
        /// Only X and Z are calculated, Z = 0.
        /// </summary>
        /// <param name="rotation"></param>
        /// <param name="magnitude"></param>
        /// <returns>Vector3</returns>
        public Vector3 VelocityFromAngleY(float rotation, float magnitude)
        {
            return new Vector3((float)Math.Cos(rotation) * magnitude,
                0, -((float)Math.Sin(rotation) * magnitude));
        }
        /// <summary>
        /// Returns a Vector3 direction of travel from angle and magnitude.
        /// Only X and Y are calculated, Z = 0.
        /// </summary>
        /// <param name="rotation"></param>
        /// <param name="magnitude"></param>
        /// <returns>Vector3</returns>
        public Vector3 VelocityFromAngleZ(float rotation, float magnitude)
        {
            return new Vector3((float)Math.Cos(rotation) * magnitude,
                (float)Math.Sin(rotation) * magnitude, 0);
        }
        /// <summary>
        /// Returns a Vector3 direction of travel from random angle and set magnitude. Y is ignored.
        /// </summary>
        /// <param name="magnitude"></param>
        /// <returns>Vector3</returns>
        public Vector3 VelocityFromAngleY(float magnitude)
        {
            float angle = RandomRadian();
            return new Vector3((float)Math.Cos(angle) * magnitude, 0,
                -((float)Math.Sin(angle) * magnitude));
        }
        /// <summary>
        /// Returns a Vector3 direction of travel from random angle and set magnitude. Z is ignored.
        /// </summary>
        /// <param name="magnitude"></param>
        /// <returns>Vector3</returns>
        public Vector3 VelocityFromAngleZ(float magnitude)
        {
            float angle = RandomRadian();
            return new Vector3((float)Math.Cos(angle) * magnitude, (float)Math.Sin(angle) * magnitude, 0);
        }

        public Vector2 RandomEdge()
        {
            return new Vector2(Helper.WindowWidth * 0.5f,
                Helper.RandomMinMax(-Helper.WindowHeight * 0.45f, Helper.WindowHeight * 0.45f));
        }
        /// <summary>
        /// Aims at target using the Y ground Plane.
        /// Only X and Z are used in the calculation.
        /// </summary>
        /// <param name="target">Vector3</param>
        /// <param name="facingAngle">float</param>
        /// <param name="magnitude">float</param>
        /// <returns></returns>
        public float AimAtTargetY(Vector3 target, float facingAngle, float magnitude)
        {
            float turnVelocity = 0;
            float targetAngle = AngleFromVectorsY(target);
            float targetLessFacing = targetAngle - facingAngle;
            float facingLessTarget = facingAngle - targetAngle;

            if (Math.Abs(targetLessFacing) > MathHelper.Pi)
            {
                if (facingAngle > targetAngle)
                {
                    facingLessTarget = ((MathHelper.TwoPi - facingAngle) + targetAngle) * -1;
                }
                else
                {
                    facingLessTarget = (MathHelper.TwoPi - targetAngle) + facingAngle;
                }
            }

            if (facingLessTarget > 0)
            {
                turnVelocity = -magnitude;
            }
            else
            {
                turnVelocity = magnitude;
            }

            return turnVelocity;
        }

        public bool AimedAtTargetY(Vector3 target, float facingAngle, float accuricy)
        {
            float targetAngle = AngleFromVectorsY(target);
            float targetLessFacing = targetAngle - facingAngle;
            float facingLessTarget = facingAngle - targetAngle;

            if (Math.Abs(targetLessFacing) > MathHelper.Pi)
            {
                if (facingAngle > targetAngle)
                {
                    facingLessTarget = ((MathHelper.TwoPi - facingAngle) + targetAngle) * -1;
                }
                else
                {
                    facingLessTarget = (MathHelper.TwoPi - targetAngle) + facingAngle;
                }
            }

            if (facingLessTarget < accuricy && facingLessTarget > -accuricy)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Aims at target using the Z ground Plane.
        /// Only X and Y are used in the calculation.
        /// </summary>
        /// <param name="target">Vector3</param>
        /// <param name="facingAngle">float</param>
        /// <param name="magnitude">float</param>
        /// <returns></returns>
        public float AimAtTargetZ(Vector3 target, float facingAngle, float magnitude)
        {
            float turnVelocity = 0;
            float targetAngle = AngleFromVectorsZ(Position, target);
            float targetLessFacing = targetAngle - facingAngle;
            float facingLessTarget = facingAngle - targetAngle;

            if (Math.Abs(targetLessFacing) > MathHelper.Pi)
            {
                if (facingAngle > targetAngle)
                {
                    facingLessTarget = ((MathHelper.TwoPi - facingAngle) + targetAngle) * -1;
                }
                else
                {
                    facingLessTarget = (MathHelper.TwoPi - targetAngle) + facingAngle;
                }
            }

            if (facingLessTarget > 0)
            {
                turnVelocity = -magnitude;
            }
            else
            {
                turnVelocity = magnitude;
            }

            return turnVelocity;
        }

        public void CheckWindowBorders()
        {
            if (Position.X > Helper.WindowWidth * 0.5f)
                Position.X = Helper.WindowWidth * 0.5f;

            if (Position.X < -Helper.WindowWidth * 0.5f)
                Position.X = -Helper.WindowWidth * 0.5f;

            if (Position.Y > Helper.WindowHeight * 0.5f)
                Position.Y = Helper.WindowHeight * 0.5f;

            if (Position.Y < -Helper.WindowHeight * 0.5f)
                Position.Y = -Helper.WindowHeight * 0.5f;
        }

        public void CheckWindowSideBorders(float width)
        {
            if (Position.X + width > Helper.WindowWidth * 0.5f)
                Position.X = width + Helper.WindowWidth * 0.5f;

            if (Position.X - width < Helper.WindowWidth * 0.5f)
                Position.X = width - Helper.WindowWidth * 0.5f;
        }

        public void CheckWindowTopBottomBorders(float height)
        {
            if (Position.Y + height > Helper.WindowHeight * 0.5f)
                Position.Y = height + Helper.WindowHeight * 0.5f;

            if (Position.Y - height < -Helper.WindowHeight * 0.5f)
                Position.Y = height - Helper.WindowHeight * 0.5f;
        }
        #endregion
    }
}
