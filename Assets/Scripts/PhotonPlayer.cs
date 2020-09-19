using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PhotonPlayer : MonoBehaviour {

    private PhotonView PV;
    public GameObject Avatar; 

    // Start is called before the first frame update
    void Start() {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine && PhotonRoom.room.myNumberInRoom % 2 == 0) {
            Avatar = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Sheep"), Vector3.zero, Quaternion.identity, 0);
        } else {
            Avatar = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Bo Peep"), Vector3.zero, Quaternion.identity, 0);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
