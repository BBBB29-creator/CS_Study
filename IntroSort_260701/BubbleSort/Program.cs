using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BubbleSort
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

            Console.WriteLine("======= [버블 정렬 및 시간 측정 결과] =======");

            // 1. 이름순 측정
            Console.WriteLine("\n--- 이름순(가나다순) 버블 정렬 ---");
            BubbleSortByName(monsters);
            PrintMonsters(monsters);

            // 2. 레벨순 측정
            Console.WriteLine("\n--- 레벨순(낮은순) 버블 정렬 ---");
            BubbleSortByLevel(monsters);
            PrintMonsters(monsters);

            // 3. 속성순 측정
            Console.WriteLine("\n--- 속성순 버블 정렬 ---");
            BubbleSortByElement(monsters);
            PrintMonsters(monsters);
        }

        // 1. 이름순 버블 정렬 (시간 측정 추가)
        static void BubbleSortByName(List<Monster> list)
        {
            Stopwatch sw = new Stopwatch(); // 타이머 생성
            sw.Start(); // 측정 시작

            for (int i = 0; i < list.Count - 1; i++)
            {
                bool isSwapped = false;
                for (int j = 0; j < list.Count - 1 - i; j++)
                {
                    if (string.Compare(list[j].Name, list[j + 1].Name) > 0)
                    {
                        (list[j], list[j + 1]) = (list[j + 1], list[j]);
                        isSwapped = true;
                    }
                }
                if (!isSwapped) break;
            }

            sw.Stop(); // 측정 종료
            // 밀리초(ms) 단위와 타이머의 정밀한 틱(Ticks) 수치를 함께 출력합니다.
            Console.WriteLine($"[정렬 완료] 소요 시간: {sw.Elapsed.TotalMilliseconds} ms ({sw.ElapsedTicks} Ticks)");
        }

        // 2. 레벨순 버블 정렬 (시간 측정 추가)
        static void BubbleSortByLevel(List<Monster> list)
        {
            Stopwatch sw = Stopwatch.StartNew(); // 생성과 동시에 시작하는 숏컷 문법

            for (int i = 0; i < list.Count - 1; i++)
            {
                bool isSwapped = false;
                for (int j = 0; j < list.Count - 1 - i; j++)
                {
                    if (list[j].Level > list[j + 1].Level)
                    {
                        (list[j], list[j + 1]) = (list[j + 1], list[j]);
                        isSwapped = true;
                    }
                }
                if (!isSwapped) break;
            }

            sw.Stop();
            Console.WriteLine($"[정렬 완료] 소요 시간: {sw.Elapsed.TotalMilliseconds} ms ({sw.ElapsedTicks} Ticks)");
        }

        // 3. 속성순 버블 정렬 (시간 측정 추가)
        static void BubbleSortByElement(List<Monster> list)
        {
            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < list.Count - 1; i++)
            {
                bool isSwapped = false;
                for (int j = 0; j < list.Count - 1 - i; j++)
                {
                    if ((int)list[j].Element > (int)list[j + 1].Element)
                    {
                        (list[j], list[j + 1]) = (list[j + 1], list[j]);
                        isSwapped = true;
                    }
                }
                if (!isSwapped) break;
            }

            sw.Stop();
            Console.WriteLine($"[정렬 완료] 소요 시간: {sw.Elapsed.TotalMilliseconds} ms ({sw.ElapsedTicks} Ticks)");
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
