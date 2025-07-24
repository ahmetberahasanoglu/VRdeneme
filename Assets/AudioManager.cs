using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Clips")]
    public AudioClip warningSound;
    public AudioClip waterSound;
    public AudioClip glassCutSound;
    public AudioClip stickSound;
    private AudioSource audioSource;

    private void Awake()
    {
       
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }


    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip,volume);
        }
        else
        {
            Debug.LogWarning("AudioClip is missing!");
        }
    }
    public void PlayMusic(AudioClip musicClip)
    {
        audioSource.clip = musicClip;
        audioSource.loop = true;
        audioSource.Play();
    }


    // public void PlayPlayerJump() => PlaySound(playerJumpSound);
    // public void PlayGameOver() => PlaySound(gameOverSound);
    //  public void PlayCoinCollect() => PlaySound(coinCollectSound);
}