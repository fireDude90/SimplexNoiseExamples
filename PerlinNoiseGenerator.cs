using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGenerator {
    public static class PerlinNoiseGenerator {
        /// <summary>
        /// Gets a value between -1.0 and 1.0 for a number <i>x</i>
        /// </summary>
        /// <param name="x">A number to be used to calculate noise</param>
        /// <returns>Value between -1.0 and 1.0</returns>
        private static float GetNoise(int x) {
            x = (x << 13) ^ x;
            return (float)(1.0 - ((x * (x * x * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0);
        }


        /// <summary>
        /// Gets a value between -1.0 and 1.0 for a coordinate pair <i>x</i> and <i>y</i>
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Value between -1.0 and 1.0 </returns>
        private static float GetNoise(int x, int y) {
            int a = x + y * 57;
            a = (a << 13) ^ a;
            return (float)(1.0 - ((a * (a * a * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0);
        }
        /// <summary>
        /// Interpolates two numbers using Cosine wave
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <param name="x">Number to interpolate over</param>
        /// <returns></returns>
        private static float CosineInterpolate(float a, float b, float x) {
            float ft = (float)(x * Math.PI);
            float f = (1 - (float)(Math.Cos(ft))) / 2;

            return (a * (1 - f) + b * (1 - f));
        }

        /// <summary>
        /// Smooths a noise value at point X
        /// </summary>
        /// <param name="x">X coordinate of smoothed value</param>
        /// <returns>Smoothed value</returns>
        public static float SmoothNoise(int x) {
            return GetNoise(x) / 2 + GetNoise(x - 1) / 4 + GetNoise(x + 1) / 4;
        }

        public static float InterpolatedNoise(float x) {
            int intX = (int)x;
            float x2 = x - intX;

            float a = SmoothNoise(intX);
            float b = SmoothNoise(intX + 1);

            return CosineInterpolate(a, b, x2);
        }

        public static float PerlinNoise(float x) {
            float total = 0;
            int persistence = 5;
            int octaves = 4;

            for (int i = 0; i <= octaves; i++) {
                float frequency = (float)Math.Pow(2, i);
                float amplitude = (float)Math.Pow(persistence, i);

                total += InterpolatedNoise(x * frequency) * amplitude;
            }

            return total;
        }
    }
}
