using UnityEngine;

public class PuzzleCompletionSound : MonoBehaviour 
{
    [Header("Completion Sound")]
    public AudioClip completionSound;
    public float volume = 0.8f;
    
    private AudioSource audioSource;
    private bool hasPlayedCompletionSound = false;
    
    void Start() 
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) 
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        audioSource.playOnAwake = false;
    }
    
    // Call this method when the puzzle is completed
    public void PlayCompletionSound() 
    {
        if (completionSound != null && !hasPlayedCompletionSound) 
        {
            audioSource.PlayOneShot(completionSound, volume);
            hasPlayedCompletionSound = true;
            
            Debug.Log("Puzzle completed! Playing sound.");
        }
    }
    
    // Call this if you want to reset the puzzle and allow the sound to play again
    public void ResetPuzzle() 
    {
        hasPlayedCompletionSound = false;
    }
}
