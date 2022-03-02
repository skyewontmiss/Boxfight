using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

namespace DreamloAPI.Manager
{
    public class DreamloManager : MonoBehaviour
    {
        //we're going to create a singleton  of the dreamlo manager so we can access it in all of our scripts if there is an instance in our scene.
        public static DreamloManager instance;


        public void FetchDreamloCallbacks(TMP_InputField input)
        {
            //we need to get the code through the UnityWebRequest module.
            StartCoroutine(FetchCode(input.text));
        }

        //this is the backend. This is what happens behind the scenes while we are trying to get our code.
        #region Backend
        IEnumerator FetchCode(string code)
        {
            //this is so we can get to the dreamlo website and download our callbacks.
            string constructedURL = "http://dreamlo.com/pc/621fa1c98f40bb12584e5fdf/redeem/" + code;
            string callback;

            UnityWebRequest www = UnityWebRequest.Get(constructedURL);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                callback = "Error|WebRequestError";
            }
            else
            {
                callback = www.downloadHandler.text;
            }

            ResponsesHandler(callback);
        }

        #endregion

        #region Responses
        /*Dreamlo has three responses. Those are:
         "ERROR|Unknown Code" - this is thrown when the user enters a code that does not exist or is incorrect.

         "ERROR|Code Already Used" - pretty self-explanatory. This is thrown when the user enters a code that has already been redeemed before.

         "OK|CodeExample" - this is thrown when the code is redeemable, has not been redeemed before, and has just been redeemed. It might not be 'OK|CodeExample', but it will be "OK|<value>'.

        we are going to use these responses to our advantage and respond to our player with the response that has been thrown at us by Dreamlo. We are going to... let's say, print some more friendly notifications to the console for now.
         */

        void ResponsesHandler(string response)
        {
            if(response == "ERROR|Unknown Code")
            {
                //means the code does not exist. We will print this to our console with an Error type of Debug.
                Debug.LogError("The code you entered is invalid.");

            } else if(response == "ERROR|Code Already Used")
            {
                //means the code exists, but has been used. We will print this to our console with a Warning type of debug.
                Debug.LogWarning("The code you entered has been redeemed already!");

            } else if(response == "OK|CodeExample")
            {
                //means everything is okay! The code has not been redeemed yet, and it's valid to be used! We will print this great news to our console with a normal type of debug.
                Debug.Log("Redeemed DLC Code!");
                //:D
            }
        }


        #endregion
    }
}


