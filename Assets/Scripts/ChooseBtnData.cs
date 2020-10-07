using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseBtnData : MonoBehaviour
{
    void Start(){
        if(Screen.width>Screen.height){
            transform.localScale = new Vector3(Screen.height*0.4f,Screen.height*0.4f,Screen.height*0.4f);
        } else{
            transform.localScale = new Vector3(Screen.width*0.4f,Screen.width*0.4f,Screen.width*0.4f);
        }
        
    }
}
