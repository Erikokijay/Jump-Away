using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField]
    public float dist, jumpF, jumpVerticalF = 5, jScaler = 1f;
    
    public GameObject main, player;

    public bool grounded = false, mayJump = false, gameSt = false;
    Rigidbody rb;
    public float jumpSpeed;
    public float timer;

    void Start(){
        transform.eulerAngles = new Vector3(0, AngleJ(),0); 
        rb = GetComponent<Rigidbody>();
    }
    private void Update() {
        //
        if(gameSt){
            timer+=Time.deltaTime;
            if(timer>jumpSpeed){
                timer = 0;
                mayJump = true;
            }
            if(mayJump && grounded) Jump();
        }
        if(transform.position.y<-15f){
            transform.position = new Vector3(0,2.45f,-6);
            grounded = true;
            rb.velocity = new Vector3(0, 0, 0);
            rb.isKinematic = true;
            if(Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.transform.position.x, player.transform.position.z)) <= 2) main.GetComponent<MainController>().getAchiv("CgkIo8msrrcEEAIQCA");
        }
    }   
    void OnCollisionStay(Collision collision)
    {
        if(collision.other.tag == "Ground" && rb.velocity.magnitude <=1 && !grounded) {
            grounded = true;
            mayJump = false;
            timer = 0;
            rb.isKinematic = true;
        }
    }

    void Jump(){
        if(main.GetComponent<MainController>().gameStart && !main.GetComponent<MainController>().pause && !main.GetComponent<MainController>().lose && main.GetComponent<MainController>().difficulty == 2){
            rb.isKinematic = false;
            jumpF = Vector3.Distance(transform.position, player.transform.position);
            jumpF *= jScaler;
            rb.AddForce(new Vector3(Mathf.Cos((90-AngleJ())*Mathf.Deg2Rad)*jumpF, Mathf.Clamp(jumpF,0,jumpVerticalF), Mathf.Sin((90-AngleJ())*Mathf.Deg2Rad)*jumpF), ForceMode.Impulse);
            transform.eulerAngles = new Vector3(0, AngleJ(),0); 
            grounded = false;
            mayJump = false;
        }
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
