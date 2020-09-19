using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PhotonLobby : MonoBehaviourPunCallbacks {
    public static PhotonLobby Lobby;

    public GameObject StartButton;
    public GameObject JoinButton;


    private void Awake() {
        Lobby = this;
    }

    // Start is called before the first frame update
    void Start() {
        StartButton.SetActive(false);
        JoinButton.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        UnityEngine.Debug.Log("Connected to master");
        PhotonNetwork.AutomaticallySyncScene = true;
        JoinButton.SetActive(true);
    }

    public void OnJoinButtonClicked() {
        JoinButton.SetActive(false);
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnStartButtonClicked() {
        StartButton.SetActive(false);
        PhotonRoom.room.OnStartButton();
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        UnityEngine.Debug.Log("Join fail: "+message);
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        UnityEngine.Debug.Log("Create fail: " + message);
        CreateRoom();
    }

    void CreateRoom() {
        //UnityEngine.Debug.Log("Creating new room...");
        string randomRoomName = "test";
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSettings.multiplayerSettings.maxPlayers };
        //UnityEngine.Debug.Log("Created Room: "+ randomRoomName);

        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }
}
