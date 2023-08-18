using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SenVolSettings : MonoBehaviour
{
    [SerializeField] public Slider sensSlider;
    [SerializeField] public AudioMixer ogMixer;
    [SerializeField] public Slider musicSlider;
    [SerializeField] public Slider soundSlider;

    private void Start()
    {
        MusicVolume();
        SoundVolume();
    }

    public void MusicVolume()
    {
        float volume = musicSlider.value;
        ogMixer.SetFloat("MusicPar", Mathf.Log10(volume) * 20);
    }

    public void SoundVolume()
    {
        float volume = soundSlider.value;
        ogMixer.SetFloat("SoundPar", Mathf.Log10(volume) * 20);
    }
}
