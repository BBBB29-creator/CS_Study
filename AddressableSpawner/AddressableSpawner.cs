using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string prefabAddress;
    [SerializeField] private Transform spawnPoint;

    // 캐싱 필드
    private GameObject spawnedInstance;
    private AsyncOperationHandle<GameObject>? spawnHandle; // 구조체이므로 Nullable(?) 처리

    /// <summary>
    /// 지정된 주소의 프리팹을 비동기로 생성합니다. (유니티 UI 버튼 연동 가능)
    /// </summary>
    public async void SpawnAsync()
    {
        // 1. 가드 절: 이미 유효한 spawnHandle이 있으면 경고 로그를 남기고 종료 (중복 로딩 방지)
        if (spawnHandle.HasValue)
        {
            Debug.LogWarning($"[AddressableSpawner] 이미 인스턴스가 생성되었거나 로딩 중입니다. (주소: {prefabAddress})");
            return;
        }

        // 2. 위치 설정: spawnPoint가 있으면 그 위치, 없으면 내 위치
        Vector3 position = spawnPoint != null ? spawnPoint.position : transform.position;
        Quaternion rotation = spawnPoint != null ? spawnPoint.rotation : transform.rotation;

        // 3. handle ← 주소로 InstantiateAsync 요청 (위치 포함) 및 spawnHandle ← handle
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(prefabAddress, position, rotation);
        spawnHandle = handle;

        // 안정성을 위한 try-catch 안전망 시작
        try
        {
            // 4. handle의 완료를 기다린다 (await 사용 조건 만족)
            await handle.Task;

            // 5. handle 상태가 성공이면 → spawnedInstance ← handle 결과
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                spawnedInstance = handle.Result;
                Debug.Log($"[AddressableSpawner] 에셋 생성 성공: {prefabAddress}");
            }
            // 6. 아니면 → 실패를 로그로 남긴다 (과제 요구사항: UI로도 알린다)
        }
        catch (Exception ex)
        {
            // 예기치 못한 치명적 오류(인터넷 끊김 등) 발생 시 게임이 멈추는 것을 방지
            Debug.LogError($"[AddressableSpawner] 예외 발생: {ex.Message}");
            NotifySpawnFailureToUI();
            ClearCache();
        }
    }

    /// <summary>
    /// 생성된 인스턴스를 Addressables 시스템을 통해 안전하게 반납합니다.
    /// </summary>
    public void ReleaseSpawned()
    {
        // spawnedInstance가 있으면 → Addressables로 인스턴스를 반납한다 (Destroy가 아니다)
        if (spawnedInstance != null)
        {
            Addressables.ReleaseInstance(spawnedInstance);
        }
        // 아직 로딩 중에 해제 요청이 들어왔다면 핸들 자체를 해제
        else if (spawnHandle.HasValue)
        {
            Addressables.Release(spawnHandle.Value);
        }

        // spawnedInstance, spawnHandle을 비운다
        ClearCache();
        Debug.Log($"[AddressableSpawner] 에셋 반납 완료: {prefabAddress}");
    }

    /// <summary>
    /// 캐시 필드들을 깨끗하게 비워주는 헬퍼 함수
    /// </summary>
    private void ClearCache()
    {
        spawnedInstance = null;
        spawnHandle = null;
    }

    /// <summary>
    /// UI 시스템에 실패 알림을 보내는 함수 (과제 요구사항 반영)
    /// </summary>
    private void NotifySpawnFailureToUI()
    {
        // TODO: 과제 요구사항에 맞는 UI 팝업창 매니저 코드가 있다면 여기에 연동하세요.
        Debug.LogWarning("[AddressableSpawner] UI 알림: 에셋을 불러오지 못했습니다. 네트워크나 주소를 확인해주세요.");
    }

    // 오브젝트가 파괴될 때 메모리 누수를 방지하기 위한 안전장치
    private void OnDestroy()
    {
        if (spawnHandle.HasValue)
        {
            ReleaseSpawned();
        }
    }
}
