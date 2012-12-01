using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace StasisCore
{
    public class TerrainRenderer
    {
        public const int MAX_TEXTURE_SIZE = 2048;
        private Game _game;
        private Effect _primitivesEffect;

        public TerrainRenderer(Game game)
        {
            _game = game;

            // Load content
            ContentManager content = new ContentManager(game as IServiceProvider);
            content.RootDirectory = "Content";
            _primitivesEffect = game.Content.Load<Effect>("effects/primitives");
        }

        // Primitives pass
        public Texture2D primitivesPass(Texture2D texture, float worldScale, TexturedVertexFormat[] vertices, int primitiveCount)
        {
            // Find boundaries
            Vector2 topLeftBoundary = new Vector2(vertices[0].position.X, vertices[0].position.Y);
            Vector2 bottomRightBoundary = new Vector2(vertices[0].position.X, vertices[0].position.Y);
            for (int i = 0; i < vertices.Length; i++)
            {
                topLeftBoundary.X = Math.Min(vertices[i].position.X, topLeftBoundary.X);
                topLeftBoundary.Y = Math.Min(vertices[i].position.Y, topLeftBoundary.Y);
                bottomRightBoundary.X = Math.Max(vertices[i].position.X, bottomRightBoundary.X);
                bottomRightBoundary.Y = Math.Max(vertices[i].position.Y, bottomRightBoundary.Y);
            }

            // Calculate width and height
            int width = (int)((bottomRightBoundary.X - topLeftBoundary.X) * worldScale);
            int height = (int)((bottomRightBoundary.Y - topLeftBoundary.Y) * worldScale);

            RenderTarget2D renderTarget = new RenderTarget2D(_game.GraphicsDevice, width, height);
            Texture2D baseTexture = new Texture2D(_game.GraphicsDevice, renderTarget.Width, renderTarget.Height);
            Color[] data = new Color[renderTarget.Width * renderTarget.Height];
            //Vector2 offset = topLeftBoundary * worldScale;

            Matrix viewMatrix = Matrix.CreateTranslation(new Vector3(topLeftBoundary, 0) * -1);
            viewMatrix *= Matrix.CreateScale(worldScale, -worldScale, 1);
            viewMatrix *= Matrix.CreateTranslation(new Vector3(-renderTarget.Width / 2, renderTarget.Height / 2, 0));
            _primitivesEffect.CurrentTechnique = _primitivesEffect.Techniques["generic"];
            _primitivesEffect.Parameters["world"].SetValue(Matrix.Identity);
            _primitivesEffect.Parameters["view"].SetValue(viewMatrix);
            _primitivesEffect.Parameters["projection"].SetValue(Matrix.CreateOrthographic(renderTarget.Width, renderTarget.Height, 0, 1));

            // Draw polygons
            _game.GraphicsDevice.SetRenderTarget(renderTarget);
            _game.GraphicsDevice.Clear(Color.Transparent);
            if (texture == null)
            {
                _primitivesEffect.CurrentTechnique.Passes["primitives"].Apply();
            }
            else
            {
                _primitivesEffect.CurrentTechnique.Passes["textured_primitives"].Apply();
                _game.GraphicsDevice.Textures[0] = texture;
            }
            _game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, primitiveCount, TexturedVertexFormat.VertexDeclaration);
            _game.GraphicsDevice.SetRenderTarget(null);

            // Save base texture
            renderTarget.GetData<Color>(data);
            baseTexture.SetData<Color>(data);

            // Cleanup
            renderTarget.Dispose();

            return baseTexture;
        }

        // Noise pass
        public Texture2D noisePass(Texture2D texture)
        {
            return null;
        }
    }
}
