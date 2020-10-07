using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundGenerator : MonoBehaviour
{
    public GameObject[] decors;
    public GameObject main;
    void Start(){
        StartCoroutine(gen());
    }

    IEnumerator gen(){
        float timeWait = Random.Range(1f,5f);
        GameObject that = Instantiate(decors[Random.Range(0, decors.Length)]);
        int t = Random.Range(-2,2);
        if(t == 0) t=-1;
        else if(t<0) t = -1;
        else t = 1;
        that.GetComponent<DecorScr>().dirMe(t, main.GetComponent<MainController>().gameSpeed, this.gameObject);
        yield return new WaitForSeconds(timeWait);
        StartCoroutine(gen());
    }
    
}
