using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollerScr : MonoBehaviour
{
    // Start is called before the first frame update
    public int s, choosedID, nearId;
    public GameObject choseBtn, main;
    public RectTransform content;//передвигаемая часть
    public GameObject[] characterChoosers;
    float wPos, widthIt, prev, loc/*начальный размер выборки*/;
    Vector2 beginTouch, endTouch;//точки нажатий
    public bool aud;
    float scrollSpeed = 0;

    AudioSource src;
    public AudioClip change;

    void Awake()
    {
        src = GetComponent<AudioSource>();
        src.clip = change;
        if(Screen.width>Screen.height){
            s = Screen.height;
        } else{
            s = Screen.width;
        }
        for(int i = 0; i<characterChoosers.Length; i++){
            if(i==0){
                characterChoosers[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-s*0.2f);
                prev = 0;
            } else{
                prev +=s*0.25f;
                characterChoosers[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(prev,-s*0.2f);
            }
        }
        widthIt = characterChoosers[0].GetComponent<RectTransform>().sizeDelta.x;
        loc = characterChoosers[0].transform.localScale.x;
        wPos = characterChoosers[characterChoosers.Length-1].GetComponent<RectTransform>().anchoredPosition.x;
        content.anchoredPosition=new Vector2(-nearId*widthIt,0);
    }

    int nearest(float pos){
        if(pos>=0) {
            return 0;
        }
        else if(-pos>=wPos){
            return characterChoosers.Length-1;
        } else{
            //Debug.Log(wPos);
            //if(-pos>=s*0.4f)
            return Mathf.FloorToInt(-pos/(s*0.25f));
        }
    }

    bool isTouching;
    void FixedUpdate()
    {
        if(Input.touchCount>0 || Input.anyKey){
            if(!isTouching) {
                isTouching = true;
            }
        } else if((Input.touchCount==0 || !Input.anyKey) && isTouching){
            isTouching = false;
        }
        else{
            content.anchoredPosition=new Vector2(Mathf.SmoothStep(content.anchoredPosition.x, -nearId*s*0.25f,0.2f),0);
        }
        if(nearId!=nearest(content.anchoredPosition.x)){
            characterChoosers[nearId].transform.localScale = new Vector3(loc, loc, loc);
            characterChoosers[nearId].GetComponent<RectTransform>().localPosition = new Vector3(characterChoosers[nearId].GetComponent<RectTransform>().localPosition.x,characterChoosers[nearId].GetComponent<RectTransform>().localPosition.y,0);
            if(aud) src.Play();
        }
        nearId=nearest(content.anchoredPosition.x);
        if(nearId>characterChoosers.Length-1) nearId = characterChoosers.Length;
        
        characterChoosers[nearId].transform.localScale = new Vector3(Mathf.SmoothStep(loc, loc*2.5f, 0.4f), Mathf.SmoothStep(loc, loc*2.5f, 0.4f), Mathf.SmoothStep(loc, loc*2.5f, 0.4f));
        characterChoosers[nearId].GetComponent<RectTransform>().localPosition = new Vector3(characterChoosers[nearId].GetComponent<RectTransform>().localPosition.x,characterChoosers[nearId].GetComponent<RectTransform>().localPosition.y,-350);

        choseBtn.GetComponent<ButtonsScript>().Refr(nearId);
    }
}
