namespace Sort_Algorithm_20260701
{


    public static class Extension
    {
        // <선택정렬>
        // 데이터 중 가장 작은 값부터 하나씩 선택하여 정렬
        // 시간복잡도 -  O(n²)
        // 공간복잡도 -  O(1)
        // 안정정렬   -  X
        public static void SelectionSort(this IList<int> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int minIndex = i;
                for (int j = i; j < list.Count; j++)
                {
                    if (list[j] < list[minIndex]) minIndex = j;
                }

                int temp = list[minIndex];
                list[minIndex] = list[i];
                list[i] = temp;
            }
        }

        // <삽입정렬>
        // 데이터를 하나씩 꺼내어 정렬된 자료 중 적합한 위치에 삽입하여 정렬
        // 시간복잡도 -  O(n²), 16개 미만일때 O(n) 에 가까움
        // 공간복잡도 -  O(1)
        // 안정정렬   -  O
        public static void InsertionSort(this IList<int> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                // 1. 데이터를 하나 꺼낸다.
                int target = list[i];
                // 2. 정렬된 자료 중 적합한 위치에 삽입한다.
                // 2-3. 반복해서
                for (int j = i; j >= 0; j--)
                {
                    // 예외 상황 : 제일 작을 값이었을 때는 맨 앞에 배치
                    if (j == 0)
                    {
                        list[0] = target;
                    }
                    // 2-1. 앞 데이터와 비교해서
                    else if (list[j - 1] <= target)
                    {
                        // 더이상 안밀어내도 될때까지 이동한다.
                        list[j] = target;
                        break;
                    }
                    else
                    {
                        // 2-2. 더 작으면 뒤로 밀어내고
                        list[j] = list[j - 1];
                    }
                }


            }
        }

        // <버블정렬>
        // 서로 인접한 데이터를 비교하여 정렬
        // 시간복잡도 -  O(n²), 인덱스를 사용하지 않는 자료구조에도 적용할 수 있는 정렬 알고리즘
        // 공간복잡도 -  O(1)
        // 안정정렬   -  O
        public static void BubbleSort(this IList<int> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count - 1; j++)
                {
                    if (list[j] > list[j + 1])
                    {
                        int temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }

        // <병합정렬>
        // 데이터를 2분할하여 정렬 후 합병
        // 데이터 갯수만큼의 추가적인 메모리가 필요
        // 시간복잡도 -  O(nlogn)
        // 공간복잡도 -  O(n)
        // 안정정렬   -  O
        public static void MergeSort(this IList<int> list) => MergeSort(list, 0, list.Count - 1);
        private static void MergeSort(IList<int> list, int start, int end)
        {
            if (start == end)
                return;

            int middle = (start + end) / 2;
            MergeSort(list, start, middle);
            MergeSort(list, middle + 1, end);
            Merge(list, start, middle, end);
        }

        private static void Merge(IList<int> list, int start, int middle, int end)
        {
            List<int> sortedList = new List<int>();
            int left = start;
            int right = middle + 1;

            while (left <= middle && right <= end)
            {
                if (list[left] < list[right])
                {
                    sortedList.Add(list[left]);
                    left++;
                }
                else
                {
                    sortedList.Add(list[right]);
                    right++;
                }
            }

            if (left > middle)
            {
                for (int i = right; i <= end; i++)
                {
                    sortedList.Add(list[i]);
                }
            }
            else // if (right > end)
            {
                for (int i = left; i <= middle; i++)
                {
                    sortedList.Add(list[i]);
                }
            }

            for (int i = 0; i < sortedList.Count; i++)
            {
                list[start + i] = sortedList[i];
            }
        }

        // <퀵정렬>
        // 하나의 피벗을 기준으로 작은값과 큰값을 2분할하여 정렬
        // 최악의 경우(피벗이 최소값 또는 최대값)인 경우 시간복잡도가 O(n²)
        // 시간복잡도 -  평균 : O(nlogn)   최악 : O(n²)
        // 공간복잡도 -  O(1)
        // 안정정렬   -  X
        public static void QuickSort(this IList<int> list) => QuickSort(list, 0, list.Count - 1);
        private static void QuickSort(IList<int> list, int start, int end)
        {
            if (start >= end)
            {
                return;
            }

            int pivot = start;
            int left = pivot + 1;
            int right = end;

            while (left <= right)
            {
                while (list[left] <= list[pivot] && left < right)
                {
                    left++;
                }
                while (list[right] > list[pivot] && left <= right)
                {
                    right--;
                }

                if (left < right)
                {
                    int temp = list[left];
                    list[left] = list[right];
                    list[right] = temp;
                }
                else
                {
                    int temp = list[pivot];
                    list[pivot] = list[right];
                    list[right] = temp;
                    break;
                }
            }

            QuickSort(list, start, right - 1);
            QuickSort(list, right + 1, end);
        }

        // <힙정렬>
        // 힙을 이용하여 우선순위가 가장 높은 요소가 가장 마지막 요소와 교체된 후 제거되는 방법을 이용
        // 배열에서 연속적인 데이터를 사용하지 않기 때문에 캐시 메모리를 효율적으로 사용할 수 없어 상대적으로 느림
        // 시간복잡도 -  O(nlogn)
        // 공간복잡도 -  O(1)
        // 안정정렬   -  X
        public static void HeapSort(IList<int> list)
        {
            MakeHeap(list);

            for (int i = list.Count - 1; i > 0; i--)
            {
                int temp = list[0];
                list[0] = list[i];
                list[i] = temp;
                Heapify(list, 0, i);
            }
        }

        private static void MakeHeap(IList<int> list)
        {
            for (int i = list.Count / 2 - 1; i >= 0; i--)
            {
                Heapify(list, i, list.Count);
            }
        }

        private static void Heapify(IList<int> list, int index, int size)
        {
            int left = index * 2 + 1;
            int right = index * 2 + 2;
            int max = index;
            if (left < size && list[left] > list[max])
            {
                max = left;
            }
            if (right < size && list[right] > list[max])
            {
                max = right;
            }

            if (max != index)
            {
                int temp = list[index];
                list[index] = list[max];
                list[max] = temp;
                Heapify(list, max, size);
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            int count = 10;

            List<int> selectionList = new List<int>();
            List<int> insertionList = new List<int>();
            List<int> bubbleList = new List<int>();
            List<int> mergeList = new List<int>();
            List<int> quickList = new List<int>();
            List<int> heapList = new List<int>();

            Console.WriteLine("랜덤 데이터 : ");
            for (int i = 0; i < count; i++)
            {
                int rand = random.Next() % 99 + 1;
                Console.Write($"{rand,3}");

                selectionList.Add(rand);
                insertionList.Add(rand);
                bubbleList.Add(rand);
                mergeList.Add(rand);
                quickList.Add(rand);
                heapList.Add(rand);
            }
            Console.WriteLine();
            Console.WriteLine();

            selectionList.SelectionSort();
            Console.WriteLine("선택 정렬 결과 : ");
            foreach (int value in selectionList)
            {
                Console.Write($"{value,3}");
            }
            Console.WriteLine();

            insertionList.InsertionSort();
            Console.WriteLine("삽입 정렬 결과 : ");
            foreach (int value in insertionList)
            {
                Console.Write($"{value,3}");
            }
            Console.WriteLine();

            bubbleList.BubbleSort();
            Console.WriteLine("버블 정렬 결과 : ");
            foreach (int value in bubbleList)
            {
                Console.Write($"{value,3}");
            }
            Console.WriteLine();

            mergeList.MergeSort();
            Console.WriteLine("병합 정렬 결과 : ");
            foreach (int value in mergeList)
            {
                Console.Write($"{value,3}");
            }
            Console.WriteLine();

            quickList.QuickSort();
            Console.WriteLine("퀵 정렬 결과 : ");
            foreach (int value in quickList)
            {
                Console.Write($"{value,3}");
            }
            Console.WriteLine();

            heapList.QuickSort();
            Console.WriteLine("힙 정렬 결과 : ");
            foreach (int value in heapList)
            {
                Console.Write($"{value,3}");
            }
            Console.WriteLine();
        }
    }
}
