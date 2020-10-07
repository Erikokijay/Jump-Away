using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonDestroy : MonoBehaviour {

	void Start () {
		if(GameObject.FindGameObjectsWithTag("DDestroy").Length>1) {
			Destroy(this.gameObject);
		} 
		DontDestroyOnLoad(this.gameObject);	
	}
}
