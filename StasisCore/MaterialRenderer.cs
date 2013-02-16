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
        private GraphicsDevice _graphicsDevice;
        private ContentManager _contentManager;
        private SpriteBatch _spriteBatch;
        private Random _random;
        private Texture2D _perlinSource;
        private Texture2D _worleySource;
        private Texture2D pixel;
        private Effect _primitivesEffect;
        private Effect _noiseEffect;
        private Effect _textureEffect;

        public Effect primitivesEffect { get { return _primitivesEffect; } }

        public MaterialRenderer(GraphicsDevice graphicsDevice, ContentManager contentManager, SpriteBatch spriteBatch, int randomTextureWidth = 32, int randomTextureHeight = 32, int seed = 1234)
        {
            _graphicsDevice = graphicsDevice;
            _contentManager = contentManager;
            _spriteBatch = spriteBatch;
            
            // Load content
            _primitivesEffect = _contentManager.Load<Effect>("../StasisCoreContent/effects/primitives");
            _noiseEffect = _contentManager.Load<Effect>("../StasisCoreContent/effects/noise");
            _textureEffect = _contentManager.Load<Effect>("../StasisCoreContent/effects/texture");

            // Create random generator
            _random = new Random(seed);

            // Initialize random texture
            Color[] _perlinSourceData = new Color[randomTextureWidth * randomTextureHeight];
            for (int i = 0; i < randomTextureWidth; i++)
            {
                for (int j = 0; j < randomTextureHeight; j++)
                    _perlinSourceData[i + j * randomTextureWidth] = new Color((float)_random.Next(3) / 2, (float)_random.Next(3) / 2, (float)_random.Next(3) / 2);
            }
            _perlinSource = new Texture2D(_graphicsDevice, randomTextureWidth, randomTextureHeight);
            _perlinSource.SetData<Color>(_perlinSourceData);

            // Initialize worley texture
            Color[] data = new Color[randomTextureWidth * randomTextureHeight];
            for (int i = 0; i < randomTextureWidth; i++)
            {
                for (int j = 0; j < randomTextureHeight; j++)
                    data[i + j * randomTextureWidth] = new Color((float)_random.NextDouble(), (float)_random.NextDouble(), (float)_random.NextDouble(), (float)_random.NextDouble());
            }
            _worleySource = new Texture2D(_graphicsDevice, randomTextureWidth, randomTextureHeight);
            _worleySource.SetData<Color>(data);

            // Initialize pixel texture
            pixel = new Texture2D(_graphicsDevice, 1, 1);
            pixel.SetData<Color>(new[] { Color.White });
        }

        // Render material
        public Texture2D renderMaterial(Material material, List<Vector2> polygonPoints, float growthFactor)
        {
            // Calculate width and height
            Vector2 topLeft = polygonPoints[0];
            Vector2 bottomRight = polygonPoints[0];
            foreach (Vector2 polygonPoint in polygonPoints)
            {
                topLeft = Vector2.Min(topLeft, polygonPoint);
                bottomRight = Vector2.Max(bottomRight, polygonPoint);
            }

            // Create canvas
            Texture2D canvas = createCanvas((int)((bottomRight.X - topLeft.X) * Settings.BASE_SCALE), (int)((bottomRight.Y - topLeft.Y) * Settings.BASE_SCALE));

            // Recursively render layers
            canvas = recursiveRenderLayers(canvas, polygonPoints, growthFactor, material.rootLayer);

            return canvas;
        }

        // recursiveRenderLayers
        private Texture2D recursiveRenderLayers(Texture2D current, List<Vector2> polygonPoints, float growthFactor, MaterialLayer layer)
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
                        current = recursiveRenderLayers(current, polygonPoints, growthFactor, childLayer);
                    current = texturePass(current, current, LayerBlendType.Opaque, 1f, rootLayer.multiplier, rootLayer.baseColor);
                    break;

                case "group":
                    // Render child layers, and do a texture pass at the end
                    MaterialGroupLayer groupLayer = layer as MaterialGroupLayer;
                    Texture2D groupCanvas = createCanvas(current.Width, current.Height);
                    foreach (MaterialLayer childLayer in groupLayer.layers)
                        groupCanvas = recursiveRenderLayers(groupCanvas, polygonPoints, growthFactor, childLayer);
                    current = texturePass(current, groupCanvas, groupLayer.blendType, 1f, groupLayer.multiplier, groupLayer.baseColor);
                    break;

                case "texture":
                    MaterialTextureLayer textureLayer = layer as MaterialTextureLayer;
                    current = texturePass(
                        current,
                        ResourceController.getTexture(textureLayer.textureUID),
                        textureLayer.blendType,
                        textureLayer.scale,
                        textureLayer.multiplier,
                        textureLayer.baseColor);
                    break;

                case "noise":
                    MaterialNoiseLayer noiseLayer = layer as MaterialNoiseLayer;
                    current = noisePass(
                        current,
                        noiseLayer.noiseType,
                        noiseLayer.position,
                        noiseLayer.scale,
                        noiseLayer.frequency,
                        noiseLayer.gain,
                        noiseLayer.lacunarity,
                        noiseLayer.multiplier,
                        noiseLayer.fbmOffset,
                        noiseLayer.colorLow,
                        noiseLayer.colorHigh,
                        noiseLayer.iterations,
                        noiseLayer.blendType,
                        noiseLayer.invert,
                        noiseLayer.worleyFeature);
                    break;

                case "uniform_scatter":
                    MaterialUniformScatterLayer uniformLayer = layer as MaterialUniformScatterLayer;
                    current = uniformScatterPass(
                        current,
                        uniformLayer.textureUIDs,
                        uniformLayer.horizontalSpacing,
                        uniformLayer.verticalSpacing,
                        uniformLayer.jitter,
                        uniformLayer.baseColor,
                        uniformLayer.randomRed,
                        uniformLayer.randomGreen,
                        uniformLayer.randomBlue,
                        uniformLayer.randomAlpha);
                    break;

                case "radial_scatter":
                    MaterialRadialScatterLayer radialLayer = layer as MaterialRadialScatterLayer;
                    current = radialScatterPass(
                        current,
                        growthFactor,
                        radialLayer.textureUIDs,
                        radialLayer.a,
                        radialLayer.b,
                        radialLayer.intersections,
                        radialLayer.maxRadius,
                        radialLayer.arms,
                        radialLayer.twinArms,
                        radialLayer.flipArms,
                        radialLayer.useAbsoluteTextureAngle,
                        radialLayer.absoluteTextureAngle,
                        radialLayer.relativeTextureAngle,
                        radialLayer.textureAngleJitter,
                        radialLayer.jitter,
                        radialLayer.centerJitter,
                        radialLayer.centerOffset,
                        radialLayer.baseColor,
                        radialLayer.randomRed,
                        radialLayer.randomGreen, 
                        radialLayer.randomBlue,
                        radialLayer.randomAlpha);
                    break;

                case "edge_scatter":
                    MaterialEdgeScatterLayer edgeLayer = layer as MaterialEdgeScatterLayer;
                    current = edgeScatterPass(
                        current,
                        polygonPoints,
                        edgeLayer.textureUIDs,
                        edgeLayer.direction,
                        edgeLayer.threshold,
                        edgeLayer.hardCutoff,
                        edgeLayer.spacing,
                        edgeLayer.useAbsoluteAngle,
                        edgeLayer.absoluteAngle,
                        edgeLayer.relativeAngle,
                        edgeLayer.angleJitter,
                        edgeLayer.jitter,
                        edgeLayer.baseColor,
                        edgeLayer.randomRed,
                        edgeLayer.randomGreen,
                        edgeLayer.randomBlue,
                        edgeLayer.randomAlpha);
                    break;
            }

            return current;
        }

        // Create canvas
        private Texture2D createCanvas(int width, int height)
        {
            Texture2D canvas = new Texture2D(_graphicsDevice, width, height);
            Color[] data = new Color[width * height];
            for (int i = 0; i < (width * height); i++)
                data[i] = Color.Transparent;
            canvas.SetData<Color>(data);
            return canvas;
        }

        // Texture pass
        private Texture2D texturePass(Texture2D current, Texture2D texture, LayerBlendType blendType, float scale, float multiplier, Color baseColor)
        {
            // Initialize render targets and textures
            RenderTarget2D renderTarget = new RenderTarget2D(_graphicsDevice, current.Width, current.Height);
            Texture2D result = new Texture2D(_graphicsDevice, renderTarget.Width, renderTarget.Height);
            Color[] data = new Color[renderTarget.Width * renderTarget.Height];
            for (int i = 0; i < (renderTarget.Width * renderTarget.Height); i++)
                data[i] = Color.Transparent;
            result.SetData<Color>(data);

            // Handle missing texture
            if (texture == null)
            {
                texture = new Texture2D(_graphicsDevice, renderTarget.Width, renderTarget.Height);
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

            // Draw
            _graphicsDevice.SetRenderTarget(renderTarget);
            _graphicsDevice.Clear(Color.Transparent);
            _graphicsDevice.Textures[1] = texture;
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, _textureEffect);
            _spriteBatch.Draw(current, current.Bounds, baseColor);
            _spriteBatch.End();
            _graphicsDevice.SetRenderTarget(null);

            // Save base texture
            renderTarget.GetData<Color>(data);
            result.SetData<Color>(data);

            // Cleanup
            renderTarget.Dispose();

            return result;
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
            RenderTarget2D renderTarget = new RenderTarget2D(_graphicsDevice, current.Width, current.Height);

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
            _graphicsDevice.SetRenderTarget(renderTarget);
            _graphicsDevice.Textures[1] = _perlinSource;
            _graphicsDevice.Textures[2] = _worleySource;
            _graphicsDevice.Clear(Color.Transparent);
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
            _spriteBatch.Begin();
            _spriteBatch.Draw(current, current.Bounds, Color.White);
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, _noiseEffect);
            _spriteBatch.Draw(current, renderTarget.Bounds, Color.White);
            _spriteBatch.End();
            _graphicsDevice.SetRenderTarget(null);

            // Store
            Color[] data = new Color[current.Width * current.Height];
            Texture2D output = new Texture2D(_graphicsDevice, current.Width, current.Height);
            renderTarget.GetData<Color>(data);
            output.SetData<Color>(data);

            return output;
        }

        // Uniform scatter pass
        public Texture2D uniformScatterPass(
            Texture2D current,
            List<string> textureUIDs,
            float horizontalSpacing, 
            float verticalSpacing, 
            float jitter, 
            Color baseColor, 
            int randomRed, 
            int randomGreen, 
            int randomBlue, 
            int randomAlpha)
        {
            // Initialize render targets and textures
            RenderTarget2D renderTarget = new RenderTarget2D(_graphicsDevice, current.Width, current.Height);
            Texture2D result = new Texture2D(_graphicsDevice, renderTarget.Width, renderTarget.Height);
            Color[] data = new Color[renderTarget.Width * renderTarget.Height];

            // Load and validate textures
            List<Texture2D> textures = new List<Texture2D>();
            foreach (string textureUID in textureUIDs)
            {
                Texture2D texture = ResourceController.getTexture(textureUID);
                if (texture == null)
                    return result;
                textures.Add(texture);
            }
            if (textures.Count == 0)
                return current;

            // Draw
            _graphicsDevice.SetRenderTarget(renderTarget);
            _graphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteBatch.Draw(current, current.Bounds, Color.White);
            for (float i = 0; i <= current.Width; i += horizontalSpacing)
            {
                for (float j = 0; j <= current.Height; j += verticalSpacing)
                {
                    Vector2 position = new Vector2(i, j) + new Vector2(StasisMathHelper.floatBetween(0, jitter, _random), StasisMathHelper.floatBetween(0, jitter, _random));
                    float angle = StasisMathHelper.floatBetween(-3.14f, 3.14f, _random);
                    Texture2D texture = textures[_random.Next(0, textures.Count)];
                    Color actualColor = getRandomColor(baseColor, randomRed, randomGreen, randomBlue, randomAlpha);
                    _spriteBatch.Draw(texture, position, texture.Bounds, actualColor, angle, new Vector2(texture.Width, texture.Height) / 2, 1f, SpriteEffects.None, 0);
                }
            }
            _spriteBatch.End();
            _graphicsDevice.SetRenderTarget(null);

            // Save render target into texture
            renderTarget.GetData<Color>(data);
            result.SetData<Color>(data);

            // Cleanup
            renderTarget.Dispose();

            return result;
        }

        // Radial scatter pass
        public Texture2D radialScatterPass(
            Texture2D current,
            float growthFactor,
            List<string> textureUIDs, 
            float a, 
            float b, 
            float intersections, 
            float maxRadius, 
            int arms, 
            bool twinArms, 
            bool flipArms,
            bool useAbsoluteTextureAngle,
            float absoluteTextureAngle,
            float relativeTextureAngle,
            float textureAngleJitter,
            float jitter, 
            float centerJitter,
            Vector2 centerOffset, 
            Color baseColor, 
            int randomRed, 
            int randomGreen, 
            int randomBlue, 
            int randomAlpha)
        {
            // Initialize render targets and textures
            RenderTarget2D renderTarget = new RenderTarget2D(_graphicsDevice, current.Width, current.Height);
            Texture2D result = new Texture2D(_graphicsDevice, renderTarget.Width, renderTarget.Height);
            Color[] data = new Color[renderTarget.Width * renderTarget.Height];

            // Load and validate textures
            List<Texture2D> textures = new List<Texture2D>();
            foreach (string textureUID in textureUIDs)
            {
                Texture2D texture = ResourceController.getTexture(textureUID);
                if (texture == null)
                    return result;
                textures.Add(texture);
            }
            if (textures.Count == 0)
                return current;

            // Modify parameters based on growth factor (r is modified later)
            intersections *= growthFactor * growthFactor * growthFactor;
            arms = (int)Math.Ceiling(arms * growthFactor * growthFactor);
            maxRadius *= growthFactor;
            //jitter *= growthFactor;
            centerJitter *= growthFactor;
            centerOffset *= growthFactor;

            // Draw
            _graphicsDevice.SetRenderTarget(renderTarget);
            _graphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteBatch.Draw(current, current.Bounds, Color.White);
            float thetaIncrement = StasisMathHelper.pi * 2 / intersections;
            float armRotationIncrement = StasisMathHelper.pi * 2 / (float)arms;
            Vector2 center = centerOffset + new Vector2(current.Width, current.Height) / 2 + new Vector2((float)(_random.NextDouble() * 2 - 1), (float)(_random.NextDouble() * 2 - 1)) * centerJitter;
            for (int i = 0; i < arms; i++)
            {
                float theta = 0;
                float r = 0;
                while (r < maxRadius)
                {
                    r = a * (float)Math.Pow(StasisMathHelper.phi, b * (2f / StasisMathHelper.pi) * theta) * growthFactor;
                    if (r < maxRadius)
                    {
                        float modifiedTheta = (theta + armRotationIncrement * i) * (flipArms ? -1f : 1f);
                        float randomAngleValue = textureAngleJitter == 0 ? 0 : StasisMathHelper.floatBetween(-textureAngleJitter, textureAngleJitter, _random);
                        float textureAngle;
                        if (useAbsoluteTextureAngle)
                        {
                            textureAngle = absoluteTextureAngle + randomAngleValue;
                        }
                        else
                        {
                            textureAngle = relativeTextureAngle + modifiedTheta + randomAngleValue;
                        }
                        Vector2 j = new Vector2((float)(_random.NextDouble() * 2 - 1) * jitter, (float)(_random.NextDouble() * 2 - 1) * jitter);
                        Texture2D texture = textures[_random.Next(textures.Count)];
                        Color actualColor = getRandomColor(baseColor, randomRed, randomGreen, randomBlue, randomAlpha);
                        _spriteBatch.Draw(texture, new Vector2(r * (float)Math.Cos(modifiedTheta), r * (float)Math.Sin(modifiedTheta)) + j + center, texture.Bounds, actualColor, textureAngle, new Vector2(texture.Width, texture.Height) / 2, 1f, SpriteEffects.None, 0);
                        if (twinArms)
                        {
                            j = new Vector2((float)(_random.NextDouble() * 2 - 1) * jitter, (float)(_random.NextDouble() * 2 - 1) * jitter);
                            _spriteBatch.Draw(texture, new Vector2(r * (float)Math.Cos(-modifiedTheta), r * (float)Math.Sin(-modifiedTheta)) + j + center, texture.Bounds, actualColor, -textureAngle, new Vector2(texture.Width, texture.Height) / 2, 1f, SpriteEffects.None, 0);
                        }
                    }
                    theta += thetaIncrement;
                }
            }
            _spriteBatch.End();
            _graphicsDevice.SetRenderTarget(null);

            // Save render target into texture
            renderTarget.GetData<Color>(data);
            result.SetData<Color>(data);

            // Cleanup
            renderTarget.Dispose();

            return result;
        }

        // Edge scatter pass
        public Texture2D edgeScatterPass(
            Texture2D current,
            List<Vector2> polygonPoints,
            List<string> textureUIDs,
            Vector2 direction,
            float threshold,
            bool hardCutoff,
            float spacing,
            bool useAbsoluteAngle,
            float absoluteAngle,
            float relativeAngle,
            float angleJitter,
            float jitter,
            Color baseColor,
            int randomRed,
            int randomGreen,
            int randomBlue,
            int randomAlpha)
        {
            // Initialize render targets and textures
            RenderTarget2D renderTarget = new RenderTarget2D(_graphicsDevice, current.Width, current.Height);
            Texture2D result = new Texture2D(_graphicsDevice, renderTarget.Width, renderTarget.Height);
            Color[] data = new Color[renderTarget.Width * renderTarget.Height];

            // Load and validate textures
            List<Texture2D> textures = new List<Texture2D>();
            foreach (string textureUID in textureUIDs)
            {
                Texture2D texture = ResourceController.getTexture(textureUID);
                if (texture == null)
                    return result;
                textures.Add(texture);
            }
            if (textures.Count == 0)
                return current;

            // Validate polygon points
            if (polygonPoints == null || polygonPoints.Count < 3)
                return current;

            // Validate parameters
            spacing = Math.Max(0.05f, spacing);

            // Calculate half-texture offset
            Vector2 topLeft = polygonPoints[0];
            Vector2 bottomRight = polygonPoints[0];
            for (int i = 0; i < polygonPoints.Count; i++)
            {
                topLeft = Vector2.Min(polygonPoints[i], topLeft);
                bottomRight = Vector2.Max(polygonPoints[i], bottomRight);
            }

            // Draw
            _graphicsDevice.SetRenderTarget(renderTarget);
            _graphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteBatch.Draw(current, current.Bounds, Color.White);
            bool hasDirection = direction.X != 0 || direction.Y != 0;
            for (int i = 0; i < polygonPoints.Count; i++)
            {
                Vector2 pointA = polygonPoints[i];
                Vector2 pointB = polygonPoints[i == polygonPoints.Count - 1 ? 0 : i + 1];
                Vector2 relative = pointB - pointA;
                Vector2 normal = relative;
                float perpDot = 0;
                normal.Normalize();
                if (hasDirection)
                {
                    direction.Normalize();
                    perpDot = direction.X * normal.Y - direction.Y * normal.X;
                }
                if (!hasDirection || perpDot > -threshold)
                {
                    float relativeLength = relative.Length();
                    float currentPosition = 0f;
                    while (currentPosition < relativeLength)
                    {
                        float angle = 0;
                        if (useAbsoluteAngle)
                            angle = absoluteAngle + StasisMathHelper.floatBetween(-angleJitter, angleJitter, _random);
                        else
                            angle = (float)Math.Atan2(relative.Y, relative.X) + relativeAngle + StasisMathHelper.floatBetween(-angleJitter, angleJitter, _random);
                        Vector2 j = new Vector2((float)_random.NextDouble() * 2 - 1, (float)_random.NextDouble() * 2 - 1) * jitter;
                        Vector2 position = pointA + normal * currentPosition + j;
                        Texture2D texture = textures[_random.Next(textures.Count)];
                        Color actualColor = getRandomColor(baseColor, randomRed, randomGreen, randomBlue, randomAlpha);
                        _spriteBatch.Draw(texture, (position - topLeft) * Settings.BASE_SCALE, texture.Bounds, actualColor, angle, new Vector2(texture.Width, texture.Height) / 2, 1f, SpriteEffects.None, 0);
                        currentPosition += spacing;
                    }
                }
            }
            _spriteBatch.End();
            _graphicsDevice.SetRenderTarget(null);

            // Save render target into texture
            renderTarget.GetData<Color>(data);
            result.SetData<Color>(data);

            // Cleanup
            renderTarget.Dispose();

            return result;
        }

        // Random color helper
        private Color getRandomColor(Color baseColor, int randomRed, int randomGreen, int randomBlue, int randomAlpha)
        {
            int tintR = randomRed < 0 ? _random.Next(randomRed, 1) : _random.Next(randomRed + 1);
            int tintG = randomGreen < 0 ? _random.Next(randomGreen, 1) : _random.Next(randomGreen + 1);
            int tintB = randomBlue < 0 ? _random.Next(randomBlue, 1) : _random.Next(randomBlue + 1);
            int tintA = randomAlpha < 0 ? _random.Next(randomAlpha, 1) : _random.Next(randomAlpha + 1);
            return new Color(
                Math.Max(0, (int)baseColor.R + tintR),
                Math.Max(0, (int)baseColor.G + tintG),
                Math.Max(0, (int)baseColor.B + tintB),
                Math.Max(0, (int)baseColor.A + tintA));
        }
    }
}
