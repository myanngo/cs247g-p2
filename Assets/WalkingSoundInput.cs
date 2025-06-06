using UnityEngine;

public class WalkingSoundInput : MonoBehaviour 
{
    [Header("Audio Settings")]
    public AudioClip[] footstepSounds;
    public float volume = 0.7f;
    public float stepInterval = 0.5f;
    
    private AudioSource audioSource;
    private float lastStepTime;
    
    void Start() 
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) 
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }
    
    void Update() 
    {
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || 
                       Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
                       Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) ||
                       Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow);
        
        if (isMoving && Time.time >= lastStepTime + stepInterval) 
        {
            PlayFootstep();
            lastStepTime = Time.time;
        }
    }
    
    void PlayFootstep() 
    {
        if (footstepSounds != null && footstepSounds.Length > 0) 
        {
            AudioClip stepSound = footstepSounds[Random.Range(0, footstepSounds.Length)];
            audioSource.PlayOneShot(stepSound, volume);
        }
    }
}