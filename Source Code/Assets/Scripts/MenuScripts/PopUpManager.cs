using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    [Header("Prompt Sideloading")]
    public string[] QuitPrompt, GamePrompt;

    [Header("Prompt Visual Assets")]
    public TMP_Text title, description, buttonText;
    public Animator promptAnimator;

    void Start()
    {
        
    }





    public void ImportGamePrompt()
    {
        promptAnimator.Play("PopupUp", 0, 0f);
        if(GamePrompt[0] == null)
        {
            title.text = "Missing Title.";

        } else
        {
            title.text = GamePrompt[0];
        }

        if (GamePrompt[2] == null)
        {
            description.text = "Missing Description.";

        }
        else
        {
            description.text = GamePrompt[1];
        }

        if (GamePrompt[2] == null)
        {
            buttonText.text = "Ok";

        }
        else
        {
            buttonText.text = GamePrompt[2];
        }


    }

    public void ImportQuitPrompt()
    {
        promptAnimator.Play("PopupUp", 0, 0f);
        if (QuitPrompt[0] == null)
        {
            title.text = "Missing Title.";

        }
        else
        {
            title.text = QuitPrompt[0];
        }

        if (QuitPrompt[2] == null)
        {
            description.text = "Missing Description.";

        }
        else
        {
            description.text = QuitPrompt[1];
        }

        if (QuitPrompt[2] == null)
        {
            buttonText.text = "Ok";

        }
        else
        {
            buttonText.text = QuitPrompt[2];
        }
    }

   
    public void OnButtonClicked()
    {
        promptAnimator.Play("PopupDown", 0, 0f);
    }


    public void OnIgnorePrompt()
    {
        promptAnimator.Play("PopupDown", 0, 0f);
    }

}
