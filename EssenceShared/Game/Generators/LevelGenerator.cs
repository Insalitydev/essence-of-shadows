using System;
using System.Collections.Generic;

namespace LevelGenerator {
    public class Generator {
        #region public_properties

        public enum LevelType {
            City,
            Forest,
            Desert
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public LevelType Type { get; private set; }
        public char[,] MapTile { get; private set; }

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

        private double maxNoise;
        private double minNoise = 2.0;
        private int[,] perlinArray;

        public Generator(int width, int height, LevelType type) {
            Width = width;
            Height = height;
            Type = type;
            GenerateLevel();
        }

        private void GenerateLevel() {
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

            perlinArray = new int[Width, Height];
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
                    if (noise.PerlinNoise(i, j) > maxNoise)
                        maxNoise = tile;
                    if (noise.PerlinNoise(i, j) < minNoise)
                        minNoise = tile;

                    //perlinArray[i, j] = intTile;
                    Console.Write(noise.PerlinNoise(i, j) + " ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("{0}\n{1}", maxNoise, minNoise);
        }

        public void Output() {
            Console.WindowWidth = Width + 1;
            Console.WindowHeight = Height + 1;
            for (int i = 0; i < Height; i++) {
                for (int j = 0; j < Width; j++) {
                    switch (MapTile[i, j]) {
                        case '0':
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case '1':
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case '#':
                            Console.ForegroundColor = ConsoleColor.Gray;
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