using System.Runtime.Intrinsics.Arm;

namespace LinearAlogorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> list = new List<int>() { 19, 20, 33, 11, 38, 99, 27, 30, 20 };


            // 문제 : 최솟값과 최댓값을 제외한 평균 점수를 구하시오.

            // 1. 전체 합을 구한다.
            // 1-1. 전체 합을 저장할 변수를 만든다.
            // 2. 최솟값, 최댓값을 합에서 뺀다.
            // 3. 평균 점수를 구한다.

            // 결과값을 저장하기 위한 변수
            int sum = 0;
            int max = list[0];
            int min = list[0];
            
            double average = 0;

            // 순회 (전체합, 최댓값, 최솟값)
            // foreach 처음부터 끝까지 순회한다. - 인터페이스가 필요하다
            for (int i = 0; i < list.Count; i++)
            {
                // 알고리즘의 풀이
                sum += list[i]; // 전체 합산

                if (list[i] > max) max = list[i]; // 최댓값 갱신
                if (list[i] < min) min = list[i]; // 최솟값 갱신
            }

            // 순회 후 결과
            // 전체 합에서 최댓값과 최솟값을 빼고, 남은 개수(Count - 2)로 나눈다.
            sum = sum - max - min;
            average = (double)sum / (list.Count - 2);

            Console.WriteLine($"최댓값: {max}, 최솟값: {min}");
            Console.WriteLine($"최고 / 최저점 제외 평균: {average:F2}");
        }

        // public static double GetAverage(List<int> List)
    }
}

