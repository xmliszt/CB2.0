using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviourPunCallbacks
{
    public GameStats gameStats;

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
    private TMP_Text warningTextForController;

    [SerializeField]
    private GameObject usernameSubmitButton;

    [SerializeField]
    private TMP_InputField joinGameInput;

    [SerializeField]
    private TMP_InputField createGameInput;

    [SerializeField]
    private Button keyboardBtn;

    [SerializeField]
    private Button gamepadBtn;

    [SerializeField]
    private TMP_Text youAreUsing;

    [SerializeField]

    private TMP_Text joinRoomWarning;

    private void Awake()
    {
        PhotonNetwork.SendRate = 40;
        PhotonNetwork.AutomaticallySyncScene = true;
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
        PhotonNetwork.CurrentRoom.EmptyRoomTtl = 0;
        PhotonNetwork.LoadLevel("GameLobby");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        joinRoomWarning.enabled = true;
        joinRoomWarning.text = message;   
    }

    void Start()
    {
        youAreUsing.text = "You are using: ...";
        joinRoomWarning.enabled = false;
        usernameSubmitButton.SetActive(false);
        warningTextForUsername.enabled = false;
        warningTextForController.enabled = false;
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
        if (gameStats.controllerType != ControllerType.nullType)
        {
            warningTextForController.enabled = false;
            userStartPanel.SetActive(false);
            gameMainPanel.SetActive(true);
            PhotonNetwork.NickName = usernameInput.text;
        }
        else
        {
            warningTextForController.enabled = true;
        }
    }

    public void CreateNewRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(createGameInput.text, roomOptions, null);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinGameInput.text);
    }

    public void onKeyboardSelected()
    {
        gameStats.controllerType = ControllerType.keyboard;
        youAreUsing.text = "You are using: keyboard";
    }

    public void onGamepadSelected()
    {
        gameStats.controllerType = ControllerType.generic;
        youAreUsing.text = "You are using: controller";
    }
}
