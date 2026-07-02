namespace SA_Problem_4
{
    internal class Program
    {
        // 숫자 = 이동 비용, 0 = 벽
        static int[,] Map =
        {
            { 1, 1, 1, 1, 1 },
            { 1, 5, 5, 5, 1 },
            { 1, 1, 1, 1, 1 }
        };

        static int[] directR = { -1, 1, 0, 0 };
        static int[] directC = { 0, 0, -1, 1 };

        struct Point
        {
            public int Row;
            public int Col;
            public Point(int row, int col) => (Row, Col) = (row, col); // 튜플
        }

        static int GetMinStaminaPath(int[,] map, Point start, Point end)
        {
            int mapRows = map.GetLength(0);
            int mapCols = map.GetLength(1);

            int[,] dist = new int[mapRows, mapCols];
            for (int r = 0; r < mapRows; r++)
                for (int c = 0; c < mapCols; c++)
                    dist[r, c] = int.MaxValue;

            PriorityQueue<Point, int> pq = new PriorityQueue<Point, int>();

            dist[start.Row, start.Col] = 0;
            pq.Enqueue(start, 0); // pq는 우선순위 큐 축약

            while (pq.Count > 0)
            {
                pq.TryDequeue(out Point current, out int currentCost);

                if (current.Row == end.Row && current.Col == end.Col)
                    return currentCost;

                if (currentCost > dist[current.Row, current.Col])
                    continue;

                for (int i = 0; i < 4; i++)
                {
                    int moveRow = current.Row + directR[i];
                    int moveCol = current.Col + directC[i];

                    if (moveRow < 0 || moveRow >= mapRows || moveCol < 0 || moveCol >= mapCols)
                        continue;

                    if (map[moveRow, moveCol] == 0)
                        continue;

                    // 새로운 칸으로 이동했을 때의 누적 체력 소모량 계산
                    int nextCost = currentCost + map[moveRow, moveCol];

                    // 새로 계산한 비용이 기존에 알고 있던 비용보다 더 저렴하다면 갱신
                    if (nextCost < dist[moveRow, moveCol])
                    {
                        dist[moveRow, moveCol] = nextCost;
                        pq.Enqueue(new Point(moveRow, moveCol), nextCost);
                    }
                }
            }

            return -1; // 도달할 수 없는 경우
        }

        static void Main(string[] args)
        {
            Point start = new Point(0, 0);
            Point end = new Point(2, 4);

            int result = GetMinStaminaPath(Map, start, end);
            
            Console.WriteLine($"체력 소모량 : {result}");
        }
    }
}
