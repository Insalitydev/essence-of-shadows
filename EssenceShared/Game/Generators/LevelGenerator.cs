using System;
using System.Collections.Generic;

namespace LevelGenerator {
    public static class Generator {
        #region public_properties

        public enum LevelType {
            City,
            Forest,
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

        private static double _maxNoise;
        private static double _minNoise = 2.0;
        private static int[,] _perlinArray;

        public static List<string> GenerateLevel(int width, int height, LevelType type)
        {
            Width = width;
            Height = height;
            Type = type; 
            MapTile = new char[Height, Width];
            //switch (Type)
            //{
            //    case LevelType.Desert:
            //        for (int i = 0; i < Height; i++)
            //            for (int j = 0; j < Width; j++)
            //                MapTile[i, j] = Tile["sand"];
            //        break;
            //    case LevelType.Forest:
            //        for (int i = 0; i < Height; i++)
            //            for (int j = 0; j < Width; j++)
            //                MapTile[i, j] = Tile["tree"];
            //        break;
            //    case LevelType.City:
            //        for (int i = 0; i < Height; i++)
            //            for (int j = 0; j < Width; j++)
            //                MapTile[i, j] = Tile["building"];
            //        break;
            //}

            _perlinArray = new int[Width, Height];
            var noise = new PerlinNoise2D();
            for (int i = 0; i < Height; i++) {
                for (int j = 0; j < Width; j++) {
                    switch (Type) {
                        case LevelType.Desert:
                            MapTile[i, j] = Tile["sand"];
                            break;
                        case LevelType.Forest:
                            MapTile[i, j] = Tile["tree"];
                            break;
                        case LevelType.City:
                            MapTile[i, j] = Tile["building"];
                            break;
                    }

                    double tile = noise.PerlinNoise(i, j);
                    if (noise.PerlinNoise(i, j) > _maxNoise)
                        _maxNoise = tile;
                    if (noise.PerlinNoise(i, j) < _minNoise)
                        _minNoise = tile;

                    //perlinArray[i, j] = intTile;
                    Console.Write(noise.PerlinNoise(i, j) + " ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("{0}\n{1}", _maxNoise, _minNoise);
            return null;
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