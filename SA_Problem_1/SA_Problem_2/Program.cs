using System;
using System.Collections.Generic;

namespace SA_Problem_2
{
    internal class Program
    {
        // 0 == 평지, 1 == 벽/장애물
        static int[,] Map =
        {
            { 0, 0, 0, 0, 1 },
            { 0, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0 },
            { 0, 1, 0, 1, 1 },
            { 1, 0, 1, 0, 1 }
        };

        static int[] directR = { -1, 1, 0, 0 };
        static int[] directC = { 0, 0, -1, 1 };

        // 좌표와 현재까지의 이동 거리를 저장할 구조체
        struct Point
        {
            public int Row;
            public int Col;
            public int Dist;

            // 튜플 문법 테스트
            public Point(int row, int col, int dist) => (Row, Col, Dist) = (row, col, dist);
        }

        static void FindNearestExit(int[,] map, int startR, int startC, List<Point> exits)
        {
            int mapRows = map.GetLength(0);
            int mapCols = map.GetLength(1);

            bool[,] isVisited = new bool[mapRows, mapCols];

            Queue<Point> queue = new Queue<Point>();

            // 시작점 세팅
            isVisited[startR, startC] = true;
            queue.Enqueue(new Point(startR, startC, 0));

            while (queue.Count > 0)
            {
                Point current = queue.Dequeue();

                // 현재 도달한 칸이 출구 목록에 있는지 확인
                foreach (var exit in exits)
                {
                    if (current.Row == exit.Row && current.Col == exit.Col)
                    {
                        // BFS 특성상 가장 먼저 매칭된 출구가 무조건 최단 거리입니다.
                        Console.WriteLine($"가장 가까운 출구: ({current.Row},{current.Col})");
                        Console.WriteLine($"거리: {current.Dist}");
                        return; // 찾으면 함수 즉시 종료
                    }
                }

                // 상하좌우 탐색
                for (int i = 0; i < 4; i++)
                {
                    int moveRow = current.Row + directR[i];
                    int moveCol = current.Col + directC[i];

                    // 맵 범위를 벗어난 경우 스킵
                    if (moveRow < 0 || moveRow >= mapRows || moveCol < 0 || moveCol >= mapCols)
                        continue;

                    // 벽이거나 이미 방문한 곳이면 스킵
                    if (map[moveRow, moveCol] == 1 || isVisited[moveRow, moveCol])
                        continue;

                    isVisited[moveRow, moveCol] = true;
                    queue.Enqueue(new Point(moveRow, moveCol, current.Dist + 1));
                }
            }
            Console.WriteLine("출구가 없습니다.");
        }

        static void Main(string[] args)
        {
            // 출구 목록 리스트 생성
            List<Point> exits = new List<Point>()
            {
                new Point(4, 4, 0),
                new Point(3, 4, 0),
                new Point(0, 4, 0)
            };

            // 가장 가까운 출구 찾기
            FindNearestExit(Map, 0, 0, exits);
        }
    }
}
