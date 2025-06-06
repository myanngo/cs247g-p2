using UnityEngine;

public class OwlHoot : MonoBehaviour 
{
    public AudioClip HootSound;
    public float minInterval = 3f;
    public float maxInterval = 8f;
    public float maxVolume = 1f;
    public float minDistance = 10f;   // Distance for full volume
    public float maxDistance = 100f;  // Distance where sound becomes silent
    public Transform player;
    
    private AudioSource audioSource;
    private float nextHootTime;
    
    void Start() 
    {
        audioSource = GetComponent<AudioSource>();
        
        if (player == null) 
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) 
            {
                player = playerObj.transform;
            }
        }
        
        ScheduleNextHoot();
    }
    
    void Update() 
    {
        if (Time.time >= nextHootTime) 
        {
            PlayHoot();
            ScheduleNextHoot();
        }
    }
    
    void PlayHoot() 
    {
        if (HootSound != null && player != null) 
        {
            Vector2 owlPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 playerPos = new Vector2(player.position.x, player.position.y);
            float distance = Vector2.Distance(owlPos, playerPos);
            
            float volume = CalculateVolume(distance);
            
            Debug.Log($"Distance: {distance:F1}, Volume: {volume:F2}");
            
            if (volume > 0.01f) 
            {
                audioSource.PlayOneShot(HootSound, volume);
            }
        }
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
    
    void ScheduleNextHoot() 
    {
        float randomInterval = Random.Range(minInterval, maxInterval);
        nextHootTime = Time.time + randomInterval;
    }
}
