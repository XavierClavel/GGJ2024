using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static float LowPitchRange = 0.9f;
    static float HighPitchRange = 1.1f;
    List<clip> audioIds;

    private static float sfxVolume = 1f;
    public static bool playingBossMusic = false;



    [Header("Audio Sources")]
    [SerializeField] private AudioSource mainMusic;
    [SerializeField] private AudioSource bossMusic;


    [Header(" ")]
    //public sfxContainer[] test;
    private static AudioSource currentMusicSource;


    public static AudioManager instance = null;

    struct clip
    {
        public string type;
        public GameObject audio;

        public clip(string type, GameObject audio)
        {    //Constructor
            this.type = type;
            this.audio = audio;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
        
        audioIds = new List<clip>();
        
    }

    private void Start()
    {
        LowPitchRange = 1f;
        HighPitchRange = 1f;
    }


    public void StopTime()
    {
        mainMusic.Pause();
        if (audioIds.Count > 0)
        {
            foreach (clip audioId in audioIds)
            {
                if (audioId.audio != null)
                {
                    audioId.audio.GetComponent<AudioSource>().Pause();
                }
            }
        }
    }

    public void ResumeTime()
    {
        //MusicSource.Play();
        if (audioIds.Count > 0)
        {
            foreach (clip audioId in audioIds)
            {
                if (audioId.audio != null)
                {
                    audioId.audio.GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    public static void PlaySfx(Transform pos, string key)
    {
        PlaySfx(pos.position, key);
    }

    public static void PlaySfx(string key)
    {
        PlaySfx(Vector3.zero, key);
    }

    public static void PlaySfx(Vector3 pos, string key)
    {
        float randomPitch = UnityEngine.Random.Range(LowPitchRange, HighPitchRange);
        if (!DataManager.dictKeyToSfx.ContainsKey(key))
        {
            Debug.LogWarning($"Sfx key '{key}' not found");
            return;
        }
        Sfx sfx = DataManager.dictKeyToSfx[key];

        instance.PlayClipAt(sfx.getClip(), pos, randomPitch, key, sfxVolume * sfx.getVolume());

    }

    void PlayClipAt(AudioClip clip, Vector3 pos, float pitch, string type, float volume)       //create temporary audio sources for each sfx in order to be able to modify pitch
    {
        GameObject audioContainer = new GameObject("TempAudio"); // create the temporary object
        audioContainer.transform.position = pos; // set its position to localize sound
        AudioSource aSource = audioContainer.AddComponent<AudioSource>();
        aSource.pitch = pitch;
        aSource.clip = clip;
        aSource.volume = volume;
        clip audioId = new clip(type, audioContainer);
        audioIds.Add(audioId);
        aSource.Play(); // start the sound
        StartCoroutine(SfxTimer(clip.length, audioId));
    }

    IEnumerator SfxTimer(float time, clip audioId)  //2nd argument initially aSource
    {
        yield return new WaitForSeconds(time);
        audioIds.Remove(audioId);
        Destroy(audioId.audio);
    }
    
    public static void playBossMusic()
    {
        playingBossMusic = true;
        instance.mainMusic.DOFade(0f, 1f).SetEase(Ease.Linear).OnComplete(instance.mainMusic.Stop);
        instance.bossMusic.Play();
        instance.bossMusic.DOFade(1f, 1f).SetEase(Ease.Linear);
    }
    
    public static void playMainMusic()
    {
        playingBossMusic = false;
        instance.bossMusic.DOFade(0f, 1f).SetEase(Ease.Linear).OnComplete(instance.bossMusic.Stop);
        instance.mainMusic.Play();
        instance.mainMusic.DOFade(1f, 1f).SetEase(Ease.Linear);
    }

}

