using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : GenericSingleton<AudioManager>
{
    [Header("AudioSource")]
    public AudioSource musicAudioSource;
    public AudioSource soundEffectsSource;

    [Header("Music")]
    public AudioClip mainMenuClip;
    public AudioClip mainGameClip;

    [Header("Sound effects")]
    public AudioClip buttonClip;
    public AudioClip sleepClip;
    public AudioClip workClip;
    public AudioClip repairClip;
    public AudioClip washClip;

    [Header("Scenes Names")]
    [SerializeField] private string MainMenuName = "MainMenu";
    [SerializeField] private string MainGameName = "MainGame";

    public override bool IsDestroyedOnLoad() => false;
    public override bool ShouldDetatchFromParent() => true;

    public override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);

        GameObject audioRoot = new GameObject("AudioSources");
        DontDestroyOnLoad(audioRoot);

        musicAudioSource = Create2DAudioSource(audioRoot, "MusicSource");
        soundEffectsSource = Create2DAudioSource(audioRoot, "SFXSource");

        LoadVolumeSettings();
        LoadMuteSettings();

        SceneManager.sceneLoaded += OnSceneLoaded;

        PlayMusic(mainMenuClip);
    }

    private AudioSource Create2DAudioSource(GameObject parent, string name)
    {
        var sourceGO = new GameObject(name);
        sourceGO.transform.SetParent(parent.transform);
        var source = sourceGO.AddComponent<AudioSource>();
        source.spatialBlend = 0f;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.playOnAwake = false;
        return source;
    }

    private void LoadVolumeSettings()
    {
        float master = PlayerPrefs.GetFloat("Volume_Master", 1f);
        float music = PlayerPrefs.GetFloat("Volume_Music", 1f);
        float sfx = PlayerPrefs.GetFloat("Volume_SFX", 1f);
        SetVolumes(master, music, sfx);
    }

    private void LoadMuteSettings()
    {
        bool masterOn = PlayerPrefs.GetInt("Mute_Master", 1) == 1;
        bool musicOn = PlayerPrefs.GetInt("Mute_Music", 1) == 1;
        bool sfxOn = PlayerPrefs.GetInt("Mute_SFX", 1) == 1;
        SetAudioSourceStates(masterOn, musicOn, sfxOn);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals(MainGameName))
        {
            PlayMusic(mainGameClip);
        }
        else if (scene.name.Equals(MainMenuName))
        {
            PlayMusic(mainMenuClip);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicAudioSource.clip == clip) return;

        musicAudioSource.clip = clip;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        if (clip != null)
        {
            soundEffectsSource.PlayOneShot(clip);
        }
    }

    public void SetVolumes(float master, float music, float sfx)
    {
        musicAudioSource.volume = music * master;
        soundEffectsSource.volume = sfx * master;
    }

    public void SetAudioSourceStates(bool masterOn, bool musicOn, bool sfxOn)
    {
        musicAudioSource.mute = !(masterOn && musicOn);
        soundEffectsSource.mute = !(masterOn && sfxOn);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}