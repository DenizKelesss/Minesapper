using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeScript : MonoBehaviour
{
    [SerializeField]
    private AudioMixer masterMix;
    [SerializeField]
    private Slider MasterSlider;
    [SerializeField]
    private Slider SfxSlider;
    [SerializeField]
    private Slider MusSlider;
    [SerializeField]
    private Toggle muteToggle;

    private float prevVol;

    public void SetMasterVolume()
    {
        float vol = MasterSlider.value;
        masterMix.SetFloat("MasterVolume", Mathf.Log10(vol)*20);
        if(muteToggle.isOn)
            muteToggle.isOn = false;
    }

    public void SetMusicVolume()
    {
        float vol = MusSlider.value;
        masterMix.SetFloat("MusicVolume", Mathf.Log10(vol) * 20);
    }

    public void SetSfxVolume()
    {
        float vol = SfxSlider.value;
        masterMix.SetFloat("SfxVolume", Mathf.Log10(vol) * 20);
    }

    public void MuteVolume()
    {
        if(muteToggle.isOn)
        {
            prevVol = MasterSlider.value;
            masterMix.SetFloat("MasterVolume", -100000);
        }
        else
            masterMix.SetFloat("MasterVolume", Mathf.Log10(prevVol) * 20);
    }
}
