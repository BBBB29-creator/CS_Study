namespace Queue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Queue<int> queue = new Queue<int>();

            // 대기열 추가
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Enqueue(4);

            // 대기열 꺼내기
            int value1 = queue.Dequeue();
            int value2 = queue.Dequeue();
            int value3 = queue.Dequeue();

            // ghkrdls
            int begin = queue.Peek();
        }
    }
}
