using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataPlayer : MonoBehaviour
{
    public static DataPlayer Instance;
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        if (!PlayerPrefs.HasKey("DateTimeOutGame"))
        {
            PlayerPrefs.SetString("DateTimeOutGame", DateTime.Now.ToString());
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    [HideInInspector]
    public double dollar;

    [HideInInspector]
    public double gold;

    [HideInInspector]
    public int sumHomeAll;

    [HideInInspector]
    public int indexSawmill;

    [HideInInspector]
    public int countSpin;
    public DetailItem[] lsItem;

    [HideInInspector]
    public string dateStartPlay;

    [HideInInspector]
    public string dateGame;

    [HideInInspector]
    public List<LocationJSON> lsLocation;
    private bool isFirst;
    private DateTime dateNowPlayer;

    public void SaveDataPlayer()
    {
        if (UIManager.Instance.lsItem[2].isOnItem)
        {
            GameManager.Instance.lsLocation[UIManager.Instance.lsItem[2].idLocation].forest.isAutoPlant = false;
            UIManager.Instance.lsItem[2].isOnItem = false;
        }
        if (UIManager.Instance.lsItem[5].isOnItem)
        {
            GameManager.Instance.lsLocation[UIManager.Instance.lsItem[5].idLocation].lsWorking[UIManager.Instance.lsItem[5].indexType].price
                        = GameManager.Instance.lsLocation[UIManager.Instance.lsItem[5].idLocation].lsWorking[UIManager.Instance.lsItem[5].indexType].price * 2f;
            UIManager.Instance.lsItem[5].isOnItem = false;
        }
        DataPlayer data = new DataPlayer();
        data.gold = GameManager.Instance.gold;
        data.dollar = GameManager.Instance.dollar;
        data.sumHomeAll = GameManager.Instance.sumHomeAll;
        data.indexSawmill = GameManager.Instance.indexSawmill;
        data.countSpin = GameManager.Instance.countSpin;
        data.lsItem = UIManager.Instance.lsItem;
        data.dateStartPlay = GameManager.Instance.dateStartPlay.ToString();
        data.dateGame = GameManager.Instance.dateGame.ToString();
        data.lsLocation = new List<LocationJSON>();
        for (int i = 0; i < GameManager.Instance.lsLocation.Count; i++)
        {
            LocationJSON locationJson = new LocationJSON();
            locationJson.id = GameManager.Instance.lsLocation[i].id;
            locationJson.nameLocation = GameManager.Instance.lsLocation[i].nameLocation;
            locationJson.indexTypeWork = GameManager.Instance.lsLocation[i].indexTypeWork;
            locationJson.countType = GameManager.Instance.lsLocation[i].countType;
            locationJson.indexType = GameManager.Instance.lsLocation[i].indexType;
            locationJson.makerType = GameManager.Instance.lsLocation[i].makerType;

            locationJson.forest = GameManager.Instance.lsLocation[i].forest;
            locationJson.lsWorking = GameManager.Instance.lsLocation[i].lsWorking;

            locationJson.lsOther = GameManager.Instance.lsLocation[i].lsOther;
            locationJson.lsRiverLeft = GameManager.Instance.lsLocation[i].lsRiverLeft;
            locationJson.lsRiverRight = GameManager.Instance.lsLocation[i].lsRiverRight;
            locationJson.lsStreet = GameManager.Instance.lsLocation[i].lsStreet;

            data.lsLocation.Add(locationJson);
        }


        string _path = Path.Combine(Application.persistentDataPath, "DataPlayer.json");
        File.WriteAllText(_path, JsonUtility.ToJson(data, true));
        File.ReadAllText(_path);
        PlayerPrefs.SetInt("Continue", 1);

        Debug.Log(SimpleJSON_DatDz.JSON.Parse(File.ReadAllText(_path)));

    }

    public void LoadDataPlayer()
    {
        dateNowPlayer = DateTime.Now;
        string _path = Path.Combine(Application.persistentDataPath, "DataPlayer.json");
        string dataAsJson = File.ReadAllText(_path);
        var objJson = SimpleJSON_DatDz.JSON.Parse(dataAsJson);
        Debug.Log(objJson);
        if (objJson != null)
        {
            StartCoroutine(IEClearLocation(objJson));
        }

    }

    public IEnumerator IEClearLocation(SimpleJSON_DatDz.JSONNode objJson)
    {
        GameManager.Instance.ClearLocation();
        yield return new WaitUntil(() => GameManager.Instance.lsLocation.Count == 0);
        GameManager.Instance.gold = objJson["gold"].AsDouble;
        GameManager.Instance.dollar = objJson["dollar"].AsDouble;
        GameManager.Instance.sumHomeAll = objJson["sumHomeAll"].AsInt;
        GameManager.Instance.indexSawmill = objJson["indexSawmill"].AsInt;
        GameManager.Instance.dateStartPlay = DateTime.Parse(objJson["dateStartPlay"]);
        GameManager.Instance.dateGame = DateTime.Parse(objJson["dateGame"]);
        UIManager.Instance.txtDollar.text = UIManager.Instance.ConvertNumber(GameManager.Instance.dollar);
        GameManager.Instance.countSpin = objJson["countSpin"].AsInt;
        UIManager.Instance.txtCountSpinMain.text = "x" + GameManager.Instance.countSpin;

        var lsItem = objJson["lsItem"].AsArray;
        for (int i = 6; i < UIManager.Instance.lsItem.Length; i++)
        {
            UIManager.Instance.lsItem[i].timeItem = lsItem[i]["timeItem"].AsFloat;
            UIManager.Instance.lsItem[i].timeItemTatol = lsItem[i]["timeItemTatol"].AsFloat;
            UIManager.Instance.lsItem[i].isOnItem = lsItem[i]["isOnItem"].AsBool;
            UIManager.Instance.lsItem[i].money = lsItem[i]["money"].AsDouble;
            UIManager.Instance.lsItem[i].random = lsItem[i]["random"].AsInt;

            if (UIManager.Instance.lsItem[i].isOnItem && i != 6)
            {
                UIManager.Instance.lsItem[i].obj.SetActive(true);
            }
        }
        var lsData = objJson["lsLocation"].AsArray;
        lsLocation = new List<LocationJSON>();
        GameManager.Instance.lsLocation = new List<Location>();
        StartCoroutine(IELoadLocationJson(lsData));
        GameManager.Instance.locationManager.gameObject.SetActive(true);
        GameManager.Instance.locationManager.SetAsFirstSibling();
    }

    public IEnumerator IELoadLocationJson(SimpleJSON_DatDz.JSONArray lsData)
    {
        long totalTime = 0;
        double dollarRecive = 0;
        double maxOutputMadeRevenue = 0;
        for (int i = 0; i < lsData.Count; i++)
        {
            int indexTypeWork = lsData[i]["indexTypeWork"].AsInt;
            GameObject obj = Instantiate(GameManager.Instance.lsItemLocation[indexTypeWork], GameManager.Instance.locationManager);
            obj.transform.SetAsFirstSibling();
            obj.name = lsData[i]["nameLocation"];
            Location location = obj.GetComponent<Location>();

            var lsOther = lsData[i]["lsOther"].AsArray;
            location.lsOther = new List<int>();
            for (int iOther = 0; iOther < lsOther.Count; iOther++)
            {
                location.lsOther.Add(lsOther[iOther].AsInt);
            }

            var lsRiverRight = lsData[i]["lsRiverRight"].AsArray;
            location.lsRiverRight = new List<int>();
            for (int iRiverRight = 0; iRiverRight < lsRiverRight.Count; iRiverRight++)
            {
                location.lsRiverRight.Add(lsRiverRight[iRiverRight].AsInt);
            }

            var lsRiverLeft = lsData[i]["lsRiverLeft"].AsArray;
            location.lsRiverLeft = new List<int>();
            for (int iRiverLeft = 0; iRiverLeft < lsRiverLeft.Count; iRiverLeft++)
            {
                location.lsRiverLeft.Add(lsRiverLeft[iRiverLeft].AsInt);
            }

            var lsStreet = lsData[i]["lsStreet"].AsArray;
            location.lsStreet = new List<int>();
            for (int iStreet = 0; iStreet < lsStreet.Count; iStreet++)
            {
                location.lsStreet.Add(lsStreet[iStreet].AsInt);
            }

            yield return new WaitUntil(() =>
            location.lsStreet.Count == lsStreet.Count
            && location.lsRiverLeft.Count == lsRiverLeft.Count
            && location.lsRiverRight.Count == lsRiverRight.Count
            && location.lsOther.Count == lsOther.Count);

            location.LoadLocationJson();

            yield return new WaitUntil(() => location.isLoadFull);

            location.id = lsData[i]["id"].AsInt;
            location.nameLocation = lsData[i]["nameLocation"];
            location.indexTypeWork = lsData[i]["indexTypeWork"].AsInt;
            location.countType = lsData[i]["countType"].AsInt;
            location.makerType = lsData[i]["makerType"].AsInt;

            location.forest.tree = lsData[i]["forest"]["tree"].AsInt;
            location.forest.isOnBtnAutoPlant = lsData[i]["forest"]["isOnBtnAutoPlant"].AsBool;
            location.forest.isAutoPlant = lsData[i]["forest"]["isAutoPlant"].AsBool;
            location.forest.forestClass.LoadTree();

            var lsWorking = lsData[i]["lsWorking"].AsArray;
            for (int j = 0; j < lsWorking.Count; j++)
            {
                if (location.countType >= j) location.lsWorking[j].icon.color = Color.white;

                location.lsWorking[j].name = lsWorking[j]["name"];
                location.lsWorking[j].id = lsWorking[j]["id"].AsInt;
                location.lsWorking[j].level = lsWorking[j]["level"].AsInt;
                location.lsWorking[j].countPlayer = lsWorking[j]["countPlayer"].AsInt;
                location.lsWorking[j].isClaim = lsWorking[j]["isClaim"].AsBool;

                location.lsWorking[j].input = lsWorking[j]["input"].AsDouble;
                location.lsWorking[j].output = lsWorking[j]["output"].AsDouble;
                location.lsWorking[j].priceOutput = lsWorking[j]["priceOutput"].AsDouble;
                location.lsWorking[j].isISO = lsWorking[j]["isISO"].AsBool;
                location.lsWorking[j].isUpgradeMachineJob = lsWorking[j]["isUpgradeMachineJob"].AsBool;
                location.lsWorking[j].isUpgradeMachineTrunk = lsWorking[j]["isUpgradeMachineTrunk"].AsBool;

                location.lsWorking[j].maxOutputMade = lsWorking[j]["maxOutputMade"].AsDouble;
                location.lsWorking[j].maxOutputMadeStart = lsWorking[j]["maxOutputMadeStart"].AsDouble;

                location.lsWorking[j].levelTruck = lsWorking[j]["levelTruck"].AsInt;
                location.lsWorking[j].priceUpgradeTruck = lsWorking[j]["priceUpgradeTruck"].AsDouble;
                location.lsWorking[j].priceUpgradeTruckStart = lsWorking[j]["priceUpgradeTruckStart"].AsDouble;
                location.lsWorking[j].priceTruckSent = lsWorking[j]["priceTruckSent"].AsDouble;
                location.lsWorking[j].priceTruckSentStart = lsWorking[j]["priceTruckSentStart"].AsDouble;
                location.lsWorking[j].currentSent = lsWorking[j]["currentSent"].AsDouble;
                location.lsWorking[j].maxSent = lsWorking[j]["maxSent"].AsDouble;
                location.lsWorking[j].maxSentStart = lsWorking[j]["maxSentStart"].AsDouble;

                location.lsWorking[j].priceUpgrade = lsWorking[j]["priceUpgrade"].AsDouble;
                location.lsWorking[j].priceUpgradeStart = lsWorking[j]["priceUpgradeStart"].AsDouble;
                location.lsWorking[j].price = lsWorking[j]["price"].AsDouble;
                location.lsWorking[j].UN2 = lsWorking[j]["UN2"].AsFloat;

                if (location.lsWorking[j].isISO)
                {
                    location.lsWorking[j].iso.SetActive(true);
                }

                if (location.lsWorking[j].id <= location.countType)
                {
                    location.lsWorking[j].info.SetActive(true);
                    if (location.lsWorking[j].textInput != null)
                    {
                        location.lsWorking[j].textInput.text = UIManager.Instance.ConvertNumber(location.lsWorking[j].input);
                    }
                    location.lsWorking[j].textOutput.text = UIManager.Instance.ConvertNumber(location.lsWorking[j].output);
                    if (location.lsWorking[j].output > 0)
                    {
                        location.lsWorking[j].truckManager.LoadTruck();
                    }
                    location.lsWorking[j].textLevel.text = UIManager.Instance.ConvertNumber(location.lsWorking[j].level);
                    location.lsWorking[j].truckManager.txtLevel.text = UIManager.Instance.ConvertNumber(location.lsWorking[j].levelTruck);
                }
                if (i != lsData.Count - 1)
                {
                    location.lsWorking[j].animLock.gameObject.SetActive(false);
                }
                else
                {
                    if (location.lsWorking[j].id <= location.countType)
                    {
                        location.lsWorking[j].animLock.gameObject.SetActive(false);
                    }
                    else if (location.lsWorking[j].id == location.countType + 1)
                    {
                        location.lsWorking[j].animLock.enabled = true;
                    }
                }
            }

            yield return new WaitUntil(() => location.lsWorking[0].price != 0);

            if (!location.forest.isAutoPlant)
            {
                if (location.forest.isOnBtnAutoPlant)
                {
                    location.forest.btnAutoPlant.gameObject.SetActive(true);
                    if (GameManager.Instance.dollar >= (double)(location.lsWorking[0].price * GameConfig.Instance.AutoPlant))
                    {
                        location.forest.btnAutoPlant.interactable = true;
                    }
                    else
                    {
                        location.forest.btnAutoPlant.interactable = false;
                    }
                }
            }
            GameManager.Instance.lsLocation.Add(location);
            UIManager.Instance.lsBtnLocationUI[i].interactable = true;

            if (location.countType == -1)
            {
                maxOutputMadeRevenue += 0;
            }
            else
            {
                maxOutputMadeRevenue += location
                .lsWorking[location.countType].maxOutputMade
                * GameConfig.Instance.r
                * GameConfig.Instance.productCost;
            }
        }
        yield return new WaitUntil(() => UIManager.Instance.lsBtnLocationUI[lsData.Count - 1].interactable);

        UIManager.Instance.handWorld.position = UIManager.Instance.lsBtnLocationUI[lsData.Count - 1].transform.GetChild(0).position - new Vector3(0f, 0.25f, 0f); ;

        int locationEnd = GameManager.Instance.lsLocation.Count - 1;
        int jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
        if (jobEnd == -1 && GameManager.Instance.lsLocation.Count > 1)
        {
            locationEnd--;
            jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
        }

        if (GameManager.Instance.lsLocation.Count > 1)
        {
            dollarRecive = GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price;
        }
        else
        {
            if (jobEnd == -1)
            {
                dollarRecive = GameManager.Instance.lsLocation[0].lsWorking[0].price;
            }
            else
            {
                dollarRecive = GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price;
            }
        }
        UIManager.Instance.txtRevenue.text
        = "Revenue : " + UIManager.Instance.ConvertNumber(maxOutputMadeRevenue) + "$/day";
        ScenesManager.Instance.isNextScene = true;
        double adddollar = 0;
        if (!isFirst)
        {
            if (UIManager.Instance.isContinue)
            {
                totalTime = (long)((TimeSpan)(dateNowPlayer - DateTime.Parse(PlayerPrefs.GetString("DateTimeOutGame")))).TotalMinutes;
                if (totalTime >= 5 && dollarRecive > 0)
                {
                    if (totalTime <= 60)
                    {
                        adddollar = (double)(0.1f * dollarRecive);
                    }
                    else if (totalTime <= 600)
                    {
                        totalTime = totalTime / 60;
                        adddollar = (double)((double)totalTime * 0.5f * dollarRecive);
                    }
                    else
                    {
                        totalTime = 10;
                        adddollar = (double)((double)totalTime * 0.5f * dollarRecive);
                    }
                    GameManager.Instance.AddDollar(+adddollar);
                    string strGive = "Offline Reward\n"
                    + UIManager.Instance.ConvertNumber(adddollar)
                    + "$";
                    UIManager.Instance.PushGiveGold(strGive);
                    PlayerPrefs.SetString("DateTimeOutGame", DateTime.Now.ToString());
                }
            }
            isFirst = true;
        }
    }

    private void OnDestroy()
    {
        if (UIManager.Instance.isSaveJson && UIManager.Instance.scene != TypeScene.HOME)
        {
            SaveDataPlayer();
        }
        PlayerPrefs.SetString("DateTimeOutGame", DateTime.Now.ToString());
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause == true)
        {
            if (UIManager.Instance.isSaveJson && UIManager.Instance.scene != TypeScene.HOME)
            {
                SaveDataPlayer();
            }
            PlayerPrefs.SetString("DateTimeOutGame", DateTime.Now.ToString());
        }
        else
        {
            dateNowPlayer = DateTime.Now;
            if (UIManager.Instance.scene != TypeScene.HOME)
            {
                int locationEnd = GameManager.Instance.lsLocation.Count - 1;
                int jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
                if (jobEnd == -1 && GameManager.Instance.lsLocation.Count > 1)
                {
                    locationEnd--;
                    jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
                }
                double dollarRecive = 0;
                double maxOutputMadeRevenue = 0;
                if (GameManager.Instance.lsLocation.Count > 1)
                {
                    dollarRecive = GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price;
                    maxOutputMadeRevenue = GameManager.Instance.lsLocation[locationEnd]
                    .lsWorking[jobEnd].maxOutputMade
                    * GameConfig.Instance.r
                    * GameConfig.Instance.productCost;
                }
                else
                {
                    if (jobEnd == -1)
                    {
                        dollarRecive = 0;
                        maxOutputMadeRevenue = 0;
                    }
                    else
                    {
                        dollarRecive = GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price;
                        maxOutputMadeRevenue = GameManager.Instance.lsLocation[locationEnd]
                        .lsWorking[jobEnd].maxOutputMade
                        * GameConfig.Instance.r
                        * GameConfig.Instance.productCost;
                    }
                }
                double adddollar = 0;
                long totalTime = (long)((TimeSpan)(dateNowPlayer - DateTime.Parse(PlayerPrefs.GetString("DateTimeOutGame")))).TotalHours;
                totalTime = (long)((TimeSpan)(dateNowPlayer - DateTime.Parse(PlayerPrefs.GetString("DateTimeOutGame")))).TotalMinutes;
                if (totalTime >= 5 && dollarRecive > 0)
                {
                    if (totalTime <= 60)
                    {
                        adddollar = (double)(0.1f * dollarRecive);
                    }
                    else if (totalTime <= 600)
                    {
                        totalTime = totalTime / 60;
                        adddollar = (double)((double)totalTime * 0.5f * dollarRecive);
                    }
                    else
                    {
                        totalTime = 10;
                        adddollar = (double)((double)totalTime * 0.5f * dollarRecive);
                    }
                    GameManager.Instance.AddDollar(+adddollar);
                    string strGive = "Offline Reward\n"
                    + UIManager.Instance.ConvertNumber(adddollar)
                    + "$";
                    UIManager.Instance.PushGiveGold(strGive);
                    PlayerPrefs.SetString("DateTimeOutGame", DateTime.Now.ToString());
                }
            }
            else
            {
                isFirst = false;
            }
        }
    }

    public void SaveExit()
    {
        StartCoroutine(IESaveDataPlayer());
    }

    public IEnumerator IESaveDataPlayer()
    {

        int sumLocaton = 0;
        DataPlayer data = new DataPlayer();
        data.gold = GameManager.Instance.gold;
        data.dollar = GameManager.Instance.dollar;
        data.sumHomeAll = GameManager.Instance.sumHomeAll;
        data.indexSawmill = GameManager.Instance.indexSawmill;
        data.dateStartPlay = GameManager.Instance.dateStartPlay.ToString();
        data.dateGame = GameManager.Instance.dateGame.ToString();
        data.countSpin = GameManager.Instance.countSpin;
        data.lsItem = UIManager.Instance.lsItem;
        data.lsLocation = new List<LocationJSON>();
        for (int i = 0; i < GameManager.Instance.lsLocation.Count; i++)
        {
            LocationJSON locationJson = new LocationJSON();
            locationJson.id = GameManager.Instance.lsLocation[i].id;
            locationJson.nameLocation = GameManager.Instance.lsLocation[i].nameLocation;
            locationJson.indexTypeWork = GameManager.Instance.lsLocation[i].indexTypeWork;
            locationJson.countType = GameManager.Instance.lsLocation[i].countType;
            locationJson.indexType = GameManager.Instance.lsLocation[i].indexType;
            locationJson.makerType = GameManager.Instance.lsLocation[i].makerType;

            locationJson.forest = GameManager.Instance.lsLocation[i].forest;
            locationJson.lsWorking = GameManager.Instance.lsLocation[i].lsWorking;

            locationJson.lsOther = GameManager.Instance.lsLocation[i].lsOther;
            locationJson.lsRiverLeft = GameManager.Instance.lsLocation[i].lsRiverLeft;
            locationJson.lsRiverRight = GameManager.Instance.lsLocation[i].lsRiverRight;
            locationJson.lsStreet = GameManager.Instance.lsLocation[i].lsStreet;

            data.lsLocation.Add(locationJson);
            sumLocaton++;
        }

        string _path = Path.Combine(Application.persistentDataPath, "DataPlayer.json");
        File.WriteAllText(_path, JsonUtility.ToJson(data, true));
        File.ReadAllText(_path);

        yield return new WaitUntil(() => sumLocaton == GameManager.Instance.lsLocation.Count);

        PlayerPrefs.SetInt("Continue", 1);

        GameManager.Instance.ClearLocation();
    }

}
