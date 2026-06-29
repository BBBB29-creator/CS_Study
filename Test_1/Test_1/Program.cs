using System;
using System.Collections.Generic; // Queue 자료구조를 쓰기 위해 필요

public class Player
{
    public int X { get; set; } = 1;
    public int Y { get; set; } = 1;
    public int[] LastPath { get; set; } = new int[] { 1, 1 };
}

class Program
{
    static int height = 12;
    static int width = 14;

    static void Main(string[] args)
    {
        // 화살표 특수문자 깨짐 방지 인코딩 적용
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int[,] map;
        Random rand = new Random();

        // [핵심 예외 처리 루프] 클리어가 가능한 맵이 뽑힐 때까지 무한 반복 생성!
        while (true)
        {
            map = GenerateRandomMap(rand);

            // 출발점(1,1)에서 목적지(10,12)까지 뚫려 있는지 컴퓨터가 먼저 테스트 플레이 진행
            if (CanClear(map, 1, 1, width - 2, height - 2))
            {
                // 길이 완벽히 이어져 있다면 맵 생성 통과!
                break;
            }
            // 만약 대각선 등으로 막혀서 못 깨는 맵이라면 유저 몰래 while문 처음으로 돌아가 재조립합니다.
        }

        Player player = new Player();
        Console.CursorVisible = false;

        // 게임 메인 루프 (기존과 동일)
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"[DEBUG] 방금 통과한 좌표 배열: [Y:{player.LastPath[0]}, X:{player.LastPath[1]}] ➡️ [Y:{player.Y}, X:{player.X}]\n");

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (player.X == x && player.Y == y) Console.Write("P");
                    else if (map[y, x] == 1) Console.Write("#");
                    else if (map[y, x] == 2) Console.Write("E");
                    else Console.Write(" ");
                }
                Console.WriteLine();
            }

            if (map[player.Y, player.X] == 2)
            {
                Console.WriteLine("\n목적지 E에 도달하여 탈출에 성공했습니다! 디버그를 종료합니다.");
                break;
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            int nextX = player.X;
            int nextY = player.Y;

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow: nextY--; break;
                case ConsoleKey.DownArrow: nextY++; break;
                case ConsoleKey.LeftArrow: nextX--; break;
                case ConsoleKey.RightArrow: nextX++; break;
                case ConsoleKey.Escape: return;
            }

            if (map[nextY, nextX] != 1)
            {
                player.LastPath[0] = player.Y;
                player.LastPath[1] = player.X;
                player.X = nextX;
                player.Y = nextY;
            }
        }
    }

    // 1. 기존의 무작위 맵 생성 및 억까 방지 통로 개방 기능 분리
    static int[,] GenerateRandomMap(Random rand)
    {
        int[,] map = new int[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (y == 0 || y == height - 1 || x == 0 || x == width - 1) map[y, x] = 1;
                else map[y, x] = (rand.Next(0, 100) < 30) ? 1 : 0; // 벽 확률 30%
            }
        }

        // 시작점과 도착지점 최소 안전장치 뚫기
        map[1, 1] = 0; map[1, 2] = 0; map[2, 1] = 0;
        map[height - 2, width - 2] = 2; // E
        map[height - 2, width - 3] = 0; map[height - 3, width - 2] = 0;

        return map;
    }

    // 2. 맵 검증용 BFS 길 찾기 알고리즘 (핵심 예외 처리 코드)
    static bool CanClear(int[,] map, int startX, int startY, int targetX, int targetY)
    {
        bool[,] visited = new bool[height, width];
        Queue<(int x, int y)> queue = new Queue<(int x, int y)>();

        queue.Enqueue((startX, startY));
        visited[startY, startX] = true;

        // 상하좌우 조사를 위한 오프셋 배열
        int[] dx = { 0, 0, -1, 1 };
        int[] dy = { -1, 1, 0, 0 };

        while (queue.Count > 0)
        {
            var curr = queue.Dequeue();

            // 목적지(E) 좌표에 도달할 수 있다면 이 맵은 탈출 가능한 착한 맵!
            if (curr.x == targetX && curr.y == targetY) return true;

            for (int i = 0; i < 4; i++)
            {
                int nx = curr.x + dx[i];
                int ny = curr.y + dy[i];

                // 맵 범위 내부이면서, 벽(1)이 아니고, 방문한 적이 없는 땅이라면 전진해보기
                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    if (map[ny, nx] != 1 && !visited[ny, nx])
                    {
                        visited[ny, nx] = true;
                        queue.Enqueue((nx, ny));
                    }
                }
            }
        }
        return false; // 큐가 다 빌 때까지 목적지를 못 찾았다면 '막힌 맵' 판정 ➡️ 리젝트
    }
}