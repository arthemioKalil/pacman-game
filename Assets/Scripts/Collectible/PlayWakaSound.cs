using UnityEngine;

public class PlayWakaSound : MonoBehaviour
{
    public AudioClip WakaClip1;
    public AudioClip WakaClip2;

    private AudioSource _audioSouce;

    private static bool _switchClip;
    private void OnDestroy()
    {
        _audioSouce = FindObjectOfType<AudioSource>();
        if (_audioSouce != null)
        {
            _audioSouce.PlayOneShot(_switchClip ? WakaClip1 : WakaClip2);
            _switchClip = !_switchClip;
        }
    }
}
