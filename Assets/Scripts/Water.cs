using UnityEngine;

public class Water : MonoBehaviour 
{
    [Header("Audio Settings")]
    public AudioClip waterSound;
    public float maxVolume = 0.8f;
    public float minDistance = 5f;    // Distance for full volume
    public float maxDistance = 50f;   // Distance where sound becomes silent
    
    [Header("Player Reference")]
    public Transform player;
    
    private AudioSource audioSource;
    
    void Start() 
    {
        // Set up the AudioSource component
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null) 
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configure for looping water sound
        audioSource.clip = waterSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0f; // Start silent
        
        // Find player automatically if not set
        if (player == null) 
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) 
            {
                player = playerObj.transform;
            }
        }
        
        // Start playing the looped sound
        if (waterSound != null) 
        {
            audioSource.Play();
        }
    }
    
    void Update() 
    {
        if (player != null && audioSource.isPlaying) 
        {
            UpdateVolume();
        }
    }
    
    void UpdateVolume() 
    {
        Vector2 waterPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerPos = new Vector2(player.position.x, player.position.y);
        float distance = Vector2.Distance(waterPos, playerPos);
        
        float targetVolume = CalculateVolume(distance);
        
        // Smooth volume transitions to avoid popping
        audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, Time.deltaTime * 2f);
    }
    
    float CalculateVolume(float distance) 
    {
        if (distance >= maxDistance) 
        {
            return 0f; // Silent when too far
        }
        
        if (distance <= minDistance) 
        {
            return maxVolume; // Full volume when very close
        }
        
        // Linear falloff between min and max distance
        float falloffRange = maxDistance - minDistance;
        float distanceInRange = distance - minDistance;
        float volumePercent = 1f - (distanceInRange / falloffRange);
        
        return maxVolume * volumePercent;
    }
}