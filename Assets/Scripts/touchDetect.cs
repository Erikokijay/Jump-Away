using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class touchDetect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    GameObject player = null, dirSprite = null, main;
    Rigidbody rb;   

    float startX, startY;
    public float forceScaler = 18, scaleSpeed = 5;

    [SerializeField]
    private float jForce = 15, maxJForce;//jump force
    void Start()
    {
        dirSprite.SetActive(true);
        dirSprite.GetComponent<RectTransform>().sizeDelta = new Vector2(0,0);
        player.GetComponent<PlayerController>().placer.SetActive(true);
        player.GetComponent<PlayerController>().placer.transform.localScale = new Vector3(0,6,1);
        rb = player.GetComponent<Rigidbody>();
        forceScaler = Screen.height*0.01f;
    }

    void Update(){
        if(!main.GetComponent<MainController>().gameStart)
        {
            scaler = false;
            player.transform.localScale = new Vector3(1,1,1);
        }
        if(!scaler){
            if(player.transform.localScale.y<1){
                player.transform.localScale = new Vector3(1,player.transform.localScale.y+scaleSpeed*Time.deltaTime/2,1);
            } else{
                player.transform.localScale = new Vector3(1,1,1);
            }
        } else{
           if(player.transform.localScale.y>0.7f){
                player.transform.localScale = new Vector3(1,player.transform.localScale.y-scaleSpeed*Time.deltaTime,1);
            } else{
                player.transform.localScale = new Vector3(1,0.7f,1);
            } 
        }
        
    }
    
    bool scaler = false;
    public void OnPointerDown(PointerEventData eventData)
    {
		startX = eventData.position.x;
        startY = eventData.position.y;
        forceScaler = Screen.height*0.01f;
        if(player.GetComponent<PlayerController>().grounded && !main.GetComponent<MainController>().lose && !main.GetComponent<MainController>().pause && player.GetComponent<PlayerController>().transform.position.y>=2f && main.GetComponent<MainController>().gameStart) 
        {
            if(main.GetComponent<MainController>().ctrlType == 0){
                player.GetComponent<PlayerController>().placer.transform.localScale = new Vector3(0,6,1);
                //player.GetComponent<PlayerController>().placer.SetActive(true);
            } else if(main.GetComponent<MainController>().ctrlType == 1){
                dirSprite.GetComponent<RectTransform>().anchoredPosition = new Vector2(startX, startY);
                dirSprite.GetComponent<RectTransform>().eulerAngles = new Vector3(45,0,AngleJ());
                dirSprite.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Clamp(Vector2.Distance(new Vector2(startX,startY),new Vector2(eventData.position.x,eventData.position.y)),0,maxJForce),Mathf.Clamp(Vector2.Distance(new Vector2(startX,startY),new Vector2(eventData.position.x,eventData.position.y)),0,maxJForce)/2);
                //dirSprite.SetActive(true);
            }
            player.GetComponent<Rigidbody>().isKinematic = true;
            scaler = true;
        } else if(!main.GetComponent<MainController>().gameStart && !main.GetComponent<MainController>().lose && !main.GetComponent<MainController>().pause)
        {
            if(main.GetComponent<MainController>().ctrlType == 0){
                player.GetComponent<PlayerController>().placer.transform.localScale = new Vector3(6,6,1);
                //player.GetComponent<PlayerController>().placer.SetActive(true);
            } else if(main.GetComponent<MainController>().ctrlType == 1){
                dirSprite.GetComponent<RectTransform>().anchoredPosition = new Vector2(startX, startY);
                dirSprite.GetComponent<RectTransform>().eulerAngles = new Vector3(45,0,AngleJ());
                dirSprite.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Clamp(Vector2.Distance(new Vector2(startX,startY),new Vector2(eventData.position.x,eventData.position.y)),0,maxJForce),Mathf.Clamp(Vector2.Distance(new Vector2(startX,startY),new Vector2(eventData.position.x,eventData.position.y)),0,maxJForce)/2);
                //dirSprite.SetActive(true);
            }
            main.GetComponent<MainController>().StartGame();
            dirSprite.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Clamp(Vector2.Distance(new Vector2(startX,startY),new Vector2(eventData.position.x,eventData.position.y)),0,maxJForce),Mathf.Clamp(Vector2.Distance(new Vector2(startX,startY),new Vector2(eventData.position.x,eventData.position.y)),0,maxJForce)/2);
            scaler = true;
        }
	}
    bool lastOr = true; //true - horizontal
    public void OnPointerUp(PointerEventData eventData)
    {
        if(main.GetComponent<MainController>().gameStart && !main.GetComponent<MainController>().lose && !main.GetComponent<MainController>().pause && player.GetComponent<PlayerController>().grounded && player.GetComponent<PlayerController>().transform.position.y>=2f ){
            if(main.GetComponent<MainController>().ctrlType == 0){
                player.GetComponent<PlayerController>().startPos = player.transform.position;
                player.GetComponent<Rigidbody>().isKinematic = false;
                jForce = Vector2.Distance(eventData.position, new Vector2(startX, startY))/(19f);
                if(jForce>maxJForce) jForce = maxJForce;
                rb.AddForce(new Vector3(Mathf.Cos(AngleJ()*Mathf.Deg2Rad)*jForce, jForce*1.35f, Mathf.Sin(AngleJ()*Mathf.Deg2Rad)*jForce), ForceMode.Impulse);
                if(jForce>0f) player.GetComponent<PlayerController>().Jump();
            } else if(main.GetComponent<MainController>().ctrlType == 1){
                player.GetComponent<PlayerController>().startPos = player.transform.position;
                player.GetComponent<Rigidbody>().isKinematic = false;
                jForce = Vector2.Distance(eventData.position, new Vector2(startX, startY))/(19f);
                if(jForce>maxJForce) jForce = maxJForce;
                rb.AddForce(new Vector3(Mathf.Cos(AngleJ()*Mathf.Deg2Rad)*jForce, jForce*1.35f, Mathf.Sin(AngleJ()*Mathf.Deg2Rad)*jForce), ForceMode.Impulse);
                if(jForce>0f) player.GetComponent<PlayerController>().Jump();
            }
            scaler = false;
        } else if(!main.GetComponent<MainController>().gameStart && !main.GetComponent<MainController>().lose){
            main.GetComponent<MainController>().StartGame();
        }
        dirSprite.GetComponent<RectTransform>().sizeDelta = new Vector2(0,0);
        player.GetComponent<PlayerController>().placer.transform.localScale = new Vector3(0,6,0);
        
	}
    public void OnDrag(PointerEventData eventData)
    {
        if(main.GetComponent<MainController>().gameStart && !main.GetComponent<MainController>().pause && !main.GetComponent<MainController>().lose && player.GetComponent<PlayerController>().grounded && player.GetComponent<PlayerController>().transform.position.y>=2f ) 
        {
            if(main.GetComponent<MainController>().ctrlType == 0){
                float dist = Vector2.Distance(eventData.position, new Vector2(startX, startY))/(19f);
                player.GetComponent<PlayerController>().transform.eulerAngles = new Vector3(0,90-AngleJ(),0);
                player.GetComponent<PlayerController>().placer.transform.localScale = new Vector3(Mathf.Clamp((dist/1.5f)/2f,0f,4.6f)*1.1f,6,1);
            } 
            else if(main.GetComponent<MainController>().ctrlType == 1){
                dirSprite.GetComponent<RectTransform>().anchoredPosition = new Vector2(startX, startY);
                dirSprite.GetComponent<RectTransform>().eulerAngles = new Vector3(45,0,AngleJ());
                player.GetComponent<PlayerController>().transform.eulerAngles = new Vector3(0,90-AngleJ(),0);
                dirSprite.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Clamp(Vector2.Distance(new Vector2(startX,startY),new Vector2(eventData.position.x,eventData.position.y))*2,0,maxJForce*forceScaler*2.2f),Mathf.Clamp(Vector2.Distance(new Vector2(startX,startY),new Vector2(eventData.position.x,eventData.position.y))*2,0,maxJForce*forceScaler*2)/2);
            }
            /*dirSprite.GetComponent<RectTransform>().anchoredPosition = new Vector2(startX, startY);
            dirSprite.GetComponent<RectTransform>().eulerAngles = new Vector3(45,0,AngleJ());
            dirSprite.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Clamp(Mathf.Sqrt((startX-eventData.position.x)*(startX-eventData.position.x)+(startY-eventData.position.y)*(startY-eventData.position.y))*2,0,maxJForce*forceScaler*2),/* Mathf.Clamp(Mathf.Sqrt((startX-eventData.position.x)*(startX-eventData.position.x)+(startY-eventData.position.y)*(startY-eventData.position.y)),0,maxJForce*forceScaler)maxJForce*forceScaler);
            //dirSprite.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Clamp(Vector2.Distance(new Vector2(startX,startY),new Vector2(eventData.position.x,eventData.position.y)),0,maxJForce*forceScaler),Mathf.Clamp(Vector2.Distance(new Vector2(startX,startY),new Vector2(eventData.position.x,eventData.position.y)),0,maxJForce*forceScaler)/2);
            dirSprite.SetActive(true);*/
        }
    }

    // Update is called once per frame
    
    float AngleJ(){//Угол 
		Vector3 mousePos=new Vector3();
        mousePos = Input.mousePosition;

        mousePos.x = startX - mousePos.x;
        mousePos.y = startY - mousePos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        
		return angle;
	} 
}
