using System;
using System.Collections.Generic;
using System.IO;
using LevelGenerator;
using Microsoft.Xna.Framework;

namespace EssenceShared.Game.Generators {
    public static class Generator {
        private static List<string> _level = new List<string>();

        private static readonly Dictionary<string, char> Tile = new Dictionary<string, char> {
            {"sand", '#'},
            {"water", '~'},
            {"townCell", '+'},
            {"cityStone", '-'},
            {"road", '|'},
            {"caveWall", '%'},
            {"dirt", '.'}
        };

        private static double _maxNoise = 1.2456439435482;
        private static double _minNoise = 0.209547579288483;
        private static double[,] _perlinArray;
        private static int[,] _intPerlinArray;
        private static int Width { get; set; }
        private static int Height { get; set; }
        private static LevelType Type { get; set; }

        public static List<string> GenerateLevel(int width, int height, LevelType type) {
            Width = width;
            Height = height;
            Type = type;
            _perlinArray = new double[Height, Width];
            _intPerlinArray = new int[Height, Width];

            // Calculate min and max noise
            var noise = new PerlinNoise2D();
            for (var i = 0; i < Height; i++) {
                for (var j = 0; j < Width; j++) {
                    _perlinArray[i, j] = noise.PerlinNoise(i, j);
                    if (_perlinArray[i, j] > _maxNoise)
                        _maxNoise = _perlinArray[i, j];
                    else if (_perlinArray[i, j] < _minNoise)
                        _minNoise = _perlinArray[i, j];
                }
            }

            // Generate perlin Noise
            var maxToMin = _maxNoise/_minNoise;
            for (var i = 0; i < Height; i++) {
                for (var j = 0; j < Width; j++) {
                    _intPerlinArray[i, j] = (int) ((_perlinArray[i, j] - _minNoise)*maxToMin);
                }
            }

            _level = NoiseToLevel();

#if DEBUG
            Console.WriteLine();
            for (var i = 0; i < Height; i++) {
                for (var j = 0; j < Width; j++) {
                    Console.Write(_intPerlinArray[i, j].ToString());
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Output();
#endif

            return _level;
        }

        private static List<string> NoiseToLevel() {
            var tileList = new List<string>();
            switch (Type) {
                case LevelType.City:
                    var roadLineList = new List<int>();
                    for (var i = 0; i < Width; i++) {
                        if (_intPerlinArray[0, i] == 1)
                            roadLineList.Add(i);
                    }

                    for (var i = 0; i < Height; i++) {
                        var line = "";
                        for (var j = 0; j < Width; j++) {
                            if (roadLineList.Contains(j))
                                line += Tile["road"];
                            else
                                line += Tile["cityStone"];
                        }
                        tileList.Add(line);
                    }
                    break;
                case LevelType.Town:
                    for (var i = 0; i < Height; i++) {
                        var line = "";
                        for (var j = 0; j < Width; j++) {
                            line += Tile["townCell"];
                        }
                        tileList.Add(line);
                    }
                    break;
                case LevelType.Desert:
                    var waterPointList = new List<Point> {new Point(0, 0)};
                    for (var i = 0; i < Height; i++) {
                        for (var j = 0; j < Width; j++) {
                            if (_intPerlinArray[i, j] == 0)
                                waterPointList.Add(new Point(i, j));
                        }
                    }
                    waterPointList.Add(new Point(Width, Height));

                    for (var i = 0; i < Height; i++) {
                        var line = "";
                        for (var j = 0; j < Width; j++) {
                            var isWater = false;
                            for (var k = 0; k < waterPointList.Count - 1; k++) {
                                if ((i == waterPointList[k + 1].X && j >= waterPointList[k].Y &&
                                     j <= waterPointList[k + 1].Y) ||
                                    (j == waterPointList[k].Y) && i >= waterPointList[k].X &&
                                    i <= waterPointList[k + 1].X) {
                                    isWater = true;
                                    break;
                                }
                            }

                            if (isWater)
                                line += Tile["water"];
                            else
                                line += Tile["sand"];
                        }
                        tileList.Add(line);
                    }
                    break;
                case LevelType.Cave:
                    var file = new StreamWriter(@"C:\Users\Public\Desktop\maps.txt");

                    for (var k = 0; k < Height; k++) {
                        for (var l = k; l < Width; l++) {
                            file.WriteLine("From {0} to {1}", k, l);
                            for (var i = 0; i < Height; i++) {
                                var line = "";
                                for (var j = 0; j < Width; j++) {
                                    if (_intPerlinArray[i, j] > k && _intPerlinArray[i, j] < l)
                                        line += Tile["dirt"];
                                    else
                                        line += Tile["caveWall"];
                                }
                                file.WriteLine(line);
                            }
                            file.WriteLine();
                        }
                    }
                    file.Close();

                    for (var i = 0; i < Height; i++) {
                        var line = "";
                        for (var j = 0; j < Width; j++) {
                            if (_intPerlinArray[i, j] > 0 && _intPerlinArray[i, j] < 4)
                                line += Tile["dirt"];
                            else
                                line += Tile["caveWall"];
                        }
                        tileList.Add(line);
                    }
                    break;
            }
            return tileList;
        }

        private static void Output() {
            foreach (var line in _level) {
                foreach (var symbol in line) {
                    switch (symbol) {
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
                    Console.Write(symbol);
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public enum LevelType {
        City,
        Town,
        Cave,
        Desert
    }
}