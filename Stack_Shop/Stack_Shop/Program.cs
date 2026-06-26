namespace Stack_Shop
{

    internal class Program
    {
        private static List<string> inventory = new List<string>();
        private static Stack<string> stack = new Stack<string>();

        private static string input = "";
        
        static int selectedItemIndex = -1;



        static void Main(string[] args)
        {
            stack.Push("메인 메뉴");

            while (stack.Count > 0)
            {
                PrintStack();

                string state = stack.Peek();
                
                if (state == "메인 메뉴")
                {
                    MainMenu();
                }
                else if (state == "아이템 구매")
                {
                    BuyItem();
                }
                else if (state == "아이템 판매")
                {
                    SellItem();
                }
                else if (state == "구매 확인")
                {
                    BuyConfirm();
                }
                else if (state == "판매 확인")
                {
                    SellConfirm();
                }

                Console.Clear();
            }
        }

        /// <summary>
        /// 스택 상황 출력(디버그용)
        /// </summary>
        public static void PrintStack()
        {
            Console.Write("스택 상황 : ");
            
            foreach (string s in stack)
            {
                Console.Write($"> {s} ");
            }
            
            Console.WriteLine();
            Console.WriteLine();
        }

        public static void MainMenu()
        {
            Console.WriteLine("=== 메인 메뉴 ===");
            Console.WriteLine("(1) 아이템 구매");
            Console.WriteLine("(2) 아이템 판매");
            Console.WriteLine("(0) 뒤로 가기");
            
            input = Console.ReadLine();

            if (input == "1")
            {
                stack.Push("아이템 구매");
            }
            else if (input == "2")
            {
                stack.Push("아이템 판매");
            }
            else if (input == "0")
            {
                stack.Pop();
            }
        }

        private static void BuyItem()
        {
            Console.WriteLine("=== 구매할 아이템 ===");
            Console.WriteLine("(1) 포션");
            Console.WriteLine("(2) 열쇠");
            Console.WriteLine("(3) 롱소드");
            Console.WriteLine("(0) 뒤로 가기");
            

            string item = string.Empty;
            
            input = Console.ReadLine();

            switch (input)
            {
                case "1":

                case "2":

                case "3":

                    stack.Push("구매 확인");
                    
                    break;
                
                case "0":
                    Console.WriteLine("(0) 뒤로 가기");
                    stack.Pop();
                    break;
            }
        }

        public static void BuyConfirm()
        {
            string item = string.Empty;

            if (input == "1")
            {
                item = "포션";
                Console.WriteLine("포션 : 사용하면 체력을 회복합니다.");
            }
            else if (input == "2")
            {
                item = "열쇠";
                Console.WriteLine("열쇠 : 사용하면 문을 엽니다.");
            }
            else if (input == "3")
            {
                item = "롱소드";
                Console.WriteLine("롱소드 : 착용하면 공격력을 올립니다.");
            }


            Console.WriteLine($"\n정말로 {item}을 구매하시겠습니까?");
            Console.WriteLine("(1) 구매 (0) 취소");
            Console.Write("선택: ");

            string buyInput = Console.ReadLine(); // 꼬임 방지

            if (buyInput == "1")
            {
                Console.WriteLine($"\n{item} 구매 완료");
                inventory.Add(item);

                Console.WriteLine("\n아무 키나 누르면 이전 단계로 돌아갑니다.");
                Console.ReadKey();

                stack.Pop();
            }
            else if (buyInput == "0")
            {
                stack.Pop();
            }
        }

        public static void SellItem()
        {
            Console.WriteLine("=== 판매할 아이템 ===");

            if (inventory.Count == 0)
            {
                Console.WriteLine("가지고 있는 아이템이 없습니다.");
            }

            for (int i = 0; i < inventory.Count; i++)
            {
                Console.WriteLine($"({i + 1}) {inventory[i]}");
            }

            Console.WriteLine("(0) 뒤로 가기");
            Console.Write("선택: ");

            string sellInput = Console.ReadLine();

            if (sellInput == "0")
            {
                stack.Pop();
                return;
            }

            if (int.TryParse(sellInput, out int choice) && choice > 0 && choice <= inventory.Count)
            {
                selectedItemIndex = choice - 1;

                stack.Push("판매 확인");
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다. 올바른 번호를 입력해 주세요.");
                Console.ReadKey();
            }
        }

        public static void SellConfirm()
        {
            if (selectedItemIndex < 0 || selectedItemIndex >= inventory.Count)
            {
                Console.WriteLine("오류: 선택된 아이템 정보가 올바르지 않습니다.");
                Console.ReadKey();
                stack.Pop();
                return;
            }

            string item = inventory[selectedItemIndex];

            Console.WriteLine($"▶ 정말로 '{item}'을 판매하시겠습니까?");
            Console.WriteLine("(1) 판매 (0) 취소");
            Console.Write("선택: ");

            string confirmInput = Console.ReadLine();

            if (confirmInput == "1")
            {
                Console.WriteLine($"\n'{item}' 판매 완료!");
                inventory.RemoveAt(selectedItemIndex);

                Console.WriteLine("\n아무 키나 누르면 메뉴로 돌아갑니다.");
                Console.ReadKey();

                stack.Pop();
            }
            else if (confirmInput == "0")
            {
                Console.WriteLine("\n판매를 취소했습니다.");
                Console.ReadKey();
                stack.Pop();
            }
        }
    }
}