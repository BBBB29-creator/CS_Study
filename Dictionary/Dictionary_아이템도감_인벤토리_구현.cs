using System;
using System.Collections.Generic;

namespace Dictionary
{
    internal readonly struct ItemData
    {
        public int Id { get; }
        public string Name { get; }
        public float Weight { get; }
        public string Description { get; }

        public ItemData(int id, string name, float weight, string desc)
        {
            Id = id; Name = name; Weight = weight; Description = desc;
        }

        public void PrintInfo()
        {
            Console.Write("\n[ID: "); Console.Write(Id); Console.Write("] "); Console.Write(Name);
            Console.Write(" | 무게: "); Console.Write(Weight); Console.WriteLine("kg");
            Console.Write("▶ 설명: "); Console.WriteLine(Description);
        }
    }

    internal class Program
    {
        private enum UIState : byte { MainMenu, DictionaryMenu, SearchResult, ShowInventory }
        private static readonly Stack<UIState> uiStack = new(4);

        // 1. [변경] 생성과 동시에 고유 키(Key)와 데이터(Value)를 쌍으로 묶어 직접 선언
        private static readonly Dictionary<int, ItemData> itemById = new(10) {
            { 1,  new(1, "물약", 0.2f, "체력을 25 회복합니다.") },
            { 2,  new(2, "대형 물약", 0.5f, "체력을 75 회복합니다.") },
            { 3,  new(3, "단검", 1.5f, "짧은 칼. 10 피해를 줍니다.") },
            { 4,  new(4, "소검", 4.0f, "일반적인 검. 20 피해를 줍니다.") },
            { 5,  new(5, "대검", 6.0f, "긴 날을 가진 검. 35 피해를 줍니다.") },
            { 6,  new(6, "투구", 3.0f, "머리를 보호해주는 방어구") },
            { 7,  new(7, "사슬 갑옷", 5.0f, "사슬로 이루어진 몸통 방어구") },
            { 8,  new(8, "장갑", 2.0f, "손에 착용하는 장비") },
            { 9,  new(9, "신발", 2.0f, "발을 보호해주는 방어구") },
            { 10, new(10, "반지", 0.1f, "손가락에 끼우는 액세서리입니다.") }
        };

        // 이름 전용 보관함은 중복 코딩을 피하기 위해 선언만 해두고 InitializeData에서 연결합니다.
        private static readonly Dictionary<string, ItemData> itemByName = new(10, StringComparer.Ordinal);
        private static readonly List<ItemData> inventory = new(10);
        private static string searchKeyword = string.Empty;

        static void Main()
        {
            InitializeData();
            uiStack.Push(UIState.MainMenu);

            while (uiStack.Count > 0)
            {
                switch (uiStack.Peek())
                {
                    case UIState.MainMenu: MainMenu(); break;
                    case UIState.DictionaryMenu: DictionaryMenu(); break;
                    case UIState.SearchResult: SearchResult(); break;
                    case UIState.ShowInventory: ShowInventory(); break;
                    default: uiStack.Pop(); break;
                }
                Console.Clear();
            }
            Console.WriteLine("프로그램을 종료합니다.");
        }

        private static void InitializeData()
        {
            // 2. [변경] 이미 생성된 itemById의 알맹이들을 바탕으로 이름 딕셔너리와 인벤토리 자동 구축
            foreach (var item in itemById.Values)
            {
                itemByName.Add(item.Name, item);
            }

            inventory.Add(itemByName["물약"]);
            inventory.Add(itemByName["대검"]);
            inventory.Add(itemByName["반지"]);
        }

        public static void MainMenu()
        {
            Console.Write("=== 메인 메뉴 ===\n[1] 아이템 도감\n[2] 인벤토리\n[0] 프로그램 종료\n입력 : ");
            string input = Console.ReadLine();

            if (input == "1") uiStack.Push(UIState.DictionaryMenu);
            else if (input == "2") uiStack.Push(UIState.ShowInventory);
            else if (input == "0") uiStack.Pop();
        }

        public static void DictionaryMenu()
        {
            Console.WriteLine("=== 아이템 도감 검색 ===\n조회할 아이템의 [ID 숫자] 또는 [아이템 이름]을 입력하세요.");
            Console.WriteLine("[1] 물약\n[2] 대형 물약\n[3] 단검\n[4] 소검\n[5] 대검\n[6] 투구\n[7] 사슬 갑옷\n[8] 장갑\n[9] 신발\n[10] 반지\n[0] 뒤로가기\n입력 : ");

            searchKeyword = Console.ReadLine()?.Trim();
            
            if (searchKeyword == "0") { uiStack.Pop(); return; }
            if (string.IsNullOrEmpty(searchKeyword)) return;

            uiStack.Push(UIState.SearchResult);
        }

        public static void SearchResult()
        {
            uiStack.Pop();
            Console.WriteLine("=== 검색 결과 ===");

            if (int.TryParse(searchKeyword, out int id))
            {
                if (itemById.TryGetValue(id, out var foundItem)) foundItem.PrintInfo();
                else Console.WriteLine($"\n'{searchKeyword}'에 해당하는 아이템을 찾을 수 없습니다.");
            }
            else
            {
                if (itemByName.TryGetValue(searchKeyword, out var foundItem)) foundItem.PrintInfo();
                else Console.WriteLine($"\n'{searchKeyword}'에 해당하는 아이템을 찾을 수 없습니다.");
            }

            Console.WriteLine("\n아무 키나 누르면 도감 메뉴로 돌아갑니다.");
            Console.ReadKey();
        }

        public static void ShowInventory()
        {
            Console.WriteLine("=== 인벤토리 ===");
            //if (inventory.Count == 0) Console.WriteLine("가방이 텅 비어 있습니다.");
            //else
            //{
                for (int i = 0; i < inventory.Count; i++)
                {
                    Console.Write("["); Console.Write(i + 1); Console.Write("] 번 아이템 ----------------");
                    inventory[i].PrintInfo(); Console.WriteLine();
                }
            //}
            Console.Write("-----------------------------------\n[0] 뒤로 가기\n선택: ");
            if (Console.ReadLine() == "0") uiStack.Pop();
        }
    }
}