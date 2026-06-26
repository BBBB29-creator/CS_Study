using System;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[,] map =
            {
                { 0, 0, 1, 0 },
                { 0, 2, 0, 0 },
                { 1, 0, 0, 0 }
            };

            // GetLength(0)은 세로 크기(행의 개수: 3), GetLength(1)은 가로 크기(열의 개수: 4)를 뜻합니다.
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    // 💡 Write는 줄바꿈을 하지 않고 가로로 붙여서 출력합니다.
                    Console.Write(map[i, j] + " ");
                }

                // 💡 한 행(가로 한 줄)을 다 그렸으면 한 줄 내려줍니다.
                Console.WriteLine();
            }
        }
    }
}
