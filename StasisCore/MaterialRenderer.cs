using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Models;

namespace StasisCore
{
    public class MaterialRenderer
    {
        public const int CHUNK_SIZE = 256;
        public const int MAX_TEXTURE_SIZE = 2048;
        private GraphicsDevice _graphicsDevice;
        private ContentManager _contentManager;
        private SpriteBatch _spriteBatch;
        private Texture2D pixel;
        private Effect _primitivesEffect;
        private Effect _textureEffect;

        public Effect primitivesEffect { get { return _primitivesEffect; } }

        public MaterialRenderer(GraphicsDevice graphicsDevice, ContentManager contentManager, SpriteBatch spriteBatch)
        {
            _graphicsDevice = graphicsDevice;
            _contentManager = contentManager;
            _spriteBatch = spriteBatch;

            // Load content
            _primitivesEffect = _contentManager.Load<Effect>("effects/primitives");
            _textureEffect = _contentManager.Load<Effect>("effects/texture");

            // Initialize pixel texture
            pixel = new Texture2D(_graphicsDevice, 1, 1);
            pixel.SetData<Color>(new[] { Color.White });
        }

        // Trim transparent edges
        public Texture2D trimTransparentEdges(Texture2D canvas)
        {
            Color[] data1d = new Color[canvas.Width * canvas.Height];
            Color[,] data2d = new Color[canvas.Width, canvas.Height];
            Color[] newData;
            int halfWidth = canvas.Width / 2;
            int halfHeight = canvas.Height / 2;
            int lastTransparentColumnFromLeft;
            int lastTransparentColumnFromRight;
            int lastTransparentRowFromTop;
            int lastTransparentRowFromBottom;
            int newWidth;
            int newHeight;
            int widthTrim;
            int heightTrim;
            Func<int, int> findTransparentColumnFromLeft = (end) =>
            {
                int lastTransparentColumn = 0;
                for (int i = 0; i < end; i++)
                {
                    for (int j = 0; j < canvas.Height; j++)
                    {
                        if (data2d[i, j].A > 0)
                        {
                            return lastTransparentColumn;
                        }
                    }
                    lastTransparentColumn = i;
                }

                return lastTransparentColumn;
            };
            Func<int, int> findTransparentColumnFromRight = (end) =>
            {
                int lastTransparentColumn = canvas.Width - 1;
                for (int i = canvas.Width - 1; i >= end; i--)
                {
                    for (int j = 0; j < canvas.Height; j++)
                    {
                        if (data2d[i, j].A > 0)
                        {
                            return lastTransparentColumn;
                        }
                    }
                    lastTransparentColumn = i;
                }

                return lastTransparentColumn;
            };
            Func<int, int> findTransparentRowFromTop = (end) =>
            {
                int lastTransparentRow = 0;
                for (int j = 0; j < end; j++)
                {
                    for (int i = 0; i < canvas.Width; i++)
                    {
                        if (data2d[i, j].A > 0)
                        {
                            return lastTransparentRow;
                        }
                    }
                    lastTransparentRow = j;
                }

                return lastTransparentRow;
            };
            Func<int, int> findTransparentRowFromBottom = (end) =>
            {
                int lastTransparentRow = canvas.Height - 1;
                for (int j = canvas.Height - 1; j >= end; j--)
                {
                    for (int i = 0; i < canvas.Width; i++)
                    {
                        if (data2d[i, j].A > 0)
                        {
                            return lastTransparentRow;
                        }
                    }
                    lastTransparentRow = j;
                }

                return lastTransparentRow;
            };

            canvas.GetData<Color>(data1d);
            for (int i = 0; i < canvas.Width; i++)
            {
                for (int j = 0; j < canvas.Height; j++)
                {
                    data2d[i, j] = data1d[i + j * canvas.Width];
                }
            }

            lastTransparentColumnFromLeft = findTransparentColumnFromLeft(halfWidth);
            lastTransparentColumnFromRight = findTransparentColumnFromRight(halfWidth);
            lastTransparentRowFromTop = findTransparentRowFromTop(halfHeight);
            lastTransparentRowFromBottom = findTransparentRowFromBottom(halfHeight);

            widthTrim = Math.Min(lastTransparentColumnFromLeft, (canvas.Width - 1) - lastTransparentColumnFromRight);
            heightTrim = Math.Min(lastTransparentRowFromTop, (canvas.Height - 1) - lastTransparentRowFromBottom);
            newWidth = canvas.Width - (widthTrim * 2);
            newHeight = canvas.Height - (heightTrim * 2);

            if (newWidth < 1 || newHeight < 1)
            {
                canvas.Dispose();
                canvas = new Texture2D(_graphicsDevice, 1, 1);
                canvas.SetData<Color>(new[] { Color.Transparent });
            }

            newData = new Color[newWidth * newHeight];
            for (int i = 0; i < newWidth; i++)
            {
                for (int j = 0; j < newHeight; j++)
                {
                    newData[i + j * newWidth] = data2d[widthTrim + i, heightTrim + j];
                }
            }
            canvas.Dispose();
            canvas = new Texture2D(_graphicsDevice, newWidth, newHeight);
            canvas.SetData<Color>(newData);

            return canvas;
        }

        // Render material
        public Texture2D renderMaterial(Material material, List<Vector2> polygonPoints, float growthFactor, bool trimTransparentEdges)
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

            // Trim transparent edges
            if (trimTransparentEdges)
                canvas = this.trimTransparentEdges(canvas);

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
                        ResourceManager.getTexture(textureLayer.textureUID),
                        textureLayer.blendType,
                        textureLayer.scale,
                        textureLayer.multiplier,
                        textureLayer.baseColor);
                    break;

                case "perlin":
                    MaterialPerlinLayer perlinLayer = layer as MaterialPerlinLayer;
                    Texture2D perlinTemporary = perlinPass(
                        current,
                        perlinLayer.seed,
                        perlinLayer.position,
                        perlinLayer.scale,
                        perlinLayer.frequency,
                        perlinLayer.gain,
                        perlinLayer.lacunarity,
                        perlinLayer.multiplier,
                        perlinLayer.fbmOffset,
                        perlinLayer.colorLow,
                        perlinLayer.colorHigh,
                        perlinLayer.iterations,
                        perlinLayer.invert);

                    // TODO: Move this code inside perlinPass?
                    if (perlinLayer.blendType == LayerBlendType.Overlay)
                        current = texturePass(current, perlinTemporary, LayerBlendType.Overlay, 1f, 1f, Color.White);
                    else if (perlinLayer.blendType == LayerBlendType.Additive)
                        current = texturePass(current, perlinTemporary, LayerBlendType.Additive, 1f, 1f, Color.White);
                    else
                        current = perlinTemporary;

                    break;

                case "worley":
                    MaterialWorleyLayer worleyLayer = layer as MaterialWorleyLayer;
                    Texture2D worleyTemporary = worleyPass(
                        current,
                        worleyLayer.seed,
                        worleyLayer.position,
                        worleyLayer.scale,
                        worleyLayer.frequency,
                        worleyLayer.gain,
                        worleyLayer.lacunarity,
                        worleyLayer.multiplier,
                        worleyLayer.fbmOffset,
                        worleyLayer.colorLow,
                        worleyLayer.colorHigh,
                        worleyLayer.iterations,
                        worleyLayer.worleyFeature,
                        worleyLayer.invert);

                    // TODO: Move this code inside worleyPass?
                    if (worleyLayer.blendType == LayerBlendType.Overlay)
                        current = texturePass(current, worleyTemporary, LayerBlendType.Overlay, 1f, 1f, Color.White);
                    else if (worleyLayer.blendType == LayerBlendType.Additive)
                        current = texturePass(current, worleyTemporary, LayerBlendType.Additive, 1f, 1f, Color.White);
                    else
                        current = worleyTemporary;

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
                        radialLayer.scaleWithGrowthFactor,
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
                        radialLayer.minTextureSize,
                        radialLayer.maxTextureSize,
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
        public Texture2D createCanvas(int width, int height)
        {
            Texture2D canvas = new Texture2D(_graphicsDevice, width, height);
            Color[] data = new Color[width * height];
            for (int i = 0; i < (width * height); i++)
                data[i] = Color.Transparent;
            canvas.SetData<Color>(data);
            return canvas;
        }

        // Texture pass
        public Texture2D texturePass(Texture2D current, Texture2D texture, LayerBlendType blendType, float scale, float multiplier, Color baseColor)
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

        // perlinWeight -- Weight function for interpolating
        public float perlinWeight(float x)
        {
            return x * x * x * (x * (x * 6 - 15) + 10);
        }

        // perlin -- Calculate perlin noise
        // reference: "Simplex Noise Demystified" http://webstaff.itn.liu.se/~stegu/simplexnoise/simplexnoise.pdf
        private float perlin(
            Vector2[,] grid,
            int gridWidth,
            int gridHeight,
            Vector2 position)
        {
            int X = (int)Math.Floor(position.X);
            int Y = (int)Math.Floor(position.Y);

            // Get relative xy coordinates of point within that cell
            float x = position.X - X;
            float y = position.Y - Y;

            // Wrap the integer cells
            int x0 = StasisMathHelper.mod(X, gridWidth);
            int x1 = StasisMathHelper.mod(X + 1, gridWidth);
            int y0 = StasisMathHelper.mod(Y, gridHeight);
            int y1 = StasisMathHelper.mod(Y + 1, gridHeight);

            // Get gradients
            Vector2 g00 = grid[x0, y0];
            Vector2 g10 = grid[x1, y0];
            Vector2 g01 = grid[x0, y1];
            Vector2 g11 = grid[x1, y1];

            // Calculate noise contributions from each of the eight corners
            float n00 = Vector2.Dot(g00, new Vector2(x, y));
            float n10 = Vector2.Dot(g10, new Vector2(x - 1, y));
            float n01 = Vector2.Dot(g01, new Vector2(x, y - 1));
            float n11 = Vector2.Dot(g11, new Vector2(x - 1, y - 1));

            // Compute the fade curve value for each of x, y, z
            float u = perlinWeight(x);
            float v = perlinWeight(y);

            // Interpolate along x the contributions from each of the corners
            float nx00 = MathHelper.Lerp(n00, n10, u);
            float nx10 = MathHelper.Lerp(n01, n11, u);

            // Interpolate the results along y
            float value = MathHelper.Lerp(nx00, nx10, v);

            return value;
        }

        // fbmPerlin -- Fractional Brownian Motion using perlin
        public float fbmPerlin(
            Vector2[,] grid,
            int gridWidth,
            int gridHeight,
            Vector2 position,
            float scale,
            float frequency,
            float gain,
            float lacunarity,
            Vector2 fbmOffset,
            int iterations)
        {
            float total = 0;
            float amplitude = gain;

            for (int i = 0; i < iterations; i++)
            {
                total += perlin(grid, gridWidth, gridHeight, position * frequency + total * fbmOffset) * amplitude;
                frequency *= lacunarity;
                amplitude *= gain;
            }

            return total;
        }

        // perlinPass -- Renders a perlin texture using supplied properties
        public Texture2D perlinPass(
            Texture2D current,
            int seed,
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
            bool invert)
        {
            Texture2D output = new Texture2D(_graphicsDevice, current.Width, current.Height);
            Color[] data = new Color[output.Width * output.Height];
            int gridWidth = 32;
            int gridHeight = 32;
            Vector2[,] grid = new Vector2[gridWidth, gridHeight];
            Random rng = new Random(seed);
            int chunkCount = (int)Math.Floor((float)output.Width / (float)CHUNK_SIZE) + 1;
            
            // Create gradient grid
            for (int i = 0; i < gridWidth; i++)
            {
                for (int j = 0; j < gridHeight; j++)
                {
                    grid[i, j] = new Vector2(
                        StasisMathHelper.floatBetween(-1, 1, rng),
                        StasisMathHelper.floatBetween(-1, 1, rng));
                }
            }

            // Calculate values
            Parallel.For(0, chunkCount, (count) =>
            {
                int startIndex = count * CHUNK_SIZE;
                int endIndex = Math.Min((count + 1) * CHUNK_SIZE, output.Width);
                for (int i = startIndex; i < endIndex; i++)
                {
                    for (int j = 0; j < output.Height; j++)
                    {
                        // Position
                        Vector2 p = new Vector2(
                            (float)i / (float)gridWidth,
                            (float)j / (float)gridHeight);
                        p += position;
                        p /= scale;

                        float value = fbmPerlin(
                            grid,
                            gridWidth,
                            gridHeight,
                            p,
                            scale,
                            frequency,
                            gain,
                            lacunarity,
                            fbmOffset,
                            iterations);

                        // Change range of value to [0, 1] instead of [-1, 1]
                        value = (value + 1) / 2f;
                        value = Math.Max(0, Math.Min(1, value));

                        // Multiply value
                        value *= multiplier;

                        // Invert value if necessary
                        if (invert)
                            value = 1 - value;

                        Color color = Color.Lerp(colorLow, colorHigh, value);
                        data[i + j * output.Width] = color;
                    }
                }
            });

            output.SetData<Color>(data);
            return output;
        }

        // worley -- Calculates a worley value
        private float worley(
            Vector2[,] grid,
            int gridWidth,
            int gridHeight,
            Vector2 position,
            WorleyFeatureType worleyFeatureType)
        {
            int xi = (int)Math.Floor(position.X);
            int yi = (int)Math.Floor(position.Y);

            float xf = position.X - (float)xi;
            float yf = position.Y - (float)yi;

            float distance1 = 9999999;
            float distance2 = 9999999;

            for (int y = -2; y <= 2; y++)
            {
                for (int x = -2; x <= 2; x++)
                {
                    // Find feature point grid indices
                    int fpx = (xi + x) % gridWidth;
                    int fpy = (yi + y) % gridHeight;
                    fpx = fpx < 0 ? fpx + gridWidth : fpx;
                    fpy = fpy < 0 ? fpy + gridHeight : fpy;

                    // Get feature point by getting gradient at grid cell and modify the coordinates
                    Vector2 fp = grid[fpx, fpy];
                    fp.X += (float)x - xf;
                    fp.Y += (float)y - yf;

                    // Calculate distances
                    float distance = fp.LengthSquared();
                    if (distance < distance1)
                    {
                        distance2 = distance1;
                        distance1 = distance;
                    }
                    else if (distance < distance2)
                    {
                        distance2 = distance;
                    }
                }
            }

            // Determine final value based on feature type
            float value = 0;

            if (worleyFeatureType == WorleyFeatureType.F1)
            {
                value = (float)Math.Sqrt(distance1);
            }
            else if (worleyFeatureType == WorleyFeatureType.F2)
            {
                value = (float)Math.Sqrt(distance2);
            }
            else if (worleyFeatureType == WorleyFeatureType.F2mF1)
            {
                value = (float)Math.Sqrt(distance2 - distance1);
            }

            return value;
        }

        // fbmWorley -- Uses Fractional Brownian Motion to calculate worley noise
        private float fbmWorley(
            Vector2[,] grid,
            int gridWidth,
            int gridHeight,
            Vector2 position,
            float frequency,
            float gain,
            float lacunarity,
            Vector2 fbmOffset,
            int iterations,
            WorleyFeatureType worleyFeatureType)
        {
            float total = 0;
	        float amplitude = gain;

	        for (int i = 0; i < iterations; i++)
	        {
		        total += worley(grid, gridWidth, gridHeight, position * frequency + total * fbmOffset, worleyFeatureType) * amplitude;
		        frequency *= lacunarity;
		        amplitude *= gain;
	        }

	        return total;
        }

        // Worley noise pass
        public Texture2D worleyPass(
            Texture2D current,
            int seed,
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
            WorleyFeatureType worleyFeatureType,
            bool invert)
        {
            Texture2D output = new Texture2D(_graphicsDevice, current.Width, current.Height);
            Color[] data = new Color[output.Width * output.Height];
            int gridWidth = 32;
            int gridHeight = 32;
            Vector2[,] grid = new Vector2[gridWidth, gridHeight];
            Random rng = new Random(seed);
            int chunkCount = (int)Math.Floor((float)output.Width / (float)CHUNK_SIZE) + 1;

            // Create gradient grid
            for (int i = 0; i < gridWidth; i++)
            {
                for (int j = 0; j < gridHeight; j++)
                {
                    grid[i, j] = new Vector2(
                        StasisMathHelper.floatBetween(-1, 1, rng),
                        StasisMathHelper.floatBetween(-1, 1, rng));
                }
            }

            Parallel.For(0, chunkCount, (count) =>
            {
                int startIndex = count * CHUNK_SIZE;
                int endIndex = Math.Min((count + 1) * CHUNK_SIZE, output.Width);
                for (int i = startIndex; i < endIndex; i++)
                {
                    for (int j = 0; j < output.Height; j++)
                    {
                        Vector2 p = new Vector2(i, j) / new Vector2(gridWidth, gridHeight);
                        p += position;
                        p /= scale;

                        float value = fbmWorley(
                            grid,
                            gridWidth,
                            gridHeight,
                            p,
                            frequency,
                            gain,
                            lacunarity,
                            fbmOffset,
                            iterations,
                            worleyFeatureType);

                        // Clamp values
                        value = Math.Max(0, Math.Min(1, value));

                        // Multiply value
                        value *= multiplier;

                        // Invert value if necessary
                        if (invert)
                            value = 1 - value;

                        Color color = Color.Lerp(colorLow, colorHigh, value);
                        data[i + j * output.Width] = color;
                    }
                }
            });

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
            Random rng = new Random();

            // Initialize render targets and textures
            RenderTarget2D renderTarget = new RenderTarget2D(_graphicsDevice, current.Width, current.Height);
            Texture2D result = new Texture2D(_graphicsDevice, renderTarget.Width, renderTarget.Height);
            Color[] data = new Color[renderTarget.Width * renderTarget.Height];

            // Load and validate textures
            List<Texture2D> textures = new List<Texture2D>();
            foreach (string textureUID in textureUIDs)
            {
                Texture2D texture = ResourceManager.getTexture(textureUID);
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
                    Vector2 position = new Vector2(i, j) + new Vector2(StasisMathHelper.floatBetween(0, jitter, rng), StasisMathHelper.floatBetween(0, jitter, rng));
                    float angle = StasisMathHelper.floatBetween(-3.14f, 3.14f, rng);
                    Texture2D texture = textures[rng.Next(0, textures.Count)];
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
            bool scaleWithGrowthFactor,
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
            float minTextureScale,
            float maxTextureScale,
            int randomRed,
            int randomGreen,
            int randomBlue,
            int randomAlpha)
        {
            Random rng = new Random();

            // Initialize render targets and textures
            RenderTarget2D renderTarget = new RenderTarget2D(_graphicsDevice, current.Width, current.Height);
            Texture2D result = new Texture2D(_graphicsDevice, renderTarget.Width, renderTarget.Height);
            Color[] data = new Color[renderTarget.Width * renderTarget.Height];

            // Load and validate textures
            List<Texture2D> textures = new List<Texture2D>();
            foreach (string textureUID in textureUIDs)
            {
                Texture2D texture = ResourceManager.getTexture(textureUID);
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
            jitter *= Math.Max(growthFactor, 0.1f);
            centerJitter *= growthFactor;
            centerOffset *= growthFactor;

            // Draw
            _graphicsDevice.SetRenderTarget(renderTarget);
            _graphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteBatch.Draw(current, current.Bounds, Color.White);
            float thetaIncrement = StasisMathHelper.pi * 2 / intersections;
            float armRotationIncrement = StasisMathHelper.pi * 2 / (float)arms;
            Vector2 center = centerOffset + new Vector2(current.Width, current.Height) / 2 + new Vector2((float)(rng.NextDouble() * 2 - 1), (float)(rng.NextDouble() * 2 - 1)) * centerJitter;
            for (int i = 0; i < arms; i++)
            {
                float theta = 0;
                float r = 0;
                while (r < maxRadius)
                {
                    r = a * (float)Math.Pow(StasisMathHelper.phi, b * (2f / StasisMathHelper.pi) * theta) * growthFactor;
                    if (r < maxRadius)
                    {
                        float textureScale = StasisMathHelper.floatBetween(minTextureScale, maxTextureScale, rng);
                        float modifiedTheta = (theta + armRotationIncrement * i) * (flipArms ? -1f : 1f);
                        float randomAngleValue = textureAngleJitter == 0 ? 0 : StasisMathHelper.floatBetween(-textureAngleJitter, textureAngleJitter, rng);
                        float textureAngle;
                        if (useAbsoluteTextureAngle)
                        {
                            textureAngle = absoluteTextureAngle + randomAngleValue;
                        }
                        else
                        {
                            textureAngle = relativeTextureAngle + modifiedTheta + randomAngleValue;
                        }
                        Vector2 j = new Vector2((float)(rng.NextDouble() * 2 - 1) * jitter, (float)(rng.NextDouble() * 2 - 1) * jitter);
                        Texture2D texture = textures[rng.Next(textures.Count)];
                        Color actualColor = getRandomColor(baseColor, randomRed, randomGreen, randomBlue, randomAlpha);
                        //float textureScale = scaleWithGrowthFactor ? growthFactor : 1f;
                        _spriteBatch.Draw(texture, new Vector2(r * (float)Math.Cos(modifiedTheta), r * (float)Math.Sin(modifiedTheta)) + j + center, texture.Bounds, actualColor, textureAngle, new Vector2(texture.Width, texture.Height) / 2, textureScale, SpriteEffects.None, 0);
                        if (twinArms)
                        {
                            j = new Vector2((float)(rng.NextDouble() * 2 - 1) * jitter, (float)(rng.NextDouble() * 2 - 1) * jitter);
                            _spriteBatch.Draw(texture, new Vector2(r * (float)Math.Cos(-modifiedTheta), r * (float)Math.Sin(-modifiedTheta)) + j + center, texture.Bounds, actualColor, -textureAngle, new Vector2(texture.Width, texture.Height) / 2, textureScale, SpriteEffects.None, 0);
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
            Random rng = new Random();

            // Initialize render targets and textures
            RenderTarget2D renderTarget = new RenderTarget2D(_graphicsDevice, current.Width, current.Height);
            Texture2D result = new Texture2D(_graphicsDevice, renderTarget.Width, renderTarget.Height);
            Color[] data = new Color[renderTarget.Width * renderTarget.Height];

            // Load and validate textures
            List<Texture2D> textures = new List<Texture2D>();
            foreach (string textureUID in textureUIDs)
            {
                Texture2D texture = ResourceManager.getTexture(textureUID);
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
                            angle = absoluteAngle + StasisMathHelper.floatBetween(-angleJitter, angleJitter, rng);
                        else
                            angle = (float)Math.Atan2(relative.Y, relative.X) + relativeAngle + StasisMathHelper.floatBetween(-angleJitter, angleJitter, rng);
                        Vector2 j = new Vector2((float)rng.NextDouble() * 2 - 1, (float)rng.NextDouble() * 2 - 1) * jitter;
                        Vector2 position = pointA + normal * currentPosition + j;
                        Texture2D texture = textures[rng.Next(textures.Count)];
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
            Random rng = new Random();
            int tintR = randomRed < 0 ? rng.Next(randomRed, 1) : rng.Next(randomRed + 1);
            int tintG = randomGreen < 0 ? rng.Next(randomGreen, 1) : rng.Next(randomGreen + 1);
            int tintB = randomBlue < 0 ? rng.Next(randomBlue, 1) : rng.Next(randomBlue + 1);
            int tintA = randomAlpha < 0 ? rng.Next(randomAlpha, 1) : rng.Next(randomAlpha + 1);
            return new Color(
                Math.Max(0, (int)baseColor.R + tintR),
                Math.Max(0, (int)baseColor.G + tintG),
                Math.Max(0, (int)baseColor.B + tintB),
                Math.Max(0, (int)baseColor.A + tintA));
        }
    }
}
