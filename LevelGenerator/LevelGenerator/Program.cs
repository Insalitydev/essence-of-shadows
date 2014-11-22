namespace LevelGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = new Generator(70, 50, Generator.LevelType.Forest);
            map.Output();
        }
    }
}
