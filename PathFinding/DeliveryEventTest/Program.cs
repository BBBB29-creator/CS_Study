using System;
using System.Collections.Generic;

namespace DeliveryEventTest
{
    public enum OrderStatus
    {
        Created,    // 주문 생성됨
        Paid,       // 결제 완료
        Shipped,    // 배송 시작
        Delivered,  // 배송 완료
        Canceled    // 주문 취소
    }

    // 이벤트 종류를 정의할 Enum
    public enum OrderEventType
    {
        StatusChanged, // '상태가 변경'되었을 때
        Delivered      // '배송이 완료'되었을 때
    }

    // 이벤트 전달에 사용할 데이터 클래스
    public struct OrderEventArgs  // GC 가동을 없애기 위해 클래스에서 스트럭트로 바꾸었다. 스택 메모리에 생기고 할 일을 마치고 알아서 사라진다.
    {
        public int OrderId { get; set; }
        public OrderStatus OldStatus { get; set; }
        public OrderStatus NewStatus { get; set; }

        public OrderEventArgs(int orderId, OrderStatus oldStatus, OrderStatus newStatus)
        {
            OrderId = orderId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }

    // 중앙 이벤트 버스 (싱글톤)
    public class DeliveryEventManager
    {
        public static DeliveryEventManager Instance { get; } = new DeliveryEventManager();
        private DeliveryEventManager() { }

        private Dictionary<OrderEventType, List<Delegate>> listeners = new Dictionary<OrderEventType, List<Delegate>>();

        // 이벤트 구독
        public void Subscribe<T>(OrderEventType eventType, Action<T> callback)
        {
            if (!listeners.ContainsKey(eventType))
            {
                listeners[eventType] = new List<Delegate>();
            }
            listeners[eventType].Add(callback);
        }

        // 이벤트 구독 해지
        public void Unsubscribe<T>(OrderEventType eventType, Action<T> callback)
        {
            if (listeners.ContainsKey(eventType))
            {
                listeners[eventType].Remove(callback);

                if (listeners[eventType].Count == 0)
                {
                    listeners.Remove(eventType);
                }
            }
        }

        public void Publish<T>(OrderEventType eventType, T data)
        {
            if (listeners.ContainsKey(eventType))
            {
                var callbacks = new List<Delegate>(listeners[eventType]);

                foreach (var listener in callbacks)
                {
                    if (listener is Action<T> action)
                    {
                        action.Invoke(data);
                    }
                }
            }
        }
    }

    // 순수한 주문 데이터 클래스 (이전에 있던 스스로 상태를 바꾸는 행동 메소드 제거됨)
    public class Order
    {
        public int OrderId { get; private set; }
        public OrderStatus Status { get; set; } // 배송 관리자가 변경할 수 있도록 set 개방

        public Order(int orderId)
        {
            OrderId = orderId;
            Status = OrderStatus.Created;
        }
    }

    // 모든 주문 처리를 통제하는 싱글톤 배송 관리자
    public class DeliveryManager
    {
        public static DeliveryManager Instance { get; } = new DeliveryManager();
        private DeliveryManager() { }

        // 상태 변경 로직이 Order 클래스에서 싱글톤 관리자 내부로 이동
        public void ChangeStatus(Order order, OrderStatus newStatus)
        {
            // 중복 배송 완료 테스트를 위한 예외 처리
            if (order.Status == OrderStatus.Delivered && newStatus == OrderStatus.Delivered)
            {
                Console.WriteLine($"[안내] 주문 {order.OrderId}는 이미 Delivered 상태입니다.\n");
                return;
            }

            OrderStatus oldStatus = order.Status;
            order.Status = newStatus;

            // 이벤트 데이터 생성
            var args = new OrderEventArgs(order.OrderId, oldStatus, newStatus);

            // [OnStatusChanged 이벤트 발생] 싱글톤이 직접 이벤트를 발행
            DeliveryEventManager.Instance.Publish(OrderEventType.StatusChanged, args);

            // [OnDelivered 이벤트 발생] 상태가 Delivered로 바뀐 경우에만 발행
            if (newStatus == OrderStatus.Delivered)
            {
                DeliveryEventManager.Instance.Publish(OrderEventType.Delivered, args);
            }
        }

        // 주문 시뮬레이션을 돌려주는 메소드 (내부의 ChangeStatus를 호출)
        public void ProcessOrder(Order order)
        {
            ChangeStatus(order, OrderStatus.Paid);
            ChangeStatus(order, OrderStatus.Shipped);
            ChangeStatus(order, OrderStatus.Delivered);
        }
    }

    // 로그 구독자
    public class LogSubscriber
    {
        public void Register()
        {
            DeliveryEventManager.Instance.Subscribe<OrderEventArgs>(OrderEventType.StatusChanged, OnStatusChanged);
            DeliveryEventManager.Instance.Subscribe<OrderEventArgs>(OrderEventType.Delivered, OnDelivered);
        }

        private void OnStatusChanged(OrderEventArgs orderData)
        {
            Console.WriteLine($"[Log] 주문 {orderData.OrderId}: {orderData.OldStatus} -> {orderData.NewStatus}");
        }

        private void OnDelivered(OrderEventArgs orderData)
        {
            Console.WriteLine($"[Log] 주문 {orderData.OrderId} 배송 완료 기록 저장");
        }
    }

    // 문자(SMS) 알림 구독자
    public class SmsSubscriber
    {
        public void Register()
        {
            DeliveryEventManager.Instance.Subscribe<OrderEventArgs>(OrderEventType.StatusChanged, OnStatusChanged);
        }

        private void OnStatusChanged(OrderEventArgs orderData)
        {
            if (orderData.NewStatus == OrderStatus.Shipped)
            {
                Console.WriteLine($"[문자] 주문 {orderData.OrderId} 상품이 발송되었습니다.");
            }
            else if (orderData.NewStatus == OrderStatus.Delivered)
            {
                Console.WriteLine($"[문자] 주문 {orderData.OrderId} 상품이 배송 완료되었습니다.");
            }
        }
    }

    // 업적 관리자 (카운트 유지용 싱글톤)
    public class AchievementManager
    {
        public static AchievementManager Instance { get; } = new AchievementManager();
        private int deliveredCount = 0;

        private AchievementManager() { }

        public void Register()
        {
            DeliveryEventManager.Instance.Subscribe<OrderEventArgs>(OrderEventType.Delivered, OnOrderDelivered);
        }

        private void OnOrderDelivered(OrderEventArgs orderData)
        {
            deliveredCount++;
            Console.WriteLine($"[업적 관리자] 현재 배송 완료 수: {deliveredCount}");

            if (deliveredCount == 3)
            {
                Console.WriteLine("[업적 달성] 첫 배송 3건 완료!");
            }
            Console.WriteLine();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // 구독자 객체 생성 및 이벤트 등록
            LogSubscriber logService = new LogSubscriber();
            SmsSubscriber smsService = new SmsSubscriber();

            logService.Register();
            smsService.Register();
            
            AchievementManager.Instance.Register();

            // 테스트용 주문 데이터 객체 생성 (new를 쓰지만 데이터 역할만 하므로 싱글톤 원칙에 위배되지 않...음? 아마?)
            Order order1 = new Order(1);
            Order order2 = new Order(2);
            Order order3 = new Order(3);
            Order order4 = new Order(4);

            Console.WriteLine("=== 1차 주문 처리 ===");
            DeliveryManager.Instance.ProcessOrder(order1);
            DeliveryManager.Instance.ProcessOrder(order2);

            Console.WriteLine("=== 2차 주문 처리 ===");
            DeliveryManager.Instance.ProcessOrder(order3);

            Console.WriteLine("=== 중복 배송 완료 테스트 ===");
            // 싱글톤 배송 관리자(Instance)를 거쳐서 처리하도록 변경
            DeliveryManager.Instance.ChangeStatus(order3, OrderStatus.Delivered);

            Console.WriteLine("=== 3차 주문 처리 ===");
            DeliveryManager.Instance.ProcessOrder(order4);
        }
    }
}
