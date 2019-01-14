using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ForestST
{
    public int tree;
    public Forest forestClass;
    public Button btnAutoPlant;
    public bool isOnBtnAutoPlant;
    public bool isAutoPlant;
    [HideInInspector]
    public float timeFelling;
    public int typeTree;
}

[System.Serializable]
public struct TypeOfWorkST
{
    [Header("Information")]
    public string name;
    public int id;
    public int level;
    public int countPlayer;
    public bool isClaim;

    [Header("UI")]
    public Text textLevel;
    public Image icon;
    public Animator anim;
    public Animator animLock;
    public Text textInput;
    public Text textOutput;
    public GameObject info;
    public GameObject iso;
    public Transform trunk;

    [Header("Parameters")]
    public double input;
    public double output;
    public double priceOutput;

    public double maxOutputMade; //C0
    public double maxOutputMadeStart; //C0

    [HideInInspector]
    public float timeWorking;
    [HideInInspector]
    public bool isXJob;

    public bool isISO;

    public bool isUpgradeMachineJob;

    public bool isUpgradeMachineTrunk;


    [Header("Transport")]
    public int levelTruck;
    public double priceUpgradeTruck;
    public double priceUpgradeTruckStart;
    public double priceTruckSent; //X0
    public double priceTruckSentStart; //X0
    public double currentSent;
    public double maxSent; //CI0
    public double maxSentStart; //CI0
    public TruckManager truckManager;

    [HideInInspector]
    public float timeCheckUpgradeMe;

    [Header("Price")]
    public double priceUpgrade;
    public double priceUpgradeStart;
    public double price; //P0
    [HideInInspector]
    public float UN2;
}

public class Location : MonoBehaviour
{
    public int id;
    public string nameLocation;
    public int indexTypeWork;
    public int countType;
    [HideInInspector]
    public int makerType;
    public Text txtNameForest;
    public ForestST forest;
    public TypeOfWorkST[] lsWorking;

    public Streets streets;
    public Others others;
    public Rivers rivers;

    #region HideInInspector
    [HideInInspector]
    public int indexType;
    [HideInInspector]
    public bool isLoaded;
    public bool isLoadFull;

    [HideInInspector]
    public List<int> lsOther;
    [HideInInspector]
    public List<int> lsRiverRight;
    [HideInInspector]
    public List<int> lsRiverLeft;
    [HideInInspector]
    public List<int> lsStreet;
    #endregion
    public void LoadLocation()
    {
        StartCoroutine(IELoadLocation());
    }
    public void LoadLocationJson()
    {
        StartCoroutine(IELoadLocationJson());
    }
    public IEnumerator IELoadLocation()
    {
        isLoaded = false;
        others.LoadOtherRandom();
        yield return new WaitUntil(() => isLoaded);
        isLoaded = false;
        streets.LoadStreetRandom();
        yield return new WaitUntil(() => isLoaded);
        isLoaded = false;
        LoadInfoTypeOfWorkST();
        yield return new WaitUntil(() => isLoaded);
        if (indexTypeWork == 0)
        {
            lsWorking[lsWorking.Length - 1].anim.SetFloat("indexMaker", makerType);
        }
        txtNameForest.text = UIManager.Instance.lsNameForest[id];
    }
    public IEnumerator IELoadLocationJson()
    {
        isLoaded = false;
        others.LoadOtherJson();
        yield return new WaitUntil(() => isLoaded);
        isLoaded = false;
        streets.LoadStreetJson();
        yield return new WaitUntil(() => isLoaded);
        isLoaded = false;
        isLoadFull = true;
        isLoaded = true;
        yield return new WaitUntil(() => isLoaded);
        if (indexTypeWork == 0)
        {
            lsWorking[lsWorking.Length - 1].anim.SetFloat("indexMaker", makerType);
        }
        txtNameForest.text = UIManager.Instance.lsNameForest[id];
    }
    public void LoadInfoTypeOfWorkST()
    {
        if (GameManager.Instance.lsLocation.Count > 1)
        {
            for (int i = 0; i < lsWorking.Length; i++)
            {
                if (i == 0)
                {
                    Location l = GameManager.Instance.lsLocation[id - 1];
                    lsWorking[i].UN2 = GameConfig.Instance.UN2;
                    lsWorking[i].price = Math.Floor((double)(GameConfig.Instance.p0i * l.lsWorking[l.countType].price));
                    lsWorking[i].maxOutputMade = Math.Floor((double)(GameConfig.Instance.c0i * l.lsWorking[l.countType].maxOutputMadeStart));
                    lsWorking[i].maxOutputMadeStart = lsWorking[i].maxOutputMade;
                    lsWorking[i].priceUpgrade = Math.Floor((double)(GameConfig.Instance.UN1i * lsWorking[i].price));
                    lsWorking[i].priceUpgradeStart = lsWorking[i].priceUpgrade;
                    lsWorking[i].priceTruckSent = 0;
                    lsWorking[i].priceTruckSentStart = lsWorking[i].priceTruckSent;
                    lsWorking[i].priceOutput = (double)GameConfig.Instance.productCost;
                    lsWorking[i].maxSent = Math.Floor(GameConfig.Instance.MaxSentStartX5 * lsWorking[i].maxOutputMade);
                    lsWorking[i].maxSentStart = lsWorking[i].maxSent;
                    lsWorking[i].priceUpgradeTruck = Math.Floor((double)(GameConfig.Instance.XN1i * lsWorking[i].price));
                    lsWorking[i].priceUpgradeTruckStart = lsWorking[i].priceUpgradeTruck;
                }
                else
                {
                    lsWorking[i].UN2 = GameConfig.Instance.UN2;
                    lsWorking[i].price = Math.Floor((double)(GameConfig.Instance.p0i * lsWorking[i - 1].price));
                    lsWorking[i].maxOutputMade = Math.Floor((double)(GameConfig.Instance.c0i * lsWorking[i - 1].maxOutputMadeStart));
                    lsWorking[i].maxOutputMadeStart = lsWorking[i].maxOutputMade;
                    lsWorking[i].priceUpgrade = Math.Floor((double)(GameConfig.Instance.UN1i * lsWorking[i].price));
                    lsWorking[i].priceUpgradeStart = lsWorking[i].priceUpgrade;
                    lsWorking[i].priceTruckSent = 0;
                    lsWorking[i].priceTruckSentStart = lsWorking[i].priceTruckSent;
                    lsWorking[i].priceOutput = (double)GameConfig.Instance.productCost;
                    lsWorking[i].maxSent = Math.Floor(GameConfig.Instance.MaxSentStartX5 * lsWorking[i].maxOutputMade);
                    lsWorking[i].maxSentStart = lsWorking[i].maxSent;
                    lsWorking[i].priceUpgradeTruck = Math.Floor((double)(GameConfig.Instance.XN1i * lsWorking[i].price));
                    lsWorking[i].priceUpgradeTruckStart = lsWorking[i].priceUpgradeTruck;
                }
            }
        }
        else
        {
            for (int i = 0; i < lsWorking.Length; i++)
            {
                if (i == 0)
                {
                    lsWorking[i].UN2 = GameConfig.Instance.UN2;
                    lsWorking[i].price = (double)(GameConfig.Instance.p0);
                    lsWorking[i].maxOutputMade = (double)(GameConfig.Instance.c0);
                    lsWorking[i].maxOutputMadeStart = lsWorking[i].maxOutputMade;
                    lsWorking[i].priceUpgrade = Math.Floor((double)(GameConfig.Instance.UN1i * lsWorking[i].price));
                    lsWorking[i].priceUpgradeStart = lsWorking[i].priceUpgrade;
                    lsWorking[i].priceTruckSent = 0;
                    lsWorking[i].priceTruckSentStart = lsWorking[i].priceTruckSent;
                    lsWorking[i].priceOutput = (double)GameConfig.Instance.productCost;
                    lsWorking[i].maxSent = Math.Floor(GameConfig.Instance.MaxSentStartX5 * lsWorking[i].maxOutputMade);
                    lsWorking[i].maxSentStart = lsWorking[i].maxSent;
                    lsWorking[i].priceUpgradeTruck = Math.Floor((double)(GameConfig.Instance.XN1i * lsWorking[i].price));
                    lsWorking[i].priceUpgradeTruckStart = lsWorking[i].priceUpgradeTruck;
                }
                else
                {
                    lsWorking[i].UN2 = GameConfig.Instance.UN2;
                    lsWorking[i].price = Math.Floor((double)(GameConfig.Instance.p0i * lsWorking[i - 1].price));
                    lsWorking[i].maxOutputMade = Math.Floor((double)(GameConfig.Instance.c0i * lsWorking[i - 1].maxOutputMadeStart));
                    lsWorking[i].maxOutputMadeStart = lsWorking[i].maxOutputMade;
                    lsWorking[i].priceUpgrade = Math.Floor((double)(GameConfig.Instance.UN1i * lsWorking[i].price));
                    lsWorking[i].priceUpgradeStart = lsWorking[i].priceUpgrade;
                    lsWorking[i].priceTruckSent = 0;
                    lsWorking[i].priceTruckSentStart = lsWorking[i].priceTruckSent;
                    lsWorking[i].priceOutput = (double)GameConfig.Instance.productCost;
                    lsWorking[i].maxSent = Math.Floor(GameConfig.Instance.MaxSentStartX5 * lsWorking[i].maxOutputMade);
                    lsWorking[i].maxSentStart = lsWorking[i].maxSent;
                    lsWorking[i].priceUpgradeTruck = Math.Floor((double)(GameConfig.Instance.XN1i * lsWorking[i].price));
                    lsWorking[i].priceUpgradeTruckStart = lsWorking[i].priceUpgradeTruck;
                }
            }
        }
        isLoaded = true;
        isLoadFull = true;
        //if (!UIManager.Instance.isContinue)
        //{
        //    ScenesManager.Instance.isNextScene = true;
        //    GameManager.Instance.sumHomeAll += lsWorking.Length;
        //}
    }


    public void CheckInfoTypeOfWorkST(int idType)
    {
        indexType = idType;
        float x2 = lsWorking[indexType].isUpgradeMachineJob ? 2 : 1;
        UIManager.Instance.UpdateInfoUpgradeJob(
            lsWorking[indexType].name,
            lsWorking[indexType].level,
            lsWorking[indexType].level + 1,
            (Math.Floor(lsWorking[indexType].maxOutputMade * GameConfig.Instance.r)),
            (Math.Floor(lsWorking[indexType].maxOutputMade * GameConfig.Instance.r +
            (x2 * lsWorking[indexType].maxOutputMadeStart * GameConfig.Instance.r / GameConfig.Instance.capIndex))),
            lsWorking[indexType].priceUpgrade
        );
        UIManager.Instance.JobUpgrade.SetActive(true);
        if (lsWorking[idType].isISO)
        {
            UIManager.Instance.btnISO.SetActive(false);
        }
        else
        {
            UIManager.Instance.btnISO.SetActive(true);

        }
        if (lsWorking[idType].isUpgradeMachineJob)
        {
            UIManager.Instance.btnUpMachineJob.SetActive(false);
        }
        else
        {
            UIManager.Instance.btnUpMachineJob.SetActive(true);

        }
        UIManager.Instance.arrXJob[0].transform.GetChild(1).gameObject.SetActive(true);
        UIManager.Instance.arrXJob[1].transform.GetChild(1).gameObject.SetActive(false);
        UIManager.Instance.arrXJob[0].color = new Color32(255, 255, 255, 255);
        UIManager.Instance.arrXJob[1].color = new Color32(255, 255, 255, 128);
        UIManager.Instance.isJobX10 = false;
    }
    public void UpgradeInfoTypeOfWorkST(int idType)
    {
        indexType = idType;
        float x2 = lsWorking[indexType].isUpgradeMachineJob ? 2 : 1;

        if (GameManager.Instance.dollar >= lsWorking[indexType].priceUpgrade)
        {
            GameManager.Instance.AddDollar(-lsWorking[indexType].priceUpgrade);
            // Update thông số
            lsWorking[indexType].level++;
            lsWorking[indexType].maxOutputMade = Math.Floor((double)(Math.Floor(lsWorking[indexType].maxOutputMade +
            (x2 * lsWorking[indexType].maxOutputMadeStart / GameConfig.Instance.capIndex))));
            lsWorking[indexType].priceUpgrade = Math.Floor((double)((double)lsWorking[indexType].priceUpgrade * (1 + GameConfig.Instance.UN2)));
            lsWorking[indexType].textLevel.text = UIManager.Instance.ConvertNumber(lsWorking[indexType].level);

            UIManager.Instance.UpdateInfoUpgradeJob(
                lsWorking[indexType].name,
                lsWorking[indexType].level,
                lsWorking[indexType].level + 1,
                (Math.Floor(lsWorking[indexType].maxOutputMade * GameConfig.Instance.r)),
                (Math.Floor(lsWorking[indexType].maxOutputMade * GameConfig.Instance.r +
                (x2 * lsWorking[indexType].maxOutputMadeStart * GameConfig.Instance.r / GameConfig.Instance.capIndex))),
                lsWorking[indexType].priceUpgrade
            );
            if (GameManager.Instance.dollar < lsWorking[indexType].priceUpgrade)
            {
                UIManager.Instance.btnUpgradeJob.interactable = false;
            }
        }
        else
        {
            UIManager.Instance.btnUpgradeJob.interactable = false;
        }
    }
    public void CheckInfoTypeOfWorkSTX10(int idType)
    {
        indexType = idType;
        float x2 = lsWorking[indexType].isUpgradeMachineJob ? 2 : 1;

        int level = lsWorking[indexType].level;
        double priceUpgradeTotal = lsWorking[indexType].priceUpgrade;
        double priceUpgradeCurrent = lsWorking[indexType].priceUpgrade;
        double maxOutputMadeCurrent = lsWorking[indexType].maxOutputMade;

        for (int i = level + 1; i < (level + 10); i++)
        {
            priceUpgradeCurrent = Math.Floor((double)(priceUpgradeCurrent * (1 + GameConfig.Instance.UN2)));
            priceUpgradeTotal += priceUpgradeCurrent;
            maxOutputMadeCurrent = Math.Floor((double)(Math.Floor(maxOutputMadeCurrent + (x2 * lsWorking[indexType].maxOutputMadeStart / GameConfig.Instance.capIndex))));
        }

        UIManager.Instance.UpdateInfoUpgradeJob(
            lsWorking[indexType].name,
            lsWorking[indexType].level,
            lsWorking[indexType].level + 10,
            (Math.Floor(lsWorking[indexType].maxOutputMade * GameConfig.Instance.r)),
            (Math.Floor((double)(maxOutputMadeCurrent * GameConfig.Instance.r))),
            priceUpgradeTotal
        );
        UIManager.Instance.JobUpgrade.SetActive(true);
    }
    public void UpgradeInfoTypeOfWorkSTX10(int idType)
    {
        indexType = idType;
        float x2 = lsWorking[indexType].isUpgradeMachineJob ? 2 : 1;

        int level = lsWorking[indexType].level;
        double priceUpgradeTotal = lsWorking[indexType].priceUpgrade;
        double priceUpgradeCurrent = lsWorking[indexType].priceUpgrade;
        double maxOutputMadeCurrent = lsWorking[indexType].maxOutputMade;

        for (int i = level + 1; i < (level + 10); i++)
        {
            priceUpgradeCurrent = Math.Floor((double)(priceUpgradeCurrent * (1 + GameConfig.Instance.UN2)));
            priceUpgradeTotal += priceUpgradeCurrent;
            maxOutputMadeCurrent = Math.Floor((double)(Math.Floor(maxOutputMadeCurrent + (x2 * lsWorking[indexType].maxOutputMadeStart / GameConfig.Instance.capIndex))));

        }

        if (GameManager.Instance.dollar >= priceUpgradeTotal)
        {
            GameManager.Instance.AddDollar(-priceUpgradeTotal);
            // Update thông số
            lsWorking[indexType].level += 10;
            lsWorking[indexType].maxOutputMade = maxOutputMadeCurrent;
            lsWorking[indexType].priceUpgrade = priceUpgradeCurrent;
            lsWorking[indexType].textLevel.text = UIManager.Instance.ConvertNumber(lsWorking[indexType].level);

            level = lsWorking[indexType].level;
            priceUpgradeTotal = lsWorking[indexType].priceUpgrade;
            priceUpgradeCurrent = lsWorking[indexType].priceUpgrade;
            maxOutputMadeCurrent = lsWorking[indexType].maxOutputMade;
            for (int i = level + 1; i < (level + 10); i++)
            {
                priceUpgradeCurrent = Math.Floor((double)(priceUpgradeCurrent * (1 + GameConfig.Instance.UN2)));
                priceUpgradeTotal += priceUpgradeCurrent;
                maxOutputMadeCurrent = Math.Floor((double)(Math.Floor(maxOutputMadeCurrent + (x2 * lsWorking[indexType].maxOutputMadeStart / GameConfig.Instance.capIndex))));

            }

            UIManager.Instance.UpdateInfoUpgradeJob(
                lsWorking[indexType].name,
                lsWorking[indexType].level,
                lsWorking[indexType].level + 10,
                (Math.Floor(lsWorking[indexType].maxOutputMade * GameConfig.Instance.r)),
                (Math.Floor((double)(maxOutputMadeCurrent * GameConfig.Instance.r))),
                priceUpgradeTotal
            );

            if (GameManager.Instance.dollar < priceUpgradeTotal)
            {
                UIManager.Instance.btnUpgradeJob.interactable = false;
            }
        }
        else
        {
            UIManager.Instance.btnUpgradeJob.interactable = false;
        }
    }


    public void CheckInfoTruck(int idType)
    {
        if (PlayerPrefs.GetInt("isTutorial") == 0 && !UIManager.Instance.isClickTrunk)
            return;
        indexType = idType;
        float x2 = lsWorking[indexType].isUpgradeMachineTrunk ? 2 : 1;

        UIManager.Instance.UpdateInfoUpgradeTruck(
            lsWorking[indexType].name,
            lsWorking[indexType].levelTruck,
            lsWorking[indexType].levelTruck + 1,
            lsWorking[indexType].maxSent,
            (Math.Floor((double)(lsWorking[indexType].maxSent + x2 * lsWorking[indexType].maxSentStart / (double)GameConfig.Instance.captruckIndex))),
            lsWorking[indexType].priceUpgradeTruck
        );
        UIManager.Instance.TruckUpgrade.SetActive(true);
        if (lsWorking[idType].isUpgradeMachineTrunk)
        {
            UIManager.Instance.btnUpMachineTrunk.SetActive(false);
        }
        else
        {
            UIManager.Instance.btnUpMachineTrunk.SetActive(true);

        }
        UIManager.Instance.arrXTrunk[0].transform.GetChild(1).gameObject.SetActive(true);
        UIManager.Instance.arrXTrunk[1].transform.GetChild(1).gameObject.SetActive(false);
        UIManager.Instance.arrXTrunk[0].color = new Color32(255, 255, 255, 255);
        UIManager.Instance.arrXTrunk[1].color = new Color32(255, 255, 255, 128);
        UIManager.Instance.isTrunkX10 = false;
        if (idType == 0)
        {
            if (PlayerPrefs.GetInt("isTutorial") == 0)
            {
                GameConfig.Instance.TruckSpeed = UIManager.Instance.speedTrunkTutorial;
                if (UIManager.Instance.objTutorial != null)
                {
                    Destroy(UIManager.Instance.objTutorial);
                }
                UIManager.Instance.ControlHandTutorial(UIManager.Instance.btnUpgradeTrunk.transform);
                UIManager.Instance.txtWait.text = "Upgrade the capacity of the truck";
            }
        }
    }
    public void UpgradeInfoTruck(int idType)
    {
        indexType = idType;
        float x2 = lsWorking[indexType].isUpgradeMachineTrunk ? 2 : 1;

        if (GameManager.Instance.dollar >= lsWorking[indexType].priceUpgradeTruck)
        {
            GameManager.Instance.AddDollar(-lsWorking[indexType].priceUpgradeTruck);
            // Update thông số
            lsWorking[indexType].levelTruck++;
            lsWorking[indexType].maxSent = Math.Floor((double)(lsWorking[indexType].maxSent + x2 * lsWorking[indexType].maxSentStart / (double)GameConfig.Instance.captruckIndex));
            lsWorking[indexType].priceUpgradeTruck = Math.Floor((double)(lsWorking[indexType].priceUpgradeTruck * (1 + GameConfig.Instance.XN2)));
            lsWorking[indexType].truckManager.txtLevel.text = UIManager.Instance.ConvertNumber(lsWorking[indexType].levelTruck);
            UIManager.Instance.UpdateInfoUpgradeTruck(
                lsWorking[indexType].name,
                lsWorking[indexType].levelTruck,
                lsWorking[indexType].levelTruck + 1,
                lsWorking[indexType].maxSent,
                (Math.Floor((double)(lsWorking[indexType].maxSent + x2 * lsWorking[indexType].maxSentStart / (double)GameConfig.Instance.captruckIndex))),
                lsWorking[indexType].priceUpgradeTruck
            );
            if (GameManager.Instance.dollar < lsWorking[indexType].priceUpgradeTruck)
            {
                UIManager.Instance.btnUpgradeTrunk.interactable = false;
            }
        }
        else
        {
            UIManager.Instance.btnUpgradeTrunk.interactable = false;
        }
    }
    public void CheckInfoTruckX10(int idType)
    {
        indexType = idType;
        float x2 = lsWorking[indexType].isUpgradeMachineTrunk ? 2 : 1;

        int levelTruck = lsWorking[indexType].levelTruck;
        double priceUpgradeTruckTotal = lsWorking[indexType].priceUpgradeTruck;
        double priceUpgradeTruckCurrent = lsWorking[indexType].priceUpgradeTruck;
        double maxSentCurrent = lsWorking[indexType].maxSent;
        for (int i = levelTruck + 1; i < (levelTruck + 10); i++)
        {
            priceUpgradeTruckCurrent = Math.Floor((double)(priceUpgradeTruckCurrent * (1 + GameConfig.Instance.XN2)));
            priceUpgradeTruckTotal += priceUpgradeTruckCurrent;
            maxSentCurrent = Math.Floor((double)(maxSentCurrent + x2 * lsWorking[indexType].maxSentStart / (double)GameConfig.Instance.captruckIndex));
        }

        UIManager.Instance.UpdateInfoUpgradeTruck(
                lsWorking[indexType].name,
                lsWorking[indexType].levelTruck,
                lsWorking[indexType].levelTruck + 10,
                lsWorking[indexType].maxSent,
                maxSentCurrent,
                priceUpgradeTruckTotal
            );
        UIManager.Instance.TruckUpgrade.SetActive(true);
    }
    public void UpgradeInfoTruckX10(int idType)
    {
        indexType = idType;
        float x2 = lsWorking[indexType].isUpgradeMachineTrunk ? 2 : 1;

        int levelTruck = lsWorking[indexType].levelTruck;
        double priceUpgradeTruckTotal = lsWorking[indexType].priceUpgradeTruck;
        double priceUpgradeTruckCurrent = lsWorking[indexType].priceUpgradeTruck;

        double maxSentCurrent = lsWorking[indexType].maxSent;
        for (int i = levelTruck + 1; i < (levelTruck + 10); i++)
        {
            priceUpgradeTruckCurrent = Math.Floor((double)(priceUpgradeTruckCurrent * (1 + GameConfig.Instance.XN2)));
            priceUpgradeTruckTotal += priceUpgradeTruckCurrent;
            maxSentCurrent = Math.Floor((double)(maxSentCurrent + x2 * lsWorking[indexType].maxSentStart / (double)GameConfig.Instance.captruckIndex));
        }

        if (GameManager.Instance.dollar >= priceUpgradeTruckTotal)
        {
            GameManager.Instance.AddDollar(-priceUpgradeTruckTotal);
            // Update thông số
            lsWorking[indexType].levelTruck += 10;
            lsWorking[indexType].maxSent = maxSentCurrent;
            lsWorking[indexType].priceUpgradeTruck = priceUpgradeTruckCurrent;
            lsWorking[indexType].truckManager.txtLevel.text = UIManager.Instance.ConvertNumber(lsWorking[indexType].levelTruck);

            levelTruck = lsWorking[indexType].levelTruck;
            priceUpgradeTruckTotal = lsWorking[indexType].priceUpgradeTruck;
            priceUpgradeTruckCurrent = lsWorking[indexType].priceUpgradeTruck;

            maxSentCurrent = lsWorking[indexType].maxSent;
            for (int i = levelTruck + 1; i < (levelTruck + 10); i++)
            {
                priceUpgradeTruckCurrent = Math.Floor((double)(priceUpgradeTruckCurrent * (1 + GameConfig.Instance.XN2)));
                priceUpgradeTruckTotal += priceUpgradeTruckCurrent;
                maxSentCurrent = Math.Floor((double)(maxSentCurrent + x2 * lsWorking[indexType].maxSentStart / (double)GameConfig.Instance.captruckIndex));
            }

            UIManager.Instance.UpdateInfoUpgradeTruck(
               lsWorking[indexType].name,
               lsWorking[indexType].levelTruck,
               lsWorking[indexType].levelTruck + 10,
               lsWorking[indexType].maxSent,
               maxSentCurrent,
               priceUpgradeTruckTotal
           );
            if (GameManager.Instance.dollar < priceUpgradeTruckTotal)
            {
                UIManager.Instance.btnUpgradeTrunk.interactable = false;
            }
        }
        else
        {
            UIManager.Instance.btnUpgradeTrunk.interactable = false;
        }
    }


    public void SellJob()
    {
        if (GameManager.Instance.dollar >= lsWorking[countType + 1].price)
        {
            GameManager.Instance.AddDollar(-lsWorking[countType + 1].price);
            countType++;
            GameManager.Instance.sumHomeAll++;
            indexType = countType;
            lsWorking[countType].icon.color = Color.white;
            lsWorking[countType].animLock.gameObject.SetActive(false);
            lsWorking[countType].info.SetActive(true);
            lsWorking[countType].textInput.text = UIManager.Instance.ConvertNumber(lsWorking[countType].input);
            lsWorking[countType].textOutput.text = UIManager.Instance.ConvertNumber(lsWorking[countType].output);
            lsWorking[countType].textLevel.text = UIManager.Instance.ConvertNumber(lsWorking[countType].level);
            lsWorking[countType].truckManager.txtLevel.text = UIManager.Instance.ConvertNumber(lsWorking[countType].levelTruck);
            if (countType + 1 == lsWorking.Length)
            {
                if (id + 1 < UIManager.Instance.lsLocationUI.Count)
                {
                    int indexLsLocation = GameManager.Instance.lsLocation.Count;
                    GameManager.Instance.CreatLocation(UIManager.Instance.lsLocationUI[indexLsLocation]);
                    UIManager.Instance.handWorld.position = UIManager.Instance.lsLocationUI[indexLsLocation].transform.GetChild(0).position - new Vector3(0f, 0.25f, 0f);

                }
            }
            else
            {
                if (GameManager.Instance.dollar >= lsWorking[countType + 1].price)
                {
                    lsWorking[countType + 1].animLock.enabled = true;
                }
                if (id == GameManager.Instance.lsLocation.Count - 1)
                {
                    double maxOutputMadeRevenue = 0;

                    for (int i = 0; i < GameManager.Instance.lsLocation.Count; i++)
                    {
                        if (GameManager.Instance.lsLocation[i].countType == -1)
                        {
                            maxOutputMadeRevenue += 0;
                        }
                        else
                        {
                            maxOutputMadeRevenue += GameManager.Instance.lsLocation[i]
                            .lsWorking[GameManager.Instance.lsLocation[i].countType].maxOutputMade
                            * GameConfig.Instance.r
                            * GameConfig.Instance.productCost;
                        }
                    }
                    UIManager.Instance.txtRevenue.text
                        = "Revenue : " + UIManager.Instance.ConvertNumber(maxOutputMadeRevenue) + "$/day";
                }
            }
            if (id > 0 && countType == 0)
            {
                GameManager.Instance.lsLocation[id - 1].forest.btnAutoPlant.gameObject.SetActive(true);
                GameManager.Instance.lsLocation[id - 1].forest.isOnBtnAutoPlant = true;
            }
            if (countType == 0)
            {
                UIManager.Instance.WarningForest.SetActive(true);
            }

        }
    }


    #region Felling
    public void FellingForest()
    {
        if (forest.tree > 0 && forest.forestClass.isGrowed)
        {
            if (lsWorking[0].isXJob)
            {
                forest.timeFelling += Time.deltaTime * (1f + GameConfig.Instance.WYS);
            }
            else
            {
                forest.timeFelling += Time.deltaTime;
            }
            if (forest.timeFelling >= (GameConfig.Instance.TimeForest / (float)forest.forestClass.lsTree.Length))
            {
                if (forest.tree > 0)
                {
                    forest.forestClass.lsTree[forest.forestClass.lsTree.Length - forest.tree].transform.GetChild(0).gameObject.SetActive(false);
                    forest.tree--;
                    if (id == GameManager.Instance.IDLocation && !forest.isAutoPlant)
                    {
                        if (forest.tree == 0 && !forest.forestClass.isGrowing && !UIManager.Instance.WarningForest.activeInHierarchy)
                        {
                            UIManager.Instance.WarningForest.SetActive(true);
                        }
                    }
                }
                if (forest.tree <= 0)
                {
                    forest.forestClass.ResetForest();
                }
                forest.timeFelling = 0;
            }
        }
    }
    public void Felling()
    {
        if (forest.tree > 0 && forest.forestClass.isGrowed)
        {
            if (lsWorking[0].isXJob)
            {
                lsWorking[0].timeWorking += Time.deltaTime * (1f + GameConfig.Instance.WYS);
            }
            else
            {
                lsWorking[0].timeWorking += Time.deltaTime;
            }
            if (lsWorking[0].timeWorking >= GameConfig.Instance.p0Time)
            {
                FellingComplete(false);
                lsWorking[0].timeWorking = 0;
            }
        }

        if (lsWorking[0].maxSent <= GameConfig.Instance.Carup * GameConfig.Instance.r * lsWorking[0].maxOutputMade)
        {
            lsWorking[0].timeCheckUpgradeMe += Time.deltaTime;
            if (lsWorking[0].timeCheckUpgradeMe >= 3f)
            {
                if (lsWorking[0].truckManager.upgradeMe.activeInHierarchy)
                {
                    lsWorking[0].truckManager.upgradeMe.SetActive(false);
                }
                else
                {
                    lsWorking[0].truckManager.upgradeMe.SetActive(true);
                }
                lsWorking[0].timeCheckUpgradeMe = 0;
            }
        }

    }

    public double FellingComplete(bool isMiniGame = true)
    {
        double outPutValue = Math.Floor((double)(GameConfig.Instance.r * lsWorking[0].maxOutputMade));
        if (isMiniGame)
        {
            lsWorking[0].countPlayer++;
            if (id == GameManager.Instance.IDLocation && !lsWorking[0].isClaim && lsWorking[0].countPlayer >= 50)
            {
                UIManager.Instance.animAchievement.enabled = true;
            }
        }
        if (id == UIManager.Instance.lsItem[4].idLocation && UIManager.Instance.lsItem[4].isOnItem)
        {
            if (id == UIManager.Instance.lsItem[0].idLocation && UIManager.Instance.lsItem[0].isOnItem)
            {
                if (UIManager.Instance.lsItem[0].indexType == 0)
                {
                    lsWorking[0].output += outPutValue * 4;
                }
                else
                {
                    lsWorking[0].output += outPutValue * 2;
                }
            }
            else
            {
                lsWorking[0].output += outPutValue * 2;
            }
        }
        else
        {
            if (id == UIManager.Instance.lsItem[0].idLocation && UIManager.Instance.lsItem[0].isOnItem && UIManager.Instance.lsItem[0].indexType == 0)
            {
                lsWorking[0].output += outPutValue * 2;
            }
            else
            {
                lsWorking[0].output += outPutValue;
            }

        }
        lsWorking[0].textOutput.text = UIManager.Instance.ConvertNumber(lsWorking[0].output);

        if (lsWorking[0].id < lsWorking.Length)
        {
            if (lsWorking[0].output > 0)
            {
                lsWorking[0].truckManager.LoadTruck();
            }
        }
        return outPutValue;
    }
    #endregion

    #region Job! Felling
    public void Job(int idType)
    {
        if (lsWorking[idType].input > 0)
        {
            if (lsWorking[idType].isXJob)
            {
                lsWorking[idType].timeWorking += Time.deltaTime * (1f + GameConfig.Instance.WYS);
            }
            else
            {
                lsWorking[idType].timeWorking += Time.deltaTime;
            }
            if (lsWorking[idType].timeWorking >= GameConfig.Instance.p0Time)
            {
                JobComplete(idType, false);
                lsWorking[idType].timeWorking = 0;
            }
        }

        if (lsWorking[idType].maxSent <= GameConfig.Instance.Carup * GameConfig.Instance.r * lsWorking[idType].maxOutputMade)
        {
            lsWorking[idType].timeCheckUpgradeMe += Time.deltaTime;
            if (lsWorking[idType].timeCheckUpgradeMe >= 3f)
            {
                if (lsWorking[idType].truckManager.upgradeMe.activeInHierarchy)
                {
                    lsWorking[idType].truckManager.upgradeMe.SetActive(false);
                }
                else
                {
                    lsWorking[idType].truckManager.upgradeMe.SetActive(true);
                }
                lsWorking[idType].timeCheckUpgradeMe = 0;
            }
        }

    }

    public double JobComplete(int idType, bool isMiniGame = true)
    {
        double materialCurrent = 0;
        double outPutValue = 0;
        if (id == UIManager.Instance.lsItem[0].idLocation && UIManager.Instance.lsItem[0].isOnItem && idType == UIManager.Instance.lsItem[0].indexType)
        {
            if (lsWorking[idType].input >= lsWorking[idType].maxOutputMade * 2)
            {
                materialCurrent = lsWorking[idType].maxOutputMade * 2;
            }
            else
            {
                materialCurrent = lsWorking[idType].input;
            }
            lsWorking[idType].input -= Math.Floor(materialCurrent);
            lsWorking[idType].textInput.text = UIManager.Instance.ConvertNumber(lsWorking[idType].input);
            outPutValue = Math.Floor((double)(GameConfig.Instance.r * materialCurrent));

            lsWorking[idType].output += outPutValue;
            lsWorking[idType].textOutput.text = UIManager.Instance.ConvertNumber(lsWorking[idType].output);
        }
        else
        {
            if (lsWorking[idType].input >= lsWorking[idType].maxOutputMade)
            {
                materialCurrent = lsWorking[idType].maxOutputMade;
            }
            else
            {
                materialCurrent = lsWorking[idType].input;
            }
            lsWorking[idType].input -= Math.Floor(materialCurrent);
            lsWorking[idType].textInput.text = UIManager.Instance.ConvertNumber(lsWorking[idType].input);
            outPutValue = Math.Floor((double)(GameConfig.Instance.r * materialCurrent));

            lsWorking[idType].output += outPutValue;
            lsWorking[idType].textOutput.text = UIManager.Instance.ConvertNumber(lsWorking[idType].output);
        }

        if (isMiniGame)
        {
            lsWorking[idType].countPlayer++;
            if (id == GameManager.Instance.IDLocation && !lsWorking[idType].isClaim && lsWorking[idType].countPlayer >= 50)
            {
                UIManager.Instance.animAchievement.enabled = true;
            }
        }

        if (lsWorking[idType].id < lsWorking.Length)
        {
            if (lsWorking[idType].output > 0)
            {
                lsWorking[idType].truckManager.LoadTruck();
            }
        }
        return outPutValue;
    }
    #endregion


    public void Update()
    {
        if (countType >= 0)
        {
            FellingForest();
            Felling();
        }
        for (int i = 1; i <= countType; i++)
        {
            Job(i);
        }

        if (!forest.isAutoPlant && forest.isOnBtnAutoPlant)
        {
            if (GameManager.Instance.dollar >= Math.Floor((double)(lsWorking[0].price * GameConfig.Instance.AutoPlant)))
            {
                forest.btnAutoPlant.interactable = true;
            }
            else
            {
                forest.btnAutoPlant.interactable = false;
            }
        }
        else
        {
            forest.btnAutoPlant.gameObject.SetActive(false);
        }
    }


    public void AutoPlant()
    {
        string str = "Do you want to spend " + UIManager.Instance.ConvertNumber(Math.Floor((double)(lsWorking[lsWorking.Length - 1].price * GameConfig.Instance.AutoPlant))) + "$ On Auto Reforestation in " + nameLocation;
        UIManager.Instance.PopupAutoPlant.SetActive(true);
        UIManager.Instance.PopupAutoPlant.GetComponent<AutoPlant>().AutoPlant_Onclick(str, () =>
        {
            if (GameManager.Instance.dollar >= Math.Floor((double)(lsWorking[0].price * GameConfig.Instance.AutoPlant)))
            {
                GameManager.Instance.AddDollar(-Math.Floor((double)(lsWorking[0].price * GameConfig.Instance.AutoPlant)));
                forest.isAutoPlant = true;
                forest.btnAutoPlant.interactable = false;
                UIManager.Instance.PopupAutoPlant.SetActive(false);
                forest.btnAutoPlant.gameObject.SetActive(false);
                if (forest.tree <= 0)
                {
                    forest.forestClass.RunCarGrow();
                    UIManager.Instance.WarningForest.SetActive(false);
                }
            }
        });
    }


    public void WordYourSelf(int idType)
    {
        UIManager.Instance.scene = TypeScene.MINIGAME;
        indexType = idType;
        GameManager.Instance.lsTypeMiniGame[indexTypeWork].lsMiniGame[indexType].miniGame.SetActive(true);
        lsWorking[indexType].isXJob = true;
        if (PlayerPrefs.GetInt("isTutorial") == 0 && UIManager.Instance.isClickHome)
        {
            if (UIManager.Instance.objTutorial != null)
            {
                Destroy(UIManager.Instance.objTutorial);
            }
            UIManager.Instance.ControlHandTutorial(UIManager.Instance.btnUpgradeTutorial);
            UIManager.Instance.txtWait.text = "Tap to Upgrade";
            UIManager.Instance.isClickHome = false;
        }
    }
    public void HomeOnclick(int idType)
    {
        indexType = idType;
        if (countType >= idType)
        {
            if (PlayerPrefs.GetInt("isTutorial") == 0 && !UIManager.Instance.isClickHome)
                return;
            WordYourSelf(idType);
        }
        else if (countType + 1 == idType)
        {
            if (PlayerPrefs.GetInt("isTutorial") == 0 && idType != 0)
                return;
            if (PlayerPrefs.GetInt("isTutorial") == 0)
            {
                if (!UIManager.Instance.popupTutorial.activeInHierarchy)
                {
                    UIManager.Instance.popupTutorial.SetActive(true);
                }
                if (UIManager.Instance.objTutorial != null)
                {
                    Destroy(UIManager.Instance.objTutorial);
                }
                UIManager.Instance.ControlHandTutorial(UIManager.Instance.btnSell.transform);
                UIManager.Instance.txtWait.text = "Buy Felling workshop";
            }
            UIManager.Instance.isJobX10 = false;
            int IDLocation = id;
            if (id >= 7)
            {
                IDLocation = 0;
            }
            UIManager.Instance.JobSell.SetActive(true);

            UIManager.Instance.UpdateInfoSellJob(
                GameManager.Instance.lsTypeMiniGame[IDLocation].lsMiniGame[indexType].name,
                GameManager.Instance.lsTypeMiniGame[IDLocation].lsMiniGame[indexType].info,
                lsWorking[idType].icon.sprite,
                lsWorking[idType].price
            );

        }
    }


    public void HideObject(GameObject obj, float time)
    {
        StartCoroutine(IEHideObject(obj, time));
    }
    public IEnumerator IEHideObject(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }

    public void ShowInfo()
    {
        UIManager.Instance.panelInfo.SetActive(true);
        UIManager.Instance.infoForest.text = GameManager.Instance.lsTypeMiniGame[id].info;
    }

    public void btnSelectTree()
    {
        UIManager.Instance.panelSeclectTree.SetActive(true);
    }

}
