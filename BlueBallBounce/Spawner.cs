using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("생성할 공")]
    public GameObject blueBallPrefab;

    [Header("날아가는 공의 속도/힘")]
    public float launchForce = 20f;

    [Header("회전력 세기 범위")]
    public float minTorque = 5f;
    public float maxTorque = 20f;

    void Update()
    {
        // 마우스 좌클릭 시 발사
        if (Input.GetMouseButtonDown(0))
        {
            LaunchBall();
        }
    }

    void LaunchBall()
    {
        if (blueBallPrefab == null) return;

        // 마우스 클릭 방향 계산
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        Vector3 launchDirection = ray.direction;
        Vector3 spawnPosition = Camera.main.transform.position + launchDirection * 1.0f;

        // 공 생성
        GameObject ball = Instantiate(blueBallPrefab, spawnPosition, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // [조건 1] Mass를 랜덤 부여하여 날아가는 정도를 제각각으로 만들기
            // (주의: AddForce 전에 Mass를 바꿔야 무게에 따른 추진력 차이가 정상 반영됩니다.)
            rb.mass = Random.Range(0.5f, 3.5f);

            // 직선 힘 부여 (Impulse)
            rb.AddForce(launchDirection * launchForce, ForceMode.Impulse);

            // [조건 2] 발사와 동시에 무작위 회전 축으로 회전력(Torque) 부여
            Vector3 randomTorque = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ).normalized * Random.Range(minTorque, maxTorque);

            rb.AddTorque(randomTorque, ForceMode.Impulse);
        }
    }
}
