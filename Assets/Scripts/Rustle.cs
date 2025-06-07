using UnityEngine;

public class Rustle : MonoBehaviour 
{
    [Header("Audio Settings")]
    public AudioClip[] rustleSounds;    // Multiple rustle sounds for variety
    public float volume = 0.6f;
    public float cooldownTime = 1f;     // Minimum time between rustles
    
    private AudioSource audioSource;
    private float lastRustleTime;
    
    void Start() 
    {
        // Set up AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) 
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        // Check if it's the player
        if (other.CompareTag("Player") && CanRustle()) 
        {
            PlayRustle();
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision) 
    {
        // Alternative for regular colliders (not triggers)
        if (collision.gameObject.CompareTag("Player") && CanRustle()) 
        {
            PlayRustle();
        }
    }
    
    bool CanRustle() 
    {
        return Time.time >= lastRustleTime + cooldownTime;
    }
    
    void PlayRustle() 
    {
        if (rustleSounds != null && rustleSounds.Length > 0) 
        {
            // Pick a random rustle sound
            AudioClip rustleSound = rustleSounds[Random.Range(0, rustleSounds.Length)];
            audioSource.PlayOneShot(rustleSound, volume);
            
            lastRustleTime = Time.time;
            
            Debug.Log("Tree rustled!");
        }
    }
}