using System;
using System.Collections.Generic;

namespace Dictionary
{
    internal class ItemData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public string Description { get; set; }

        public ItemData(int id, string name, float weight, string desc)
        {
            Id = id; Name = name; Weight = weight; Description = desc;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"\n[ID: {Id}] {Name} | 무게: {Weight}kg");
            Console.WriteLine($"▶ 설명: {Description}");
        }
    }

    internal class Program
    {

        private static Stack<string> uiStack = new(new[] { "메인 메뉴" });

        private static Dictionary<int, ItemData> itemById = new();
        private static Dictionary<string, ItemData> itemByName = new();

        private static List<ItemData> inventory = new();

        static void Main()
        {
            InitializeData(); // 데이터 및 인벤토리 초기화

            while (uiStack.Count > 0)
            {
                string state = uiStack.Peek();

                Action action = state switch
                {
                    "메인 메뉴" => MainMenu,
                    "아이템 도감" => DictionaryMenu,
                    "검색 결과" => SearchResult,
                    "인벤토리" => ShowInventory,
                    _ => () => uiStack.Pop()
                };

                action();
                Console.Clear();
            }
            Console.WriteLine("프로그램을 종료합니다.");
        }

        private static void InitializeData()
        {
            // 도감 데이터
            var rawData = new List<ItemData> {
                new(1, "물약", 0.2f, "체력을 25 회복합니다."),
                new(2, "대형 물약", 0.5f, "체력을 75 회복합니다."),
                new(3, "단검", 1.5f, "짧은 칼. 10 피해를 줍니다."),
                new(4, "소검", 4.0f, "일반적인 검. 20 피해를 줍니다."),
                new(5, "대검", 6.0f, "긴 날을 가진 검. 35 피해를 줍니다."),
                new(6, "투구", 3.0f, "머리를 보호해주는 방어구"),
                new(7, "사슬 갑옷", 5.0f, "사슬로 이루어진 몸통 방어구"),
                new(8, "장갑", 2.0f, "손에 착용하는 장비"),
                new(9, "신발", 2.0f, "발을 보호해주는 방어구"),
                new(10, "반지", 0.1f, "손가락에 끼우는 액세서리입니다.")
            };

            foreach (var item in rawData)
            {
                itemById.Add(item.Id, item);
                itemByName.Add(item.Name, item);
            }

            inventory.Add(itemByName["물약"]);
            inventory.Add(itemByName["대검"]);
            inventory.Add(itemByName["반지"]);
        }


        public static void MainMenu()
        {
            Console.WriteLine("=== 메인 메뉴 ===");
            Console.WriteLine("[1] 아이템 도감");
            Console.WriteLine("[2] 인벤토리");
            Console.WriteLine("[0] 프로그램 종료");
            Console.Write("선택: ");

            string input = Console.ReadLine();
            if (input == "1") uiStack.Push("아이템 도감");
            else if (input == "2") uiStack.Push("인벤토리");
            else if (input == "0") uiStack.Pop();
        }

        public static void DictionaryMenu()
        {
            Console.WriteLine("=== 아이템 도감 검색 ===");
            Console.WriteLine("조회할 아이템의 [ID 숫자] 또는 [아이템 이름]을 입력하세요.");
            Console.WriteLine("[1] 물약\n[2] 대형 물약\n[3] 단검\n[4] 소검\n[5] 대검\n[6] 투구\n[7] 사슬 갑옷\n[8] 장갑\n[9] 신발\n[10] 반지");
            Console.Write("입력: ");

            string input = Console.ReadLine()?.Trim();

            if (input == "0") { uiStack.Pop(); return; }
            if (string.IsNullOrEmpty(input)) return;

            uiStack.Push(input);
            uiStack.Push("검색 결과");
        }

        public static void SearchResult()
        {
            uiStack.Pop();
            string keyword = uiStack.Pop();

            ItemData foundItem = null;
            if (int.TryParse(keyword, out int id)) itemById.TryGetValue(id, out foundItem);
            else itemByName.TryGetValue(keyword, out foundItem);

            Console.WriteLine("=== 검색 결과 ===");
            if (foundItem != null) foundItem.PrintInfo();
            else Console.WriteLine($"\n'{keyword}'에 해당하는 아이템을 찾을 수 없습니다.");

            Console.WriteLine("\n아무 키나 누르면 도감 메뉴로 돌아갑니다.");
            Console.ReadKey();
        }


        public static void ShowInventory()
        {
            Console.WriteLine("=== 인벤토리 ===");

            if (inventory.Count == 0)
            {
                Console.WriteLine("가방이 텅 비어 있습니다.");
            }
            else
            {

                for (int i = 0; i < inventory.Count; i++)
                {
                    Console.Write($"[{i + 1}] 번 아이템 ----------------");
                    inventory[i].PrintInfo();
                    Console.WriteLine();
                }
            }

            Console.WriteLine("-----------------------------------");
            Console.WriteLine("[0] 뒤로 가기");
            Console.Write("선택: ");

            if (Console.ReadLine() == "0") uiStack.Pop();
        }
    }
}

