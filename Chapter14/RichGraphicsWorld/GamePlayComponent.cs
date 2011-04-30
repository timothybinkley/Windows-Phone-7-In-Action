using System;
using Microsoft.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RichGraphicsWorld
{
    public class GamePlayComponent
    {
        IGameplayInput input;

        Model ground;
        Matrix groundWorld;
        Model[] models = new Model[5];
        ShapeInfo[] shapes = new ShapeInfo[66];

        Vector3 unitX = Vector3.UnitX;
        Vector3 up = Vector3.Up;

        BoundingSphere playerPosition = new BoundingSphere(Vector3.Zero, 1.0f);
        float playerRotation = 0.0f;
        Matrix cameraProjection;
        Matrix cameraView;

        const float velocity = 0.5f;
        const float rotationVelocity = MathHelper.Pi / 90.0f;
        readonly Vector3 cameraOffset = new Vector3(0, 5, 0);
        readonly Vector3 lookAhead = new Vector3(0, 0, 20);

        Matrix[] transforms = new Matrix[2];
        private int collisionCount;

        public void Initialize(ContentManager content, IGameplayInput inputService)
        {
            input = inputService;

            ground = content.Load<Model>("ground");
            models[0] = content.Load<Model>("cube");
            models[1] = content.Load<Model>("cone");
            models[2] = content.Load<Model>("sphere");
            models[3] = content.Load<Model>("torus");
            models[4] = content.Load<Model>("octohedron");

            CalculateWorlds();
            CalculateView();
            CalculateProjection();
        }

        private void CalculateWorlds()
        {
            groundWorld = Matrix.CreateWorld(Vector3.Zero, Vector3.UnitX,
               Vector3.Up) * Matrix.CreateScale(50.0f, 1.0f, 50.0f);

            int currentModel = 0;
            int objectIndex = 0;
            Vector3 position = new Vector3();
            for (float x = -100.0f; x <= 100.0f; x += 20.0f)
            {
                for (float z = 0.0f; z <= 100.0f; z += 20.0f)
                {
                    position.X = x;
                    position.Z = z;
                    var shape = new ShapeInfo { ModelIndex = currentModel };

                    shape.Sphere.Center.X = x;
                    shape.Sphere.Center.Z = z;
                    shape.Sphere.Center.Y = models[currentModel].Meshes[0].BoundingSphere.Center.Y;
                    shape.Sphere.Radius = models[currentModel].Meshes[0].BoundingSphere.Radius;

                    Matrix.CreateWorld(ref position, ref unitX, ref up, out shape.World);
                    shapes[objectIndex] = shape;
                    objectIndex++;
                }
                currentModel++;
                if (currentModel >= models.Length)
                    currentModel = 0;
            }
        }

        private void CalculateView()
        {
            Vector3 cameraPosition = playerPosition.Center + cameraOffset;
            Vector3 cameraTarget = cameraPosition + Vector3.Transform(lookAhead,
                Matrix.CreateFromAxisAngle(Vector3.UnitY, playerRotation));

            Matrix.CreateLookAt(ref cameraPosition, ref cameraTarget,
                ref up, out cameraView);
        }

        private void CalculateProjection()
        {
            Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                .6f, 1.0f, 100.0f, out cameraProjection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns>true if an update occurred, false otherwise</returns>
        public bool Update()
        {
            bool playerMoved = false;

            Vector3 direction = Vector3.Transform(Vector3.UnitZ,
                        Matrix.CreateFromAxisAngle(Vector3.UnitY, playerRotation));

            if (input.MoveForward)
            {
                playerPosition.Center += direction * velocity;
                playerMoved = true;
            }

            if (input.MoveBackward)
            {
                playerPosition.Center -= direction * velocity;
                playerMoved = true;
            }

            if (input.TurnLeft)
            {
                playerRotation += rotationVelocity;
                if (playerRotation > MathHelper.TwoPi)
                    playerRotation -= MathHelper.TwoPi;
                playerMoved = true;
            }

            if (input.TurnRight)
            {
                playerRotation -= rotationVelocity;
                if (playerRotation < 0)
                    playerRotation += MathHelper.TwoPi;
                playerMoved = true;
            }

            if (playerMoved)
            {
                CalculateView();
            }

            bool collision = false;

            for (int index = 0; index < shapes.Length; index++)
            {
                if (!shapes[index].Collected && playerPosition.Intersects(shapes[index].Sphere))
                {
                    collision = true;
                    collisionCount++;
                    shapes[index].Collected = true;
                    VibrateController.Default.Start(TimeSpan.FromMilliseconds(20.0));
                }
            }

            return playerMoved || collision;
        }


        public void Draw()
        {
            DrawModel(ref ground, ref cameraView, ref cameraProjection, ref groundWorld);

            for (int index = 0; index < shapes.Length; index++)
            {
                var shape = shapes[index];
                if (!shape.Collected)
                {
                    Model model = models[shape.ModelIndex];
                    DrawModel(ref model, ref cameraView, ref cameraProjection, ref shape.World);
                }
            }
        }

        private void DrawModel(ref Model model, ref Matrix view, ref Matrix projection, ref Matrix world)
        {
            model.CopyAbsoluteBoneTransformsTo(transforms);

            for (int mIndex = 0; mIndex < model.Meshes.Count; mIndex++)
            {
                ModelMesh mesh = model.Meshes[mIndex];
                for (int eIndex = 0; eIndex < mesh.Effects.Count; eIndex++)
                {
                    BasicEffect effect = (BasicEffect)mesh.Effects[eIndex];
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * world;
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }



    }
}
