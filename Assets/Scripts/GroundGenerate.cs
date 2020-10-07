using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerate : MonoBehaviour
{
    public GameObject[] grounds;
    const float maxDist = 4.6f;
    [SerializeField]
    GameObject[] meshes, dMeshes, cMeshes;
    public float maxX;

    void Start(){
        maxX = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        genNow(true);
    }

    public bool first = false;
    public void continueBacker(float distBack){
        for(int i = 0; i<grounds.Length; i++){
            grounds[i].transform.position = new Vector3(grounds[i].transform.position.x, grounds[i].transform.position.y, grounds[i].transform.position.z-distBack);
        }
    }

    public void genNow(bool start){
        maxX = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        first = false;
        if(start){
            for(int i=0; i<grounds.Length; i++){
                int r = Random.Range(0,meshes.Length);
                grounds[i] = Instantiate(meshes[r]);
            }
        } else{
            for(int i=0; i<grounds.Length; i++){
                int r = Random.Range(0,meshes.Length);
                Destroy(grounds[i].gameObject);
                grounds[i] = Instantiate(meshes[r]);
            }
        }
        GetComponent<MainController>().player.GetComponent<PlayerController>().lastGround = grounds[13];
        grounds[13].transform.position = new Vector3(0,2.4f, 3.5f);
        for( int i = grounds.Length-2; i>=0; i--){
            int rad = grounds[i].GetComponent<GroundScr>().radius;
            if(i<grounds.Length-1){
                int rad2 = grounds[i+1].GetComponent<GroundScr>().radius;//радиус после идущего
                float minDist = 0.8f+rad2*0.065f+rad*0.065f;//минимальная дистанция по z (катет)
                float dist = Random.Range(minDist,maxDist);//дистанция (гипотенуза)
                float xMove = Random.Range(-(Mathf.Sqrt((dist*dist)-(minDist*minDist))),Mathf.Sqrt((dist*dist)-(minDist*minDist)));
                //Mathf.Clamp(grounds[i].transform.position.x+xMove, maxX+0.7f, -maxX-0.7f);
                //Random.Range(maxX+0.7f, -maxX-0.7f)
                grounds[i].transform.position = new Vector3(Mathf.Clamp(xMove, maxX+1f, -maxX-1f), 2.4f,grounds[i+1].transform.position.z+minDist);//сдвиг по x (второй катет)
                grounds[i].transform.eulerAngles = new Vector3(0, Random.Range(0,360), 0);
            } else{
                int rad2 =  grounds[0].GetComponent<GroundScr>().radius;
                float minDist = 0.8f+rad2*0.065f+rad*0.065f;
                float dist = Random.Range(minDist,maxDist);
                float xMove = Random.Range(-(Mathf.Sqrt((dist*dist)-(minDist*minDist))),Mathf.Sqrt((dist*dist)-(minDist*minDist)));
                //Mathf.Clamp(grounds[0].transform.position.x+xMove,-3.4f,3.4f);*/
                grounds[i].transform.position = new Vector3(Mathf.Clamp(xMove,maxX+1f, -maxX-1f),2.4f,grounds[0].transform.position.z+minDist);
                grounds[i].transform.eulerAngles = new Vector3(0, Random.Range(0,360), 0);
            }
            if(grounds[i].GetComponent<GroundScr>().places.Length>0){
                for(int j=0; j<grounds[i].GetComponent<GroundScr>().places.Length-1; j++){
                    int r = Random.Range(0,3);
                    if(r==1 && !grounds[i].GetComponent<GroundScr>().poses[j]){//generate danger
                        if(GetComponent<MainController>().difficulty>0) {
                            int rand = Random.Range(0,dMeshes.Length);
                            
                            if(rand > 1){
                                grounds[i].GetComponent<GroundScr>().create(dMeshes[rand],j, GetComponent<MainController>().player);
                            } else{
                                grounds[i].GetComponent<GroundScr>().create(dMeshes[rand],j);
                            }
                        }
                    } else if(r==2){//generate coins
                        int randC = Random.Range(0,8);
                        if(randC == 3 || randC==5){
                            grounds[i].GetComponent<GroundScr>().create(cMeshes[1],j);
                        } else if(randC == 6){
                            grounds[i].GetComponent<GroundScr>().create(cMeshes[2],j);
                        } else{
                            grounds[i].GetComponent<GroundScr>().create(cMeshes[0],j);
                        }
                    }
                }
            }
        }
    }

    void Update(){
       // ground Width = 0.065*radius
       for( int i = grounds.Length-1; i>=0; i--){
            if(grounds[i].transform.position.z<=-8f-grounds[i].GetComponent<GroundScr>().radius*0.065f){
                int r = Random.Range(0,meshes.Length);
                int rad = grounds[i].GetComponent<GroundScr>().radius;
                Destroy(grounds[i].gameObject);
                grounds[i] = Instantiate(meshes[r]);
                if(i == 12) first = true;
                if(i<grounds.Length-1){
                    int rad2 = grounds[i+1].GetComponent<GroundScr>().radius;//радиус после идущего
                    float minDist = 0.8f+rad2*0.065f+rad*0.065f;//минимальная дистанция по z (катет)
                    float dist = Random.Range(minDist,maxDist);//дистанция (гипотенуза)
                    float xMove = Random.Range(-(Mathf.Sqrt((dist*dist)-(minDist*minDist))),Mathf.Sqrt((dist*dist)-(minDist*minDist)));
                    //Mathf.Clamp(grounds[i].transform.position.x+xMove, maxX+0.7f, -maxX-0.7f);
                    //Random.Range(maxX+0.7f, -maxX-0.7f)
                    grounds[i].transform.position = new Vector3(Mathf.Clamp(xMove, maxX+1f, -maxX-1f), 2.4f,grounds[i+1].transform.position.z+minDist);//сдвиг по x (второй катет)
                    grounds[i].transform.eulerAngles = new Vector3(0, Random.Range(0,360), 0);
                } else{
                    int rad2 = grounds[0].GetComponent<GroundScr>().radius;
                    float minDist = 0.8f+rad2*0.065f+rad*0.065f;
                    float dist = Random.Range(minDist,maxDist);
                    float xMove = Random.Range(-(Mathf.Sqrt((dist*dist)-(minDist*minDist))),Mathf.Sqrt((dist*dist)-(minDist*minDist)));
                    //Mathf.Clamp(grounds[0].transform.position.x+xMove,-3.4f,3.4f);*/
                    grounds[i].transform.position = new Vector3(Mathf.Clamp(xMove,maxX+1f, -maxX-1f),2.4f,grounds[0].transform.position.z+minDist);
                    grounds[i].transform.eulerAngles = new Vector3(0, Random.Range(0,360), 0);
                }
                
                if(grounds[i].GetComponent<GroundScr>().places.Length>0){
                    for(int j=0; j<grounds[i].GetComponent<GroundScr>().places.Length-1; j++){
                        int rr = Random.Range(0,3);
                        if(rr==1 && !grounds[i].GetComponent<GroundScr>().poses[j]){//generate danger
                            if(GetComponent<MainController>().difficulty>0) {
                            int rand = Random.Range(0,dMeshes.Length);
                            
                            if(rand > 1){
                                grounds[i].GetComponent<GroundScr>().create(dMeshes[rand],j, GetComponent<MainController>().player);
                            } else{
                                grounds[i].GetComponent<GroundScr>().create(dMeshes[rand],j);
                            }
                        }
                        } else if(rr==2){//generate coins
                            int randC = Random.Range(0,8);
                            if(randC == 3 || randC==5){
                                grounds[i].GetComponent<GroundScr>().create(cMeshes[1],j);
                            } else if(randC == 6){
                                grounds[i].GetComponent<GroundScr>().create(cMeshes[2],j);
                            } else{
                                grounds[i].GetComponent<GroundScr>().create(cMeshes[0],j);
                            }
                        }
                    }
                }

            }
            if(GetComponent<MainController>().gameStart && !GetComponent<MainController>().lose && !GetComponent<MainController>().pause){
                grounds[i].transform.position = new Vector3(grounds[i].transform.position.x, grounds[i].transform.position.y, grounds[i].transform.position.z-GetComponent<MainController>().gameSpeed*Time.deltaTime);
            }
        }
    }
}
