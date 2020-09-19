using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PhotonLobby : MonoBehaviourPunCallbacks {
    public static PhotonLobby Lobby;

    public GameObject StartButton;


    private void Awake() {
        Lobby = this;
    }

    // Start is called before the first frame update
    void Start() {
        StartButton.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        UnityEngine.Debug.Log("Connected to master");
        PhotonNetwork.AutomaticallySyncScene = true;
        StartButton.SetActive(true);
    }

    public void OnStartButtonClicked() {
        StartButton.SetActive(false);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        CreateRoom();
    }

    public override void OnJoinedRoom() {
        UnityEngine.Debug.Log("Joined Room!!!");
    }

    void CreateRoom() {
        UnityEngine.Debug.Log("Creating new room...");
        int randomRoomName = Random.Range(0, 1000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSettings.multiplayerSettings.maxPlayers };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }
    // Update is called once per frame
    void Update() {
        
    }
}
