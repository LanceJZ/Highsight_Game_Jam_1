using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Highsight_Game_Jam_1
{
    class Camera : PositionedObject
    {
        #region Fields
        Matrix CameraRotation;
        Matrix CachedViewMatrix;
        Matrix CameraOriginalProjection;
        Vector3 LookAt = Vector3.Forward;
        Vector3 BaseCameraReference = Vector3.Backward;
        public bool NeedViewResync = true;
        public bool NeedLookUpResync;
        #endregion

        #region Properties
        public Matrix Projection { get; private set; }
        public Matrix WideProjection { get; private set; }
        public Matrix OrthographicProjection { get; private set; }

        public Matrix View
        {
            get
            {
                if (NeedLookUpResync)
                    UpdateLookAt();

                if (NeedViewResync)
                {
                    CameraRotation = Matrix.CreateFromAxisAngle(CameraRotation.Forward, Rotation.Z)
                        * Matrix.CreateFromAxisAngle(CameraRotation.Right, Rotation.X)
                        * Matrix.CreateFromAxisAngle(CameraRotation.Up, Rotation.Y);

                    CachedViewMatrix = Matrix.CreateLookAt(Position, LookAt, CameraRotation.Up);

                    //NeedViewResync = false;
                }

                return CachedViewMatrix;
            }
        }
        #endregion
        #region Constructor
        public Camera(Game game, Vector3 position, Vector3 rotation, float aspectRatio,
            float nearClip, float farClip) : base(game)
        {
            if (nearClip <= 0)
                nearClip = 0.1f;

            if (nearClip <= farClip + 0.01f)
                farClip += 0.1f;

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                aspectRatio, nearClip, farClip);
            WideProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                aspectRatio / 2, nearClip, farClip);
            OrthographicProjection = Matrix.CreateOrthographic(game.Window.ClientBounds.Width * 1.5f,
                Game.Window.ClientBounds.Height * 1.875f, nearClip, farClip);

            MoveTo(position, rotation);
        }
        #endregion

        #region Helper Methods
        public void MakeOrthGraphic()
        {
            CameraOriginalProjection = Projection;
            Projection = OrthographicProjection;
            NeedViewResync = true;
        }

        void UpdateLookAt()
        {
            Vector3 lookAtOffset = Vector3.Transform(BaseCameraReference, RotateMatrix(Rotation));
            LookAt = Position + lookAtOffset;
            NeedViewResync = true;
        }

        public void MoveTo(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
            UpdateLookAt();
        }

        public Matrix RotateMatrix(Vector3 rotation)
        {
            NeedViewResync = true;
            return Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);
        }
        #endregion

    }
}
