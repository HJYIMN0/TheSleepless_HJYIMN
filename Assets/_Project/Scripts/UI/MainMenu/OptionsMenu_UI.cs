using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu_UI : MonoBehaviour
{
    [Header("Toggles")]
    [SerializeField] private Toggle masterToggle;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle sfxToggle;

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        // Loading SaveStates
        float master = PlayerPrefs.GetFloat("Volume_Master", 1f);
        float music = PlayerPrefs.GetFloat("Volume_Music", 1f);
        float sfx = PlayerPrefs.GetFloat("Volume_SFX", 1f);

        bool masterMute = PlayerPrefs.GetInt("Mute_Master", 1) == 1;
        bool musicMute = PlayerPrefs.GetInt("Mute_Music", 1) == 1;
        bool sfxMute = PlayerPrefs.GetInt("Mute_SFX", 1) == 1;

        masterSlider.value = master;
        musicSlider.value = music;
        sfxSlider.value = sfx;

        masterToggle.isOn = masterMute;
        musicToggle.isOn = musicMute;
        sfxToggle.isOn = sfxMute;

        ApplyVolumes();
        ApplyMuteStates();

        // Assegna listener
        masterSlider.onValueChanged.AddListener(_ => OnVolumeChanged());
        musicSlider.onValueChanged.AddListener(_ => OnVolumeChanged());
        sfxSlider.onValueChanged.AddListener(_ => OnVolumeChanged());

        masterToggle.onValueChanged.AddListener(_ => OnToggleChanged());
        musicToggle.onValueChanged.AddListener(_ => OnToggleChanged());
        sfxToggle.onValueChanged.AddListener(_ => OnToggleChanged());
    }

    public void OnVolumeChanged()
    {
        PlayerPrefs.SetFloat("Volume_Master", masterSlider.value);
        PlayerPrefs.SetFloat("Volume_Music", musicSlider.value);
        PlayerPrefs.SetFloat("Volume_SFX", sfxSlider.value);
        PlayerPrefs.Save();

        ApplyVolumes();
    }

    public void OnToggleChanged()
    {
        bool masterOn = masterToggle.isOn;

        // Se master è spento, forza gli altri due a spenti
        if (!masterOn)
        {
            musicToggle.isOn = false;
            sfxToggle.isOn = false;
        }

        PlayerPrefs.SetInt("Mute_Master", masterOn ? 1 : 0);
        PlayerPrefs.SetInt("Mute_Music", musicToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("Mute_SFX", sfxToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();

        ApplyMuteStates();
    }

    public void ApplyVolumes()
    {
        AudioManager.Instance.SetVolumes(
            masterSlider.value,
            musicSlider.value,
            sfxSlider.value
        );
    }

    public void ApplyMuteStates()
    {
        bool masterOn = masterToggle.isOn;
        bool musicOn = musicToggle.isOn;
        bool sfxOn = sfxToggle.isOn;

        AudioManager.Instance.SetAudioSourceStates(masterOn, musicOn, sfxOn);
    }
}