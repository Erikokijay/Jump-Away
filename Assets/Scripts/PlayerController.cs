using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    // Start is called before the first frame update
    public bool grounded = true;
    public GameObject model, main, coinPlayer, placer, jumpPlace, particle;
    public Transform test;
    Rigidbody rb;
    float grX = 0f, grZ = 0f;
    AudioSource uSr;
    public AudioClip jumpClip, coinClip, loseClip, fallClip, boomClip;

    public Vector3 startPos, endPos = new Vector3 (0, 2.21f, 3.5f);

    public void Jump () {
        if (main.GetComponent<MainController> ().gameStart) {
            grounded = false;
            if (main.GetComponent<MainController> ().audio) {
                uSr.clip = jumpClip;
                uSr.Play ();
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        /*if(!grounded && collision.other.tag == "Ground" && collision.other.transform.position.y-0.5f <= transform.position.y) {
            grounded = true;
        }*/
    }

    void LoseAction () {
        particle.transform.position = this.transform.position;
        particle.GetComponent<ParticleSystem> ().Play ();
        //this.gameObject.SetActive (false);
        main.GetComponent<MainController> ().getAchiv (" CgkIo8msrrcEEAIQBw");
        this.gameObject.transform.position = new Vector3(0,-19f,0);
    }

    public GameObject lastGround;
    public float posed, mySpeed;
    void OnCollisionEnter (Collision collision) {
        if (!grounded && collision.other.tag == "Ground" && collision.other.transform.position.y - 0.25f <= transform.position.y && !isJump) {
            grounded = true;
            main.GetComponent<MainController> ().jumpForGame++;
            main.GetComponent<MainController> ().allJumps++;
            endPos = transform.position;
            posed = transform.position.z - collision.other.transform.position.z;
            lastGround = collision.other.gameObject;
            main.GetComponent<MainController> ().score += Mathf.RoundToInt (Vector3.Distance (lastGround.transform.position, endPos) *10);
            main.GetComponent<MainController> ().ScoreText.GetComponent<Text> ().text = main.GetComponent<MainController>().score.ToString ();
            if (main.GetComponent<MainController> ().jumpForGame % 12 == 0 && main.GetComponent<MainController> ().gameSpeed < 3f) {
                main.GetComponent<MainController> ().gameSpeed += 0.05f;
                main.GetComponent<MainController> ().saveAll(true);
            }
            print(Vector3.Distance (lastGround.transform.position, endPos));
            if (main.GetComponent<MainController> ().audio) {
                uSr.clip = fallClip;
                uSr.Play ();
            }
            main.GetComponent<MainController> ().getAchiv ("CgkIo8msrrcEEAIQAg");
            if (main.GetComponent<MainController> ().jumpForGame == 20) {
                main.GetComponent<MainController> ().getAchiv ("CgkIo8msrrcEEAIQBg");
            }
            if (main.GetComponent<MainController> ().jumpForGame == 50) {
                main.GetComponent<MainController> ().getAchiv ("CgkIo8msrrcEEAIQDA");
            }
            if (main.GetComponent<MainController> ().allJumps == 50) {
                main.GetComponent<MainController> ().getAchiv ("CgkIo8msrrcEEAIQBQ");
            }
        }
    }
    void OnTriggerEnter (Collider collision) {
        if (collision.tag == "Enemy" && !main.GetComponent<MainController> ().lose && collision.GetComponent<EnemyController> ().gameSt) {
            rb.velocity = new Vector3 (0, 0, 0);
            rb.isKinematic = true;
            collision.GetComponent<EnemyController> ().gameSt = false;
            collision.GetComponent<Rigidbody> ().isKinematic = true;
            collision.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
            main.GetComponent<MainController> ().EndGame ();
            if (main.GetComponent<MainController> ().audio) {
                uSr.clip = loseClip;
                uSr.Play ();
            }
            LoseAction ();
        } else if (collision.tag == "Danger" && !main.GetComponent<MainController> ().lose) {
            rb.velocity = new Vector3 (0, 0, 0);
            rb.isKinematic = true;
            main.GetComponent<MainController> ().EndGame ();
            if (main.GetComponent<MainController> ().audio) {
                uSr.clip = loseClip;
                uSr.Play ();
            }
            LoseAction ();
        } else if (collision.tag == "Coin") {
            main.GetComponent<MainController> ().moneyForGame += collision.GetComponent<CoindScr> ().bonus;
            main.GetComponent<MainController> ().allMoney += collision.GetComponent<CoindScr> ().bonus;
            main.GetComponent<MainController> ().CoinText.GetComponent<Text> ().text = main.GetComponent<MainController> ().allMoney.ToString ();
            if (main.GetComponent<MainController> ().audio) {
                coinPlayer.GetComponent<AudioSource> ().clip = coinClip;
                coinPlayer.GetComponent<AudioSource> ().Play ();
            }
            main.GetComponent<MainController> ().getAchiv (" CgkIo8msrrcEEAIQAw");
            if (main.GetComponent<MainController> ().allMoney >= 100) main.GetComponent<MainController> ().getAchiv (" CgkIo8msrrcEEAIQBA");
            if (main.GetComponent<MainController> ().moneyForGame >= 20) main.GetComponent<MainController> ().getAchiv ("CgkIo8msrrcEEAIQDQ");
            Destroy (collision.gameObject);
            main.GetComponent<MainController> ().saveAll(true);
        } else if (collision.tag == "Ground") {
            lastGround = collision.gameObject;
        }
    }
    bool isJump = false;
    Vector3 posJ;
    public void jumpTo (float j) {
        if (main.GetComponent<MainController> ().gameStart) {
            grounded = false;
            if (main.GetComponent<MainController> ().audio) {
                uSr.clip = jumpClip;
                uSr.Play ();
            }
        }
        posJ = jumpPlace.transform.position;
        isJump = true;
    }

    void Start () {
        rb = GetComponent<Rigidbody> ();
        uSr = GetComponent<AudioSource> ();
    }
    public float fallPos;
    void Update () {
        if (((transform.position.y < 0.25f && main.GetComponent<MainController> ().gameStart) || (transform.position.z < -4f && main.GetComponent<MainController> ().gameStart)) && !main.GetComponent<MainController> ().lose && !main.GetComponent<MainController> ().timerAl) { //lose
            main.GetComponent<MainController> ().EndGame ();
            fallPos = transform.position.z;
            if (main.GetComponent<MainController> ().audio) {
                uSr.clip = loseClip;
                uSr.Play ();
            }
        }
        if (transform.position.y < -20f) {
            rb.isKinematic = true;
            transform.position = new Vector3(0, -18f, 0);
        }
        if (isJump && (model.transform.localPosition.z < jumpPlace.transform.localPosition.z)) {
            model.transform.localPosition = new Vector3 (0, 0, model.transform.localPosition.z + Time.deltaTime * mySpeed);
            print ((jumpPlace.transform.localPosition.z) / (mySpeed / Time.deltaTime));
            GetComponent<MeshCollider> ().enabled = false;
        } else if (isJump && !grounded) {
            isJump = false;
            grounded = true;
            transform.position = jumpPlace.transform.position;
            model.transform.localPosition = new Vector3 (0, 0, 0);
            GetComponent<MeshCollider> ().enabled = false;
        }

        float r = Mathf.Clamp (transform.position.y / 2.4f, 0.8f, 1.5f);
        model.transform.localScale = new Vector3 (r, r, r);
    }
}