using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private string gameVersion = "1.0";

    [SerializeField]
    private GameObject userStartPanel;

    [SerializeField]
    private GameObject gameMainPanel;

    [SerializeField]
    private TMP_InputField usernameInput;

    [SerializeField]
    private TMP_Text warningTextForUsername;

    [SerializeField]
    private GameObject usernameSubmitButton;

    [SerializeField]
    private TMP_InputField joinGameInput;

    [SerializeField]
    private TMP_InputField createGameInput;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("connected!");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug
            .LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}",
            cause);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameLobby");
    }

    void Start()
    {
        usernameSubmitButton.SetActive(false);
        warningTextForUsername.enabled = false;
        gameMainPanel.SetActive(false);
        userStartPanel.SetActive(true);
    }

    public void onChangeUsernameInput()
    {
        if (usernameInput.text.Length < 3 || usernameInput.text.Length > 6)
        {
            warningTextForUsername.enabled = true;
            usernameSubmitButton.SetActive(false);
        }
        else
        {
            warningTextForUsername.enabled = false;
            usernameSubmitButton.SetActive(true);
        }
    }

    public void SubmitUsername()
    {
        userStartPanel.SetActive(false);
        gameMainPanel.SetActive(true);
        PhotonNetwork.NickName = usernameInput.text;
    }

    public void CreateNewRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(createGameInput.text, roomOptions, null);
    }

    public void JoinRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(joinGameInput.text, roomOptions, TypedLobby.Default);
    }
}
