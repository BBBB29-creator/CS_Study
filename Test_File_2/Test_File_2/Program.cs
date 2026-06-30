namespace Test_File_2
{
    class Program
    {
        // 뒷 큰수를 계산하여 배열로 반환하는 알고리즘 함수
        static int[] Solution(int[] numbers)
        {
            int[] answer = new int[numbers.Length];
            
            Stack<int> stack = new Stack<int>();

            for (int i = 0; i < answer.Length; i++)
            {
                answer[i] = -1;
            }

            for (int i = 0; i < numbers.Length; i++)
            {
                while (stack.Count > 0 && numbers[stack.Peek()] < numbers[i])
                {
                    int index = stack.Pop();
                    answer[index] = numbers[i];
                }
                stack.Push(i);
            }

            return answer;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=== 뒷 큰수 찾기 프로그램 ===");
            Console.WriteLine("4개 이상 6개 이하의 숫자를 공백(' ')으로 구분해서 입력하세요.");
            Console.Write("입력 (예: 2 3 3 5 또는 9 1 5 3 6 2): ");

            // 1. 키보드로 한 줄을 입력받습니다.
            string input = Console.ReadLine();

            // 2. 공백을 기준으로 문자열을 쪼갭니다.
            string[] words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // 3. 입력된 숫자의 개수가 4개~6개 사이인지 검사합니다.
            if (words.Length < 4 || words.Length > 6)
            {
                Console.WriteLine($"\n입력 오류: 숫자는 4 ~ 6개만 입력 가능합니다. (현재 입력: {words.Length}개)");
                return; // 프로그램 종료
            }

            // 4. 잘라낸 문자열을 정수 배열(최대 크기 6)로 변환합니다.
            int[] numbers = new int[words.Length];
            
            for (int i = 0; i < words.Length; i++)
            {
                numbers[i] = int.Parse(words[i]);
            }

            // 5. 알고리즘 함수를 호출하여 정답 배열을 받아옵니다.
            int[] result = Solution(numbers);

            // 6. 결과를 보기 좋게 화면에 출력합니다.
            Console.WriteLine("\n[실행 결과]");
            Console.WriteLine($"입력 배열: [{string.Join(", ", numbers)}]");
            Console.WriteLine($"결과 배열: [{string.Join(", ", result)}]");
        }
    }
}
