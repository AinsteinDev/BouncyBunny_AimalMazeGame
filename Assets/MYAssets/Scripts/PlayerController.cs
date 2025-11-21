using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float hopDistance = 1f;         // How far each hop is (1 unit)
    public float hopSpeed = 6f;            // How fast the hopping tween moves
    public float squashAmount = 0.15f;    // how much squash/stretch
    public LayerMask wallMask;             // Walls to block movement

    private Vector2 swipeStart;
    private Vector2 currentDir;
    private bool isMoving = false;
    private bool fingerDown = false;


    [Header("SfxVfx")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private ParticleSystem hopPS;
    [SerializeField] private AudioClip hopSfx;
    [SerializeField] private AudioClip hopLandSfx;
 
    private void Start()
    {
       
    }

    void Update()
    {
        HandleTouch();
    }

    void HandleTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            // 🛑 If touch started on UI, ignore movement
            if (IsTouchOverUI(t))
                return;

            if (t.phase == TouchPhase.Began)
            {
                fingerDown = true;
                swipeStart = t.position;
            }

            if (t.phase == TouchPhase.Moved && fingerDown)
            {
                Vector2 delta = t.position - swipeStart;

                // If swipe is strong enough, determine direction
                if (delta.magnitude > 20f)
                {
                    Vector2 normalized = delta.normalized;

                    // Allow full analog movement
                    currentDir = normalized;

                    if (!isMoving)
                        StartCoroutine(HopRoutine());
                }
            }

            if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            {
                fingerDown = false;
                currentDir = Vector2.zero;
            }
        }
        else
        {
            // No touch → no movement
            fingerDown = false;
            currentDir = Vector2.zero;
        }
    }

    System.Collections.IEnumerator HopRoutine()
    {
        isMoving = true;

        while (fingerDown && currentDir != Vector2.zero)
        {
            // Calculate next hop target
            Vector3 nextPos = transform.position + new Vector3(currentDir.x, 0, currentDir.y) * hopDistance;

            // Wall blocking?
            if (Physics.CheckSphere(nextPos, 0.3f, wallMask))
                break;

            // Face hop direction
            Vector3 forward = new Vector3(currentDir.x, 0, currentDir.y);
            transform.rotation = Quaternion.LookRotation(forward, Vector3.up);

            // Hop arc variables
            Vector3 startPos = transform.position;
            float t = 0f;
            float hopHeight = 0.5f;        // how high the bunny jumps

            PlayHopSfx(hopSfx);// JumpSfx

            while (t < 1f)
            {
                t += Time.deltaTime * hopSpeed;

                // ARC (parabola)
                float arc = Mathf.Sin(t * Mathf.PI) * hopHeight;
                Vector3 flat = Vector3.Lerp(startPos, nextPos, t);
                transform.position = flat + Vector3.up * arc;

                // ---- SQUASH & STRETCH ----
                // Stretch at mid jump, squash at start/end
                float stretch = 1f + Mathf.Sin(t * Mathf.PI) * squashAmount;
                float squash = 1f - Mathf.Sin(t * Mathf.PI) * (squashAmount * 0.5f);
                transform.localScale = new Vector3(squash, stretch, squash);

                yield return null;
            }

           // PlayHopSfx(hopLandSfx);// LandSfx
            hopPS.Play();//LandPS

            // Snap to correct final position
            transform.position = nextPos;
            transform.localScale = Vector3.one;

            yield return null;
        }

        // Reset
        transform.localScale = Vector3.one;
        isMoving = false;
    }
    private  bool IsTouchOverUI(Touch t)
    {
        // mobile
        if (EventSystem.current.IsPointerOverGameObject(t.fingerId))
            return true;

        // editor (mouse)
        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        return false;
    }
    private void PlayHopSfx(AudioClip clip)
    {
        audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(clip);
       
    }
}
