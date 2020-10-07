using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerScr : MonoBehaviour
{
    public bool isLookingOnPlayer = false, forClock = true;
    public float timeAway = 2f;
    public Mesh frame1, frame2;//1-not danger, 2 - danger
    int curentFrame = 1;
    public GameObject player;
    void Start(){
        if(forClock){
            StartCoroutine(action());
        }
    }

    void Update(){
        if(isLookingOnPlayer) transform.eulerAngles = new Vector3(0, AngleJ(), 0);
    }

    IEnumerator action(){
        yield return new WaitForSeconds(timeAway);
        if(curentFrame==1) {
            GetComponent<MeshCollider>().sharedMesh = frame2;
            GetComponent<MeshFilter>().mesh = frame2;
            this.gameObject.tag = "Danger";
            curentFrame = 0;
        }
        else {
            GetComponent<MeshCollider>().sharedMesh = frame1;
            GetComponent<MeshFilter>().mesh = frame1;
            this.gameObject.tag = "Untagged";
            curentFrame = 1;
        }
        StartCoroutine(action());
    }

    public void setPl(GameObject pl){
        player = pl;
    }

    float AngleJ(){//Угол оружия
		Vector3 mousePos=new Vector3();
        mousePos = player.transform.position;

        mousePos.x =  mousePos.x-transform.position.x;
        mousePos.y =  mousePos.z-transform.position.z;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        
		return 90-angle;
	} 
}