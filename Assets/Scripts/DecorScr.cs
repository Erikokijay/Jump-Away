using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorScr : MonoBehaviour
{
    public int radius = 1;//количество вокселей от центра до крайней точки
    public float dir = 1f;//направление + (вправо) или - (влево)
    public float speedAway, downSpeed;
    float maxX;
    void Start(){
        if(forClock){
            StartCoroutine(action());
        }
    }
    public bool forClock = false, dirT = false;
    public float timeAway = 2f;
    public Mesh frame1, frame2;//1-not danger, 2 - danger
    int curentFrame = 1;
    GameObject main;
    public void dirMe(float direction, float speedDown, GameObject m){ 
        main = m;
        maxX = main.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        if(direction > 0){
            speedAway*=Mathf.FloorToInt(direction);
            if(dirT) transform.eulerAngles = new Vector3(0,transform.eulerAngles.y*Mathf.FloorToInt(direction),0);
            this.transform.position = new Vector3(maxX-radius*0.3f, Random.Range(-1.5f, 4), Random.Range(-3f, 15f));
        } else{
            speedAway*=Mathf.FloorToInt(direction);
            if(dirT) transform.eulerAngles = new Vector3(0,transform.eulerAngles.y*Mathf.FloorToInt(direction),0);
            this.transform.position = new Vector3(-maxX+radius*0.3f, Random.Range(-1.5f, 4), Random.Range(-3f, 15f));
        }
    }

    IEnumerator action(){
        yield return new WaitForSeconds(timeAway);
        if(curentFrame==1) {
            GetComponent<MeshFilter>().mesh = frame2;
            curentFrame = 0;
        }
        else {
            GetComponent<MeshFilter>().mesh = frame1;
            curentFrame = 1;
        }
        StartCoroutine(action());
    }

    void Update(){
        if((transform.position.x<maxX-radius*0.3f-0.5f && dir > 0) || (transform.position.x>-maxX+radius*0.3f+0.5f && dir < 0) || transform.position.z<-5f-radius*0.3f){
            Destroy(this.gameObject);
        }else{
            transform.position = new Vector3(transform.position.x+speedAway*Time.deltaTime*dir, transform.position.y, transform.position.z);
            if(main.GetComponent<MainController>().gameStart && (!main.GetComponent<MainController>().lose && !main.GetComponent<MainController>().pause)){
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z-downSpeed*Time.deltaTime*main.GetComponent<MainController>().gameSpeed);
            }
        }
    }

}