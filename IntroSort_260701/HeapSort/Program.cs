using System;
using System.Collections.Generic;

namespace HeapSort
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

            Console.WriteLine("======= [힙 정렬 결과] =======");

            // 1. 이름순 힙 정렬
            Console.WriteLine("\n--- 이름순(가나다순) 힙 정렬 ---");
            HeapSortByName(monsters);
            PrintMonsters(monsters);

            // 2. Level순 힙 정렬
            Console.WriteLine("\n--- 레벨순(낮은순) 힙 정렬 ---");
            HeapSortByLevel(monsters);
            PrintMonsters(monsters);

            // 3. 속성순 힙 정렬
            Console.WriteLine("\n--- 속성순 힙 정렬 ---");
            HeapSortByElement(monsters);
            PrintMonsters(monsters);
        }

        // ==========================================
        // 1. 이름순 힙 정렬
        // ==========================================
        static void HeapSortByName(List<Monster> list)
        {
            int n = list.Count;

            // 최대 힙 구조로 빌드업
            for (int i = n / 2 - 1; i >= 0; i--)
                HeapifyByName(list, n, i);

            // 하나씩 꺼내서 맨 뒤로 보냄 (스왑 후 다시 힙 구조 정렬)
            for (int i = n - 1; i > 0; i--)
            {
                (list[0], list[i]) = (list[i], list[0]);
                HeapifyByName(list, i, 0);
            }
        }

        static void HeapifyByName(List<Monster> list, int n, int i)
        {
            int largest = i;
            int left = 2 * i + 1;  // 왼쪽 자식 노드 방 번호
            int right = 2 * i + 2; // 오른쪽 자식 노드 방 번호

            if (left < n && string.Compare(list[left].Name, list[largest].Name) > 0) largest = left;
            if (right < n && string.Compare(list[right].Name, list[largest].Name) > 0) largest = right;

            if (largest != i)
            {
                (list[i], list[largest]) = (list[largest], list[i]);
                HeapifyByName(list, n, largest);
            }
        }

        // ==========================================
        // 2. 레벨순 힙 정렬 (정석적인 숫자형 힙 구조)
        // ==========================================
        static void HeapSortByLevel(List<Monster> list)
        {
            int n = list.Count;

            // 1단계: 원본 리스트를 최대 힙(부모가 자식보다 무조건 큰 구조)으로 만듭니다.
            for (int i = n / 2 - 1; i >= 0; i--)
                HeapifyByLevel(list, n, i);

            // 2단계: 최상단 루트(가장 큰 값)를 맨 뒤로 보내고, 남은 트리로 다시 힙을 만듭니다.
            for (int i = n - 1; i > 0; i--)
            {
                (list[0], list[i]) = (list[i], list[0]); // 가장 큰 값을 맨 뒤 방과 교환 (스왑)
                HeapifyByLevel(list, i, 0);               // 줄어든 트리 크기(i)로 재정렬
            }
        }

        static void HeapifyByLevel(List<Monster> list, int n, int i)
        {
            int largest = i;       // 현재 내가 부모 노드라고 가정
            int left = 2 * i + 1;  // 공식에 따른 왼쪽 자식 인덱스 [티스토리]
            int right = 2 * i + 2; // 공식에 따른 오른쪽 자식 인덱스 [티스토리]

            // 왼쪽 자식이 나보다 레벨이 더 크다면 자식을 왕좌로 임명
            if (left < n && list[left].Level > list[largest].Level) largest = left;

            // 오른쪽 자식이 나보다 레벨이 더 크다면 자식을 왕좌로 임명
            if (right < n && list[right].Level > list[largest].Level) largest = right;

            // 왕좌가 바뀌었다면 자리를 교환하고, 밑으로 내려가서 다시 수색(재귀)
            if (largest != i)
            {
                (list[i], list[largest]) = (list[largest], list[i]);
                HeapifyByLevel(list, n, largest);
            }
        }

        // ==========================================
        // 3. 속성순 힙 정렬
        // ==========================================
        static void HeapSortByElement(List<Monster> list)
        {
            int n = list.Count;

            for (int i = n / 2 - 1; i >= 0; i--)
                HeapifyByElement(list, n, i);

            for (int i = n - 1; i > 0; i--)
            {
                (list[0], list[i]) = (list[i], list[0]);
                HeapifyByElement(list, i, 0);
            }
        }

        static void HeapifyByElement(List<Monster> list, int n, int i)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n && (int)list[left].Element > (int)list[largest].Element) largest = left;
            if (right < n && (int)list[right].Element > (int)list[largest].Element) largest = right;

            if (largest != i)
            {
                (list[i], list[largest]) = (list[largest], list[i]);
                HeapifyByElement(list, n, largest);
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