using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] private AudioClip shockClip;
    [SerializeField] private AudioClip dashClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayShock()
    {
        audioSource.clip = shockClip;
        audioSource.Play();
    }
    
    public void PlayDash()
    {
        audioSource.clip = dashClip;
        audioSource.Play();
    }

}
