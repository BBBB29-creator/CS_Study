namespace Test_File_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("스킬트리");
        }

        public class Solution
        {
            public int solution(string skill, string[] skill_trees)
            {
                int answer = 0;

                // 1. [최적화] 각 스킬의 순서를 딕셔너리에 미리 매핑합니다.
                // 예: ['C': 0, 'B': 1, 'D': 2] -> 이제 인덱스를 찾을 때 내부 루프를 돌지 않고 단번에(O(1)) 찾습니다!
                Dictionary<char, int> skillOrder = new Dictionary<char, int>();
                for (int i = 0; i < skill.Length; i++)
                {
                    skillOrder[skill[i]] = i;
                }

                foreach (string tree in skill_trees)
                {
                    if (IsValidSkillTree(skillOrder, tree))
                    {
                        answer++;
                    }
                }

                return answer;
            }

            private bool IsValidSkillTree(Dictionary<char, int> skillOrder, string tree)
            {
                int skillIndex = 0;

                foreach (char c in tree)
                {
                    // 2. 딕셔너리에 없는 스킬(선행 관계없는 스킬)은 즉시 패스
                    if (!skillOrder.ContainsKey(c)) continue;

                    // 3. 딕셔너리에서 단번에 순서 값을 꺼내와 비교 (IndexOf의 성능 문제 해결!)
                    int targetIdx = skillOrder[c];

                    if (targetIdx != skillIndex) return false;

                    skillIndex++;
                }

                return true;
            }
        }
    }
}
