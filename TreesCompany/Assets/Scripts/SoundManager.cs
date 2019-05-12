using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.

    public AudioSource happyMusicSource;                 //Drag a reference to the audio source which will play the music.
    public AudioSource hellMusicSource;
    public AudioSource neutralMusicSource;


    public AudioSource ambientBirdsSource;
    public AudioSource ambientFactorySource;

    public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             
    public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.
    public static float FadeTime = 3.0f;

    private AudioSource CurrentMusicSource;


    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        CurrentMusicSource = neutralMusicSource;
    }

    public void PlayMusicSource(AudioSource audio)
    {
        if (CurrentMusicSource != null)
        {
            CurrentMusicSource.Stop();
        }

        CurrentMusicSource = audio;
        CurrentMusicSource.Play();
    }

    public AudioSource GetCurrentMusicSource()
    {
        return CurrentMusicSource;
    }

    //Used to play single sound clips.
    public void PlaySingle(AudioClip clip)
    {
        //Set the clip of our efxSource audio source to the clip passed in as a parameter.
        efxSource.clip = clip;

        //Play the clip.
        efxSource.Play();
    }

    public void PlayFactoryLoop()
    {
        ambientFactorySource.loop = true;
        ambientFactorySource.Play();
    }

    public void InterruptFactoryLoop()
    {
        ambientFactorySource.Stop();
    }


    //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
    public void RandomizeSfx(params AudioClip[] clips)
    {
        //Generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, clips.Length);

        //Choose a random pitch to play back our clip at between our high and low pitch ranges.
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        //Set the pitch of the audio source to the randomly chosen pitch.
        efxSource.pitch = randomPitch;

        //Set the clip to the clip at our randomly chosen index.
        efxSource.clip = clips[randomIndex];

        //Play the clip.
        efxSource.Play();
    }

    public void Transition(AudioSource nextAudio)
    {
        if (nextAudio.isPlaying)
            return;

        StartCoroutine(FadeOut(CurrentMusicSource));

        CurrentMusicSource = nextAudio;

        StartCoroutine(FadeIn(nextAudio));
    }

    private IEnumerator FadeOut(AudioSource audioSource)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    private IEnumerator FadeIn(AudioSource audioSource)
    {
        float finalVolume = 1;

        if(!audioSource.isPlaying)
        {
            audioSource.volume = 0;
            audioSource.Play();
        }

        while (audioSource.volume < 1)
        {
            audioSource.volume += finalVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
    }



}