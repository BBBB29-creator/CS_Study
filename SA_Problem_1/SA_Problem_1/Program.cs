namespace SA_Problem_1
{
    internal class Program
    {
        // 입력받은 맵 데이터 (0 == 평지, 1 == 벽/장애물)
        static int[,] Map =
        {
            { 0, 0, 0, 0, 0 },
            { 0, 1, 1, 0, 0 },
            { 0, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 0 },
            { 0, 0, 0, 0, 0 }
        };

        static int[] directR = { -1, 1, 0, 0 };
        static int[] directC = { 0, 0, -1, 1 };

        // 좌표와 현재까지의 이동 거리를 함께 저장할 구조체
        struct Point
        {
            public int Row;
            public int Col;
            public int Dist;

            public Point(int row, int col, int dist) => (Row, Col, Dist) = (row, col, dist);
        }

        static int CountReachableCells(int[,] map, int startR, int startC, int k)
        {
            int mapRows = map.GetLength(0);
            int mapCols = map.GetLength(1);

            // 방문 여부 확인용 배열 및 BFS 큐 선언
            bool[,] isVisited = new bool[mapRows, mapCols];
            Queue<Point> queue = new Queue<Point>();

            // 시작점 세팅 (시작점의 거리는 0)
            isVisited[startR, startC] = true;
            queue.Enqueue(new Point(startR, startC, 0));

            int reachableCount = 0;

            while (queue.Count > 0)
            {
                Point current = queue.Dequeue();

                // 현재 칸의 거리가 K 이하인 경우에만 카운트를 증가
                if (current.Dist <= k)
                {
                    reachableCount++;
                }
                else
                {
                    // 이미 거리가 K를 초과했다면 이 위로는 더 이상 탐색할 필요가 없습니다.
                    continue;
                }

                // 상하좌우 이웃 칸 탐색
                for (int i = 0; i < 4; i++)
                {
                    int moveRow = current.Row + directR[i];
                    int moveCol = current.Col + directC[i];

                    // 맵 범위를 벗어난 경우 스킵
                    if (moveRow < 0 || moveRow >= mapRows || moveCol < 0 || moveCol >= mapCols)
                        continue;

                    // 벽(1)이거나 이미 방문한 곳이면 스킵
                    if (map[moveRow, moveCol] == 1 || isVisited[moveRow, moveCol])
                        continue;

                    // 방문 표시 후 큐에 삽입 (거리를 1 늘려줍니다)
                    isVisited[moveRow, moveCol] = true;
                    queue.Enqueue(new Point(moveRow, moveCol, current.Dist + 1));
                }
            }

            return reachableCount;
        }

        static void Main(string[] args)
        {
            int K = 4;
            int result = CountReachableCells(Map, 0, 0, K);

            Console.WriteLine($"거리 {K} 이하로 도달 가능한 칸 수: {result}개");
        }
    }
}