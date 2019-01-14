using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements; // only compile Ads code on supported platforms
#endif

public class LoadDataJson : MonoBehaviour
{
    public static LoadDataJson Instance;
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    private string gameConfig = "GameConfig";
    private string information = "Information";

    void Start()
    {
        LoadGameConfig();
        Ads.Instance.RequestAd();
        Ads.Instance.RequestBanner();
#if UNITY_ADS
        InitUnityAds();
#endif
        Purchaser.Instance.Init();
        Ads.Instance.HideBanner();
        GetDateLauch();
    }

    void InitUnityAds()
    {
#if UNITY_ANDROID  
        Advertisement.Initialize(GameConfig.Instance.idUnityAds_android, true);
#elif UNITY_IOS
        Advertisement.Initialize(GameConfig.Instance.idUnityAds_ios, true);
#endif
    }

    public void LoadGameConfig()
    {
        var objJson = SimpleJSON_DatDz.JSON.Parse(loadJson(gameConfig));
        Debug.Log(objJson);
        //Debug.Log("<color=yellow>Done: </color>LoadGameConfig !");
        if (objJson != null)
        {
            GameConfig.Instance.dollarStart = objJson["dollarStart"].AsDouble;
            GameConfig.Instance.goldStart = objJson["goldStart"].AsDouble;
            GameConfig.Instance.goldToDollar = objJson["goldToDollar"].AsInt;
            GameConfig.Instance.dollarVideoAd = objJson["dollarVideoAd"].AsInt;
            GameConfig.Instance.timeInterAd = objJson["timeInterAd"].AsInt;
            GameConfig.Instance.fellingTime = objJson["fellingTime"].AsInt;
            GameConfig.Instance.growTime = objJson["growTime"].AsInt;
            GameConfig.Instance.p0 = objJson["p0"].AsInt;
            GameConfig.Instance.p0Time = objJson["p0Time"].AsFloat;
            GameConfig.Instance.c0 = objJson["c0"].AsFloat;
            GameConfig.Instance.productCost = objJson["productCost"].AsFloat;
            GameConfig.Instance.p0i = objJson["p0i"].AsFloat;
            GameConfig.Instance.c0i = objJson["c0i"].AsFloat;
            GameConfig.Instance.r = objJson["r"].AsFloat;
            GameConfig.Instance.UN2 = objJson["UN2"].AsFloat;
            GameConfig.Instance.UN1i = objJson["UN1i"].AsFloat;
            GameConfig.Instance.truckTime = objJson["truckTime"].AsFloat;
            GameConfig.Instance.x0 = objJson["x0"].AsInt;
            GameConfig.Instance.x0i = objJson["x0i"].AsFloat;
            GameConfig.Instance.XN2 = objJson["XN2"].AsFloat;
            GameConfig.Instance.XN1i = objJson["XN1i"].AsFloat;
            GameConfig.Instance.XT2 = objJson["XT2"].AsFloat;
            GameConfig.Instance.XT1i = objJson["XT1i"].AsFloat;
            GameConfig.Instance.capIndex = objJson["capIndex"].AsFloat;
            GameConfig.Instance.captruckIndex = objJson["captruckIndex"].AsFloat;
            GameConfig.Instance.WYS = objJson["WYS"].AsFloat;
            GameConfig.Instance.AutoPlant = objJson["AutoPlant"].AsFloat;
            GameConfig.Instance.TruckSpeed = objJson["TruckSpeed"].AsFloat;
            UIManager.Instance.speedTrunkTutorial = objJson["TruckSpeed"].AsFloat;
            GameConfig.Instance.TimeForest = objJson["TimeForest"].AsFloat;
            GameConfig.Instance.MaxSentStartX5 = objJson["MaxSentStartX5"].AsInt;
            GameConfig.Instance.idUnityAds_ios = objJson["idUnityAds_ios"];
            GameConfig.Instance.idUnityAds_android = objJson["idUnityAds_android"];
            GameConfig.Instance.idInter_android = objJson["idInter_android"];
            GameConfig.Instance.idInter_ios = objJson["idInter_ios"];
            GameConfig.Instance.idBanner_ios = objJson["idBanner_ios"];
            GameConfig.Instance.idBanner_android = objJson["idBanner_android"];
            GameConfig.Instance.kProductID50 = objJson["kProductID50"];
            GameConfig.Instance.kProductID300 = objJson["kProductID300"];
            GameConfig.Instance.kProductID5000 = objJson["kProductID5000"];
            GameConfig.Instance.link_ios = objJson["link_ios"];
            GameConfig.Instance.link_android = objJson["link_android"];
            GameConfig.Instance.string_Share = objJson["string_Share"];
            GameConfig.Instance.Carup = objJson["Carup"].AsFloat;
            GameConfig.Instance.Pinc = objJson["Pinc"].AsInt;
            GameConfig.Instance.Iso = objJson["Iso"].AsFloat;
            GameConfig.Instance.Upmachine = objJson["Upmachine"].AsFloat;
            GameConfig.Instance.Prein = objJson["Prein"].AsFloat;
            GameConfig.Instance.Rein = objJson["Rein"].AsInt;
            GameConfig.Instance.Pfire = objJson["Pfire"].AsFloat;
            GameConfig.Instance.Tchal = objJson["Tchal"].AsInt;
            GameConfig.Instance.Uptruck = objJson["Uptruck"].AsFloat;

            for (int j = 0; j < objJson["richness"].Count; j++)
            {
                GameConfig.Instance.richness.Add(objJson["richness"][j]);
            }
        }

        LoadInformation();
    }
    //Information _info;
    void LoadInformation()
    {
        var objJson = SimpleJSON_DatDz.JSON.Parse(loadJson(information));
        //Debug.Log(objJson);

        if (objJson != null)
        {
            for (int i = 0; i < objJson.Count; i++)
            {
                GameManager.Instance.lsTypeMiniGame[i].name = objJson[i][0]["name"];
                GameManager.Instance.lsTypeMiniGame[i].info = objJson[i][0]["info"];
                for (int j = 1; j < objJson[i].Count; j++)
                {
                    GameManager.Instance.lsTypeMiniGame[i].lsMiniGame[j - 1].name = objJson[i][j]["name"];
                    GameManager.Instance.lsTypeMiniGame[i].lsMiniGame[j - 1].info = objJson[i][j]["info"];
                }
            }
        }
    }

    string loadJson(string _nameJson)
    {
        TextAsset _text = Resources.Load(_nameJson) as TextAsset;
        return _text.text;
    }
    double dollarRecive = 0;
    int goldExchange = 0;
    public void GoldToDollar()
    {
        dollarRecive = GameManager.Instance.PriceHomeEnd() * 0.5f;
        goldExchange = 5 + GameManager.Instance.sumHomeAll;

        if (GameManager.Instance.gold >= goldExchange)
        {
            GameManager.Instance.gold -= goldExchange;
            GameManager.Instance.AddDollar(+dollarRecive);
            //UIManager.Instance.PushGiveGold("You have received " + UIManager.Instance.ConvertNumber(dollarRecive) + "$");
            UIManager.Instance.imgGoldToDollar_Anim.GetComponent<Animator>().Play("ExchangeGold 1");
            UIManager.Instance.buttonExchangeGold.interactable = false;
            Invoke("WaitExchange", 1f);
            if (GameManager.Instance.gold > 10)// && Mathf.Abs(PlayerPrefs.GetInt("GoldPre", 0) - PlayerPrefs.GetInt("Gold", 10)) >= 50)
            {
                PlayerPrefs.SetInt("GoldPre", (int)GameManager.Instance.gold);
                StorageService storageService = App42API.BuildStorageService();
                storageService.UpdateDocumentByKeyValue("Db", "Data", "id", GameConfig.id, JsonUtility.ToJson(new SaveGold(GameConfig.id, (int)GameManager.Instance.gold)), new UnityCallBack2());
            }
        }
        else
        {
            UIManager.Instance.panelDollar.SetActive(false);
            UIManager.Instance.panelGold.SetActive(true);
        }
    }

    void WaitExchange()
    {
        UIManager.Instance.buttonExchangeGold.interactable = true;
    }

    public void RestoreProgess()
    {
        //loading.SetActive(true);
        StorageService storageService = App42API.BuildStorageService();
        storageService.FindDocumentByKeyValue("Db", "Data", "id", GameConfig.id, new UnityCallBack3());
        UIManager.Instance.panelSetting.SetActive(false);

        UIManager.Instance.PushGiveGold("Waiting ...");
    }

    #region ===GIFT DAY===
    private System.DateTime dateLastLaunch;
    private System.DateTime today;
    /// <summary>
    /// Lay ra ngay khi chay game
    /// </summary>
    void GetDateLauch()
    {
        if (PlayerPrefs.GetInt("isTutorial") != 1)
            return;

        if (PlayerPrefs.HasKey("DateLastLaunch"))
        {
            dateLastLaunch = System.Convert.ToDateTime(PlayerPrefs.GetString("DateLastLaunch"));
        }
        else
        {
            dateLastLaunch = System.DateTime.Now;
            PlayerPrefs.SetString("DateLastLaunch", dateLastLaunch.ToString());
        }
        CheckDateGift();
    }

    double GetDatePassed()
    {
        Debug.Log(dateLastLaunch);
        today = System.DateTime.Now;
        Debug.Log(today);
        System.TimeSpan elapsed = today.Subtract(dateLastLaunch);
        double hours = elapsed.TotalHours;

        return hours;
    }

    public void CheckDateGift()
    {
        Debug.Log(GetDatePassed());        
        if (PlayerPrefs.GetInt("DayGift") > 15)
        {
            PlayerPrefs.SetInt("DayGift", 0);
        }
        int _dayGift = PlayerPrefs.GetInt("DayGift");

        if (GetDatePassed() >= 24)
        {
            UIManager.Instance.panelGiftDay.SetActive(true);
            UIManager.Instance.itemsGiftDay[_dayGift].Set_ClaimNow();
            for (int i = 0; i < UIManager.Instance.itemsGiftDay.Count; i++)
            {
                if (i < _dayGift)
                {
                    UIManager.Instance.itemsGiftDay[i].Set_Claimed();
                }
                else if (i == _dayGift)
                {
                    UIManager.Instance.itemsGiftDay[i].Set_ClaimNow();
                }
                else
                {
                    UIManager.Instance.itemsGiftDay[i].Set_ClaimNot();
                }

            }
        }
        else
        {
            if (UIManager.Instance.panelGiftDay.activeSelf)
                UIManager.Instance.panelGiftDay.SetActive(false);
        }
    }
    #endregion

}
