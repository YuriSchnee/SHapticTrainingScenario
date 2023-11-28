using System;
using UnityEngine;


namespace jp.co.jetman.common.graph
{
    public class GaussianGraph
    {
        public static Vector2[] Generate(int _plots, float _mean, float _height = 1.0f)
        {
            Vector2[] dataPoints = new Vector2[_plots];

            // Calculate the maximum y-value for normalization
            float maxY = Gaussian(_mean, _mean); // Gaussian peak at the middle

            for (int i = 0; i < _plots; i++)
            {
                float x = i / (_plots - 1f);
                float y = Gaussian(x, _mean) / maxY * _height;
                dataPoints[i] = new Vector2(x, y);
            }

            return dataPoints;
        }

        private static float Gaussian(float _x, float _mean)
        {
            float stdDev = 0.1f;     // Standard deviation (controls "width" of the bell curve)

            float exponent = -Mathf.Pow(_x - _mean, 2) / (2f * stdDev * stdDev);
            return Mathf.Exp(exponent);
        }
    }
}