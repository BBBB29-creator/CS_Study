namespace GraphMap
{
    internal class Program
    {
        public class Player
        {
            public int CurrentArea { get; set; } = 0; // 현재 머무르고 있는 지역 번호 (0: 마을)
        }

        class GameMap
        {
            static void Main(string[] args)
            {
                // 1. 지역 번호(0~7)에 대응하는 고유 장소 명칭
                string[] areaNames = { "마을", "숲", "초원", "성", "마왕성", "바다", "던전", "산" };

                // 2. [요청 사안 반영] 각 지역으로 이어지는 길(true) 데이터 설정 (행: 출발지, 열: 목적지)
                // 예: map[0, 1] = true 이면 '0번 마을'에서 '1번 숲'으로 가는 길이 존재함을 의미
                bool[,] map = new bool[8, 8];

                map[0, 1] = true; map[0, 2] = true; map[0, 3] = true;
                map[1, 0] = true; map[1, 2] = true; map[1, 6] = true;
                map[2, 0] = true; map[2, 1] = true; map[2, 4] = true; map[2, 5] = true;
                map[3, 0] = true; map[3, 7] = true;
                map[4, 2] = true; map[4, 6] = true; map[4, 7] = true;
                map[5, 2] = true; map[5, 6] = true; map[5, 7] = true;
                map[6, 1] = true; map[6, 4] = true; map[6, 5] = true;
                map[7, 3] = true; map[7, 4] = true; map[7, 5] = true;

                Player player = new Player();

                // 게임 메인 루프
                while (true)
                {
                    Console.Clear();
                    int current = player.CurrentArea;

                    // 3. 현재 위치 정보 출력
                    Console.WriteLine("==================================================");
                    Console.WriteLine($"  [🏠 현재 위치: {current}번 {areaNames[current]}]");
                    Console.WriteLine("==================================================");
                    Console.WriteLine("\n[이동 가능한 주변 지역 목록 및 상태]");

                    // 4. 현재 위치에서 갈 수 있는 길인지 판정하여 목록 출력
                    for (int i = 0; i < 8; i++)
                    {
                        if (i == current) continue; // 제자리는 출력 제외

                        // 요청하신 '길 데이터(map)'를 검사하여 true인 곳만 이동 가능으로 표시
                        string status = map[current, i] ? "이동 가능" : "🚫 막힌 길(이동 불가)";
                        Console.WriteLine($" [{i}] {areaNames[i]} - 상태: {status}");
                    }
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine(" [ESC] 게임 종료");
                    Console.WriteLine("--------------------------------------------------");

                    // 5. 유저 입력 처리
                    Console.Write("이동하고 싶은 지역의 번호를 입력하세요: ");
                    string input = Console.ReadLine();

                    // ESC 종료 처리 (입력 없이 바로 엔터 치는 상황 대비)
                    if (string.IsNullOrEmpty(input)) continue;

                    // 6. 이동 의사 확인 및 최종 위치 갱신
                    int targetArea;
                    if (int.TryParse(input, out targetArea) && targetArea >= 0 && targetArea < 8)
                    {
                        if (targetArea == current)
                        {
                            Console.WriteLine("\n이미 해당 지역에 머물고 있습니다! (제자리 유지)");
                            System.Threading.Thread.Sleep(1000);
                            continue;
                        }

                        // 길 데이터를 기반으로 갈 수 있는 정당한 길(true)인지 체크
                        if (map[current, targetArea] == true)
                        {
                            Console.Write($"\n[{targetArea}번 {areaNames[targetArea]}](으)로 이동하시겠습니까? (Y/N): ");
                            string confirm = Console.ReadLine().ToUpper();

                            if (confirm == "Y")
                            {
                                player.CurrentArea = targetArea; // 플레이어 지역 변경
                                Console.WriteLine($"\n{areaNames[targetArea]}(으)로 걸어갑니다...");
                                System.Threading.Thread.Sleep(1000);
                            }
                            else
                            {
                                Console.WriteLine("\n이동을 취소했습니다. 제자리에 표기됩니다.");
                                System.Threading.Thread.Sleep(1000);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"\n❌ 이동 실패: {areaNames[current]}에서 {areaNames[targetArea]}(으)로 가는 길이 없습니다!");
                            System.Threading.Thread.Sleep(1500);
                        }
                    }
                    else if (input.ToUpper() == "ESC")
                    {
                        Console.WriteLine("\n게임들을 종료합니다.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\n올바른 번호(0~7) 또는 ESC를 입력해주세요. (제자리 유지)");
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
        }
    }
}
