using UnityEngine;

public class SoundController: MonoBehaviour
{ 
    public void PlaySound(string soundName)
    {
        AudioManager.Instance.Play(soundName);
    }
}
