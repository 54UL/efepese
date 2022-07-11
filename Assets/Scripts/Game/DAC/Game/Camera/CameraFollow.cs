using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    public Transform target;

 
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(target.position.x-5,target.position.y+9,target.position.z-15);
        this.transform.LookAt(target);
	}
}
