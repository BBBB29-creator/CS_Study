namespace SA_Problem_3
{
    internal class Program
    {
        // 1 == 작물, 0 == 땅
        static int[,] Map =
        {
        { 1, 1, 0, 0, 1 },
        { 0, 1, 0, 0, 1 },
        { 0, 0, 0, 1, 1 },
        { 1, 0, 0, 1, 0 },
        { 1, 1, 0, 0, 1 }
    };

        static int mapRows;
        static int mapCols;
        static bool[,] isVisited;

        static int[] directR = { -1, 1, 0, 0 };
        static int[] directC = { 0, 0, -1, 1 };

        // 재귀 함수
        static void DFS(int r, int c)
        {
            isVisited[r, c] = true;

            for (int i = 0; i < 4; i++)
            {
                int moveRow = r + directR[i];
                int moveCol = c + directC[i];

                // 맵 범위를 벗어난 경우 스킵
                if (moveRow < 0 || moveRow >= mapRows || moveCol < 0 || moveCol >= mapCols)
                    continue;

                // 작물이 있는 칸(1)이고, 방문하지 않았다면 이어서 탐색 (재귀)
                if (Map[moveRow, moveCol] == 1 && !isVisited[moveRow, moveCol])
                {
                    DFS(moveRow, moveCol);
                }
            }
        }

        static int CountFarmAreas()
        {
            mapRows = Map.GetLength(0);
            mapCols = Map.GetLength(1);
            
            isVisited = new bool[mapRows, mapCols];

            int areaCount = 0;

            // 맵 순회
            for (int r = 0; r < mapRows; r++)
            {
                for (int c = 0; c < mapCols; c++)
                {
                    if (Map[r, c] == 1 && !isVisited[r, c])
                    {
                        areaCount++;

                        DFS(r, c);
                    }
                }
            }

            return areaCount;
        }

        static void Main(string[] args)
        {
            int result = CountFarmAreas();
            
            Console.WriteLine($"독립된 밭 구역 개수: {result}개");
        }
    }
}