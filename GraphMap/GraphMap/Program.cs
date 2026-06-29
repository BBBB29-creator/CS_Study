namespace GraphMap
{
    internal class Program
    {
        public class Player
        {
            public int CurrentArea { get; set; } = 0;
            public int[] LastPath { get; set; } = new int[] { 0, 0 }; // 딱 2개짜리 길 배열
        }

        class GameMap
        {
            static void Main(string[] args)
            {
                string[] areaNames = { "마을", "숲", "초원", "성", "마왕성", "바다", "던전", "산" };
                int[][] travelPaths = new int[][]
                {
            new int[] { 1, 2, 3 }, new int[] { 0, 2, 6 }, new int[] { 0, 1, 4, 5 }, new int[] { 0, 7 },
            new int[] { 2, 6, 7 }, new int[] { 2, 6, 7 }, new int[] { 1, 4, 5 }, new int[] { 3, 4, 5 }
                };

                Player player = new Player();

                while (true)
                {
                    Console.Clear();
                    // [원래 목적] 방금 통과한 길 배열만 콘솔 맨 위에 아주 간단하게 한 줄 표기 (핵심만 남김)
                    Console.WriteLine($"[DEBUG] 방금 통과한 길 배열: [{player.LastPath[0]}, {player.LastPath[1]}]\n");

                    Console.WriteLine($"================== [ 현재 위치: {areaNames[player.CurrentArea]} ] ==================");
                    
                    foreach (int next in travelPaths[player.CurrentArea])
                        Console.WriteLine($" [{next}] {areaNames[next]} 이동 가능");
                    Console.WriteLine("==================================================================");

                    Console.Write("이동할 번호 입력 (종료는 ESC): ");
                    
                    string input = Console.ReadLine();
                    
                    if (input.ToUpper() == "ESC") break;

                    if (int.TryParse(input, out int target) && Array.Exists(travelPaths[player.CurrentArea], id => id == target))
                    {
                        Console.Write($"[{areaNames[target]}](으)로 이동하시겠습니까? (Y/N): ");
                        if (Console.ReadLine().ToUpper() == "Y")
                        {
                            // 이동 성공 시 출발지와 목적지를 배열에 기억
                            player.LastPath[0] = player.CurrentArea;
                            player.LastPath[1] = target;
                            player.CurrentArea = target;
                        }
                    }
                }
            }
        }
    }
}