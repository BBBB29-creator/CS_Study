namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            List<string> inventory = new List<string> { "롱소드", "포션", "키" };

            // 인서트 테스트
            inventory.Insert(0, "롱소드");
            inventory.Insert(1, "포션");
            inventory.Insert(2, "키");


            while (true)
            {

                // 1. 메뉴 화면
                Console.WriteLine("(1) 아이템 얻기");
                Console.WriteLine("(2) 아이템 버리기");
                Console.WriteLine("(3) 인벤토리 상황 보기");
                Console.WriteLine("(0) 종료");
                Console.WriteLine("\n메뉴를 고르세요 (1~3) : ");

                // 2. 입력 받기
                string input = Console.ReadLine();

                // 3. 입력 조건 분기
                switch (input)
                {
                    case "1": // 아무거나 다 입력되는 문제가 있음. - 해결?
                        
                        Console.WriteLine("\n[아이템 얻기] 새로운 아이템을 발견했습니다!");
                        Console.Write("얻을 아이템 이름을 입력하세요: ");
                        
                        string newItem = Console.ReadLine();

                        if (inventory.Contains(newItem))
                        {
                            inventory.Add(newItem);

                            Console.WriteLine($"'{newItem}'을(를) 인벤토리에 넣었습니다.");
                        }
                        else
                        {
                            Console.WriteLine("- 게임 내에 존재하지 않는 아이템입니다.");
                        }
                        break;

                    case "2":
                        
                        Console.WriteLine("\n[아이템 버리기]");
                        
                        if (inventory.Count == 0)
                        {
                            Console.WriteLine("버릴 아이템이 없습니다. 인벤토리가 비어있습니다.");
                        }
                        else
                        {
                            Console.WriteLine($"현재 보유 아이템 개수: {inventory.Count}개\n");

                            for (int i = 0; i < inventory.Count; i++)
                            {
                                Console.WriteLine($"- [{i + 1}] {inventory[i]}");
                            }

                            Console.Write("\n버릴 아이템 이름을 정확히 입력하세요: ");
                            
                            string throwItem = Console.ReadLine();

                            // 리스트에서 해당 아이템 삭제 시도
                            if (inventory.Remove(throwItem))
                            {
                                Console.WriteLine($"'{throwItem}'을(를) 버렸습니다.");
                            }
                            else
                            {
                                Console.WriteLine("인벤토리에 해당 아이템이 존재하지 않습니다.");
                                Console.WriteLine("\n버릴 아이템 이름을 정확히 입력하세요: ");

                            }
                        }
                        break;

                    case "3":
                        
                        Console.WriteLine("\n[인벤토리 상황 보기]");
                        
                        if (inventory.Count == 0)
                        {
                            Console.WriteLine("인벤토리가 비어 있습니다.");
                        }
                        else
                        {
                            Console.WriteLine($"현재 보유 아이템 개수: {inventory.Count}개\n");
                            
                            for (int i = 0; i < inventory.Count; i++)
                            {
                                Console.WriteLine($"- [{i + 1}] {inventory[i]}");
                            }
                        }
                        break;

                    case "0":
                        
                        Console.WriteLine("\n종료합니다.");
                        
                        return; // Main 메서드를 종료하여 프로그램 끝내기

                    default:
                        Console.WriteLine("\n잘못된 입력입니다. 1, 2, 3 번호 중 하나를 입력해 주세요.");
                        break;
                }

                // 결과를 확인한 후 다음 루프로 넘어가도록 대기
                Console.WriteLine("\n[Enter] 키를 누르면 메뉴로 돌아갑니다.");
                Console.ReadLine();


            }
        }
    }
}
