using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Sheep : BaseMovement {

    public Tilemap destructableTilemap;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        base.Update();

        if (Input.GetKey(KeyCode.D)) {
            DestroyRock();
        }

        if (Input.GetKey(KeyCode.F)) {
            Teleport();
        }
    }

    void DestroyRock() {
        // Destroys the rock in front of you
        UnityEngine.Debug.Log("DestroyRock Called");

        int[] playerDirection = directions[(int)base.currDirection];

        float[] res = new float[playerDirection.Length];
        for (int i = 0; i < playerDirection.Length; i++) {
            res[i] = playerDirection[i];
            if ((int)base.currDirection % 2 == 1) {
                res[i] /= (float)Math.Sqrt(2);
            }
        }

        Vector3 v = new Vector3(res[0], res[1], 0);
        Vector3 ramPosition = transform.position + v;

        Collider2D collision = Physics2D.OverlapCircle(new Vector2(ramPosition.x, ramPosition.y), .001f);
        if(collision != null) { //destroy rock
            destructableTilemap.SetTile(destructableTilemap.WorldToCell(ramPosition), null);
        }
    }

    void Teleport() {
        // Well teleports across the map
    }
    public void MoveToJail() {
    	Vector3 newLocation = new Vector3((float) 2.5, (float) -1.5, 0);
    	transform.position = newLocation;
    	// TODO add conditions to move coords of sheep to jail
    }

}
