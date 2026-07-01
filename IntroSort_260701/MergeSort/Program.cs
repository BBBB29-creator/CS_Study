namespace MergeSort
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

            Console.WriteLine("======= [병합 정렬 결과] =======");

            // 1. 이름순 병합 정렬
            Console.WriteLine("\n--- 이름순(가나다순) 병합 정렬 ---");
            MergeSortByName(monsters, 0, monsters.Count - 1);
            PrintMonsters(monsters);

            // 2. 레벨순 병합 정렬
            Console.WriteLine("\n--- 레벨순(낮은순) 병합 정렬 ---");
            MergeSortByLevel(monsters, 0, monsters.Count - 1);
            PrintMonsters(monsters);

            // 3. 속성순 병합 정렬
            Console.WriteLine("\n--- 속성순 병합 정렬 ---");
            MergeSortByElement(monsters, 0, monsters.Count - 1);
            PrintMonsters(monsters);
        }

        // ==========================================
        // 1. 이름순 병합 정렬
        // ==========================================
        static void MergeSortByName(List<Monster> list, int left, int right)
        {
            if (left >= right) return;

            int mid = (left + right) / 2;
            MergeSortByName(list, left, mid);
            MergeSortByName(list, mid + 1, right);
            MergeByName(list, left, mid, right);
        }

        static void MergeByName(List<Monster> list, int left, int mid, int right)
        {
            List<Monster> temp = new List<Monster>(new Monster[right - left + 1]);
            int i = left, j = mid + 1, k = 0;

            while (i <= mid && j <= right)
            {
                if (string.Compare(list[i].Name, list[j].Name) <= 0) temp[k++] = list[i++];
                else temp[k++] = list[j++];
            }
            while (i <= mid) temp[k++] = list[i++];
            while (j <= right) temp[k++] = list[j++];

            for (i = 0; i < temp.Count; i++) list[left + i] = temp[i];
        }

        // ==========================================
        // 2. 레벨순 병합 정렬 (정석 구조)
        // ==========================================
        static void MergeSortByLevel(List<Monster> list, int left, int right)
        {
            if (left >= right) return; // 1개 이하로 쪼개지면 재귀 탈출

            int mid = (left + right) / 2;          // 반으로 쪼개기 위한 중간 지점 계산 [티스토리]
            MergeSortByLevel(list, left, mid);      // 왼쪽 구역 반으로 또 쪼개기 (재귀) [티스토리]
            MergeSortByLevel(list, mid + 1, right);  // 오른쪽 구역 반으로 또 쪼개기 (재귀) [티스토리]

            MergeByLevel(list, left, mid, right);   // 쪼개진 두 그룹을 크기 순으로 합치기 [티스토리]
        }

        static void MergeByLevel(List<Monster> list, int left, int mid, int right)
        {
            // 합친 결과를 임시로 저장할 빈 주머니(정렬용 임시 공간) 생성
            List<Monster> temp = new List<Monster>(new Monster[right - left + 1]);

            int i = left;      // 왼쪽 그룹의 시작점 가리키는 포인터
            int j = mid + 1;   // 오른쪽 그룹의 시작점 가리키는 포인터
            int k = 0;         // 임시 주머니(temp)의 인덱스 포인터

            // 양쪽 그룹에 비교할 데이터가 남아있는 동안 반복
            while (i <= mid && j <= right)
            {
                // 왼쪽 몬스터 레벨이 더 작거나 같다면 임시 주머니에 먼저 쏙 넣음
                if (list[i].Level <= list[j].Level)
                {
                    temp[k++] = list[i++];
                }
                // 오른쪽 몬스터 레벨이 더 작다면 오른쪽 놈을 임시 주머니에 넣음
                else
                {
                    temp[k++] = list[j++];
                }
            }

            // 한쪽 그룹이 먼저 다 떨어졌을 때, 남은 다른 쪽 그룹 데이터들을 싹 쓸어 담기 [티스토리]
            while (i <= mid) temp[k++] = list[i++];
            while (j <= right) temp[k++] = list[j++];

            // 완벽하게 정렬되어 채워진 임시 주머니(temp)의 내용을 원본 리스트에 고스란히 복사 [티스토리]
            for (i = 0; i < temp.Count; i++)
            {
                list[left + i] = temp[i];
            }
        }

        // ==========================================
        // 3. 속성순 병합 정렬
        // ==========================================
        static void MergeSortByElement(List<Monster> list, int left, int right)
        {
            if (left >= right) return;

            int mid = (left + right) / 2;
            
            MergeSortByElement(list, left, mid);
            MergeSortByElement(list, mid + 1, right);
            MergeByElement(list, left, mid, right);
        }

        static void MergeByElement(List<Monster> list, int left, int mid, int right)
        {
            List<Monster> temp = new List<Monster>(new Monster[right - left + 1]);
            int i = left, j = mid + 1, k = 0;

            while (i <= mid && j <= right)
            {
                if ((int)list[i].Element <= (int)list[j].Element) temp[k++] = list[i++];
                else temp[k++] = list[j++];
            }
            while (i <= mid) temp[k++] = list[i++];
            while (j <= right) temp[k++] = list[j++];

            for (i = 0; i < temp.Count; i++) list[left + i] = temp[i];
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