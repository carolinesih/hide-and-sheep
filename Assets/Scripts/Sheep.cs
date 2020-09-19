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
    public Tilemap teleporterTilemap;

    public int rockCD;
    public int teleportCD;

    private DateTime lastBreakTime = DateTime.Now;
    private DateTime lastTPTime = DateTime.Now;

    private ArrayList teleporters = new ArrayList();
    private ArrayList teleporterCoord = new ArrayList();

    private int[] teleporterMapping = new int[] {
        5, 3, 4, 1, 2, 0
    };

    // Start is called before the first frame update
    void Start() {
        destructableTilemap = GameObject.FindGameObjectWithTag("tilemap_destructible").GetComponent<Tilemap>();
        teleporterTilemap = GameObject.FindGameObjectWithTag("tilemap_teleporter").GetComponent<Tilemap>();
        getTeleporters();
    }

    // Update is called once per frame
    void Update() {
        base.Update();

        if (Input.GetKey(KeyCode.D) && canDestroyRock()) {
            DestroyRock();
        }

        if (Input.GetKey(KeyCode.F) && canTeleport()) {
            Teleport();
        }
    }

    bool canDestroyRock() {
        return lastBreakTime.AddSeconds(rockCD) <= DateTime.Now;
    }

    void DestroyRock() {
        // Destroys the rock in front of you
        UnityEngine.Debug.Log("DestroyRock Called");
        lastBreakTime = DateTime.Now;

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

    bool canTeleport() {
        return lastTPTime.AddSeconds(teleportCD) <= DateTime.Now;
    }

    void Teleport() {
        // Well teleports across the map
 
        //UnityEngine.Debug.Log("Teleport Called");

        TileBase t = teleporterTilemap.GetTile(Vector3Int.FloorToInt(transform.position));

        if (t != null) {
            lastTPTime = DateTime.Now;
            int tpID = teleporters.IndexOf(t.GetInstanceID());
            int newTPID = teleporterMapping[tpID];
            transform.position = (Vector3) teleporterCoord[newTPID];
        } 

    }

    // initialize teleporter array
    void getTeleporters() {

        Vector3Int o = teleporterTilemap.origin;
        BoundsInt bounds = teleporterTilemap.cellBounds;
        TileBase[] allTiles = teleporterTilemap.GetTilesBlock(bounds);
        for (int i = 0; i < bounds.size.x; i++) {
            for (int j = 0; j < bounds.size.y; j++) {
                TileBase t = allTiles[i + j * bounds.size.x];
                if (t != null) {
                    teleporters.Add(t.GetInstanceID());
                    teleporterCoord.Add(new Vector3(o.x + i, o.y + j,0));
                }
            }
        }
    }

    public void MoveToJail() {
    	Vector3 newLocation = new Vector3((float) 2.5, (float) -1.5, 0);
    	transform.position = newLocation;
    	// TODO add conditions to move coords of sheep to jail
    }

}
