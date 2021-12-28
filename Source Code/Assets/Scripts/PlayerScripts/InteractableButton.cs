using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class InteractableButton : MonoBehaviour
{
    GameObject myself;
    Animator animator;
    public AudioClip buttonClick;

    public bool isForChristmasRickRoll;

    [Header("Christmas Rickroll Assets")]
    public GameObject video;
    public VideoPlayer videoPlayer;


    


    private void Awake()
    {
        myself = this.gameObject;
        animator = myself.GetComponent<Animator>();
    }

    public void OnClicked()
    {

        if(isForChristmasRickRoll == true)
        {
            StartCoroutine(ChristmasRickrollCoroutine());

        } else
        {
            StartCoroutine(OnClickedCoroutine());
        }

    }


    IEnumerator ChristmasRickrollCoroutine()
    {
        animator.Play("ButtonPressed", 0);
        yield return new WaitForSeconds(0.12f);
        AudioSource ButtonSFX = myself.AddComponent<AudioSource>();
        ButtonSFX.clip = buttonClick;
        ButtonSFX.Play();
        yield return new WaitForSeconds(buttonClick.length);
        Destroy(ButtonSFX);
        video.SetActive(true);
        videoPlayer.Play();
        yield return new WaitForSeconds((float) videoPlayer.length);
        video.SetActive(false);
        videoPlayer.Stop();
    }


    IEnumerator OnClickedCoroutine()
    {
        animator.Play("ButtonPressed", 0);
        yield return new WaitForSeconds(0.12f);
        AudioSource ButtonSFX = myself.AddComponent<AudioSource>();
        ButtonSFX.clip = buttonClick;
        ButtonSFX.Play();
        yield return new WaitForSeconds(buttonClick.length);
        Destroy(ButtonSFX);
    }
}
