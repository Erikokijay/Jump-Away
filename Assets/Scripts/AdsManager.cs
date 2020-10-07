using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;
public class AdsManager : MonoBehaviour, IUnityAdsListener  {
    // Start is called before the first frame update
    #if UNITY_IOS
        private string gameId = "3678810";
    #elif UNITY_ANDROID
        private string gameId = "3678811";
    #endif
    private BannerView bannerView;
    public bool testMode = true, isLoad = false, adOn = false, bn = false;
    // Initialize the Ads listener and service:
    void Start () {
        Advertisement.AddListener (this);
        Advertisement.Initialize(gameId, testMode);
        MobileAds.Initialize(initStatus => { });
        this.RequestBanner();
    }

     private void RequestBanner()
    {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-1079615982415693/4101411511";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
        #else
            string adUnitId = "ca-app-pub-1079615982415693/4101411511";
        #endif

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request. 
        this.bannerView.LoadAd(request);
        this.bannerView.Show();
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) {
        if(GetComponent<MainController>().lose && GetComponent<MainController>().loseCount<4 && !GetComponent<MainController>().aliving && !bn){
            GetComponent<MainController>().GameContinue(false);
        }
        bn = false;
        adOn = false;
    }

    public void OnUnityAdsReady (string placementId) {
        // If the ready Placement is rewarded, show the ad:
        adOn = true;
        
    }

    public void OnUnityAdsDidError (string message) {
        // Log the error.
    }

    public void OnUnityAdsDidStart (string placementId) {
        // Optional actions to take when the end-users triggers an ad.
        adOn = true;
    } 

    public bool isLoaded(string pl) {
        return Advertisement.IsReady(pl);
    }

    public void showMe(string pl){
        adOn = true;
        Advertisement.Show(pl);
    }
}