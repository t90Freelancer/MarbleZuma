
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{

    public AudioClip BackgroundMusic;

    public AudioClip WinMusic;

    public AudioClip LooseMusic;

    private AudioSource _source;

    void Awake()
    {
        _source = GetComponent<AudioSource>();
        _source.clip = BackgroundMusic;
        _source.loop = true;
        _source.Play();
    }

    public void Win()
    {
        _source.Stop();
        _source.clip = WinMusic;
        _source.loop = false;
        _source.Play();
    }
    
    public void Loose()
    {
        _source.Stop();
        _source.clip = LooseMusic;
        _source.loop = false;
        _source.Play();
    }
}
