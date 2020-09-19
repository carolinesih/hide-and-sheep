using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;

namespace TimeArithmetic {
    public class BoPeep : BaseMovement {

        public float flashRange;
        public int flashCD; // in seconds]
        public int hookCD; // in seconds
        public int hookRadius; // radius hook can reach
        public int hookWidth; // width of range hook can reach (ex. 30 degrees)
        private DateTime lastFlashTime = DateTime.Now;
        private DateTime lastHookTime = DateTime.Now;

        public Tilemap destructableTileMap;
        public Tilemap indestructableTileMap;
        public Tilemap jailTileMap;

        private PhotonView PV;


        // Start is called before the first frame update
        void Start() {
            destructableTileMap = GameObject.FindGameObjectWithTag("tilemap_destructible").GetComponent<Tilemap>();
            indestructableTileMap = GameObject.FindGameObjectWithTag("tilemap_indestructible").GetComponent<Tilemap>();
            jailTileMap = GameObject.FindGameObjectWithTag("tilemap_jail").GetComponent<Tilemap>();

            PV = GetComponent<PhotonView>();
        }

        // Update is called once per frame
        void Update() {
            if (!PV.IsMine)
                return;

            base.Update();
            // UnityEngine.Debug.Log("position: " + transform.position);
            // UnityEngine.Debug.Log("direction: " + base.currDirection);
            if (Input.GetKey(KeyCode.D) && canHook()) {
                Hook();
            }

            if (Input.GetKey(KeyCode.F) && canFlash()) {
                Flash();
            }
        }

        bool canHook() {
            if (lastHookTime.AddSeconds(hookCD) <= DateTime.Now) {
                return true;
            }
            return false;
        }

        bool canFlash() {
            if (lastFlashTime.AddSeconds(flashCD) <= DateTime.Now) {
                return true;
            }
            return false;
        }

        void Hook() {
            Sheep[] sheep = FindObjectsOfType(typeof(Sheep)) as Sheep[];
            foreach(Sheep individual in sheep)
            {
                //UnityEngine.Debug.Log("bo peep is at " + transform.position + " and sheep is at " + individual.transform.position);

                if  (
                    Math.Abs(transform.position.x - individual.transform.position.x) < hookRadius && 
                    Math.Abs(transform.position.y - individual.transform.position.y) < hookRadius) {
                    lastHookTime = DateTime.Now;
                    individual.MoveToJail();
                    return;
                }
            }
        }

        // Ability to Flash over Walls 
        void Flash() {
            int[] flashDirection = directions[(int)base.currDirection];

            float[] res = new float[flashDirection.Length];
            for (int i = 0; i < flashDirection.Length; i++) {
                res[i] = flashDirection[i] * flashRange;
                if ((int)base.currDirection % 2 == 1) {
                    res[i] /= (float)Math.Sqrt(2);
                }
            }

            Vector3 vector = new Vector3(res[0], res[1], 0);

            Vector3 endDest = transform.position + vector;

            TileBase dt = destructableTileMap.GetTile(destructableTileMap.WorldToCell(endDest));
            TileBase it = indestructableTileMap.GetTile(indestructableTileMap.WorldToCell(endDest));
            TileBase jt = jailTileMap.GetTile(indestructableTileMap.WorldToCell(endDest));


            if (dt == null && it == null) {
                transform.position = endDest;
                lastFlashTime = DateTime.Now;
            } else {
                vector = new Vector3(flashDirection[0], flashDirection[1], 0);
                endDest = transform.position + vector;
                dt = destructableTileMap.GetTile(destructableTileMap.WorldToCell(endDest));
                it = indestructableTileMap.GetTile(indestructableTileMap.WorldToCell(endDest));
                jt = jailTileMap.GetTile(indestructableTileMap.WorldToCell(endDest));

                if (dt == null && it == null) {
                    transform.position = endDest;
                    lastFlashTime = DateTime.Now;
                }
            }
            /*Collider2D collider = Physics2D.OverlapCircle(new Vector2(endDest.x, endDest.y), 0.2f);
            if (collider == null) {
                UnityEngine.Debug.Log("No collider");
                transform.position = endDest;
            } else {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), 
                    new Vector2(flashDirection[0], flashDirection[1]) * 100);
                UnityEngine.Debug.Log("Raycast: "+hit.collider);

                //transform.position = new Vector3(closestPoint.x, closestPoint.y, 0);
            }*/
        }
    }
}
