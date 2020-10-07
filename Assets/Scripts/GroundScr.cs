using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScr : MonoBehaviour
{
    public int radius = 1;//количество вокселей от центра до крайней точки
    public Vector3[] places;
    public bool[] poses;//true - монеты, false - всё что угодно

    public void create(GameObject obj, int pl){
        GameObject that = Instantiate(obj, this.transform.position, transform.rotation);
        that.transform.parent = this.transform;
        that.transform.localPosition = new Vector3(places[pl].x, 0f, places[pl].z);
    } 
    public void create(GameObject obj, int pl, GameObject player){
        GameObject that = Instantiate(obj, this.transform.position, transform.rotation);
        that.GetComponent<DangerScr>().setPl(player);
        that.transform.parent = this.transform;
        that.transform.localPosition = new Vector3(places[pl].x, 0f, places[pl].z);
    } 
}
