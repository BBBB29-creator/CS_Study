namespace QuickSort
{
    public enum MonsterElement { 물, 불, 흙, 전기 }

    public class Monster
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public MonsterElement Element { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Monster> monsters = new()
            {
                new() { Name = "물변귀", Level = 50, Element = MonsterElement.물 },
                new() { Name = "불변귀", Level = 10, Element = MonsterElement.불 },
                new() { Name = "땅변귀", Level = 30, Element = MonsterElement.흙 },
                new() { Name = "뇌변귀", Level = 40, Element = MonsterElement.전기 }
            };

            Console.WriteLine("======= [퀵 정렬 결과] =======");

            // 1. 이름순 퀵 정렬
            Console.WriteLine("\n--- 이름순(가나다순) 퀵 정렬 ---");
            QuickSortByName(monsters, 0, monsters.Count - 1);
            PrintMonsters(monsters);

            // 2. 레벨순 퀵 정렬
            Console.WriteLine("\n--- 레벨순(낮은순) 퀵 정렬 ---");
            QuickSortByLevel(monsters, 0, monsters.Count - 1);
            PrintMonsters(monsters);

            // 3. 속성순 퀵 정렬
            Console.WriteLine("\n--- 속성순 퀵 정렬 ---");
            QuickSortByElement(monsters, 0, monsters.Count - 1);
            PrintMonsters(monsters);
        }

        // --- 1. 이름순 퀵 정렬 ---
        static void QuickSortByName(List<Monster> list, int left, int right)
        {
            if (left >= right) return;

            int pivotIndex = PartitionByName(list, left, right);
            QuickSortByName(list, left, pivotIndex);
            QuickSortByName(list, pivotIndex + 1, right);
        }

        static int PartitionByName(List<Monster> list, int left, int right)
        {
            string pivot = list[(left + right) / 2].Name;
            int i = left - 1;
            int j = right + 1;

            while (true)
            {
                do { i++; } while (string.Compare(list[i].Name, pivot) < 0);
                do { j--; } while (string.Compare(list[j].Name, pivot) > 0);

                if (i >= j) return j;
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        // --- 2. 레벨순 퀵 정렬 ---
        static void QuickSortByLevel(List<Monster> list, int left, int right)
        {
            if (left >= right) return; // 정렬할 범위가 1개 이하면 종료 (기저 조건)

            int pivotIndex = PartitionByLevel(list, left, right); // 반으로 쪼개고 기준점 얻기
            
            QuickSortByLevel(list, left, pivotIndex);             // 기준점 왼쪽 정렬 (재귀 호출)
            QuickSortByLevel(list, pivotIndex + 1, right);         // 기준점 오른쪽 정렬 (재귀 호출)
        }

        static int PartitionByLevel(List<Monster> list, int left, int right)
        {
            int pivot = list[(left + right) / 2].Level; // 배열의 중간값을 기준(Pivot)으로 설정
            int i = left - 1;
            int j = right + 1;

            while (true)
            {
                // 왼쪽에서 기준점보다 큰 값을 찾을 때까지 전진
                do { i++; } while (list[i].Level < pivot);

                // 오른쪽에서 기준점보다 작은 값을 찾을 때까지 후진
                do { j--; } while (list[j].Level > pivot);

                if (i >= j) return j; // 서로 엇갈렸다면 쪼개기 경계면(j)을 반환

                (list[i], list[j]) = (list[j], list[i]); // 찾은 두 녀석의 위치를 바꿈 (스왑)
            }
        }

        // --- 3. 속성순 퀵 정렬 ---
        static void QuickSortByElement(List<Monster> list, int left, int right)
        {
            if (left >= right) return;

            int pivotIndex = PartitionByElement(list, left, right);
            
            QuickSortByElement(list, left, pivotIndex);
            QuickSortByElement(list, pivotIndex + 1, right);
        }

        static int PartitionByElement(List<Monster> list, int left, int right)
        {
            int pivot = (int)list[(left + right) / 2].Element;
            int i = left - 1;
            int j = right + 1;

            while (true)
            {
                do { i++; } while ((int)list[i].Element < pivot);
                do { j--; } while ((int)list[j].Element > pivot);

                if (i >= j) return j;
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        static void PrintMonsters(List<Monster> list)
        {
            foreach (var m in list)
            {
                Console.WriteLine($"이름: {m.Name,-5} | 레벨: {m.Level,-3} | 속성: {m.Element}");
            }
        }
    }
}
