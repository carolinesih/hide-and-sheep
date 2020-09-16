using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TimeArithmetic {
    public class BoPeep : BaseMovement {

        public float flashRange;
        public int flashCD; // in seconds
        private DateTime lastFlashTime = DateTime.Now;

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            base.Update();
            // UnityEngine.Debug.Log("position: " + transform.position);
            // UnityEngine.Debug.Log("direction: " + base.currDirection);
            if (Input.GetKey(KeyCode.D)) {
                
            }

            if (Input.GetKey(KeyCode.F) && canFlash()) {
                Flash();
            }
        }

        bool canFlash() {
            if (lastFlashTime.AddSeconds(flashCD) <= DateTime.Now) {
                return true;
            }
            return false;
        }


        // Ability to Flash over Walls 
        void Flash() {
            lastFlashTime = DateTime.Now;
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
            
            Collider2D collision = Physics2D.OverlapCircle(new Vector2(endDest.x, endDest.y), .001f);
            if (collision == null) {
                transform.position = endDest;
            } else {
                Vector3 closestPoint = collision.ClosestPoint(transform.position);
                transform.position = closestPoint;
            }
        }

        void Catch() {

        }
    }
}
