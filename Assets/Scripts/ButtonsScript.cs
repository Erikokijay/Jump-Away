using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsScript : MonoBehaviour
{
    public GameObject main, scroller, txt, clicker;
    MainController mS;
    AudioSource aSr;
    public Sprite on, onCl, off, offCl;
    public AudioClip clickClip, buyClip, noneClip;

    public void Refr(int near){
        if(mS.chars[Mathf.Clamp(near,0,mS.chars.Length)]>0){
            if(mS.allMoney-mS.chars[Mathf.Clamp(near,0,mS.chars.Length)]>=0 || mS.chars[Mathf.Clamp(near,0,mS.chars.Length)]>1000 ){
                GetComponent<Button>().interactable = true;
                GetComponent<Image>().color = Color.gray;//######
            } else{
                //GetComponent<Button>().interactable = false;
                GetComponent<Image>().color = Color.red;
            }
            if(mS.chars[Mathf.Clamp(near,0,mS.chars.Length)]<=1000){
                txt.GetComponent<Text>().text = mS.chars[Mathf.Clamp(near,0,mS.chars.Length)].ToString();
            } else{
                int pr = mS.chars[Mathf.Clamp(near,0,mS.chars.Length)]-1000;
                txt.GetComponent<Text>().text = pr.ToString()+" $";
            }
            
            //txt.GetComponent<Image>().color = Color.white;
        } else if(mS.chars[Mathf.Clamp(near,0,mS.chars.Length)]==0){
            txt.GetComponent<Text>().text = "Choose";
            GetComponent<Button>().interactable = true;
            GetComponent<Image>().color = Color.gray;
        } else if(mS.chars[Mathf.Clamp(near,0,mS.chars.Length)] == -1){
            txt.GetComponent<Text>().text = "Choosed";
            GetComponent<Button>().interactable = true;
            GetComponent<Image>().color = Color.white;
        }
    }

    SpriteState st;
    public void AudChange(bool t){
        if(t) mS.audio = !mS.audio;
        if(mS.audio == true) {
            GetComponent<Image>().sprite = on;
            st.pressedSprite = onCl;
            GetComponent<Button>().spriteState = st;
        }
        else {
            GetComponent<Image>().sprite = off;
            st.pressedSprite = offCl;
            GetComponent<Button>().spriteState = st;
        }
        mS.saveAll(true);
    }
    public void MusicChange(bool t){
        if(t) mS.music = !mS.music;
        if(mS.music == true) {
            if(!mS.aSr.isPlaying) mS.aSr.Play();
            GetComponent<Image>().sprite = on;
            st.pressedSprite = onCl;
            GetComponent<Button>().spriteState = st;
        }
        else {
            mS.aSr.Pause();
            GetComponent<Image>().sprite = off;
            st.pressedSprite = offCl;
            GetComponent<Button>().spriteState = st;
        }
        mS.saveAll(true);
    }

    void Start(){
        aSr = clicker.GetComponent<AudioSource>();
        mS = main.GetComponent<MainController>();
        if(this.gameObject.name == "audioCheck") AudChange(false);
        if(this.gameObject.name == "musicCheck") MusicChange(false);
        if(this.gameObject.name == "BuyBtn"){
            Refr(mS.curentBtnChoose);
            /*if(Screen.width>Screen.height){
                GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height*0.6f, Screen.height*0.15f);
                GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-Screen.height*0.5f+ Screen.height*0.1f);
            } else{
                GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.6f, Screen.width*0.15f);
                GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-Screen.width*0.5f+ Screen.height*0.1f);
            }*/
        }
        if(this.gameObject.name == "Dificulty"){
            if(mS.difficulty==0){
                GetComponent<Image>().color = new Color(135f/255f, 255f/255f,130f/255f);
                mS.DifText.GetComponent<Text>().text = "Low";
            } else if(mS.difficulty==1){
                GetComponent<Image>().color = new Color(255f/255f, 175f/255f, 15f/255f);
                mS.DifText.GetComponent<Text>().text = "Medium";
            } else{
                GetComponent<Image>().color = new Color(255f/255f, 0, 0);
                mS.DifText.GetComponent<Text>().text = "Hard";
            }
        }
    }
    void changeDif(){
        if(mS.difficulty<2)
        {
            mS.difficulty++;
            if(mS.difficulty==1){
                GetComponent<Image>().color = new Color(255f/255f, 175f/255f, 15f/255f);
                mS.DifText.GetComponent<Text>().text = "Medium";
            } else{
                GetComponent<Image>().color = new Color(255f/255f, 0, 0);
                mS.DifText.GetComponent<Text>().text = "Hard";
            } 
        }
        else {
            mS.difficulty = 0;
            GetComponent<Image>().color = new Color(135f/255f, 255f/255f,130f/255f);
            mS.DifText.GetComponent<Text>().text = "Low";
        }
        mS.saveAll(true);
    }
    void charsChecker(){
        string f = "";
        for(int i=0; i<mS.chars.Length; i++){
            if(i<mS.chars.Length)
                f+=mS.chars[i].ToString()+",";
            else f+=mS.chars[i].ToString();
        }
        mS.saveAll(false);
    }
    public void Click(){
        if(this.gameObject.name == "BuyBtn"){
            if(mS.allMoney-mS.chars[Mathf.Clamp(scroller.GetComponent<ScrollerScr>().nearId,0,mS.chars.Length)]>=0 && mS.chars[Mathf.Clamp(scroller.GetComponent<ScrollerScr>().nearId,0,mS.chars.Length)]<=1000){//for game money
                if(mS.chars[Mathf.Clamp(scroller.GetComponent<ScrollerScr>().nearId,0,mS.chars.Length)]>0){//buying now
                    mS.player.GetComponent<PlayerController>().model.GetComponent<MeshFilter>().mesh = mS.allChars[Mathf.Clamp(scroller.GetComponent<ScrollerScr>().nearId,0,mS.chars.Length-1)];
                    mS.player.GetComponent<MeshCollider>().sharedMesh = mS.allChars[Mathf.Clamp(scroller.GetComponent<ScrollerScr>().nearId,0,mS.chars.Length-1)];
                    mS.allMoney-=mS.chars[Mathf.Clamp(scroller.GetComponent<ScrollerScr>().nearId,0,mS.chars.Length-1)];
                    mS.chars[Mathf.Clamp(scroller.GetComponent<ScrollerScr>().nearId,0,mS.chars.Length-1)] = -1;
                    mS.chars[mS.curentBtnChoose] = 0;
                    mS.curentBtnChoose = Mathf.Clamp(scroller.GetComponent<ScrollerScr>().nearId,0,mS.chars.Length-1);
                    txt.GetComponent<Text>().text = "Choosed";//-1 = choosed, 0 = can choose, >0 = buy
                    mS.CoinText.GetComponent<Text>().text = mS.allMoney.ToString();
                    GetComponent<Image>().color = Color.white;
                    if(mS.audio){
                        aSr.clip = buyClip;
                        aSr.Play();
                    }
                    mS.getAchiv("CgkIo8msrrcEEAIQCQ");
                    //mS.SaveChars();
                } else if(mS.chars[Mathf.Clamp(scroller.GetComponent<ScrollerScr>().nearId,0,mS.chars.Length)]==0){//can choose
                    mS.player.GetComponent<PlayerController>().model.GetComponent<MeshFilter>().mesh = mS.allChars[Mathf.Clamp(scroller.GetComponent<ScrollerScr>().nearId,0,mS.chars.Length-1)];
                    mS.player.GetComponent<MeshCollider>().sharedMesh = mS.allChars[Mathf.Clamp(scroller.GetComponent<ScrollerScr>().nearId,0,mS.chars.Length-1)];
                    mS.chars[Mathf.Clamp(scroller.GetComponent<ScrollerScr>().nearId,0,mS.chars.Length-1)] = -1;
                    mS.chars[mS.curentBtnChoose] = 0;
                    mS.curentBtnChoose = Mathf.Clamp(scroller.GetComponent<ScrollerScr>().nearId,0,mS.chars.Length-1);
                    txt.GetComponent<Text>().text = "Choosed";//-1 = choosed, 0 = can choose, >0 = buy
                    GetComponent<Image>().color = Color.white;
                    if(mS.audio){
                        aSr.clip = clickClip;
                        aSr.Play();
                    }
                } else{
                    if(mS.audio){
                        aSr.clip = clickClip;
                        aSr.Play();
                    }
                }
                mS.saveAll(false);
                charsChecker();
                //mS.player.GetComponent<MeshCollider>().mesh = mS.allChars[Mathf.Clamp(scroller.GetComponent<ScrollerScr>().nearId,0,mS.chars.Length-1)];
            } else if(mS.chars[Mathf.Clamp(scroller.GetComponent<ScrollerScr>().nearId,0,mS.chars.Length)]>1000){//for real money
                mS.getAchiv("CgkIo8msrrcEEAIQDw");
                main.GetComponent<IAPManager>().BuyNonConsumable(mS.scroller.GetComponent<ScrollerScr>().characterChoosers[Mathf.Clamp(scroller.GetComponent<ScrollerScr>().nearId,0,mS.chars.Length)].name.ToLower());
                charsChecker();
                main.GetComponent<MainController>().saveAll(false);
            }
             else{//have no money
                if(mS.audio){
                    aSr.clip = noneClip;
                    aSr.Play();
                }
            }
        } else{
            switch(this.gameObject.name){
                case "PauseBtn" : mS.Pause(); break;
                case "CharacterChooseBtn" : mS.Choose(); break;
                case "HomeBtn" : mS.goToHome(); break;
                case "Close": mS.CloseAll(); break;
                case "PauseBreak": mS.Continue(); break;
                case "DoubleCoins": mS.DoubleCoins(); break;
                case "ShareBtn": mS.Share(); break;
                case "LeaderboardBtn": mS.Leaders(); break;
                case "SettingsBtn": mS.Settings(); break;
                case "BonusBtn": mS.BonusCoins(); break;
                case "audioCheck": AudChange(true); break;
                case "musicCheck": MusicChange(true); break;
                case "ContinueBtn": mS.Continue(); break;
                case "GameContinueBtn": if(GetComponent<Button>().interactable) mS.playAdVideo(); break;
                case "ForMoneyContinueBtn": mS.GameContinue(true);break;
                case "ControlChanger": mS.changeCtrl();break;
                case "Dificulty": changeDif(); break;
                case "AdBlock": main.GetComponent<MainController>().adBoughten = true; main.GetComponent<MainController>().saveAll(false); main.GetComponent<IAPManager>().BuyNonConsumable("adblock"); break;
                case "FBLink": Application.OpenURL("https://facebook.com/kijaygames"); break;
                case "TWLink": Application.OpenURL("https://twitter.com/ErikoKijay"); break;
                case "VKLink": Application.OpenURL("https://vk.com/kijaygames"); break;
                case "GJLink": Application.OpenURL("https://gamejolt.com/@KIJAY"); break;
                default: Debug.Log("OOps"); break;
            }
            if(mS.audio){
                aSr.clip = clickClip;
                aSr.Play();
            }
        }
    }
    
}
