using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip music;
    void Start()
    {
        if(PlayerPrefs.GetString("music","de")=="de")
        {
            PlayMusic();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic()
    {
        AudioSource musicSource = transform.Find("Music").GetComponent<AudioSource>();
        musicSource.clip=music;
        musicSource.Play();
    }

    public void PauseMusic()
    {
        AudioSource musicSource = transform.Find("Music").GetComponent<AudioSource>();
        musicSource.clip=music;
        musicSource.Pause();
    }

    public void PlaySFX(AudioClip clip,float speed)
    {
        AudioSource sfxSource = transform.Find("SFX").GetComponent<AudioSource>();
        sfxSource.pitch = speed;
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX2(AudioClip clip,float speed)
    {
        AudioSource sfxSource = transform.Find("SFX2").GetComponent<AudioSource>();
        sfxSource.pitch = speed;
        sfxSource.PlayOneShot(clip);
    }

}
