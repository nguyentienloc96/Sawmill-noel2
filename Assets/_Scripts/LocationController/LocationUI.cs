using UnityEngine;
using UnityEngine.UI;

public class LocationUI : MonoBehaviour
{

    public int id;
    public string nameLocationUI;
    public int indexTypeWork;
    public Image imageLocation;

    public void Start()
    {
        imageLocation.alphaHitTestMinimumThreshold = 0.5f;
    }

    public void OnclickLocation()
    {
        UIManager.Instance.animAchievement.Rebind();
        UIManager.Instance.animAchievement.enabled = false;
        UIManager.Instance.scene = TypeScene.LOCATION;
        AudioManager.Instance.Play("Click");
        GameManager.Instance.IDLocation = id;
        GameManager.Instance.LoadLocation();
        UIManager.Instance.worldManager.transform.SetAsFirstSibling();
        UIManager.Instance.btnAchievement.SetActive(true);
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
            UIManager.Instance.ControlHandTutorial(GameManager.Instance.lsLocation[0].lsWorking[0].icon.transform);
            UIManager.Instance.txtWait.text = "Tap to buy workshop";
        }
        if (!UIManager.Instance.locationManager.gameObject.activeInHierarchy)
            UIManager.Instance.locationManager.SetActive(true);

        if (!GameManager.Instance.lsLocation[id].forest.isAutoPlant && GameManager.Instance.lsLocation[id].countType >= 0)
        {
            if (GameManager.Instance.lsLocation[id].forest.tree == 0 && !GameManager.Instance.lsLocation[id].forest.forestClass.isGrowing)
            {
                UIManager.Instance.WarningForest.SetActive(true);
            }
            else
            {
                UIManager.Instance.WarningForest.SetActive(false);
            }
        }
        else
        {
            UIManager.Instance.WarningForest.SetActive(false);
        }

        for(int i = 0;i< GameManager.Instance.lsLocation[id].countType; i++)
        {
            if(!GameManager.Instance.lsLocation[id].lsWorking[i].isClaim && GameManager.Instance.lsLocation[id].lsWorking[i].countPlayer >= 50)
            {
                UIManager.Instance.animAchievement.enabled = true;
                break;
            }
        }
    }
}
