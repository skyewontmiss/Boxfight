using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] Menu[] menus;

    public Slider slider;

    [SerializeField] AudioSource[] songs;
    int currentSong;

    void Awake()
    {
        Instance = this;
        foreach (AudioSource speaker in songs)
        {
            speaker.volume = 0f;
        }

        if(PlayerPrefs.HasKey("Jukebox Song"))
        {
            songs[PlayerPrefs.GetInt("Jukebox Song")].volume = PlayerPrefs.GetFloat("MusicVolume");

        } else
        {
            songs[0].volume = PlayerPrefs.GetFloat("MusicVolume");
        }
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                menus[i].Open();
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    public void SwitchSong(int songToSwitchTo)
    {
        foreach (AudioSource speaker in songs)
        {
            speaker.volume = 0f;
        }
        PlayerPrefs.SetInt("Jukebox Song", songToSwitchTo);
        currentSong = songToSwitchTo;

        songs[songToSwitchTo].volume = PlayerPrefs.GetFloat("MusicVolume");
    }


    public void Start()
    {
        // remember volume level from last time
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            slider.value = PlayerPrefs.GetFloat("MusicVolume");
        }

    }

    void Update()
    {
        if (PlayerPrefs.HasKey("Jukebox Song"))
        {
            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                songs[PlayerPrefs.GetInt("Jukebox Song")].volume = PlayerPrefs.GetFloat("MusicVolume");
            }
            else
            {
                songs[currentSong].volume = 0.5f;
            }
             
        } else
        {
            songs[currentSong].volume = 0.5f;
        }


    }

    public void UpdateVolumeWithSlider()
    {
        PlayerPrefs.SetFloat("MusicVolume", slider.value);
        PlayerPrefs.Save();
    }

}