using System;
using System.Collections.Generic;
using System.Linq;

namespace LevelGenerator {
    public static class Generator {
        #region public_properties

        public enum LevelType {
            City,
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

            var valueOfWater = 0;
            int srcX = 0, srcY = 0, dstX = 0, dstY = 0;
            var maxToMin = _maxNoise/_minNoise;
            for (int i = 0; i < Height; i++) {
                for (int j = 0; j < Width; j++) {
                    _intPerlinArray[i, j] = (int) ((_perlinArray[i, j] - _minNoise)*maxToMin);
                    //if (_intPerlinArray[i, j] == valueOfWater) {
                    //    dstX = srcX;
                    //    dstY = srcY;
                    //    srcX = i;
                    //    srcY = j;
                    //}
                }
            }

            //for (int i = srcX; i > dstX; i--) {
            //    dstY--;
            //    _intPerlinArray[i, dstY] = valueOfWater;
            //        if (_intPerlinArray[i - 1, dstY] == valueOfWater) {
            //            i--;
            //            _intPerlinArray[i, dstY] = valueOfWater;
            //        }
            //    if (dstY - 1 >= 0) {
            //        if (_intPerlinArray[i, dstY - 1] == valueOfWater) {
            //            dstY--;
            //            _intPerlinArray[i, dstY] = valueOfWater;
            //        }
            //        if (_intPerlinArray[i - 1, dstY - 1] != valueOfWater) continue;
            //        i--;
            //        dstY--;
            //        _intPerlinArray[i, dstY] = valueOfWater;
            //    }
            //}

            //for (int i = 0; i < Height; i++)
            //{
            //    for (int j = 0; j < Width; j++)
            //    {
            //        Console.Write(_intPerlinArray[i, j].ToString());
            //    }
            //    Console.WriteLine();
            //}

            var tileList = new List<string>();
            for (int i = 0; i < Height; i++) {
                var line = "";
                for (int j = 0; j < Width; j++) {
                    if (_intPerlinArray[i, j] == 0)
                        line += '~';
                    else
                        line += '#';
                }
                tileList.Add(line);
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