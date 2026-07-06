namespace PathFinding
{
    public struct DamageInfo { public int damage; public string attacker; }
    public struct ItemInfo { public string itemName; public int count; }

    internal class Program
    {

        public enum GameEvent
        {
            PlayerDamaged,
            PlayerHealed,
            PlayerDied,
            EnemyKilld,
            itempickup
        }

        public class EventManager
        {
            public static EventManager Instance { get; } = new EventManager();

            // 게임 이벤트에 따른 메소드들을 델리게이트의 리스트에 저장하여 사용
            private Dictionary<GameEvent, List<Delegate>> listeners = new Dictionary<GameEvent, List<Delegate>>();

            // 이벤트를 구독하는 제네릭 메소드
            public void subscribe<T>(GameEvent gameevent, Action<T> callback)
            {
                // 1. 딕셔너리에 해당 키가 없다면 새로운 리스트를 만들어 추가.
                if (!listeners.ContainsKey(gameevent))
                {
                    listeners[gameevent] = new List<Delegate>();
                }

                // 2. 전달받은 콜백 메소드를 리스트에 저장.
                listeners[gameevent].Add(callback);
            }

            // 이벤트를 구독 해제하는 제네릭 메소드
            public void unsubscribe<T>(GameEvent gameevent, Action<T> callback)
            {
                // 1. 해당 게임 이벤트 키가 존재하는지 검사.
                if (listeners.ContainsKey(gameevent))
                {
                    // 2. 리스트에서 일치하는 콜백 메소드를 제외.
                    listeners[gameevent].Remove(callback);

                    // 3. 메모리 관리 최적화를 위해 리스트가 완전히 비었다면 딕셔너리에서 키를 삭제.
                    if (listeners[gameevent].Count == 0)
                    {
                        listeners.Remove(gameevent);
                    }
                }
            }

            // 구독한 이벤트들을 발행하는 제네릭 메소드
            public void Publish<T>(GameEvent gameEvent, T date)
            {
                /// if (!listeners.ContainsKey(gameevent)) return;
                /// foreach(Delegate callback in Listeners[gameEvent].ToList())
                /// {
                ///     (Action<T>callback).Invoke(data);
                /// }
                // 1. 해당 이벤트에 등록된 리스너(구독자)가 있는지 확인.
                if (listeners.ContainsKey(gameEvent))
                {
                    // 2. 이벤트 실행 도중(Invoke) 구독 해제가 일어나 리스트가 바뀌는 에러를 막기 위해 리스트를 복사.
                    var callbacks = new List<Delegate>(listeners[gameEvent]);

                    foreach (var listener in callbacks)
                    {
                        // 3. 부모 타입인 Delegate를 실제 사용 가능한 Action<T> 형태로 안전하게 형변환하여 호출(Invoke).
                        if (listener is Action<T> action)
                        {
                            action.Invoke(date);
                        }
                    }
                }
            }

            static void Main(string[] args)
            {
                ///Action<int> playerDamaged = (damage) => Console.WriteLine("
                EventManager.Instance.subscribe<DamageInfo>(GameEvent.PlayerDamaged, OnPlayerDamaged);

                // 방법 B: 람다식(익명 함수)을 활용하여 즉석에서 구독
                EventManager.Instance.subscribe<ItemInfo>(GameEvent.itempickup, (item) => 
                {
                    Console.WriteLine($"[람다 구독] {item.itemName}을(를) {item.count}개 주웠습니다!");
                });


                // ==========================================
                // 2. 이벤트 발행 (Publish) 문법
                // ==========================================
                Console.WriteLine("--- 게임 이벤트 발생 시뮬레이션 ---");

                // 데미지 데이터 생성 후 발행
                DamageInfo dmg = new DamageInfo { damage = 25, attacker = "슬라임" };
                EventManager.Instance.Publish<DamageInfo>(GameEvent.PlayerDamaged, dmg);

                // 아이템 데이터 생성 후 발행
                ItemInfo item = new ItemInfo { itemName = "물약", count = 5 };
                EventManager.Instance.Publish<ItemInfo>(GameEvent.itempickup, item);


                // ==========================================
                // 3. 이벤트 구독 해제 (Unsubscribe) 문법
                // ==========================================
                // 더 이상 데미지 이벤트를 받지 않도록 해제
                EventManager.Instance.unsubscribe<DamageInfo>(GameEvent.PlayerDamaged, OnPlayerDamaged);

                Console.WriteLine("\n--- 구독 해제 후 다시 발행 ---");
                // 해제했기 때문에 OnPlayerDamaged 메서드는 호출되지 않습니다.
                EventManager.Instance.Publish<DamageInfo>(GameEvent.PlayerDamaged, dmg);
            }

            private static void OnPlayerDamaged(DamageInfo info)
            {
                Console.WriteLine($"[메서드 구독] 플레이어가 {info.attacker}에게 {info.damage}의 피해를 입었습니다!");
            }
        }
    }
}
