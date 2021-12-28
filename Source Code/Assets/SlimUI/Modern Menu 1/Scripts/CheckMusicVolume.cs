using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SlimUI.ModernMenu{


    public class CheckMusicVolume : MonoBehaviour
    {

        public Slider slider;
        public GameObject MusicManager;
        private AudioSource audioInstance;

        private void Awake()
        {
            audioInstance = MusicManager.GetComponent<AudioSource>();
        }

        public void Start()
        {
            // remember volume level from last time
            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                audioInstance.volume = PlayerPrefs.GetFloat("MusicVolume");
                slider.value = PlayerPrefs.GetFloat("MusicVolume");
            }

        }

        public void UpdateVolume()
        {
            audioInstance.volume = PlayerPrefs.GetFloat("MusicVolume");
        }

        public void UpdateVolumeWithSlider()
        {
            audioInstance.volume = slider.value;
            PlayerPrefs.SetFloat("MusicVolume", slider.value);
            PlayerPrefs.Save();
        }

        public void ChangeSong(AudioClip song)
        {
            audioInstance.clip = song;
        }
    }
}