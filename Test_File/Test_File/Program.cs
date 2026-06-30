namespace Test_File
{
    using System;

    class Program
    {
        // 사용자가 입력한 숫자 n을 받아 소수 개수를 계산하는 함수 (알고리즘)
        static int Solution(int n)
        {
            int count = 0;
            bool[] isPrime = new bool[n + 1];

            // 0과 1은 소수가 아니므로 제외 처리
            isPrime[0] = true;
            isPrime[1] = true;

            // 소수 외 다른 배수 제거 처리
            for (int i = 2; i * i <= n; i++)
            {
                if (isPrime[i] == true) continue;

                for (int j = i * i; j <= n; j += i)
                {
                    isPrime[j] = true;
                }
            }

            // 지워지지 않고 남은 소수의 개수를 누적
            for (int i = 2; i <= n; i++)
            {
                if (isPrime[i] == false) count++;
            }

            return count; // 계산된 개수 반환


            static void Main(string[] args)
            {

                Console.Write("숫자(n)를 입력: ");

                string input = Console.ReadLine();

                int n = int.Parse(input);

                // Solution 함수를 호출하여 소수의 개수를 반환받음.
                int result = Solution(n);

                Console.WriteLine($"1부터 {n} 사이에 있는 소수의 개수: {result}개");
            }
        }
    }
}
