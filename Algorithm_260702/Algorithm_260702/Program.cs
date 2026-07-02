using System.Runtime.Serialization.Formatters;

namespace SearchAlgorithm_260702
{
    internal class Program
    {
        struct Node
        {
            public int Row;
            public int Col;


            public Node(int row, int col)
            {
                this.Row = row;
                this.Col = col;
            }
        }

        static int[,] Map =
        {
            { 0, 0, 0, 0, 0},
            { 0, 1, 1, 0, 0},
            { 0, 1, 0, 0, 0},
            { 0, 0, 0, 1, 0},
            { 0, 0, 0, 0, 0}
        };

        static int[] directR = { -1, 1, 0 , 0 };
        static int[] directC = { 0, 0, -1 , 1 };

        // 최단 거리를 반환하는 함수
        // param name = "startR" > 시작 함수
        // param name = "startC" > 시작 함수
        // param name = "endR" > 시작 함수
        // param name = "endC" > 시작 함수

        static int BFSPath(int startR, int startC, int endR, int endC)
        {
            // 맵의 크기
            int mapRows = Map.GetLength(0);
            int mapCols = Map.GetLength(1);


            // mapRows, mapCols 안의 최단 거리
            // dist[0, 1] == 저 위치까지의 최단 거리를 가지고 있다.
            // 만약 아직 확인하지 않았다면 -1
            //       { 0, 0, 0, 0, 0},     { 0, 1, 2, 3, 4},
            //       { 0, 1, 1, 0, 0},     { 1, -1, -1, 4, 5},
            //       { 0, 1, 0, 0, 0},     { 2, -1, 41, 5, 6},
            //       { 0, 0, 0, 1, 0},     { 3, -1, -1, -1, -1},
            //       { 0, 0, 0, 0, 0}      { 4, -1, -1, -1, -1}

            int[,] dist = new int[mapRows, mapCols];

            for(int i = 0; i < mapRows; i++)
            {
                for(int j = 0; j < mapCols; j++)
                {
                    dist[i, j] = -1;
                }
            }
            // 가까운 곳부터 탐색을 해서 넣어두면 가까운 곳부터 나오게 됨.
            Queue<Node> bfsQueue = new Queue<Node>();

                // 큐와 distance 배열에 시작점을 넣어준다. (초기화)
            bfsQueue.Enqueue(new Node(startR, startC));
            dist[startR, startC] = 0;


            // 큐가 공백이 될 때 까지.
            while(bfsQueue.Count > 0)
            {
                // 큐에서 현재 좌표를 하나 꺼낸다. 가장 먼저 들어간 노드.
                Node currentNode = bfsQueue.Dequeue();

                // 현재 위치
                int currentRow = currentNode.Row;
                int currentCol = currentNode.Col;


                if(currentRow == endR && currentCol == endC)
                {
                    return dist[currentRow, currentCol];
                }
                // 현재 위치에서 상하좌우를 전부 확인한다.
                for(int i = 0; i < 4; i++)
                {
                    // 행의 위치와 열의 위치를 상하좌우 이동하며 판단할 수 있도록 해준다.
                    int moveRow = currentRow + directR[i];
                    int moveCol = currentCol + directC[i];

                    // 맵의 범위를 넘었거나 / 벽이거나 / 이미 방문했으면 continue
                    if (moveRow < 0 || moveRow >= mapRows || moveCol < 0 || moveCol >= mapCols)
                        continue;

                    if (Map[moveRow, moveCol] == 1)
                        continue;

                    if (dist[moveRow, moveCol] != -1)
                        continue;

                    // 위의 내용을 전부 만족하지 않는다면, 거리를 갱신하고 큐에 추가
                    // 
                    dist[moveRow, moveCol] = dist[currentRow, currentCol] + 1;
                    bfsQueue.Enqueue(new Node(moveRow, moveCol));
                }
                // 큐에서 하나를 빼서 그 위치부터 주변을 순회하고
                // 마지막에는 갱신한 내용을 넣어줄 것이기 때문에.
                // 큐가 비었다 == 더 이상 갱신할게 없다. == 전부 순회했다.
            }

            // 시작점에서 도작첨까지에서최단 거리를 저장 해두어야 한다.
            // 한 칸씩 탐색해가면서, 현재까지의 거리를 저장.
            // 도착하면 해당 변수의 내용이 최단거리임.
            
            // 도착점에 도달하지 못한 경우,
            return -1;
        }
        
        static void Main(string[] args)
        {
            int result = BFSPath(0, 0, 3, 4);
           
            Console.WriteLine($"도착 지점까지의 최소 거리 : {result}");
        }
    }
}
