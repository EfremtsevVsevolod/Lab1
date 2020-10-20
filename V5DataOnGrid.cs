using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Lab1
{
    class V5DataOnGrid : V5Data
    {
        public Grid2D Grid { get; set; }
        public Vector2[,] ValuesGrid { get; set; } = { };

        public V5DataOnGrid(string serviceInfo, DateTime measurementTime, Grid2D grid) :
            base(serviceInfo, measurementTime)
        {
            Grid = grid;
        }

        public static implicit operator V5DataCollection(V5DataOnGrid v5DataOnGridInstance)
        {
            V5DataCollection v5DataCollectionInstance =
                new V5DataCollection(v5DataOnGridInstance.ServiceInfo,
                                     v5DataOnGridInstance.MeasurementTime);

            for (int i = 0; i < v5DataOnGridInstance.Grid.NodesNumberX; ++i)
            {
                for (int j = 0; j < v5DataOnGridInstance.Grid.NodesNumberY; ++j)
                {
                    float x = v5DataOnGridInstance.Grid.StepSizeX * i;
                    float y = v5DataOnGridInstance.Grid.StepSizeY * j;
                    v5DataCollectionInstance.ValuesDct[new Vector2(x, y)] =
                        v5DataOnGridInstance.ValuesGrid[i, j];
                }
            }

            return v5DataCollectionInstance;
        }

        public void InitRandom(float minValue, float maxValue)
        {
            ValuesGrid = new Vector2[Grid.NodesNumberX, Grid.NodesNumberY];
            Random rand = new Random();
            for (int i = 0; i < Grid.NodesNumberX; ++i)
            {
                for (int j = 0; j < Grid.NodesNumberY; ++j)
                {
                    ValuesGrid[i, j] =
                        new Vector2(minValue + (maxValue - minValue) * (float)rand.NextDouble(),
                                    minValue + (maxValue - minValue) * (float)rand.NextDouble());
                }
            }
        }

        public override Vector2[] NearEqual(float eps)
        {
            List<Vector2> NearValues = new List<Vector2>();
            foreach (Vector2 value in ValuesGrid)
            {
                if (Math.Abs(value.X - value.Y) < eps)
                {
                    NearValues.Add(value);
                }
            }
            return NearValues.ToArray();
        }

        public override string ToString()
        {
            return "Type: V5DataOnGrid\n" + base.ToString() + "\n" + Grid.ToString();
        }

        public override string ToLongString()
        {
            StringBuilder strBulder = new StringBuilder(ToString() + "\n");

            for (int i = 0; i < Grid.NodesNumberX; ++i)
            {
                for (int j = 0; j < Grid.NodesNumberY; ++j)
                {
                    strBulder.Append($"x = {i * Grid.StepSizeX}  y = {j * Grid.StepSizeY}: ");
                    strBulder.Append($"{ValuesGrid[i, j]}\n");
                }
            }
            return strBulder.ToString();
        }
    }
}
