using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BlueBall : MonoBehaviour
{
    private AudioSource audioSource;
    private Rigidbody rb;
    private bool isSucceeded = false; // 성공 구역 안착 여부 (더 이상 반응 안 하도록 차단하는 플래그)
    private int currentBounceCount = 0; // 현재 충돌 횟수 누적 변수

    [Header("사운드 설정")]
    public AudioClip bounceSound;
    public AudioClip successSound; // [추가] 성공 구역 안착 시 소리
    public float minCollisionSpeed = 0.5f;
    public float maxCollisionSpeed = 15f;

    [Header("태그별 충돌 파티클 (이펙트)")]
    public ParticleSystem groundParticle; // Ground 태그 충돌용
    public ParticleSystem wallParticle;   // Wall 태그 충돌용
    public ParticleSystem ballParticle;   // Ball 태그 충돌용
    public ParticleSystem successParticle; // [추가] 성공 구역 안착용 이펙트

    [Header("공의 수명 제한 설정")]
    public int maxBounceCount = 5;      // 최대 충돌 허용 횟수
    public float maxLifeTime = 10.0f;    // 최대 생존 시간 (초)

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        audioSource.playOnAwake = false;

        // [조건 3] 일정 시간(maxLifeTime)이 지나면 스스로 사라지게 예약
        Destroy(gameObject, maxLifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 이미 목표 구역에 안착해 성공했다면 물리/이펙트 반응을 완전히 무시합니다.
        if (isSucceeded) return;

        // 1. 속도 비례 사운드 재생 (기존 로직 유지)
        float hitSpeed = collision.relativeVelocity.magnitude;
        if (hitSpeed > minCollisionSpeed)
        {
            float calculatedVolume = Mathf.InverseLerp(minCollisionSpeed, maxCollisionSpeed, hitSpeed);
            audioSource.PlayOneShot(bounceSound, calculatedVolume);
        }

        // 2. [조건 1] 부딪히는 대상의 태그에 따라 서로 다른 색의 파티클 재생
        // collision.contacts[0].point는 부딪힌 정확한 차원 좌표(위치)입니다.
        Vector3 hitPoint = collision.contacts[0].point;

        if (collision.gameObject.CompareTag("Ground"))
        {
            PlayParticle(groundParticle, hitPoint);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            PlayParticle(wallParticle, hitPoint);
        }
        else if (collision.gameObject.CompareTag("Ball"))
        {
            PlayParticle(ballParticle, hitPoint);
        }

        // 3. [조건 3] 부딪힐 때마다 횟수 누적 ➡️ 일정 횟수 이상 튀면 소멸
        currentBounceCount++;
        if (currentBounceCount >= maxBounceCount)
        {
            // 이펙트를 살리기 위해 공만 즉시 파괴
            Destroy(gameObject);
        }
    }

    // [조건 2] 정해진 목표 구역에 안착했을 때 처리 (Trigger 구역 활용 권장)
    // 목표 구역 오브젝트의 Collider에 'Is Trigger'가 체크되어 있어야 이 함수가 실행됩니다.
    private void OnTriggerEnter(Collider other)
    {
        // 이미 성공했거나, 목표 구역(태그: Goal)이 아니라면 무시
        if (isSucceeded || !other.CompareTag("Goal")) return;

        // 성공 판정 켜기 (더 이상 충돌 연출이나 반응을 하지 않음)
        isSucceeded = true;

        // 물리 움직임 완전히 정지 (자리에 안착 고정)
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true; // 더 이상 물리 엔진의 영향을 받지 않게 고정
        }

        // 성공 사운드 및 이펙트 재생
        if (successSound != null)
        {
            audioSource.PlayOneShot(successSound, 1.0f);
        }
        PlayParticle(successParticle, transform.position);
    }

    // 파티클 프리팹을 생성하고 자동 파괴해주는 헬퍼 함수
    void PlayParticle(ParticleSystem particlePrefab, Vector3 position)
    {
        if (particlePrefab != null)
        {
            // 부딪힌 위치에 파티클 생성
            ParticleSystem instance = Instantiate(particlePrefab, position, Quaternion.identity);
            instance.Play();
            // 파티클 재생이 끝나면 메모리에서 자동 삭제 (기본 수명 2초 뒤 삭제)
            Destroy(instance.gameObject, 2.0f);
        }
    }
}
