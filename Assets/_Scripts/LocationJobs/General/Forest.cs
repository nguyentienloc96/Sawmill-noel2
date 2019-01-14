using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Forest : MonoBehaviour
{
    public Location location;

    public int index = 0;

    public bool isRun;

    public GameObject car;

    public GameObject[] lsTree;

    private float speedCar = 1.5f;

    private bool isGrow;

    public bool isGrowed;

    public bool isGrowing;

    public void LoadTree()
    {
        if (location.forest.tree > 0)
        {
            car.SetActive(false);
            for (int i = 0; i < lsTree.Length; i++)
            {
                lsTree[i].transform.localScale = new Vector3(1f, 1f, 1f);
                if (i < lsTree.Length - location.forest.tree)
                {
                    lsTree[i].transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    lsTree[i].transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            isGrowed = true;
        }
        else
        {
            if (location.forest.isAutoPlant)
            {
                RunCarGrow();
            }
        }
    }

    public void ResetForest()
    {
        index = 0;
        car.SetActive(true);
        car.transform.GetChild(1).GetComponent<Image>().enabled = true;
        car.transform.position = lsTree[index].transform.position;
        car.transform.right = lsTree[index + 1].transform.position - lsTree[index].transform.position;

        for (int i = 0; i < transform.childCount; i++)
        {
            lsTree[i].transform.localScale = new Vector3(0f, 0f, 0f);
            lsTree[i].transform.GetChild(0).gameObject.SetActive(true);
        }
        isGrowed = false;

        if (location.forest.isAutoPlant)
        {
            RunCarGrow();
        }
    }

    public void BtnRunCarGrow()
    {
        if (!location.forest.isAutoPlant && location.countType >= 0)
        {
            RunCarGrow();
            if (PlayerPrefs.GetInt("isTutorial") == 0)
            {
                UIManager.Instance.txtWait.text = "Wait to plant trees";
            }
            if (location.id == GameManager.Instance.IDLocation)
            {
                if (UIManager.Instance.WarningForest.activeInHierarchy)
                {
                    UIManager.Instance.WarningForest.SetActive(false);
                }
            }
        }
    }

    public void RunCarGrow()
    {
        car.transform.GetChild(1).GetComponent<Image>().enabled = false;
        lsTree[index].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        isRun = true;
        isGrowing = true;
    }

    public void Update()
    {
        if (isRun)
        {
            car.transform.position = Vector3.MoveTowards(car.transform.position, lsTree[index + 1].transform.position, speedCar * Time.deltaTime);
            if (index + 1 < lsTree.Length && car.transform.position == lsTree[index + 1].transform.position)
            {
                index++;
                lsTree[index].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                if (index + 1 < lsTree.Length)
                {
                    car.transform.right = lsTree[index + 1].transform.position - lsTree[index].transform.position;
                    Vector3 angleCar = car.transform.localEulerAngles;
                    if (angleCar.z != 0)
                    {
                        if (index % 2 == 0)
                        {
                            angleCar.y = 180;
                        }
                        else
                        {
                            angleCar.y = 0;
                        }
                    }
                    angleCar.z = 0;

                    car.transform.localEulerAngles = angleCar;
                }
                else
                {
                    isRun = false;
                    car.SetActive(false);
                    isGrow = true;
                }
            }
        }
        else
        {
            if (isGrow)
            {
                GrowTrees();
            }
        }

        if (location.countType < 0)
        {
            if (car.transform.GetChild(0).GetComponent<Button>().interactable)
                car.transform.GetChild(0).GetComponent<Button>().interactable = false;
            if (car.transform.GetChild(1).gameObject.activeInHierarchy)
                car.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            if (!car.transform.GetChild(0).GetComponent<Button>().interactable)
                car.transform.GetChild(0).GetComponent<Button>().interactable = true;
            if (!car.transform.GetChild(1).gameObject.activeInHierarchy)
                car.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void GrowTrees()
    {
        for (int i = 0; i < lsTree.Length; i++)
        {
            if (i == 0)
            {
                lsTree[i].transform.DOScale(new Vector3(1f, 1f, 1f), GameConfig.Instance.growTime)
                    .OnComplete(() =>
                    {
                        isGrowed = true;
                        isGrowing = false;
                        location.forest.tree = lsTree.Length;
                        if (PlayerPrefs.GetInt("isTutorial") == 0)
                        {
                            UIManager.Instance.isClickHome = true;
                            if (!UIManager.Instance.popupTutorial.activeInHierarchy)
                            {
                                UIManager.Instance.popupTutorial.SetActive(true);
                            }
                            if (UIManager.Instance.objTutorial != null)
                            {
                                Destroy(UIManager.Instance.objTutorial);
                            }
                            UIManager.Instance.ControlHandTutorial(location.lsWorking[0].icon.transform);
                            UIManager.Instance.txtWait.text = "Tap the Felling Workshop";
                        }
                    });
            }
            else
            {
                lsTree[i].transform.DOScale(new Vector3(1f, 1f, 1f), GameConfig.Instance.growTime);
            }
        }
        isGrow = false;
    }
}
