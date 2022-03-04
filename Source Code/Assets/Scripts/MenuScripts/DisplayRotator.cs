using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayRotator : MonoBehaviour
{


    public Transform objectToRotate;

    private void Start()
    {
        CreateUV();
    }


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
            objectToRotate.transform.SetPositionAndRotation(objectToRotate.position, new Quaternion(0, -180, 0, 0));
        }

    }

    public void CreateUV()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector2[] UVs = new Vector2[mesh.vertices.Length];
        // Front
        UVs[0] = new Vector2(0.0f, 0.0f);
        UVs[1] = new Vector2(0.333f, 0.0f);
        UVs[2] = new Vector2(0.0f, 0.333f);
        UVs[3] = new Vector2(0.333f, 0.333f);
        // Top
        UVs[4] = new Vector2(0.334f, 0.333f);
        UVs[5] = new Vector2(0.666f, 0.333f);
        UVs[8] = new Vector2(0.334f, 0.0f);
        UVs[9] = new Vector2(0.666f, 0.0f);
        // Back
        UVs[6] = new Vector2(1.0f, 0.0f);
        UVs[7] = new Vector2(0.667f, 0.0f);
        UVs[10] = new Vector2(1.0f, 0.333f);
        UVs[11] = new Vector2(0.667f, 0.333f);
        // Bottom
        UVs[12] = new Vector2(0.0f, 0.334f);
        UVs[13] = new Vector2(0.0f, 0.666f);
        UVs[14] = new Vector2(0.333f, 0.666f);
        UVs[15] = new Vector2(0.333f, 0.334f);
        // Left
        UVs[16] = new Vector2(0.334f, 0.334f);
        UVs[17] = new Vector2(0.334f, 0.666f);
        UVs[18] = new Vector2(0.666f, 0.666f);
        UVs[19] = new Vector2(0.666f, 0.334f);
        // Right        
        UVs[20] = new Vector2(0.667f, 0.334f);
        UVs[21] = new Vector2(0.667f, 0.666f);
        UVs[22] = new Vector2(1.0f, 0.666f);
        UVs[23] = new Vector2(1.0f, 0.334f);
        mesh.uv = UVs;
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
