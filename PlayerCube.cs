using Unity.VisualScripting;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float Speed;

    private Renderer myRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        myRenderer = GetComponent<Renderer>();
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
}
