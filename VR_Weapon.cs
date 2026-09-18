using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VR_Weapon : MonoBehaviour
{
    public enum FireMode { Single, Auto }

    [Header("총기 설정")]
    public FireMode fireMode = FireMode.Auto;
    public float fireRate = 0.1f;
    public float damage = 25f;
    public float range = 100f;

    [Header("진동(Haptic) 설정")]
    [Range(0, 1)] public float hapticIntensity = 0.7f; // 진동 세기 (0:무진동 ~ 1:최대)
    public float hapticDuration = 0.08f;               // 진동 시간

    [Header("컴포넌트 연결")]
    public Transform muzzlePoint;
    // -------------------------------------------------------------
    // [변경점 1] 인스펙터 창에서 파티클 시스템을 연결할 수 있는 변수를 추가합니다.
    public ParticleSystem muzzleSmoke;
    // -------------------------------------------------------------

    private bool isPullingTrigger = false;
    private float nextFireTime = 0f;
    private Coroutine autoFireCoroutine;

    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    public void StartShooting()
    {
        if (fireMode == FireMode.Single)
        {
            ShootRaycast();
        }
        else if (fireMode == FireMode.Auto)
        {
            isPullingTrigger = true;
            if (autoFireCoroutine == null)
            {
                autoFireCoroutine = StartCoroutine(AutoFireRoutine());
            }
        }
    }

    public void StopShooting()
    {
        isPullingTrigger = false;
        if (autoFireCoroutine != null)
        {
            StopCoroutine(autoFireCoroutine);
            autoFireCoroutine = null;
        }
    }

    private IEnumerator AutoFireRoutine()
    {
        while (isPullingTrigger)
        {
            if (Time.time >= nextFireTime)
            {
                ShootRaycast();
                nextFireTime = Time.time + fireRate;
            }
            yield return null;
        }
    }

    private void ShootRaycast()
    {
        if (muzzlePoint == null) return;

        // -------------------------------------------------------------
        // [변경점 2] 총을 쏠 때 파티클 시스템이 비어있지 않다면 연기를 뿜어냅니다!
        if (muzzleSmoke != null)
        {
            muzzleSmoke.Play();
        }
        // -------------------------------------------------------------

        // 1. 레이캐스트 발사
        RaycastHit hit;
        if (Physics.Raycast(muzzlePoint.position, muzzlePoint.forward, out hit, range))
        {
            Debug.Log($"적중 대상: {hit.collider.name}");
        }

        // 2. 진동(Haptic) 발생 로직
        TriggerHapticFeedback();
    }

    private void TriggerHapticFeedback()
    {
        if (grabInteractable == null) return;

        foreach (var interactor in grabInteractable.interactorsSelecting)
        {
            string interactorName = interactor.transform.name.ToLower();
            UnityEngine.InputSystem.InputDevice device = null;

            if (interactorName.Contains("left"))
            {
                device = UnityEngine.InputSystem.XR.XRController.leftHand;
            }
            else
            {
                device = UnityEngine.InputSystem.XR.XRController.rightHand;
            }

            if (device != null)
            {
                var command = UnityEngine.InputSystem.XR.Haptics.SendHapticImpulseCommand.Create(0, hapticIntensity, hapticDuration);
                device.ExecuteCommand(ref command);
            }
        }
    }
}
