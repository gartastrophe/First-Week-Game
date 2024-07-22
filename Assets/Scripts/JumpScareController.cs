using System.Collections;
using UnityEngine;

public class JumpScareController : MonoBehaviour
{
    public GameObject scarePrefab; 
    public AudioClip scareAudio; 
    public float scareDuration = 2.0f; 
    public float shakeIntensity = 0.5f;
    public float shakeDuration = 0.5f; 

    private AudioSource audioSource; 
    private Camera playerCamera; 
    private Vector3 originalCamPos;
    private Quaternion originalCamRot;

    private PlayerControls playerControls;
    private MouseLook mouseLook; 

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        playerCamera = GetComponentInChildren<Camera>();
        originalCamPos = playerCamera.transform.localPosition;
        originalCamRot = playerCamera.transform.localRotation;

        if (scarePrefab != null)
        {
            scarePrefab.SetActive(false);
        }

        playerControls = GetComponent<PlayerControls>();
        mouseLook = playerCamera.GetComponent<MouseLook>();
    }

    public void TriggerJumpScare()
    {
        StartCoroutine(JumpScareRoutine());
    }

    private IEnumerator JumpScareRoutine()
    {
        if (playerControls != null)
        {
            playerControls.enabled = false;
        }
        if (mouseLook != null)
        {
            mouseLook.enabled = false;
        }

        playerCamera.transform.localRotation = Quaternion.identity;

        if (scarePrefab != null)
        {
            scarePrefab.SetActive(true);
        }

        audioSource.clip = scareAudio;
        audioSource.Play();

        StartCoroutine(ShakeCamera());

        yield return new WaitForSeconds(scareDuration);

        if (scarePrefab != null)
        {
            scarePrefab.SetActive(false);
        }

        StopAllCoroutines();

        playerCamera.transform.localPosition = originalCamPos;
        playerCamera.transform.localRotation = originalCamRot;

        if (playerControls != null)
        {
            playerControls.enabled = true;
        }
        if (mouseLook != null)
        {
            mouseLook.enabled = true;
        }
    }

    private IEnumerator ShakeCamera()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / shakeDuration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;
            x *= shakeIntensity * damper;
            y *= shakeIntensity * damper;

            playerCamera.transform.localPosition = new Vector3(originalCamPos.x + x, originalCamPos.y + y, originalCamPos.z);

            yield return null;
        }

        playerCamera.transform.localPosition = originalCamPos;
    }
}
