using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    
    //이 부분은 강의에서 옛날 나온 것과 거의 그대로 - 시간 부족
    public static SoundManager Instance;

    [SerializeField][Range(0f, 1f)] private float soundEffectVolume;
    [SerializeField][Range(0f, 1f)] private float musicVolume;

    private SoundPool soundPool;
    private AudioSource musicAudioSource;
    public AudioClip clip;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }


        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.volume = musicVolume;
        musicAudioSource.loop = true;

        soundPool = GetComponent<SoundPool>();
        soundPool.CreatePool(transform);
    }


    public void ChangeBackGroundMusic(AudioClip clip, float soundVolume)
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = clip;
        musicAudioSource.volume = soundVolume;
        musicAudioSource.Play();

    }
    public void PlayClip(AudioClip clip)
    {
        GameObject obj = soundPool.SpawnFromPool("SoundSource");
        obj.SetActive(true);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, soundEffectVolume);
    }

}