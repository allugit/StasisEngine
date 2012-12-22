using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Resources;
using StasisCore.Models;
using StasisCore.Controllers;

namespace StasisCore
{
    public class MaterialRenderer
    {
        public const int MAX_TEXTURE_SIZE = 2048;
        private Game _game;
        private SpriteBatch _spriteBatch;
        private Random _random;
        private Texture2D _perlinSource;
        private Texture2D _worleySource;
        private Texture2D pixel;
        private Effect _primitivesEffect;
        private Effect _noiseEffect;
        private Effect _textureEffect;

        public MaterialRenderer(Game game, SpriteBatch spriteBatch, int randomTextureWidth = 32, int randomTextureHeight = 32, int seed = 1234)
        {
            _game = game;
            _spriteBatch = spriteBatch;
            
            // Load content
            _primitivesEffect = game.Content.Load<Effect>("../StasisCoreContent/effects/primitives");
            _noiseEffect = game.Content.Load<Effect>("../StasisCoreContent/effects/noise");
            _textureEffect = game.Content.Load<Effect>("../StasisCoreContent/effects/texture");

            // Create random generator
            _random = new Random(seed);

            // Initialize random texture
            Color[] _perlinSourceData = new Color[randomTextureWidth * randomTextureHeight];
            for (int i = 0; i < randomTextureWidth; i++)
            {
                for (int j = 0; j < randomTextureHeight; j++)
                    _perlinSourceData[i + j * randomTextureWidth] = new Color((float)_random.Next(3) / 2, (float)_random.Next(3) / 2, (float)_random.Next(3) / 2);
            }
            _perlinSource = new Texture2D(game.GraphicsDevice, randomTextureWidth, randomTextureHeight);
            _perlinSource.SetData<Color>(_perlinSourceData);

            // Initialize worley texture
            Color[] data = new Color[randomTextureWidth * randomTextureHeight];
            for (int i = 0; i < randomTextureWidth; i++)
            {
                for (int j = 0; j < randomTextureHeight; j++)
                    data[i + j * randomTextureWidth] = new Color((float)_random.NextDouble(), (float)_random.NextDouble(), (float)_random.NextDouble(), (float)_random.NextDouble());
            }
            _worleySource = new Texture2D(game.GraphicsDevice, randomTextureWidth, randomTextureHeight);
            _worleySource.SetData<Color>(data);

            // Initialize pixel texture
            pixel = new Texture2D(_game.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new[] { Color.White });
        }

        // Render material
        public Texture2D renderMaterial(Material material, int textureWidth, int textureHeight)
        {
            // Create canvas
            Texture2D canvas = createCanvas(textureWidth, textureHeight);

            // Recursively render layers
            canvas = recursiveRenderLayers(canvas, material.rootLayer);

            return canvas;
        }

        // recursiveRenderLayers
        private Texture2D recursiveRenderLayers(Texture2D current, MaterialLayer layer)
        {
            // Stop rendering at disabled layers
            if (!layer.enabled)
                return current;

            switch (layer.type)
            {
                case "root":
                    // Render child layers without doing anything else
                    MaterialGroupLayer rootLayer = layer as MaterialGroupLayer;
                    foreach (MaterialLayer childLayer in rootLayer.layers)
                        current = recursiveRenderLayers(current, childLayer);
                    break;

                case "group":
                    // Render child layers, and do a texture pass at the end
                    MaterialGroupLayer groupLayer = layer as MaterialGroupLayer;
                    Texture2D groupCanvas = createCanvas(current.Width, current.Height);
                    foreach (MaterialLayer childLayer in groupLayer.layers)
                        groupCanvas = recursiveRenderLayers(groupCanvas, childLayer);
                    current = texturePass(current, groupCanvas, groupLayer.blendType, 1f, groupLayer.multiplier);
                    break;

                case "texture":
                    MaterialTextureLayer textureLayer = layer as MaterialTextureLayer;
                    current = texturePass(current, ResourceController.getTexture(textureLayer.textureUID), textureLayer.blendType, textureLayer.scale, textureLayer.multiplier);
                    break;

                case "noise":
                    MaterialNoiseLayer noiseLayer = layer as MaterialNoiseLayer;
                    current = noisePass(current, noiseLayer.noiseType, noiseLayer.position, noiseLayer.scale, noiseLayer.frequency, noiseLayer.gain, noiseLayer.lacunarity, noiseLayer.multiplier, noiseLayer.fbmOffset, noiseLayer.colorLow, noiseLayer.colorHigh, noiseLayer.iterations, noiseLayer.blendType, noiseLayer.invert, noiseLayer.worleyFeature);
                    break;

                case "scatter":
                    throw new NotImplementedException();
            }

            return current;
        }

        // Create canvas
        private Texture2D createCanvas(int width, int height)
        {
            /*
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
            */

            Texture2D canvas = new Texture2D(_game.GraphicsDevice, width, height);
            Color[] data = new Color[width * height];
            for (int i = 0; i < (width * height); i++)
                data[i] = Color.Transparent;
            canvas.SetData<Color>(data);
            return canvas;
        }

        /*
        // Render layer
        private Texture2D renderLayer(Texture2D current, TerrainLayerResource resource)
        {
            //Debug.Assert(resource.layers != null);

            // Don't render disabled layers
            if (!resource.enabled)
                return current;

            switch (resource.type)
            {
                case TerrainLayerType.Base:
                    PrimitivesProperties primitivesProperties = (resource as TerrainPrimitivesLayerResource).properties as PrimitivesProperties;
                    Texture2D baseTexture = TextureController.getTexture(primitivesProperties.textureTag);
                    result = primitivesPass(baseTexture, worldScale, vertices, primitiveCount);
                    break;

                case TerrainLayerType.Texture:
                    current = texturePass(current, (resource.properties as TextureProperties));
                    break;
                
                case TerrainLayerType.Noise:
                    current = noisePass(current, (resource.properties as NoiseProperties));
                    break;
            }

            // Recursively render layers
            foreach (TerrainLayerResource layer in resource.layers)
                current = renderLayer(current, layer);

            return current;
        }
        */

        /*
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
        */

        /*
        // Overloaded texture pass (normal)
        private Texture2D texturePass(Texture2D current, TextureLayerProperties options)
        {
            throw new NotImplementedException();
            //return texturePass(current, TextureController.getTexture(options.textureTag), options.blendType, options.scale, options.multiplier);
        }
        // Overloaded texture pass (group)
        private Texture2D texturePass(Texture2D canvas, Texture2D groupCanvas, GroupLayerProperties options)
        {
            return texturePass(canvas, groupCanvas, options.blendType, 1f, 1f);
        }
        */

        // Texture pass
        private Texture2D texturePass(Texture2D current, Texture2D texture, LayerBlendType blendType, float scale, float multiplier)
        {
            // Initialize render targets and textures
            RenderTarget2D renderTarget = new RenderTarget2D(_game.GraphicsDevice, current.Width, current.Height);
            Texture2D baseTexture = new Texture2D(_game.GraphicsDevice, renderTarget.Width, renderTarget.Height);
            Color[] data = new Color[renderTarget.Width * renderTarget.Height];
            for (int i = 0; i < (renderTarget.Width * renderTarget.Height); i++)
                data[i] = Color.Transparent;
            baseTexture.SetData<Color>(data);

            // Handle missing texture
            if (texture == null)
            {
                texture = new Texture2D(_game.GraphicsDevice, renderTarget.Width, renderTarget.Height);
                texture.SetData<Color>(data);
            }

            // Initialize shader
            switch (blendType)
            {
                case LayerBlendType.Opaque:
                    _textureEffect.CurrentTechnique = _textureEffect.Techniques["opaque"];
                    break;

                case LayerBlendType.Additive:
                    _textureEffect.CurrentTechnique = _textureEffect.Techniques["additive"];
                    break;

                case LayerBlendType.Overlay:
                    _textureEffect.CurrentTechnique = _textureEffect.Techniques["overlay"];
                    break;
            }
            _textureEffect.Parameters["canvasSize"].SetValue(new Vector2(current.Width, current.Height));
            _textureEffect.Parameters["textureSize"].SetValue(new Vector2(texture.Width, texture.Height));
            _textureEffect.Parameters["scale"].SetValue(scale);
            _textureEffect.Parameters["multiplier"].SetValue(multiplier);
            
            // Switch render target
            _game.GraphicsDevice.SetRenderTarget(renderTarget);
            _game.GraphicsDevice.Clear(Color.Transparent);
            _game.GraphicsDevice.Textures[1] = texture;

            // Draw
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, _textureEffect);
            _spriteBatch.Draw(current, current.Bounds, Color.White);
            _spriteBatch.End();

            // Switch render target
            _game.GraphicsDevice.SetRenderTarget(null);

            // Save base texture
            renderTarget.GetData<Color>(data);
            baseTexture.SetData<Color>(data);

            // Cleanup
            renderTarget.Dispose();

            return baseTexture;
        }

        // Noise pass
        private Texture2D noisePass(Texture2D current,
            NoiseType noiseType,
            Vector2 position,
            float scale,
            float frequency,
            float gain,
            float lacunarity,
            float multiplier,
            Vector2 fbmOffset,
            Color colorLow,
            Color colorHigh,
            int iterations,
            LayerBlendType blendType,
            bool invert,
            WorleyFeatureType worleyFeature)
        {
            // Initialize vertex shader properties
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, current.Width, current.Height, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            Matrix matrixTransform = halfPixelOffset * projection;
            _noiseEffect.Parameters["matrixTransform"].SetValue(matrixTransform);

            // Initialize render target
            RenderTarget2D renderTarget = new RenderTarget2D(_game.GraphicsDevice, current.Width, current.Height);

            // Aspect ratio
            float shortest = Math.Min(current.Width, current.Height);
            Vector2 aspectRatio = new Vector2(current.Width / shortest, current.Height / shortest);

            // Set options based on noise type and blend type
            Vector2 noiseSize = Vector2.Zero;
            if (noiseType == NoiseType.Perlin)
            {
                _noiseEffect.CurrentTechnique = _noiseEffect.Techniques["perlin_noise"];
                noiseSize = new Vector2(_perlinSource.Width, _perlinSource.Height);
            }
            else
            {
                _noiseEffect.CurrentTechnique = _noiseEffect.Techniques["worley_noise"];
                noiseSize = new Vector2(_worleySource.Width, _worleySource.Height);
            }

            // Draw noise effect to render target
            _game.GraphicsDevice.SetRenderTarget(renderTarget);
            _game.GraphicsDevice.Textures[1] = _perlinSource;
            _game.GraphicsDevice.Textures[2] = _worleySource;
            _game.GraphicsDevice.Clear(Color.Transparent);
            _noiseEffect.Parameters["aspectRatio"].SetValue(aspectRatio);
            _noiseEffect.Parameters["offset"].SetValue(position);
            _noiseEffect.Parameters["noiseScale"].SetValue(scale);
            _noiseEffect.Parameters["renderSize"].SetValue(new Vector2(current.Width, current.Height));
            _noiseEffect.Parameters["noiseSize"].SetValue(noiseSize);
            _noiseEffect.Parameters["noiseFrequency"].SetValue(frequency);
            _noiseEffect.Parameters["noiseGain"].SetValue(gain);
            _noiseEffect.Parameters["noiseLacunarity"].SetValue(lacunarity);
            _noiseEffect.Parameters["multiplier"].SetValue(multiplier);
            _noiseEffect.Parameters["fbmOffset"].SetValue(fbmOffset);
            _noiseEffect.Parameters["noiseLowColor"].SetValue(colorLow.ToVector4());
            _noiseEffect.Parameters["noiseHighColor"].SetValue(colorHigh.ToVector4());
            _noiseEffect.Parameters["fbmIterations"].SetValue(iterations);
            _noiseEffect.Parameters["blendType"].SetValue((int)blendType);
            _noiseEffect.Parameters["invert"].SetValue(invert);
            _noiseEffect.Parameters["worleyFeature"].SetValue((int)worleyFeature);
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, _noiseEffect);
            _spriteBatch.Draw(current, renderTarget.Bounds, Color.White);
            _spriteBatch.End();
            _game.GraphicsDevice.SetRenderTarget(null);

            // Store
            Color[] data = new Color[current.Width * current.Height];
            Texture2D output = new Texture2D(_game.GraphicsDevice, current.Width, current.Height);
            renderTarget.GetData<Color>(data);
            output.SetData<Color>(data);

            return output;
        }
    }
}
