using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class BaseMovement : MonoBehaviour {

    public int rotateSpeed;
    public int movementSpeed;
   
    public enum Direction {North=0, NorthEast=1, East=2, SouthEast=3, South=4, SouthWest=5, West=6, NorthWest=7};
    public Direction currDirection;
    public int[][] directions = new int[][] {
        new int[] {0,1},
        new int[] {1,1},
        new int[] {1,0},
        new int[] {1,-1},
        new int[] {0,-1},
        new int[] {-1,-1},
        new int[] {-1,0},
        new int[] {-1,1}
    };


	// Use this for initialization
	void Start () {
        currDirection = Direction.North;
	}
	
	// Update is called once per frame
	public void Update () {
        Move();
	}

    private void Move() {
        int x = 0;
        int y = 0;
        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.Translate(Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime, 0, 0);
            x += 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.Translate(Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime, 0, 0);
            x -= 1;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            transform.Translate(0, Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime, 0);
            y += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            transform.Translate(0, Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime, 0);
            y -= 1;
        }

        if (x != 0 || y != 0) {
            FacingDirection(x, y);
        }
    }

    private void FacingDirection(int x, int y) {
        for (int i = 0; i < directions.Length; i++) {
            if (directions[i][0] == x && directions[i][1] == y) {
                currDirection = (Direction) i;
            }
        }
    }
}