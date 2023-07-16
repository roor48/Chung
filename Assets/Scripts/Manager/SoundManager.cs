using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixer masterMixer;
    
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        masterMixer.SetFloat("Master", PlayerPrefs.GetFloat("MasterVol"));
        masterSlider.value = PlayerPrefs.GetFloat("MasterVol");
        masterMixer.SetFloat("BGM", PlayerPrefs.GetFloat("BGMVol"));
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVol");
        masterMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFXVol"));
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVol");
    }
    
    public void SetMasterVol()
    {
        PlayerPrefs.SetFloat("MasterVol", masterSlider.value);
        masterMixer.SetFloat("Master", masterSlider.value);
    }

    public void SetBGMVol()
    {
        PlayerPrefs.SetFloat("BGMVol", bgmSlider.value);
        masterMixer.SetFloat("BGM", bgmSlider.value);
    }

    // ReSharper disable once InconsistentNaming
    public void SetSFXVol()
    {
        PlayerPrefs.SetFloat("SFXVol", sfxSlider.value);
        masterMixer.SetFloat("SFX", sfxSlider.value);
    }
}
