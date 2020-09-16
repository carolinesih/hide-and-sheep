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
        public int hookRadius; // radius hook can reach
        public int hookWidth; // width of range hook can reach (ex. 30 degrees)
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
                Hook();
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

        void Hook() {
            Sheep[] sheep = FindObjectsOfType(typeof(Sheep)) as Sheep[];
            foreach(Sheep individual in sheep)
            {
                UnityEngine.Debug.Log("bo peep is at " + transform.position + " and sheep is at " + individual.transform.position);
                if  (
                    Math.Abs(transform.position.x - individual.transform.position.x) < hookRadius && 
                    Math.Abs(transform.position.y - individual.transform.position.y) < hookRadius)
                {
                    individual.MoveToJail();
                }
            }
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
