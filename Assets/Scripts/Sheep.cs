using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : BaseMovement {

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        base.Update();
    }

    public void MoveToJail() {
    	Vector3 newLocation = new Vector3((float) 2.5, (float) -1.5, 0);
    	transform.position = newLocation;
    	// TODO add conditions to move coords of sheep to jail
    }

}
