using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
//using UnityEngine.Advertisements;
using DG.Tweening;
using System;

public class Ads : MonoBehaviour
{
    [Header("Admob")]
    InterstitialAd interstitalAd;
    BannerView bannerView;

    bool isLoadAds = false;
    //bool isShowAds = false;

    public GameObject panelPlane;
    public Text txtPlaneVideoAds;
    public Text txtPlaneReciveDollar;

    [Header("Time")]
    public float timeAds = 1;
    public float timeRate = 1;
    int countRate = 0;
    public static Ads Instance = new Ads();
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    void Start()
    {
        //PlayerPrefs.DeleteAll();
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize("ca-app-pub-4738062221647171~1836833926");
        //RequestAd();
        // MobileAds.SetiOSAppPauseOnBackground(true);
    }

    void Update()
    {
        timeAds += Time.deltaTime;
        timeRate += Time.deltaTime;
        ShowInterstitialAd();
        RateInApp();
    }

    #region ===ADMOB===


    public void RequestAd()
    {

#if UNITY_ANDROID        
        if (!isLoadAds && GameConfig.Instance.idInter_android != null && GameConfig.Instance.idInter_android != "")
        {
            interstitalAd = new InterstitialAd(GameConfig.Instance.idInter_android);
            AdRequest requestInterAd = new AdRequest.Builder().Build();
            interstitalAd.LoadAd(requestInterAd);
            isLoadAds = true;
            Debug.Log("Load Ads - " + interstitalAd.IsLoaded().ToString());
        }
#elif UNITY_IOS
        if (!isLoadAds && GameConfig.Instance.idInter_ios != null && GameConfig.Instance.idInter_ios != "")
        {
            interstitalAd = new InterstitialAd(GameConfig.Instance.idInter_ios);
            AdRequest requestInterAd = new AdRequest.Builder().Build();
            interstitalAd.LoadAd(requestInterAd);
            isLoadAds = true;
            Debug.Log("Load Ads - " + interstitalAd.IsLoaded().ToString());
        }
#else
        GameConfig.Instance.idInter_android = "ca-app-pub-6285794272989840/5632501293"; //test
        if (!isLoadAds && GameConfig.Instance.idInter_android != null)
        {
            interstitalAd = new InterstitialAd(GameConfig.Instance.idInter_android);
            AdRequest requestInterAd = new AdRequest.Builder().Build();
            interstitalAd.LoadAd(requestInterAd);
            isLoadAds = true;
            Debug.Log("Load Ads - " + interstitalAd.IsLoaded().ToString());
        }
#endif

    }

    public void ShowInterstitialAd()
    {
        if (timeAds < GameConfig.Instance.timeInterAd)
        {
            RequestAd();
            return;
        }

        if (interstitalAd != null)
        {
            if (PlayerPrefs.GetInt("NoAds") == 0)
            {
                if (interstitalAd.IsLoaded())
                {
                    interstitalAd.Show();
                    isLoadAds = false;
                    timeAds = 0;
                    Debug.Log("Show Ads");
                }
            }
            else
            {
                timeAds = 0;
            }
        }
        else
        {
            Debug.Log("Null");
            RequestAd();
        }
    }

    public void RequestBanner()
    {
#if UNITY_ANDROID   
        if (GameConfig.Instance.idBanner_android != null)
        {
            bannerView = new BannerView(GameConfig.Instance.idBanner_android, AdSize.Banner, AdPosition.Bottom);
            AdRequest requestBanner = new AdRequest.Builder().Build();
            bannerView.LoadAd(requestBanner);
        }
#elif UNITY_IOS
        if (GameConfig.Instance.idBanner_ios != null)
        {
            bannerView = new BannerView(GameConfig.Instance.idBanner_ios, AdSize.Banner, AdPosition.Bottom);
            AdRequest requestBanner = new AdRequest.Builder().Build();
            bannerView.LoadAd(requestBanner);
        }
#endif
    }

    public void ShowBanner()
    {
        if (bannerView != null)
        {
            if (PlayerPrefs.GetInt("NoBanner") == 0)
            {
                bannerView.Show();
                Debug.Log("Show Banner");
            }
        }
    }

    public void HideBanner()
    {
        if (bannerView != null)
        {
            bannerView.Hide();
            Debug.Log("Hide Banner");
        }
    }
    #endregion

    #region ===UNITY ADS===

    public void SuccessPlaneReciveDollar()
    {
        panelPlane.SetActive(false);
        int locationEnd = GameManager.Instance.lsLocation.Count - 1;
        int jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
        double dollarRecive = 0;
        if (GameManager.Instance.lsLocation.Count > 1)
        {
            if (jobEnd == -1)
            {
                locationEnd--;
                jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
            }
            dollarRecive = GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price / 10;
        }
        else
        {
            if (jobEnd == -1)
            {
                dollarRecive = GameManager.Instance.lsLocation[0].lsWorking[0].price;
            }
            else
            {
                dollarRecive = GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price / 10;
            }

        }
        //Debug.Log(GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price + " / " + dollarRecive);
        GameManager.Instance.AddDollar(+Math.Floor(dollarRecive));        // số tiền nhà cuối
        UIManager.Instance.PushGiveGold("You have recived " + UIManager.Instance.ConvertNumber(dollarRecive) + "$");
        panelPlane.SetActive(false);
    }

    public void SuccessAdsUnity()
    {
        //Debug.Log("Cong tien : " + GameConfig.Instance.dollarVideoAd);
        int locationEnd = GameManager.Instance.lsLocation.Count - 1;
        int jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
        double dollarRecive = 0;
        if (GameManager.Instance.lsLocation.Count > 1)
        {
            if (jobEnd == -1)
            {
                locationEnd--;
                jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
            }
            dollarRecive = GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price / 2;
        }
        else
        {
            if (jobEnd == -1)
            {
                dollarRecive = GameManager.Instance.lsLocation[0].lsWorking[0].price;
            }
            else
            {
                dollarRecive = GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price / 2;
            }
        }
        Debug.Log(GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price + " / " + dollarRecive);
        GameManager.Instance.AddDollar(+Math.Floor(dollarRecive)); // số tiền nhà cuối
        UIManager.Instance.PushGiveGold("You have recived " + UIManager.Instance.ConvertNumber(dollarRecive) + "$");
        panelPlane.SetActive(false);
    }

    public void GiveAds()
    {
        //Debug.Log("Cong tien : " + GameConfig.Instance.dollarVideoAd);
        int locationEnd = GameManager.Instance.lsLocation.Count - 1;
        int jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
        double dollarRecive = 0;
        if (GameManager.Instance.lsLocation.Count > 1)
        {
            if (jobEnd == -1)
            {
                locationEnd--;
                jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
            }
            dollarRecive = GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price / 5;
        }
        else
        {
            if (jobEnd == -1)
            {
                dollarRecive = GameManager.Instance.lsLocation[0].lsWorking[0].price;
            }
            else
            {
                dollarRecive = GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price / 5;
            }
        }
        Debug.Log(GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price + " / " + dollarRecive);
        GameManager.Instance.AddDollar(+Math.Floor(dollarRecive)); // số tiền nhà cuối
        UIManager.Instance.PushGiveGold("You have recived " + UIManager.Instance.ConvertNumber(dollarRecive) + "$");
        panelPlane.SetActive(false);
    }
    #endregion

    #region ===RATE IN APP===
    void RateInApp()
    {
        if (timeRate >= 10 * 7 && PlayerPrefs.GetInt("isTutorial") == 1 && countRate == 0)// && PlayerPrefs.GetInt("isRate") != 1)
        {
            EPPZ.Rate.Rate.RequestReviewIfAppropriate();
            countRate = 1;
        }
    }
    #endregion
}
