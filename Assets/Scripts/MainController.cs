using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;
using static GPGSManager.GPGSManager;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class Saves {
    public int allJumps, maxJumps, allMoney, maxScore, ctrlType, difficulty, currentLang, curentBtnChoose;
    public float gameSpeed;
    public bool adBoughten, music, audio, traned;
    public int[] chars;
}
public class MainController : MonoBehaviour {

    //Загружаемые данные
    public int allJumps, maxJumps, jumpForGame, allMoney = 20, moneyForGame, curentBtnChoose = 0, score, maxScore; //
    public bool gameStart = false, lose = false, pause = false;
    //#3DA8DF
    //Загружаемые
    public float gameSpeed = 2.4f, allGameSpeed;
    //Купленные персонажи
    public int[] chars;
    public GameObject[] allG;

    [SerializeField]
    public GameObject EndGroup, StartGroup, PauseGroup, ChooseGroup, Enemy, PauseButton, SettingsGroup, Seconder, MainInterface, OverBG, CloseBtn, Taper, GameGroup, ScoreText, MaxScoreText, CoinText, MoneyIcon, ContinueGruop, ClockCounter, GameReady, DifText, scroller;
    public GameObject player, saveManager;
    public bool audio = true, adBoughten, traned, music = true;
    public AudioSource aSr;
    public AudioClip[] musics;
    public int currentLang = 0, difficulty = 1, ctrlType = 0; //0- low, 1-medium, 2-hard
    public int loseCount = 0;
    public Mesh[] allChars;
    int scrSize;
    public Material mb;

    void ready () {
        GameReady.SetActive (false);
    }

    string svPath;
    public void saveAll (bool fast) {
        sv.adBoughten = this.adBoughten;
        sv.allJumps = this.allJumps;
        sv.difficulty = this.difficulty;
        sv.allMoney = this.allMoney;
        sv.audio = this.audio;
        sv.chars = this.chars;
        sv.music = this.music;
        sv.ctrlType = this.ctrlType;
        sv.curentBtnChoose = this.curentBtnChoose;
        sv.currentLang = this.currentLang;
        sv.maxScore = this.maxScore;
        sv.gameSpeed = this.allGameSpeed;
        sv.maxJumps = this.maxJumps;

        string saveData = JsonUtility.ToJson (sv);
        PlayerPrefs.SetString ("jaSav", saveData);

        if (!fast) {
            byte[] barray = Encoding.UTF8.GetBytes (saveData);
            WriteSaveData (barray);
        }
    }

    private void OnApplicationPause (bool pauseStatus) {
        if (pauseStatus) {
            saveAll (false);
        }
    }

    private void OnApplicationQuit () {
        saveAll (false);
    }

    public void GetAllData () {
        if (PlayerPrefs.HasKey ("jaSav") && PlayerPrefs.GetString("jaSav")!="" && PlayerPrefs.GetString("jaSav")!=null) {
            sv = JsonUtility.FromJson<Saves> (PlayerPrefs.GetString ("jaSav"));
            /*if (success) {
                ReadSaveData (DEFAULT_SAVE_NAME, (status, metadata) => {
                    if (status == GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.Success) {
                        string data = Encoding.UTF8.GetString (metadata, 0, metadata.Length - 1);
                        sv = JsonUtility.FromJson<Saves>(data);
                    } else {
                        sv = JsonUtility.FromJson<Saves> (PlayerPrefs.GetString ("jaSav"));
                    }
                });
            } else {*/

            this.adBoughten = sv.adBoughten;
            this.allJumps = sv.allJumps;
            this.difficulty = sv.difficulty;
            this.allMoney = sv.allMoney;
            this.audio = sv.audio;
            this.chars = sv.chars;
            this.music = sv.music;
            this.ctrlType = sv.ctrlType;
            this.curentBtnChoose = sv.curentBtnChoose;
            this.currentLang = sv.currentLang;
            this.maxScore = sv.maxScore;
            this.gameSpeed = sv.gameSpeed;
            this.allGameSpeed = sv.gameSpeed;
            this.maxJumps = sv.maxJumps;
        } else {
            saveAll (true);
        }

    }

    public void getAchiv (string id) {
        Social.ReportProgress (id, 100.0f, (bool succes) => {
            Debug.Log ("yoopy");
        });
    }
    Saves sv = new Saves();
    void Start () {
        /*Initialize(false);
        Auth((success)=>{

        });*/
        GetAllData ();
        if (ctrlType == 0) {
            ctrlImg.GetComponent<Image> ().sprite = ctrl1;
        } else if (ctrlType == 1) {
            ctrlImg.GetComponent<Image> ().sprite = ctrl2;
        }
        GetComponent<LangChanger> ().ChangeLang (currentLang);
        player.GetComponent<PlayerController> ().model.GetComponent<MeshFilter> ().mesh = allChars[curentBtnChoose];
        player.GetComponent<MeshCollider> ().sharedMesh = allChars[curentBtnChoose];
        ScoreText.GetComponent<Text> ().text = "0";
        CoinText.GetComponent<Text> ().text = allMoney.ToString ();
        aSr = GetComponent<AudioSource> ();
        allGameSpeed = gameSpeed;
        if (Screen.width > Screen.height) {
            last = false;
            scrSize = Screen.height;
            GetComponent<Camera> ().orthographicSize = 4.3f;
            MainInterface.GetComponent<RectTransform> ().anchorMin = new Vector2 (0.3f, 0f);
            MainInterface.GetComponent<RectTransform> ().anchorMax = new Vector2 (0.7f, 1f);
            CloseBtn.GetComponent<RectTransform> ().sizeDelta = new Vector2 (scrSize * 0.15f, scrSize * 0.15f);
            CloseBtn.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (Screen.width / 2 - scrSize * 0.15f, Screen.height / 2 - scrSize * 0.15f);
        } else {
            last = true;
            scrSize = Screen.width;
            GetComponent<Camera> ().orthographicSize = 7.5f;
            MainInterface.GetComponent<RectTransform> ().anchorMin = new Vector2 (0f, 0f);
            MainInterface.GetComponent<RectTransform> ().anchorMax = new Vector2 (1f, 1f);
            CloseBtn.GetComponent<RectTransform> ().sizeDelta = new Vector2 (scrSize * 0.15f, scrSize * 0.15f);
            CloseBtn.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (scrSize / 2 - scrSize * 0.15f, Screen.height / 2 - scrSize * 0.15f);
        }
        scroller.GetComponent<ScrollerScr> ().nearId = curentBtnChoose;
        scroller.GetComponent<ScrollerScr> ().choosedID = curentBtnChoose;
        scroller.GetComponent<ScrollerScr> ().content.anchoredPosition = new Vector2 (-curentBtnChoose * scrSize * 0.25f, 0);
        MaxScoreText.GetComponent<Text> ().text = "Max Score: " + maxScore.ToString ();
        Taper.GetComponent<RectTransform> ().sizeDelta = new Vector2 (scrSize * 0.2f, scrSize * 0.2f);

    }
    bool last = true; //true -horizontal
    void Update () {
        if (timerAl) ClockCounter.GetComponent<Image> ().fillAmount -= (1f / 5f) * Time.deltaTime;
        mb.SetTextureOffset ("_MainTex", new Vector2 (mb.GetTextureOffset ("_MainTex").x + Time.deltaTime / 50, mb.GetTextureOffset ("_MainTex").y));
        if (gameStart && !lose && !pause) {
            for (int i = 0; i < allG.Length; i++) {
                if (i != 2) {
                    allG[i].transform.position = new Vector3 (allG[i].transform.position.x, allG[i].transform.position.y, allG[i].transform.position.z - gameSpeed * Time.deltaTime);
                } else if (allG[i].transform.position.z >= -13f) {
                    allG[i].transform.position = new Vector3 (allG[i].transform.position.x, allG[i].transform.position.y, allG[i].transform.position.z - gameSpeed * Time.deltaTime);
                } else {
                    Enemy.transform.position = new Vector3 (0, 2.45f, -6);
                }

            }
            if (player.transform.position.z >= 4.5f) {
                gameSpeed = (player.transform.position.z - 3.5f) * 2.5f;
            } else {
                gameSpeed = allGameSpeed;
            }
            mb.SetTextureOffset ("_MainTex", new Vector2 (mb.GetTextureOffset ("_MainTex").x, mb.GetTextureOffset ("_MainTex").y + Time.deltaTime * gameSpeed / 25));
        }
        if (Screen.width > Screen.height) {
            if (!last) {
                GetComponent<Camera> ().orthographicSize = 4.3f;
                last = true;
                MainInterface.GetComponent<RectTransform> ().anchorMin = new Vector2 (0.3f, 0f);
                MainInterface.GetComponent<RectTransform> ().anchorMax = new Vector2 (0.7f, 1f);
                CloseBtn.GetComponent<RectTransform> ().sizeDelta = new Vector2 (scrSize * 0.15f, scrSize * 0.15f);
                CloseBtn.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (Screen.width / 2 - scrSize * 0.1f, Screen.height / 2 - scrSize * 0.1f);
                PauseButton.GetComponent<RectTransform> ().sizeDelta = new Vector2 (scrSize * 0.15f, scrSize * 0.15f);
                PauseButton.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (Screen.width / 2 - scrSize * 0.1f, Screen.height / 2 - scrSize * 0.1f);
                MoneyIcon.GetComponent<RectTransform> ().sizeDelta = new Vector2 (scrSize * 0.08f, scrSize * 0.08f);
                MoneyIcon.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-Screen.width / 2 + scrSize * 0.18f, Screen.height / 2 - scrSize * 0.1f);
                CoinText.GetComponent<RectTransform> ().sizeDelta = new Vector2 (scrSize * 0.6f, scrSize * 0.05f);
                CoinText.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-Screen.width / 2 + scrSize * 0.55f, Screen.height / 2 - scrSize * 0.1f);
                GetComponent<GroundGenerate> ().maxX = GetComponent<Camera> ().ViewportToWorldPoint (new Vector3 (0, 0, 0)).x;
                Taper.GetComponent<RectTransform> ().sizeDelta = new Vector2 (scrSize * 0.2f, scrSize * 0.2f);
            }
        } else {
            if (last) {
                GetComponent<Camera> ().orthographicSize = 7.5f;
                last = false;
                MainInterface.GetComponent<RectTransform> ().anchorMin = new Vector2 (0f, 0f);
                MainInterface.GetComponent<RectTransform> ().anchorMax = new Vector2 (1f, 1f);
                CloseBtn.GetComponent<RectTransform> ().sizeDelta = new Vector2 (scrSize * 0.15f, scrSize * 0.15f);
                CloseBtn.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (scrSize / 2 - scrSize * 0.13f, Screen.height / 2 - scrSize * 0.13f);
                PauseButton.GetComponent<RectTransform> ().sizeDelta = new Vector2 (scrSize * 0.15f, scrSize * 0.15f);
                PauseButton.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (scrSize / 2 - scrSize * 0.13f, Screen.height / 2 - scrSize * 0.13f);
                MoneyIcon.GetComponent<RectTransform> ().sizeDelta = new Vector2 (scrSize * 0.08f, scrSize * 0.08f);
                MoneyIcon.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-Screen.width / 2 + scrSize * 0.1f, Screen.height / 2 - scrSize * 0.1f);
                CoinText.GetComponent<RectTransform> ().sizeDelta = new Vector2 (scrSize * 0.6f, scrSize * 0.05f);
                CoinText.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-Screen.width / 2 + scrSize * 0.46f, Screen.height / 2 - scrSize * 0.1f);
                GetComponent<GroundGenerate> ().maxX = GetComponent<Camera> ().ViewportToWorldPoint (new Vector3 (0, 0, 0)).x;
                Taper.GetComponent<RectTransform> ().sizeDelta = new Vector2 (scrSize * 0.2f, scrSize * 0.2f);
            }
        }
        if (music) {
            if (!aSr.isPlaying) {
                aSr.clip = musics[UnityEngine.Random.Range (0, musics.Length - 1)];
                aSr.Play ();
            }
        } else if (aSr.isPlaying) {
            aSr.Pause ();
        }
    }

    public void StartGame () {
        if (gameStart == false) {
            gameStart = true;
            StartGroup.SetActive (false);
            PauseButton.SetActive (true);
            GameGroup.SetActive (true);
            Enemy.GetComponent<EnemyController> ().gameSt = true;
        }
    }
    public GameObject BonusBtn;
    public void Choose () { //выбор персонажа
        ChooseGroup.SetActive (true);
        StartGroup.SetActive (false);
        gameStart = false;
        BonusBtn.GetComponent<Button> ().interactable = getAd ();
        OverBG.SetActive (true);
        CloseBtn.SetActive (true);
    }

    public void Pause () {
        PauseButton.SetActive (false);
        GameGroup.SetActive (false);
        PauseGroup.SetActive (true);
        pause = true;
        OverBG.SetActive (true);
    }

    public void BonusCoins () {
        if (GetComponent<AdsManager> ().isLoaded ("rewardedVideo")) {
            GetComponent<AdsManager> ().showMe ("rewardedVideo");
            GetComponent<AdsManager> ().bn = true;
            allMoney += 10;
            CoinText.GetComponent<Text> ().text = allMoney.ToString ();
        }
    }
    bool dbl = false;
    public void DoubleCoins () {
        if (GetComponent<AdsManager> ().isLoaded ("rewardedVideo") && !dbl) {
            GetComponent<AdsManager> ().showMe ("rewardedVideo");
            allMoney += moneyForGame;
            GetComponent<AdsManager> ().bn = true;
            CoinText.GetComponent<Text> ().text = allMoney.ToString ();
            dbl = true;
        }
        doubleCoin.GetComponent<Button> ().interactable = false;
    }

    public void Share () {
        StartCoroutine (TakeSSAndShare ());
    }

    private IEnumerator TakeSSAndShare () {
        yield return new WaitForEndOfFrame ();

        Texture2D ss = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply ();

        string filePath = Path.Combine (Application.temporaryCachePath, "shared_img.png");
        File.WriteAllBytes (filePath, ss.EncodeToPNG ());
        if (score != 1) {
            new NativeShare ().AddFile (filePath).SetSubject ("Jump Away").SetText ("Hey, I scored " + score.ToString () + " points. Try to score more points https://play.google.com/store/apps/details?id=com.kijay.jumpaway").Share ();
        } else {
            new NativeShare ().AddFile (filePath).SetSubject ("Jump Away").SetText ("Hey, I scored " + score.ToString () + " point. Try to score more points https://play.google.com/store/apps/details?id=com.kijay.jumpaway").Share ();
        }

        // Share on WhatsApp only, if installed (Android only)
        //if( NativeShare.TargetExists( "com.whatsapp" ) )
        //	new NativeShare().AddFile( filePath ).SetText( "Hello world!" ).SetTarget( "com.whatsapp" ).Share();
    }

    public void Settings () {
        SettingsGroup.SetActive (true);
        gameStart = false;
        OverBG.SetActive (true);
        CloseBtn.SetActive (true);
        StartGroup.SetActive (false);
    }

    public void Leaders () {
        Social.ShowLeaderboardUI();
    }

    public bool getAd () {
        return GetComponent<AdsManager> ().isLoaded ("rewardedVideo");
    }

    public GameObject videoContinuer, castle;
    public Text adPrice;
    public void GameContinue (bool forMoney) {
        print (loseCount);
        if (forMoney) {
            if (loseCount == 1 && allMoney - 20 >= 0) {
                allMoney -= 20;
                CoinText.GetComponent<Text> ().text = allMoney.ToString ();
            } else if (loseCount == 2 && allMoney - 50 >= 0) {
                allMoney -= 50;
                CoinText.GetComponent<Text> ().text = allMoney.ToString ();
            } else if (loseCount == 3 && allMoney - 100 >= 0) {
                allMoney -= 100;
                CoinText.GetComponent<Text> ().text = allMoney.ToString ();
            }
            saveAll (true);
        } else {

        }
        player.SetActive (true);
        PauseGroup.SetActive (false);
        PauseButton.SetActive (false);
        GameGroup.SetActive (false);
        EndGroup.SetActive (false);
        Seconder.SetActive (true);
        OverBG.SetActive (false);
        ContinueGruop.SetActive (false);
        StartCoroutine (cont ());
        GetComponent<GroundGenerate> ().continueBacker (player.GetComponent<PlayerController> ().lastGround.transform.position.z - 5f);
        player.GetComponent<PlayerController> ().grounded = true;
        int i = 0;
        foreach (Transform child in player.GetComponent<PlayerController> ().lastGround.transform) {
            if (i != 0) GameObject.Destroy (child.gameObject);
            i++;
        }
        if (!GetComponent<GroundGenerate> ().first) castle.transform.position = new Vector3 (0, 2.4f, 1.7f);
        player.GetComponent<PlayerController> ().lastGround.transform.position = new Vector3 (0, 2.4f, 5f);
        player.transform.position = new Vector3 (0, 2.4f, 5f);
        Enemy.GetComponent<EnemyController> ().gameSt = false;
        Enemy.transform.position = new Vector3 (0, 2.32f, -6);
        Enemy.GetComponent<EnemyController> ().grounded = true;
        StopCoroutine (countTimeAlive ());
        alSeconds = 5;
    }

    public void Continue () {
        StartCoroutine (cont ());
        PauseGroup.SetActive (false);
        PauseButton.SetActive (false);
        GameGroup.SetActive (false);
        EndGroup.SetActive (false);
        Seconder.SetActive (true);
        OverBG.SetActive (false);
    }

    public void playAdVideo () {
        GetComponent<AdsManager> ().showMe ("rewardedVideo");
    }

    public GameObject ctrlImg;
    public Sprite ctrl1, ctrl2;
    public void changeCtrl () {
        if (ctrlType == 0) {
            ctrlType = 1;
            ctrlImg.GetComponent<Image> ().sprite = ctrl2;
        } else if (ctrlType == 1) {
            ctrlType = 0;
            ctrlImg.GetComponent<Image> ().sprite = ctrl1;
        }
        saveAll (true);
    }

    int alSeconds = 5;
    public bool timerAl = false;
    public IEnumerator countTimeAlive () {
        if (!pause) {
            if (!aliving) {
                if (loseCount == 1) {
                    adPrice.text = "20";
                } else if (loseCount == 2) {
                    adPrice.text = "50";
                } else if (loseCount == 3) {
                    adPrice.text = "100";
                }
                videoContinuer.GetComponent<Button> ().interactable = getAd ();
                timerAl = true;
                PauseButton.SetActive (false);
                GameGroup.SetActive (false);
                int n = 0;
                if (loseCount > 1) n = -1;
                else n = 0;
                if (alSeconds > n) {
                    ContinueGruop.SetActive (true);
                    yield return new WaitForSeconds (1);
                    alSeconds--;
                    StartCoroutine (countTimeAlive ());
                } else {
                    alSeconds = 5;
                    loseCount = 4;
                    EndGame ();
                    ContinueGruop.SetActive (false);
                    timerAl = false;
                    ClockCounter.GetComponent<Image> ().fillAmount = 1;
                }
            } else {
                timerAl = false;
                ClockCounter.GetComponent<Image> ().fillAmount = 1;
            }
        }
    }
    int seconds = 3;
    public bool aliving = false;
    IEnumerator cont () {
        lose = true;
        aliving = true;
        if (seconds > 0 && (pause || lose)) {
            Seconder.GetComponent<Text> ().text = seconds.ToString ();
            yield return new WaitForSeconds (1);
            seconds--;
            StartCoroutine (cont ());
        } else if (seconds == 0) {
            Enemy.GetComponent<EnemyController> ().timer = 0;
            pause = false;
            lose = false;
            aliving = false;
            Seconder.GetComponent<Text> ().text = "Start!";
            PauseButton.SetActive (true);
            GameGroup.SetActive (true);
            Enemy.GetComponent<EnemyController> ().gameSt = true;
            yield return new WaitForSeconds (1);
            Seconder.SetActive (false);
            seconds = 3;
        }
    }

    public void goToHome () {
        if (!lose && getAd () && !adBoughten) GetComponent<AdsManager> ().showMe ("rewardedVideo");
        lose = false;
        dbl = false;
        doubleCoin.GetComponent<Button> ().interactable = true;
        gameStart = false;
        pause = false;
        score = 0;
        moneyForGame = 0;
        EndGroup.SetActive (false);
        PauseButton.SetActive (false);
        GameGroup.SetActive (false);
        StartGroup.SetActive (true);
        PauseGroup.SetActive (false);
        OverBG.SetActive (false);
        player.transform.position = new Vector3 (0, 2.4f, 3.5f);
        castle.transform.position = new Vector3 (0, 2.4f, 0.3f);
        player.SetActive (true);
        player.GetComponent<PlayerController> ().grounded = true;
        player.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
        player.GetComponent<Rigidbody> ().isKinematic = false;
        player.GetComponent<PlayerController> ().transform.eulerAngles = new Vector3 (0, 0, 0);
        GetComponent<GroundGenerate> ().genNow (false);
        Enemy.transform.position = new Vector3 (1f, 2.5f, -5f);
        Enemy.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
        Enemy.GetComponent<Rigidbody> ().isKinematic = false;
        Enemy.GetComponent<EnemyController> ().mayJump = false;
        Enemy.GetComponent<EnemyController> ().gameSt = false;
        Enemy.transform.position = new Vector3 (0, 2.32f, -6);
        Enemy.GetComponent<EnemyController> ().grounded = true;
    }

    public void CloseAll () {
        OverBG.SetActive (false);
        StartGroup.SetActive (true);
        EndGroup.SetActive (false);
        PauseButton.SetActive (false);
        GameGroup.SetActive (false);
        PauseGroup.SetActive (false);
        SettingsGroup.SetActive (false);
        ChooseGroup.SetActive (false);
        CloseBtn.SetActive (false);
    }

    public GameObject endImg, particlEnd, sounder, forMoney, doubleCoin;
    public Text endText;
    public Sprite endSpr, highScSpr;
    public AudioClip winClip;
    public void EndGame () {
        if (loseCount < 3 && !lose) {
            loseCount++;
            if (getAd ()) {
                videoContinuer.GetComponent<Button> ().interactable = true;
                StartCoroutine (countTimeAlive ());
                if (loseCount == 1 && allMoney - 20 >= 0) {
                    forMoney.GetComponent<Button> ().interactable = true;
                    adPrice.text = "20";
                } else if (loseCount == 2 && allMoney - 50 >= 0) {
                    forMoney.GetComponent<Button> ().interactable = true;
                    adPrice.text = "50";
                } else if (loseCount == 3 && allMoney - 100 >= 0) {
                    forMoney.GetComponent<Button> ().interactable = true;
                    adPrice.text = "100";
                } else {
                    forMoney.GetComponent<Button> ().interactable = false;
                }
            } else {
                videoContinuer.GetComponent<Button> ().interactable = false;
                if (loseCount == 1 && allMoney - 20 >= 0) {
                    forMoney.GetComponent<Button> ().interactable = true;
                    StartCoroutine (countTimeAlive ());
                    adPrice.text = "20";
                } else if (loseCount == 2 && allMoney - 50 >= 0) {
                    forMoney.GetComponent<Button> ().interactable = true;
                    StartCoroutine (countTimeAlive ());
                    adPrice.text = "50";
                } else if (loseCount == 3 && allMoney - 100 >= 0) {
                    forMoney.GetComponent<Button> ().interactable = true;
                    StartCoroutine (countTimeAlive ());
                    adPrice.text = "100";
                } else {
                    forMoney.GetComponent<Button> ().interactable = false;
                    loseCount = 4;
                    EndGame ();
                }
            }
            OverBG.SetActive (true);
        } else {
            doubleCoin.GetComponent<Button> ().interactable = getAd ();
            loseCount = 0;
            gameStart = false;
            pause = false;
            OverBG.SetActive (true);
            EndGroup.SetActive (true);
            PauseButton.SetActive (false);
            GameGroup.SetActive (false);
            PauseGroup.SetActive (false);
            if (jumpForGame > maxJumps) {
                maxJumps = jumpForGame;
                if (maxJumps >= 50) getAchiv ("CgkIo8msrrcEEAIQBQ");
            }
            if (score > maxScore) {
                maxScore = score;
                MaxScoreText.GetComponent<Text> ().text = "Max Score: " + maxScore.ToString ();
                endText.text = "New Highscore";
                endImg.GetComponent<Image> ().sprite = highScSpr;
                particlEnd.GetComponent<ParticleSystem> ().Play ();
                if (audio) {
                    sounder.GetComponent<AudioSource> ().clip = winClip;
                    sounder.GetComponent<AudioSource> ().Play ();
                }
                Social.ReportScore (score, "CgkIo8msrrcEEAIQAQ", (bool succes) => {
                    Debug.Log ("Ye");
                });
                //getAchiv("CgkIo8msrrcEEAIQEA");
            } else {
                endText.text = "Game Over";
                endImg.GetComponent<Image> ().sprite = endSpr;
            }
            player.transform.localScale = new Vector3 (1, 1, 1);
            ScoreText.GetComponent<Text> ().text = "0";
            saveAll (false);
            Enemy.GetComponent<EnemyController> ().gameSt = false;
            player.GetComponent<PlayerController> ().grounded = true;
        }
        lose = true;
    }

}