using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks {

    public static PhotonRoom room;
    private PhotonView PV;

    public bool isGameLoaded;
    public int currentScene;

    Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;

    public int playersInGame;

    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    public float lessThanMaxPlayers;
    private float atMaxPlayers;
    private float timeToStart;

    void Awake() {
        if (PhotonRoom.room == null) {
            PhotonRoom.room = this;
        } else {
            if (PhotonRoom.room != this) {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable() {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable() {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    void Start() {
        PV = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 6;
        timeToStart = startingTime;
    }

    public void OnStartButton() {
        if (MultiplayerSettings.multiplayerSettings.delayStart) {
            if (playersInRoom == 1) {
                RestartTimer();
            }

            if (!isGameLoaded) {
                if (readyToStart) {
                    atMaxPlayers -= Time.deltaTime;
                    lessThanMaxPlayers = atMaxPlayers;
                    timeToStart = atMaxPlayers;
                } else if (readyToCount) {
                    lessThanMaxPlayers -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayers;
                }

                if (timeToStart <= 0) {
                    StartGame();
                }
            }
        }
    }

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        if (PhotonNetwork.IsMasterClient) {
            PhotonLobby.Lobby.StartButton.SetActive(true);
        }

        UnityEngine.Debug.Log("Joined Room!!!");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();
        if (MultiplayerSettings.multiplayerSettings.delayStart) {
            UnityEngine.Debug.Log("Number of player in room: " + playersInRoom);

            if (playersInRoom > 1) {
                readyToCount = true;
            }

            if (playersInRoom >= MultiplayerSettings.multiplayerSettings.minPlayers) {
                readyToStart = true;
            }

            if (playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers) {
                if (!PhotonNetwork.IsMasterClient) {
                    return;
                }
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        } else {
            StartGame();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        base.OnPlayerEnteredRoom(newPlayer);
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;
        if (MultiplayerSettings.multiplayerSettings.delayStart) {
            if (playersInRoom > 1) {
                readyToCount = true;
            }

            if (playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers) {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient) {
                    return;
                }
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }


    void StartGame() {
        UnityEngine.Debug.Log("Starting game");
        isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (MultiplayerSettings.multiplayerSettings.delayStart) {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }

        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.multiplayerScene);
    }

    void RestartTimer() {
        lessThanMaxPlayers = startingTime;
        timeToStart = startingTime;
        atMaxPlayers = 6;
        readyToCount = false;
        readyToStart = false;
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode) {
        currentScene = scene.buildIndex;
        if (currentScene == MultiplayerSettings.multiplayerSettings.multiplayerScene) {
            isGameLoaded = true;
            
            if (MultiplayerSettings.multiplayerSettings.delayStart) {
                PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            } else {
                RPC_CreatePlayer();
            }

        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene() {
        playersInGame++;
        if (playersInGame == PhotonNetwork.PlayerList.Length) {
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer() {
        if (myNumberInRoom % 2 == 0) {
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Bo Peep"), Vector3.zero, Quaternion.identity);
        } else {
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Sheep"), Vector3.zero, Quaternion.identity);
        }
    }
}
