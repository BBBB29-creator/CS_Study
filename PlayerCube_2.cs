using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;  // 텍스트 ui 사용에 필수

public class PlayerBall : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float Speed;

    private Renderer myRenderer;

    private int Score = 0;

    private Vector3 startPosition; // 시작 위치 저장
    private int trapCount = 0;  // 함정 접촉 카운트

    [SerializeField] private TextMeshProUGUI statusText;  // 텍스트 ui를 연결할 변수

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        myRenderer = GetComponent<Renderer>();

        startPosition = transform.position;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (statusText != null)
        {
            // 요구하신 포맷 문자열 적용 (보정용 보간 문자열 '$' 사용)
            statusText.text = $"점수 : {Score} \n 함정 밟은 횟수 : {trapCount}/ 5";
        }
    }


    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveSpeed = new Vector3(x, 0, z).normalized;

        rb.MovePosition(rb.position +  moveSpeed * Speed * Time.deltaTime);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("충돌 확인");
            myRenderer.material.color = Color.red;
        }
        else if (collision.gameObject.CompareTag("Trap"))
        {
            trapCount++; // 함정 밟은 횟수 1 증가 (trapCount = trapCount + 1; 과 같음)
            Debug.Log($"함정 발동. 현재 밟은 횟수: {trapCount} / 5");
            UpdateUI();

            // 5번 누적되었는지 'if'문으로 체크
            if (trapCount >= 5)
            {
                Debug.Log("함정을 5번 밟았습니다! 초기 위치로 워프합니다.");

                // 플레이어 위치를 처음 시작 위치로 강제 이동
                transform.position = startPosition;

                // [중요] 물리 가속도가 남아있으면 워프 후에도 튕겨 나갈 수 있으므로 속도 초기화
                rb.linearVelocity = Vector3.zero;  // 그냥 벨로시티를 쓰면 구형 문법이라고 오류남. 리니어 붙여야 함.
                rb.angularVelocity = Vector3.zero;

                // 다음 판을 위해 함정 카운트를 다시 0으로 리셋
                trapCount = 0;
                UpdateUI();
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("접촉 중");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("접촉 해제");
            myRenderer.material.color = Color.blue;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Score++;
            Debug.Log("코인 획득");
            UpdateUI();

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Item_Heart"))
        {
            trapCount--;
            Debug.Log("체력 회복");
            UpdateUI();

            Destroy(other.gameObject);
        }
        
    }
}
