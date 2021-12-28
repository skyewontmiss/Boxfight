using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayRotator : MonoBehaviour
{


    public Transform objectToRotate;


    private void Update()
    {

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            OnArrowClicked("Left Arrow");
        } else
        if (Input.GetKey(KeyCode.RightArrow))
        {
            OnArrowClicked("Right Arrow");
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            OnArrowClicked("Up Arrow");
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            OnArrowClicked("Down Arrow");
        }
        if (Input.GetKey(KeyCode.R))
        {
            objectToRotate.transform.SetPositionAndRotation(objectToRotate.position, new Quaternion(0, 0, 0, 0));
        }

    }

    void OnArrowClicked(string arrow)
    {
        if (arrow == "Left Arrow")
        {

            objectToRotate.transform.Rotate(0, 10, 0);
        }
        else if (arrow == "Right Arrow")
        {
            objectToRotate.transform.Rotate(0, -10, 0);

        }
        else if (arrow == "Up Arrow")
        {

            objectToRotate.transform.Rotate(10, 0, 0);
        }
        else if (arrow == "Down Arrow")
        {
            objectToRotate.transform.Rotate(-10, 0, 0);

        } else
        {
            //returns error
            Debug.LogWarning("Error:  no arrow key assigned to in your code!");
        }
    }


}
