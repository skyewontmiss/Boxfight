using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayFabAuthentication : MonoBehaviourPunCallbacks
{
    public string PlayerIdCache = "";
    public static PlayFabAuthentication instance;


    //login UI instances
    public TMP_InputField Login_User, Login_Password;
    public TMP_InputField Register_User, Register_Password, Register_Email;
    public TMP_Text errorText, loggedInText;
    public string Username;
    public bool isAllowing, isRegistering;



    public GameObject BackToMenu, AccountCheckToClose;
    public Animator menuSwitchAnimator, myAnimator;

    public string errorr;

    private void Awake()
    {
        instance = this;
        isRegistering = false;
        //AuthenticateWithPlayFab();

    }



    public void StartOnPhotonConnected()
    {
        if (PlayerPrefs.HasKey("Username") || PlayerPrefs.HasKey("Password"))
        {
            LoginWithPlayFabRequest request = new LoginWithPlayFabRequest();
            request.Username = PlayerPrefs.GetString("Username");
            request.Password = PlayerPrefs.GetString("Password");
            PlayFabClientAPI.LoginWithPlayFab(request, RequestToken, OnAuthenticationError);
            AccountCheckToClose.SetActive(true);
        }
        else
        {
            Debug.Log("don");
        }
    }

    public void Update()
    {
        if(isAllowing == false)
        {
            if(Input.GetKey(KeyCode.KeypadEnter))
            {
                if(isRegistering == true)
                {
                    PlayFabRegister();

                } else if(isRegistering == false)
                {
                    PlayFabLogin();

                } else
                {
                    return;
                }
            }
        }
    }


    public void PlayFabLogin()
    {
        //LoginWithCustomIDRequest request = new LoginWithCustomIDRequest();

        //request.CreateAccount = true;
        //request.CustomId = PlayFabSettings.DeviceUniqueIdentifier;

        //PlayFabClientAPI.LoginWithCustomID(request,RequestToken, OnAuthenticationError);
        LoginWithPlayFabRequest request = new LoginWithPlayFabRequest();
        request.Username = Login_User.text;
        request.Password = Login_Password.text;
        PlayFabClientAPI.LoginWithPlayFab(request, RequestToken, OnAuthenticationError);
        PlayerPrefs.SetString("Username", request.Username);
        PlayerPrefs.SetString("Password", request.Password);
        PlayerPrefs.Save();
    }

    public void PlayFabRegister()
    {
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest();
        request.Username = Register_User.text;
        request.Password = Register_Password.text;
        request.Email = Register_Email.text;

        PlayFabClientAPI.RegisterPlayFabUser(request, result => { myAnimator.Play("ChangelogMenu", 0, 0f); AccountCheckToClose.SetActive(false); }, OnAuthenticationError);
    }

    void RequestToken(LoginResult result)
    {
        PlayerIdCache = result.PlayFabId;
        GetPhotonAuthenticationTokenRequest request = new GetPhotonAuthenticationTokenRequest();
        request.PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
        PlayFabClientAPI.GetPhotonAuthenticationToken(request, AuthenticateWithPhoton, OnAuthenticationError);
    }


    void AuthenticateWithPhoton(GetPhotonAuthenticationTokenResult result)
    {
        var CustomAuthentication = new AuthenticationValues { AuthType = CustomAuthenticationType.Custom };

        CustomAuthentication.AddAuthParameter("username", PlayerIdCache);

        CustomAuthentication.AddAuthParameter("token", result.PhotonCustomAuthenticationToken);
        PhotonNetwork.AuthValues = CustomAuthentication;
        myAnimator.Play("ChangelogLogInToLoggedIn", 0, 0f);
        menuSwitchAnimator.Play("PlayFabToMenu", 0, 0f);
        AccountCheckToClose.SetActive(false);
        loggedInText.text = "Already logged in as: " + PlayerPrefs.GetString("Username") + ".";
        PhotonNetwork.NickName = PlayerPrefs.GetString("Username");
    }

    void OnAuthenticationError(PlayFabError error)
    {
        AccountCheckToClose.SetActive(false);
        BackToMenu.SetActive(true);
        errorr = error.ToString();

        if(errorr.Contains("/Client/LoginWithPlayFab: User not found"))
        {
            errorr = "We can't find a player with these details. Register an account.";

        } else if(errorr.Contains("some weird Playfab error happened. Check what it is: /Client/LoginWithPlayFab: Invalid input parameters"))
        {
            errorr = "You probably got your password or your username wrong. Try log in again.";
        } else
        {
            errorr = error.ToString();
        }
        Debug.LogError("some weird Playfab error happened. Check what it is: " + error);
        errorText.text = errorr;
    }

    public void PlayfabLogOut()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        PlayerPrefs.DeleteKey("Username");
        PlayerPrefs.DeleteKey("Password");
        myAnimator.Play("ChangelogLoggedInToLogIn", 0, 0f);
    }


}
