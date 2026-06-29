namespace TileMapGame_Ver1._0
{
    public class Player
    {
        public int X { get; set; } = 1; // 시작 X 좌표 (P의 위치)
        public int Y { get; set; } = 1; // 시작 Y 좌표
        public int[] LastPath { get; set; } = new int[] { 1, 1 }; // [출발지 Y, 도착지 X] 체크용 길 배열
    }

    class Program
    {
        static void Main(string[] args)
        {
            int[,] map = new int[12, 14]
            {
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, // 0
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 1 (P 시작점)
            { 1, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1 }, // 2
            { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 3
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 4
            { 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 5
            { 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, // 6
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 7
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 8
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 9
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1 }, // 10 (E 목적지)
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }  // 11
            };

            Player player = new Player();
            Console.CursorVisible = false;

            // 게임 메인 루프
            while (true)
            {
                Console.Clear();

                // 2. 최상단 디버그 로그 출력 (컴팩트한 [출발, 도착] 배열 형태)
                Console.WriteLine($"[DEBUG] 방금 통과한 좌표 배열: [Y:{player.LastPath[0]}, X:{player.LastPath[1]}] => [Y:{player.Y}, X:{player.X}]\n");

                // 3. 콘솔 화면에 맵 그리기
                for (int y = 0; y < 12; y++)
                {
                    for (int x = 0; x < 14; x++)
                    {
                        if (player.X == x && player.Y == y)
                        {
                            Console.Write("P"); // 플레이어
                        }
                        else if (map[y, x] == 1)
                        {
                            Console.Write("#"); // 벽
                        }
                        else if (map[y, x] == 2)
                        {
                            Console.Write("E"); // 목적지
                        }
                        else
                        {
                            Console.Write(" "); // 지나갈 수 있는 땅
                        }
                    }
                    Console.WriteLine();
                }

                // 4. [핵심 조건] 플레이어가 목적지(E) 좌표에 도달했는지 체크
                if (map[player.Y, player.X] == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Green; // 목적에 도달하면 문구가 초록색으로 바뀜
                    Console.WriteLine("\n=================================");
                    Console.WriteLine("목적지 E에 도달했습니다!");
                    Console.WriteLine("정상적으로 탈출하여 게임을 종료합니다.");
                    Console.WriteLine("=================================");
                    Console.ResetColor();

                    System.Threading.Thread.Sleep(2000); // 종료 전 메시지를 볼 수 있게 2초 대기
                    break; // while 루프를 탈출하여 디버그 프로그램 종료
                }

                Console.WriteLine("\n[방향키로 이동 / ESC 누르면 즉시 종료]");

                // 5. 방향키 입력 감지 및 가상 이동 좌표 계산
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                
                int nextX = player.X;
                int nextY = player.Y;

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow: nextY--; break;
                    case ConsoleKey.DownArrow: nextY++; break;
                    case ConsoleKey.LeftArrow: nextX--; break;
                    case ConsoleKey.RightArrow: nextX++; break;
                    case ConsoleKey.Escape: return; // ESC 누르면 즉시 종료
                }

                // 6. 최적화된 이동 조건 검증 (이동할 곳이 벽(# / 1)이 아니어야만 실제 이동 승인)
                if (map[nextY, nextX] != 1)
                {
                    // 디버그용 기록: 이동하기 전의 원래 좌표를 LastPath에 백업
                    player.LastPath[0] = player.Y;
                    player.LastPath[1] = player.X;

                    // 플레이어 실제 좌표 갱신
                    player.X = nextX;
                    player.Y = nextY;
                }
            }
        }
    }
}
