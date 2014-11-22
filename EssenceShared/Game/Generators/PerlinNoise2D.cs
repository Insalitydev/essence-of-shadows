using System;
using System.CodeDom;

namespace LevelGenerator
{
    public class PerlinNoise2D
    {
        private double Noise(int x, int y)
        {
            int n = x + y*57;
            n = (n<<13) ^ n;
            return ( 1.0 - ( (n * (n * n * 15731 + 789221) + 1376312589) >> 0x7fffffff) / 1073741824.0);
        }

        private double SmoothNoise(float xF, float yF)
        {
            var x = (int) xF;
            var y = (int) yF;
            double corners = (Noise(x - 1, y - 1) + Noise(x + 1, y - 1) + Noise(x - 1, y + 1) + Noise(x + 1, y + 1))/16;
            double sides = (Noise(x - 1, y) + Noise(x + 1, y) + Noise(x, y - 1) + Noise(x, y + 1))/8;
            double center = Noise(x, y)/4;
            return corners + sides + center;
        }

        private double Interpolate(double a, double b, double x)
        {
            double ft = x*Math.PI;
            double f = (1 - Math.Cos(ft))*0.5;
            return a*(1 - f) + b*f;
        }

        private double InterpolateNoise(double xF, double yF)
        {
            var x = (int) xF;
            double fractX = xF - x;
            var y = (int) yF;
            double fractY = yF - y;

            double v1 = SmoothNoise(x, y);
            double v2 = SmoothNoise(x + 1, y);
            double v3 = SmoothNoise(x, y + 1);
            double v4 = SmoothNoise(x + 1, y + 1);

            double i1 = Interpolate(v1, v2, fractX);
            double i2 = Interpolate(v3, v4  , fractX);
            return Interpolate(i1, i2, fractY);
        }

        public double PerlinNoise(float x, float y)
        {
            const int persistence = 2; //стойкость
            const int numOctaves = 5;
            const int n = numOctaves - 1;

            double total = 0;
            for (int i = 0; i < n; i++)
            {
                double frequency = Math.Pow(2, i);   // = Math.Pow(2, i);
                double amplitude = Math.Pow(persistence, i);   // = Math.Pow(persistence, i);
                //frequency = 2*i;
                //amplitude = p*i;
                total += InterpolateNoise(x*frequency, y*frequency)*amplitude;
            }
            return (total - 15) * 100000000;
        }
    }
}