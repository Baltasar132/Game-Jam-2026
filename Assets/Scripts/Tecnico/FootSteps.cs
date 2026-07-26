using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip footstepSound;

    public void PlayFootstep()
    {
        audioSource.pitch = Random.Range(.9f, 1.1f);
        audioSource.PlayOneShot(footstepSound);
    }
}
