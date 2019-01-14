using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public struct MiniGame
{
    public string name;
    public string info;
    public Text inputMiniGame;
    public Text outputMiniGame;
    public GameObject miniGame;
}

[System.Serializable]
public struct TypeMiniGame
{
    public string name;
    public string info;
    public MiniGame[] lsMiniGame;
}

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    #region DateTime
    [Header("DateTime")]
    public DateTime dateGame;
    public DateTime dateStartPlay;
    public List<Text> listDate;
    private float time;
    #endregion

    #region GamePlay
    [Header("GamePlay")]
    public double gold;
    public double dollar;
    public int sumHomeAll;
    public int indexSawmill;
    public int IDLocation;
    public int countSpin;

    public Transform locationManager;
    public List<Location> lsLocation;
    public GameObject[] lsItemLocation;
    public GameObject[] arrPrefabOther;
    public GameObject[] arrPrefabsStreet;


    #endregion

    #region MiniGame
    public TypeMiniGame[] lsTypeMiniGame;
    public GameObject effectAddOutput;
    #endregion
    public bool isReset;
    private void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
        LoadDate();
        if (isReset)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void LoadDate()
    {
        dateGame = DateTime.Now;
        SetDate();
    }

    public void SetDate()
    {
        string daystring = dateGame.Day.ToString("00");
        listDate[0].text = daystring;

        string monthstring = dateGame.Month.ToString("00");
        listDate[1].text = monthstring;

        string yearstring = dateGame.Year.ToString("0000");
        listDate[2].text = yearstring;
    }

    public void LoadLocation()
    {
        UIManager.Instance.txtRevenue.enabled = false;
        lsLocation[IDLocation].gameObject.SetActive(true);
        lsLocation[IDLocation].transform.localPosition = Vector3.zero;
        for (int animL = 0; animL < lsLocation[IDLocation].lsWorking.Length; animL++)
        {
            lsLocation[IDLocation].lsWorking[animL].anim.enabled = true;
        }
        for (int animR = 0; animR < lsLocation[IDLocation].rivers.arrAnim.Count; animR++)
        {
            lsLocation[IDLocation].rivers.arrAnim[animR].enabled = true;
        }
        for (int animO = 0; animO < lsLocation[IDLocation].others.arrAnim.Count; animO++)
        {
            lsLocation[IDLocation].others.arrAnim[animO].enabled = true;
        }
        for (int i = 0; i < lsLocation.Count; i++)
        {
            if (i != IDLocation)
            {
                lsLocation[i].transform.localPosition = new Vector3(3000f, 0f, 0f);
                for (int animLH = 0; animLH < lsLocation[i].lsWorking.Length; animLH++)
                {
                    lsLocation[i].lsWorking[animLH].anim.enabled = false;
                }
                for (int animRH = 0; animRH < lsLocation[i].rivers.arrAnim.Count; animRH++)
                {
                    lsLocation[i].rivers.arrAnim[animRH].enabled = false;
                }
                for (int animOH = 0; animOH < lsLocation[i].others.arrAnim.Count; animOH++)
                {
                    lsLocation[i].others.arrAnim[animOH].enabled = false;
                }
                if (lsLocation.Count > 5)
                {
                    if (i < (lsLocation.Count - 3))
                    {
                        lsLocation[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        lsLocation[i].gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void ClearLocation()
    {
        for (int i = 0; i < lsLocation.Count; i++)
        {
            UIManager.Instance.lsBtnLocationUI[lsLocation[i].id].interactable = false;
            Destroy(lsLocation[i].gameObject);
        }
        lsLocation.Clear();
    }

    public void CreatLocation(LocationUI locationUI, bool isStart = false)
    {
        GameObject objLocation = Instantiate(lsItemLocation[locationUI.indexTypeWork], locationManager);
        objLocation.name = locationUI.nameLocationUI;
        objLocation.transform.SetAsFirstSibling();
        objLocation.transform.localPosition = new Vector3(3000f, 0f, 0f);
        Location location = objLocation.GetComponent<Location>();
        location.id = locationUI.id;
        if (location.indexTypeWork == 0)
        {
            location.makerType = indexSawmill;
            indexSawmill++;
        }
        location.nameLocation = locationUI.nameLocationUI;
        UIManager.Instance.lsBtnLocationUI[location.id].interactable = true;
        lsLocation.Add(location);
        location.LoadLocation();
        location.lsWorking[0].animLock.enabled = true;
        if (isStart)
        {
            UIManager.Instance.txtRevenue.text = "Revenue : 0$/day";
        }
    }

    public void BonusAds(double dollarBonus, double goldBonus)
    {
        if (goldBonus > 0)
        {
            gold += goldBonus;
        }
        if (dollarBonus > 0)
        {
            AddDollar(+dollarBonus);
        }
    }

    public void AddDollar(double dollarBonus)
    {
        dollar += dollarBonus;
        if (dollar < 0)
        {
            dollar = 0;
        }

        if (lsLocation.Count < UIManager.Instance.lsBtnLocationUI.Count)
        {
            int IDLocationEnd = lsLocation.Count - 1;
            if (lsLocation[IDLocationEnd].countType <= (lsLocation[IDLocationEnd].lsWorking.Length - 1))
            {
                int CountTypeLocationEnd = lsLocation[IDLocationEnd].countType;
                if (dollar >= lsLocation[IDLocationEnd].lsWorking[CountTypeLocationEnd + 1].price)
                {
                    lsLocation[IDLocationEnd].lsWorking[CountTypeLocationEnd + 1].animLock.enabled = true;
                }
                else
                {
                    lsLocation[IDLocationEnd].lsWorking[CountTypeLocationEnd + 1].animLock.enabled = false;
                }
            }
        }
    }

    public void AddOutPut(double numberAddOutput, Sprite icon, Vector3 startMove, Vector3 endMove, UnityAction actionLoadScenesDone = null)
    {
        effectAddOutput.SetActive(true);
        effectAddOutput.transform.position = startMove;
        effectAddOutput.GetComponent<TextMesh>().text = "+" + UIManager.Instance.ConvertNumber(numberAddOutput);
        effectAddOutput.transform.GetChild(0).GetComponent<Image>().sprite = icon;
        effectAddOutput.transform.DOMove(startMove + new Vector3(0f, 1f, 0f), 0.5f).OnComplete(() =>
        {
            effectAddOutput.transform.DOMove(effectAddOutput.transform.position + new Vector3(0f, 0f, 1f), 0.25f).OnComplete(() =>
            {

                effectAddOutput.transform.DOMove(endMove, 0.25f).OnComplete(() =>
                {
                    effectAddOutput.SetActive(false);
                    actionLoadScenesDone();
                });
            });
        });
    }

    public IEnumerator IEAddNumber(double a, double b, Text txtA)
    {
        double c = b / 5f;

        for (int i = 0; i < 5; i++)
        {
            a += c;
            txtA.text = UIManager.Instance.ConvertNumber(a);

            yield return new WaitForSeconds(1f);
        }

        Math.Floor(a);
    }

    public void AddOutPutMiniGame(int IndexType)
    {
        int IDLocationMiniGame = IDLocation;
        if (IDLocation >= 7)
        {
            IDLocationMiniGame = 0;
        }
        lsTypeMiniGame[IDLocationMiniGame].lsMiniGame[IndexType].outputMiniGame.transform
            .DOScale(new Vector3(1.25f, 1.25f, 1.25f), 0.25f)
            .OnComplete(() => lsTypeMiniGame[IDLocationMiniGame].lsMiniGame[IndexType].outputMiniGame.transform
            .DOScale(Vector3.one, 0.25f));
    }
    private void Update()
    {
        time += Time.deltaTime;
        if (time >= GameConfig.Instance.p0Time)
        {
            int month = dateGame.Month;
            int year = dateGame.Year;
            dateGame = dateGame.AddDays(1f);
            SetDate();
            time = 0;
        }

        for (int i = 0; i < UIManager.Instance.lsItem.Length; i++)
        {
            if (UIManager.Instance.lsItem[i].isOnItem)
            {
                UIManager.Instance.lsItem[i].timeItem -= Time.deltaTime;
                if (i != 6)
                {
                    UIManager.Instance.lsItem[i].imgItem.fillAmount = UIManager.Instance.lsItem[i].timeItem / UIManager.Instance.lsItem[i].timeItemTatol;
                }
                else
                {
                    UIManager.Instance.imgCheckTime.fillAmount = UIManager.Instance.lsItem[i].timeItem / UIManager.Instance.lsItem[i].timeItemTatol;
                }
                if (UIManager.Instance.lsItem[i].timeItem <= 0)
                {
                    UIManager.Instance.lsItem[i].timeItem = 0;
                    UIManager.Instance.lsItem[i].isOnItem = false;
                    if (i != 6)
                    {
                        UIManager.Instance.lsItem[i].obj.SetActive(false);
                    }

                    if (i == 2)
                    {
                        lsLocation[UIManager.Instance.lsItem[i].idLocation].forest.isAutoPlant = false;
                    }
                    else if (i == 3)
                    {
                        GameConfig.Instance.TruckSpeed = GameConfig.Instance.TruckSpeed / 2f;
                    }
                    else if (i == 5)
                    {
                        lsLocation[UIManager.Instance.lsItem[i].idLocation].lsWorking[UIManager.Instance.lsItem[i].indexType].price
                        = lsLocation[UIManager.Instance.lsItem[i].idLocation].lsWorking[UIManager.Instance.lsItem[i].indexType].price * 2f;
                        UIManager.Instance.UpdateInfoSellJob(
                           lsTypeMiniGame[IDLocation].lsMiniGame[UIManager.Instance.lsItem[i].indexType].name,
                           lsTypeMiniGame[IDLocation].lsMiniGame[UIManager.Instance.lsItem[i].indexType].info,
                           lsLocation[UIManager.Instance.lsItem[i].idLocation].lsWorking[UIManager.Instance.lsItem[i].indexType].icon.sprite,
                           lsLocation[UIManager.Instance.lsItem[i].idLocation].lsWorking[UIManager.Instance.lsItem[i].indexType].price
                       );
                    }else if(i == 6)
                    {
                        countSpin++;
                        UIManager.Instance.txtCountSpinMain.text = "x" + countSpin;
                    }

                    else if (i == 7)
                    {
                        dollar += UIManager.Instance.lsItem[i].money * UIManager.Instance.lsItem[i].random;
                        UIManager.Instance.panelGiveXMoney.SetActive(true);
                        UIManager.Instance.txtInfoGiveXMoney.text = "Congratulation, You’ve received " + UIManager.Instance.ConvertNumber(UIManager.Instance.lsItem[i].money * UIManager.Instance.lsItem[i].random) + "$ from your investment!";
                        Invoke("HideGiveXMoney", 3f);
                    }
                }
            }
        }
    }

    public void RandomGive()
    {
        int r;
        int locationEnd = lsLocation.Count - 1;
        int jobEnd = lsLocation[locationEnd].countType;
        if (locationEnd <= 0)
        {
            if (jobEnd == -1)
            {
                r = 0;
            }
            else
            {
                //r = UnityEngine.Random.Range(0, 8);
                r = 7;
                // anh muốn test cái item nào anh thay ròng trên bằng cho số bất kì từ 0-6 là được anh nhá
            }
        }
        else
        {
            r = UnityEngine.Random.Range(0, 8);
            if (jobEnd == -1)
            {
                locationEnd--;
                jobEnd = lsLocation[locationEnd].countType;
            }
        }
        if (r == 0)
        {
            Ads.Instance.SuccessPlaneReciveDollar();
        }
        else if (r == 7)
        {
            countSpin++;
            UIManager.Instance.txtCountSpinMain.text = "x" + countSpin;
            UIManager.Instance.adsSpin.SetActive(false);
            UIManager.Instance.bgSpin.color = new Color32(255, 255, 255, 255);
            UIManager.Instance.txtCountSpin.text = "x" + countSpin;
            UIManager.Instance.panelGiveRandom.SetActive(true);
            UIManager.Instance.iconRandom.sprite = UIManager.Instance.spGiveSpin;

            UIManager.Instance.infoRandom.text = "You have been rewarded with a draw of lucky spin";
        }
        else if (r == 8)
        {
            UIManager.Instance.lsItem[r - 1].random = UnityEngine.Random.Range(100, 500);
            UIManager.Instance.lsItem[r - 1].money = dollar;
            UIManager.Instance.panelGiveXXXMoney.SetActive(true);
            UIManager.Instance.txtGiveXXXMoney.text = UIManager.Instance.lsItem[r - 1].infoItem.Replace("X", UIManager.Instance.lsItem[r - 1].random.ToString());

        }
        else
        {
            UIManager.Instance.lsItem[r - 1].idLocation = locationEnd;
            UIManager.Instance.lsItem[r - 1].indexType = jobEnd;
            UIManager.Instance.lsItem[r - 1].obj.SetActive(true);
            UIManager.Instance.lsItem[r - 1].timeItem = 30;
            UIManager.Instance.lsItem[r - 1].timeItemTatol = 30;
            UIManager.Instance.lsItem[r - 1].isOnItem = true;
            UIManager.Instance.lsItem[r - 1].txtNameItem.text = "";
            UIManager.Instance.panelGiveRandom.SetActive(true);
            UIManager.Instance.iconRandom.sprite = UIManager.Instance.lsItem[r - 1].imgItem.transform.GetChild(0).GetComponent<Image>().sprite;
        }


        if (r == 1)
        {
            UIManager.Instance.infoRandom.text = UIManager.Instance.lsItem[r - 1].infoItem.Replace("X", ((int)(UIManager.Instance.lsItem[r - 1].timeItemTatol / GameConfig.Instance.p0Time)).ToString());
        }
        else if (r == 2)
        {
            UIManager.Instance.infoRandom.text = UIManager.Instance.lsItem[r - 1].infoItem.Replace("X", ((int)(UIManager.Instance.lsItem[r - 1].timeItemTatol / GameConfig.Instance.p0Time)).ToString());
        }
        else if (r == 3)
        {
            UIManager.Instance.lsItem[r - 1].timeItem = 10 * 60;
            UIManager.Instance.lsItem[r - 1].timeItemTatol = 10 * 60;
            lsLocation[UIManager.Instance.lsItem[r - 1].idLocation].forest.isAutoPlant = true;
            if (lsLocation[UIManager.Instance.lsItem[r - 1].idLocation].forest.tree <= 0)
            {
                lsLocation[UIManager.Instance.lsItem[r - 1].idLocation].forest.forestClass.RunCarGrow();
                UIManager.Instance.WarningForest.SetActive(false);
            }
            UIManager.Instance.infoRandom.text = UIManager.Instance.lsItem[r - 1].infoItem.Replace("X", ((int)(UIManager.Instance.lsItem[r - 1].timeItemTatol / GameConfig.Instance.p0Time)).ToString());

        }
        else if (r == 4)
        {
            GameConfig.Instance.TruckSpeed = GameConfig.Instance.TruckSpeed * 2f;
            UIManager.Instance.infoRandom.text = UIManager.Instance.lsItem[r - 1].infoItem.Replace("X", ((int)(UIManager.Instance.lsItem[r - 1].timeItemTatol / GameConfig.Instance.p0Time)).ToString());

        }
        else if (r == 5)
        {
            UIManager.Instance.infoRandom.text = UIManager.Instance.lsItem[r - 1].infoItem.Replace("X", ((int)(UIManager.Instance.lsItem[r - 1].timeItemTatol / GameConfig.Instance.p0Time)).ToString());
        }
        else if (r == 6)
        {
            UIManager.Instance.lsItem[r - 1].idLocation = lsLocation.Count - 1;
            UIManager.Instance.lsItem[r - 1].indexType = lsLocation[locationEnd].countType + 1;
            lsLocation[UIManager.Instance.lsItem[r - 1].idLocation].lsWorking[UIManager.Instance.lsItem[r - 1].indexType].price
                = lsLocation[UIManager.Instance.lsItem[r - 1].idLocation].lsWorking[UIManager.Instance.lsItem[r - 1].indexType].price / 2f;
            UIManager.Instance.infoRandom.text = UIManager.Instance.lsItem[r - 1].infoItem.Replace("X", ((int)(UIManager.Instance.lsItem[r - 1].timeItemTatol / GameConfig.Instance.p0Time)).ToString());
            UIManager.Instance.UpdateInfoSellJob(
                lsTypeMiniGame[IDLocation].lsMiniGame[UIManager.Instance.lsItem[r - 1].indexType].name,
                lsTypeMiniGame[IDLocation].lsMiniGame[UIManager.Instance.lsItem[r - 1].indexType].info,
                lsLocation[UIManager.Instance.lsItem[r - 1].idLocation].lsWorking[UIManager.Instance.lsItem[r - 1].indexType].icon.sprite,
                lsLocation[UIManager.Instance.lsItem[r - 1].idLocation].lsWorking[UIManager.Instance.lsItem[r - 1].indexType].price
            );
        }

        Ads.Instance.panelPlane.SetActive(false);
        Invoke("HideGiveRandom", 3f);
    }

    public void HideGiveRandom()
    {
        UIManager.Instance.panelGiveRandom.SetActive(false);
    }

    public void HideGiveXMoney()
    {
        UIManager.Instance.panelGiveXMoney.SetActive(false);
    }

    public void GiveSpin()
    {
        countSpin++;
        UIManager.Instance.imgCheckTime.fillAmount = 0;
        UIManager.Instance.lsItem[6].timeItem = 0;
        UIManager.Instance.lsItem[6].timeItemTatol = 15 * 60;
        UIManager.Instance.lsItem[6].isOnItem = false;
        UIManager.Instance.adsSpin.SetActive(false);
        UIManager.Instance.bgSpin.color = new Color32(255, 255, 255, 255);
        UIManager.Instance.txtCountSpinMain.text = "x" + countSpin;
        UIManager.Instance.txtCountSpin.text = "x" + countSpin;
        UIManager.Instance.panelGiveRandom.SetActive(true);
        UIManager.Instance.iconRandom.sprite = UIManager.Instance.spGiveSpin;
        UIManager.Instance.infoRandom.text = "You have been rewarded with a draw of lucky spin";
        Invoke("HideGiveRandom", 3f);
    }

    public void BtnYesOnclick()
    {
        UIManager.Instance.lsItem[7].obj.SetActive(true);
        UIManager.Instance.lsItem[7].isOnItem = true;
        UIManager.Instance.lsItem[7].txtNameItem.text = "";
        UIManager.Instance.lsItem[7].timeItem = 365 * GameConfig.Instance.p0Time;
        UIManager.Instance.lsItem[7].timeItemTatol = 365 * GameConfig.Instance.p0Time;
        dollar -= UIManager.Instance.lsItem[7].money;
        UIManager.Instance.panelGiveXXXMoney.SetActive(false);
    }

    public void BtnNoOnclick()
    {
        UIManager.Instance.panelGiveXXXMoney.SetActive(false);

    }


    public double PriceHomeEnd()
    {
        int locationEnd = lsLocation.Count - 1;
        int jobEnd = lsLocation[locationEnd].countType;
        double dollarRecive = 0;
        if (lsLocation.Count > 1)
        {
            if (jobEnd == -1)
            {
                locationEnd--;
                jobEnd = lsLocation[locationEnd].countType;
            }
            dollarRecive = lsLocation[locationEnd].lsWorking[jobEnd].price;
        }
        else
        {
            if (jobEnd == -1)
            {
                dollarRecive = lsLocation[0].lsWorking[0].price;
            }
            else
            {
                dollarRecive = lsLocation[locationEnd].lsWorking[jobEnd].price;
            }
        }
        return dollarRecive;
    }
}
