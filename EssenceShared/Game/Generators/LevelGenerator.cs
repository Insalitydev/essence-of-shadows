using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace LevelGenerator {
    public static class Generator {
        #region public_properties

        public enum LevelType {
            City,
            Town,
            Cave,
            Desert
        }

        private static int Width { get; set; }
        private static int Height { get; set; }
        private static LevelType Type { get; set; }
        private static char[,] MapTile { get; set; }

        #endregion

        private static readonly Dictionary<string, char> Tile = new Dictionary<string, char> {
            {"sand", '#'},
            {"water", '~'},
            {"townCell", '+'},
            {"cityStone", '-'},
            {"road", '|'},
            {"caveWall", '%'},
            {"dirt", '.'},
        };

        private static double _maxNoise = 1.2456439435482;
        private static double _minNoise = 0.209547579288483;
        private static double[,] _perlinArray;
        private static int[,] _intPerlinArray;

        public static List<string> GenerateLevel(int width, int height, LevelType type) {
            Width = width;
            Height = height;
            Type = type;
            MapTile = new char[Height, Width];
            _perlinArray = new double[Height, Width];
            _intPerlinArray = new int[Height, Width];

            // Calculate min and max noise
            var noise = new PerlinNoise2D();
            for (int i = 0; i < Height; i++) {
                for (int j = 0; j < Width; j++) {
                    _perlinArray[i, j] = noise.PerlinNoise(i, j);
                    if (_perlinArray[i, j] > _maxNoise)
                        _maxNoise = _perlinArray[i, j];
                    else if (_perlinArray[i, j] < _minNoise)
                        _minNoise = _perlinArray[i, j];
                }
            }

            // Generate perlin Noise
            var maxToMin = _maxNoise/_minNoise;
            for (int i = 0; i < Height; i++) {
                for (int j = 0; j < Width; j++) {
                    _intPerlinArray[i, j] = (int) ((_perlinArray[i, j] - _minNoise)*maxToMin);
                }
            }

            var level = NoiseToLevel();

#if DEBUG
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Console.Write(_intPerlinArray[i, j].ToString());
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            for (int i = 0; i < Height; i++)
            {
                Console.WriteLine(level[i]);
            }
#endif

            return level;
        }

        private static List<string> NoiseToLevel() {
            var tileList = new List<string>();
            switch (Type) {
                case LevelType.City:
                    var roadLineList = new List<int>();
                    for (int i = 0; i < Width; i++) {
                        if (_intPerlinArray[0,i] == 1)
                            roadLineList.Add(i);
                    }

                    for (int i = 0; i < Height; i++) {
                        var line = "";
                        for (int j = 0; j < Width; j++) {
                            if (roadLineList.Contains(j))
                                line += '|';
                            else
                                line += '-';
                        }
                        tileList.Add(line);
                    }
                    break;
                case LevelType.Town:
                    for (int i = 0; i < Height; i++) {
                        var line = "";
                        for (int j = 0; j < Width; j++) {
                            line += '+';
                        }
                        tileList.Add(line);
                    }
                    break;
                case LevelType.Desert:
                    var waterPointList = new List<Point>() {new Point(0,0)};
                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            if (_intPerlinArray[i, j] == 0)
                                waterPointList.Add(new Point(i,j));
                        }
                    }
                    waterPointList.Add(new Point(Width, Height));

                    for (int i = 0; i < Height; i++) {
                        var line = "";
                        for (int j = 0; j < Width; j++) {
                            var isWater = false;
                            for (int k = 0; k < waterPointList.Count - 1; k++) {
//                                if ((i == waterPointList[k].X && i <= waterPointList[k + 1].X && j <= waterPointList[k].Y) || (j == waterPointList[k].Y && j <= waterPointList[k + 1].Y && i <= waterPointList[k + 1].X))
                                if ((i == waterPointList[k + 1].X && j >= waterPointList[k].Y && j <= waterPointList[k + 1].Y) ||
                                    (j == waterPointList[k].Y) && i >= waterPointList[k].X && i <= waterPointList[k + 1].X)
                                {
                                    isWater = true;
                                    break;
                                }
                            }

                            if (isWater)
                                line += '~';
                            else
                                line += '#';
                        }
                        tileList.Add(line);
                    }
                    break;
            }
            return tileList;
        }

        private static void Output() {
            Console.WindowWidth = Width + 1;
            Console.WindowHeight = Height + 1;
            for (int i = 0; i < Height; i++) {
                for (int j = 0; j < Width; j++) {
                    switch (MapTile[i, j]) {
                        case '~':
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case '%':
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                        case '+':
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                        case '-':
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case '|':
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            break;
                        case '.':
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case '#':
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }
                    Console.Write(MapTile[i, j]);
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}