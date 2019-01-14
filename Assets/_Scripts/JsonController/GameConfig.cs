using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SocialPlatforms.GameCenter;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.storage;

public class GameConfig : MonoBehaviour
{
    public static GameConfig Instance;
    public static string id = "";
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    public double dollarStart;
    public double goldStart;
    public int goldToDollar;
    public int dollarVideoAd;
    public int timeInterAd;
    public int fellingTime;
    public int growTime;
    public int p0;
    public float p0Time;
    public float c0;
    public float productCost;
    public float p0i;
    public float c0i;
    public float r;
    public float UN2;
    public float UN1i;
    public float truckTime;
    public int x0;
    public float x0i;
    public float XN2;
    public float XN1i;
    public float XT2;
    public float XT1i;
    public float capIndex;
    public float captruckIndex;
    public float WYS;
    public float AutoPlant;
    public float TruckSpeed;
    public float TimeForest;
    public int MaxSentStartX5;
    public string idUnityAds_ios;
    public string idUnityAds_android;
    public string idInter_android;
    public string idInter_ios;
    public string idBanner_ios;
    public string idBanner_android;
    public string link_ios;
    public string link_android;
    public string string_Share;
    public string kProductID50 = "consumable";
    public string kProductID300 = "consumable";
    public string kProductID5000 = "consumable";
    public List<string> richness = new List<string>();
    public float Carup;
    public int Pinc;
    public float Iso;
    public float Upmachine;
    public float Prein;
    public int Rein;
    public float Pfire;
    public int Tchal;
    public float Uptruck;


    string app42_apiKey = "41b8289bb02efae4f37f1c9d891b09bb43f6f801bdbbf17a557bc4598ddf836b";
    string app42_secretKey = "35d9a321b8d4cfc3b375b5f212f15ffab98bb2b53e4b9da20d22881fc01a0efa";

    //public List<ListInformation> lstInfo = new List<ListInformation>();

    [HideInInspector]
    //public ListInformation _lstInfo;
    void Start()
    {
        if (id == "")
        {
            App42API.Initialize(app42_apiKey, app42_secretKey);
            //GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
            Social.localUser.Authenticate(success =>
            {
                if (success)
                {
                    id = Social.localUser.id;
                    //Debug.Log(id);
                    StorageService storageService = App42API.BuildStorageService();
                    storageService.FindDocumentByKeyValue("Db", "Data", "id", id, new UnityCallBack1());
                }
                else
                    Debug.Log("Failed to authenticate");
            });
        }
    }
}

//[System.Serializable]
//public struct Information
//{
//    public string name;
//    public string info;
//}

//[System.Serializable]
//public struct ListInformation
//{
//    public List<Information> listInformation;
//}

public class SaveGold
{
    public string id;
    public int gold;
    public SaveGold(string id, int gold)
    {
        this.id = id;
        this.gold = gold;
    }
}
