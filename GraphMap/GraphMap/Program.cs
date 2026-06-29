namespace GraphMap
{
    internal class Program
    {
        public class Player
        {
            public int CurrentArea { get; set; } = 0; // 0: 마을

            // [수정] 누적 리스트를 지우고, 딱 [출발지, 도착지] 2개만 담는 고정 배열로 변경!
            public int[] LastPath { get; set; } = new int[2] { 0, 0 };
        }

        class GameMap
        {
            static string lastDebugMessage = "게임이 시작되었습니다. 초기화 완료.";

            static void SetDebugLog(string message)
            {
                lastDebugMessage = message;
            }

            // 디버그 UI 출력 함수 수정
            static void PrintDebugUI(Player player, string[] areaNames)
            {
                Console.WriteLine("=================== [DEBUGGER (실시간)] ===================");

                int from = player.LastPath[0];
                int to = player.LastPath[1];

                Console.WriteLine($"방금 통과한 길 배열 : [{from}, {to}]");
                Console.WriteLine($"현재 이동 구간 추적 : {areaNames[from]} => {areaNames[to]}");
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine($"[최신 이벤트] {lastDebugMessage}");
                Console.WriteLine("===========================================================\n");
            }

            static void Main(string[] args)
            {
                string[] areaNames = { "마을", "숲", "초원", "성", "마왕성", "바다", "던전", "산" };

                int[][] travelPaths = new int[][]
                {
            new int[] { 1, 2, 3 },    // 마을
            new int[] { 0, 2, 6 },    // 숲
            new int[] { 0, 1, 4, 5 }, // 초원
            new int[] { 0, 7 },       // 성
            new int[] { 2, 6, 7 },    // 마왕성
            new int[] { 2, 6, 7 },    // 바다
            new int[] { 1, 4, 5 },    // 던전
            new int[] { 3, 4, 5 }     // 산
                };

                Player player = new Player();

                while (true)
                {
                    Console.Clear();
                    PrintDebugUI(player, areaNames);

                    // 메인 게임 화면
                    Console.WriteLine("==================================================");
                    Console.WriteLine($"[현재 위치: {player.CurrentArea}번 {areaNames[player.CurrentArea]}]");
                    Console.WriteLine("==================================================");
                    Console.WriteLine("\n[이동 가능한 주변 지역 목록]");

                    foreach (int nextArea in travelPaths[player.CurrentArea])
                    {
                        Console.WriteLine($" [{nextArea}] {areaNames[nextArea]} (이동 가능)");
                    }
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine(" [ESC] 게임 종료");
                    Console.WriteLine("--------------------------------------------------");

                    Console.Write("이동하고 싶은 지역의 번호를 입력하세요: ");
                    
                    string input = Console.ReadLine();

                    if (string.IsNullOrEmpty(input)) continue;
                    if (input.ToUpper() == "ESC") { break; }

                    int target;

                    bool isNumber = int.TryParse(input, out target);
                    bool canMove = isNumber && target >= 0 && target < 8 && Array.Exists(travelPaths[player.CurrentArea], id => id == target);

                    string outputMessage = canMove switch
                    {
                        true => ConfirmAndMove(player, player.CurrentArea, target, areaNames[target]),
                        false => target == player.CurrentArea
                            ? "제자리."
                            : $"잘못된 입력값 '{input}' 입니다. 이동할 수 없는 경로입니다."
                    };

                    SetDebugLog(outputMessage);
                }
            }

            static string ConfirmAndMove(Player p, int current, int target, string targetName)
            {
                Console.Write($"\n[{target}번 {targetName}](으)로 이동하시겠습니까? (Y/N): ");
                if (Console.ReadLine().ToUpper() == "Y")
                {
                    // 이동이 확정되는 순간 [출발지, 도착지] 배열 값을 새로 완전히 덮어씌웁니다.
                    p.LastPath[0] = current; // 출발 지역 번호 (예: 0)
                    p.LastPath[1] = target;  // 도착 지역 번호 (예: 1)

                    p.CurrentArea = target; // 실제 플레이어 위치 변경

                    return $"성공: {targetName}(으)로 성공적으로 이동했습니다.";
                }
                return "취소: 이동을 취소하여 현재 지역에 머무릅니다.";
            }
        }
    }
}
