using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Models;

namespace StasisCore
{
    public enum NoiseType
    {
        Perlin = 0,
        Worley,
        InverseWorley
    };

    public class TerrainRenderer
    {
        public const int MAX_TEXTURE_SIZE = 2048;
        private Game _game;
        private SpriteBatch _spriteBatch;
        private Random _random;
        private Color[] _randomTextureData;
        private Texture2D _randomTexture;
        private Texture2D _worleyTexture;
        private Effect _primitivesEffect;
        private Effect _noiseEffect;

        public TerrainRenderer(Game game, SpriteBatch spriteBatch, int randomTextureWidth = 32, int randomTextureHeight = 32, int seed = 1234)
        {
            _game = game;
            _spriteBatch = spriteBatch;
            
            // Load content
            _primitivesEffect = game.Content.Load<Effect>("../StasisCoreContent/effects/primitives");
            _noiseEffect = game.Content.Load<Effect>("../StasisCoreContent/effects/noise");

            // Create random generator
            _random = new Random(seed);

            // Initialize random texture
            _randomTextureData = new Color[randomTextureWidth * randomTextureHeight];
            for (int i = 0; i < randomTextureWidth; i++)
            {
                for (int j = 0; j < randomTextureHeight; j++)
                    _randomTextureData[i + j * randomTextureWidth] = new Color((float)_random.Next(3) / 2, (float)_random.Next(3) / 2, (float)_random.Next(3) / 2);
            }
            _randomTexture = new Texture2D(game.GraphicsDevice, randomTextureWidth, randomTextureHeight);
            _randomTexture.SetData<Color>(_randomTextureData);

            // Initialize worley texture
            Color[] data = new Color[randomTextureWidth * randomTextureHeight];
            for (int i = 0; i < randomTextureWidth; i++)
            {
                for (int j = 0; j < randomTextureHeight; j++)
                    data[i + j * randomTextureWidth] = new Color((float)_random.NextDouble(), (float)_random.NextDouble(), (float)_random.NextDouble(), (float)_random.NextDouble());
            }
            _worleyTexture = new Texture2D(game.GraphicsDevice, randomTextureWidth, randomTextureHeight);
            _worleyTexture.SetData<Color>(data);
        }

        // Render layer
        public Texture2D renderLayer(Texture2D result, TerrainLayer layer, float worldScale, TexturedVertexFormat[] vertices, int primitiveCount)
        {
            switch (layer.type)
            {
                case TerrainLayerType.Base:
                    Texture2D baseTexture = null;   // TEMPORARY -- should attempt to look up texture based on tag
                    result = primitivesPass(baseTexture, worldScale, vertices, primitiveCount);
                    break;
                
                case TerrainLayerType.Noise:
                    result = noisePass(result, (layer.properties as NoiseOptions));
                    break;
            }

            return result;
        }

        // Primitives pass
        private Texture2D primitivesPass(Texture2D texture, float worldScale, TexturedVertexFormat[] vertices, int primitiveCount)
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
        private Texture2D noisePass(Texture2D texture, NoiseOptions options)
        {
            // Initialize vertex shader properties
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, texture.Width, texture.Height, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            Matrix matrixTransform = halfPixelOffset * projection;
            _noiseEffect.Parameters["matrixTransform"].SetValue(matrixTransform);

            // Initialize render target
            RenderTarget2D renderTarget = new RenderTarget2D(_game.GraphicsDevice, texture.Width, texture.Height);

            // Aspect ratio
            float shortest = Math.Min(texture.Width, texture.Height);
            Vector2 aspectRatio = new Vector2(texture.Width / shortest, texture.Height / shortest);

            // Set options based on noise type
            Vector2 noiseSize = Vector2.Zero;
            bool perlinBasis = false;
            bool worleyBasis = false;
            bool invWorleyBasis = false;
            switch (options.noiseType)
            {
                case NoiseType.Perlin:
                    noiseSize = new Vector2(_randomTexture.Width, _randomTexture.Height);
                    perlinBasis = true;
                    break;

                case NoiseType.Worley:
                    noiseSize = new Vector2(_worleyTexture.Width, _worleyTexture.Height);
                    worleyBasis = true;
                    break;

                case NoiseType.InverseWorley:
                    noiseSize = new Vector2(_worleyTexture.Width, _worleyTexture.Height);
                    invWorleyBasis = true;
                    break;
            }

            // Draw noise effect to render target
            _game.GraphicsDevice.SetRenderTarget(renderTarget);
            _game.GraphicsDevice.Textures[1] = _randomTexture;
            _game.GraphicsDevice.Textures[2] = _worleyTexture;
            _game.GraphicsDevice.Clear(Color.Transparent);
            _noiseEffect.Parameters["aspectRatio"].SetValue(aspectRatio);
            _noiseEffect.Parameters["offset"].SetValue(options.position);
            _noiseEffect.Parameters["noiseScale"].SetValue(options.scale);
            _noiseEffect.Parameters["renderSize"].SetValue(new Vector2(texture.Width, texture.Height));
            _noiseEffect.Parameters["noiseSize"].SetValue(noiseSize);
            _noiseEffect.Parameters["noiseFrequency"].SetValue(options.noiseFrequency);
            _noiseEffect.Parameters["noiseGain"].SetValue(options.noiseGain);
            _noiseEffect.Parameters["noiseLacunarity"].SetValue(options.noiseLacunarity);
            _noiseEffect.Parameters["multiplier"].SetValue(options.multiplier);
            _noiseEffect.Parameters["fbmOffset"].SetValue(options.fbmOffset);
            _noiseEffect.Parameters["fbmPerlinBasis"].SetValue(perlinBasis);
            _noiseEffect.Parameters["fbmCellBasis"].SetValue(worleyBasis);
            _noiseEffect.Parameters["fbmInvCellBasis"].SetValue(invWorleyBasis);
            _noiseEffect.Parameters["fbmScale"].SetValue(options.fbmScale);
            _noiseEffect.Parameters["noiseLowColor"].SetValue(options.colorRangeLow.ToVector4());
            _noiseEffect.Parameters["noiseHighColor"].SetValue(options.colorRangeHigh.ToVector4());
            _noiseEffect.Parameters["fbmIterations"].SetValue(options.iterations);
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, _noiseEffect);
            _spriteBatch.Draw(texture, renderTarget.Bounds, Color.White);
            _spriteBatch.End();
            _game.GraphicsDevice.SetRenderTarget(null);

            // Store
            Color[] data = new Color[texture.Width * texture.Height];
            Texture2D output = new Texture2D(_game.GraphicsDevice, texture.Width, texture.Height);
            renderTarget.GetData<Color>(data);
            output.SetData<Color>(data);

            return output;
        }
    }
}
