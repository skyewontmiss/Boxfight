using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveChanger : MonoBehaviour
{

    public GameObject thirdPersonHolder, firstPersonHolder;
    public MonoBehaviour thirdPersonController, firstPersonController;
    public bool isThirdPerson;
    public KeyCode PerspectiveChangerKeyCode;

    void Start()
    {
        thirdPersonController.enabled = true;
        thirdPersonHolder.SetActive(true);
        firstPersonHolder.SetActive(false);
        firstPersonController.enabled = false;
        isThirdPerson = true;
        StartCoroutine(TogglePerspective());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator TogglePerspective()
    {
        while(true)
        {
            if (Input.GetKey(PerspectiveChangerKeyCode))
            {
                if (isThirdPerson)
                {
                    thirdPersonController.enabled = false;
                    thirdPersonHolder.SetActive(false);
                    firstPersonHolder.SetActive(true);
                    firstPersonController.enabled = true;
                    isThirdPerson = false;

                }
                else
                {
                    thirdPersonController.enabled = true;
                    thirdPersonHolder.SetActive(true);
                    firstPersonHolder.SetActive(false);
                    firstPersonController.enabled = false;
                    isThirdPerson = true;
                }
            }
            yield return new WaitWhile(() => Input.GetKey(PerspectiveChangerKeyCode));
            yield return null;
        }
    }
}
