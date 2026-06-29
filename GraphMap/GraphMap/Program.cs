namespace GraphMap
{
    internal class Program
    {
        public class Player
        {
            public int CurrentArea { get; set; } = 0; // 0: 마을
        }

        class GameMap
        {
            static void Main(string[] args)
            {
                string[] areaNames = { "마을", "숲", "초원", "성", "마왕성", "바다", "던전", "산" };

                // [최적화 1] 출발지에서 갈 수 있는 목적지 번호들을 리스트(배열)로 명확히 관리
                // map[0] = 마을에서 갈 수 있는 지역 번호들 (1번 숲, 2번 초원, 3번 성)
                int[][] travelPaths = new int[][]
                {
            new int[] { 1, 2, 3 }, // [0] 마을 -> 숲, 초원, 성
            new int[] { 0, 2, 6 }, // [1] 숲 -> 마을, 초원, 던전
            new int[] { 0, 1, 4, 5 }, // [2] 초원 -> 마을, 숲, 마왕성, 바다
            new int[] { 0, 7 },    // [3] 성 -> 마을, 산
            new int[] { 2, 6, 7 }, // [4] 마왕성 -> 초원, 던전, 산
            new int[] { 2, 6, 7 }, // [5] 바다 -> 초원, 던전, 산
            new int[] { 1, 4, 5 }, // [6] 던전 -> 숲, 마왕성, 바다
            new int[] { 3, 4, 5 }  // [7] 산 -> 성, 마왕성, 바다
                };

                Player player = new Player();

                while (true)
                {
                    Console.Clear();
                    int current = player.CurrentArea;

                    Console.WriteLine("==================================================");
                    Console.WriteLine($"  [현재 위치: {current}번 {areaNames[current]}]");
                    Console.WriteLine("==================================================");
                    Console.WriteLine("\n[이동 가능한 주변 지역 목록]");

                    // [최적화 2] if문 없이 현재 지역에서 이동 가능한 데이터만 콕 집어서 출력
                    foreach (int nextArea in travelPaths[current])
                    {
                        Console.WriteLine($" [{nextArea}] {areaNames[nextArea]} (이동 가능)");
                    }
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine(" [ESC] 게임 종료");
                    Console.WriteLine("--------------------------------------------------");

                    Console.Write("이동하고 싶은 지역의 번호를 입력하세요: ");
                    string input = Console.ReadLine();

                    // ESC 처리 및 공백 예외 처리
                    if (string.IsNullOrEmpty(input)) continue;
                    if (input.ToUpper() == "ESC") { Console.WriteLine("\n게임 종료."); break; }

                    // [최적화 3] 복잡한 조건문들을 제거하고 단 하나의 함수(Array.Exists)로 이동 가능 여부 판정
                    int target;
                    bool isNumber = int.TryParse(input, out target);
                    bool canMove = isNumber && target >= 0 && target < 8 && Array.Exists(travelPaths[current], id => id == target);

                    // [최적화 4] C# 최신 Switch 문법을 사용하여 if-else 구조를 한눈에 보이게 맵핑
                    string outputMessage = canMove switch
                    {
                        true => ConfirmAndMove(player, current, target, areaNames[target]),
                        false => target == current
                            ? "\n이미 해당 지역에 머물고 있습니다. (제자리 유지)"
                            : "\n이동 실패: 갈 수 없는 지역 또는 길이 막혀 있습니다!"
                    };

                    Console.WriteLine(outputMessage);
                    System.Threading.Thread.Sleep(1200);
                }
            }

            // [최적화 5] 이동 의사 확인 로직을 별도 메서드로 완전히 분리하여 Main 함수 가독성 극대화
            static string ConfirmAndMove(Player p, int current, int target, string targetName)
            {
                Console.Write($"\n[{target}번 {targetName}](으)로 이동하시겠습니까? (Y/N): ");
                if (Console.ReadLine().ToUpper() == "Y")
                {
                    p.CurrentArea = target;
                    return $"\n{targetName}(으)로 걸어갑니다...";
                }
                return "\n이동을 취소했습니다.";
            }
        }
    }
}
