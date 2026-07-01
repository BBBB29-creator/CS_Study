using System.Linq.Expressions;
using System.Xml.Linq;

namespace InsertSort
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

            // 1. 이름순 정렬 및 출력
            Console.WriteLine("▶ 이름순(가나다순) 정렬");
            SortByName(monsters);
            PrintMonsters(monsters);

            // 2. 레벨순 정렬 및 출력
            Console.WriteLine("\n▶ 레벨순(높은순) 정렬");
            SortByLevel(monsters);
            PrintMonsters(monsters);

            // 3. 속성순(enum 순서: 물->불->흙->전기) 정렬 및 출력
            Console.WriteLine("\n▶ 속성순 정렬");
            SortByElement(monsters);
            PrintMonsters(monsters);
        }

        // [1] 이름순 삽입 정렬 (문자열 비교는 String.Compare를 사용합니다)
        static void SortByName(List<Monster> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                Monster key = list[i];
                int j = i - 1;

                while (j >= 0 && string.Compare(list[j].Name, key.Name) > 0)
                {
                    list[j + 1] = list[j];
                    j--;
                }
                list[j + 1] = key;
            }
        }

        // [2] 레벨순 삽입 정렬 (역순)
        static void SortByLevel(List<Monster> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                Monster key = list[i];
                int j = i - 1;

                while (j >= 0 && list[j].Level < key.Level) // 두번째 부등호만 바꿔주면 된다.
                {
                    list[j + 1] = list[j];
                    j--;
                }
                list[j + 1] = key;
            }
        }

        // [3] 속성순 삽입 정렬
        static void SortByElement(List<Monster> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                Monster key = list[i];
                int j = i - 1;

                while (j >= 0 && (int)list[j].Element > (int)key.Element)
                {
                    list[j + 1] = list[j];
                    j--;
                }
                list[j + 1] = key;
            }
        }

        // 출력을 편하게 도와주는 헬퍼 함수
        static void PrintMonsters(List<Monster> list)
        {
            foreach (var m in list)
            {
                Console.WriteLine($"이름: {m.Name,-5} | 레벨: {m.Level,-3} | 속성: {m.Element}");
            }
        }
    }
}
