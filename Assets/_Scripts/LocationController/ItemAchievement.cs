using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ItemAchievement : MonoBehaviour
{
    public int IndexType;
    public Location location;
    public Text infoItem;
    public Button btnClaim;
    public Image clock;
    public Text Statistical;
    public Image imgStatistical;
    public Animator goldClaim;

    public void GetItemAchievement()
    {
        if (IndexType <= location.countType)
        {
            btnClaim.gameObject.SetActive(true);
            clock.gameObject.SetActive(false);

            if (location.lsWorking[IndexType].countPlayer >= 50)
            {
                btnClaim.interactable = true;
                goldClaim.enabled = true;
            }
            else
            {
                btnClaim.interactable = false;
            }
        }
        else
        {
            btnClaim.gameObject.SetActive(false);
            clock.gameObject.SetActive(true);
        }
        int IDLocation = location.id;
        if (location.id >= 7)
        {
            IDLocation = 0;
        }
        Statistical.text = location.lsWorking[IndexType].countPlayer + "/50";
        imgStatistical.fillAmount = ((float)(location.lsWorking[IndexType].countPlayer > 50 ? 50 : location.lsWorking[IndexType].countPlayer) / 50f);
        infoItem.text = "Work 50 shifts at " + GameManager.Instance.lsTypeMiniGame[IDLocation].lsMiniGame[IndexType].name + " Workshop";
    }

    public void BtnClaim()
    {
        AudioManager.Instance.Play("TingClaim");
        
        GameManager.Instance.AddOutPut(5,
            UIManager.Instance.spGoldClaim,
            transform.position,
            UIManager.Instance.txtGold.transform.position,
            () =>
                {
                    int count = 0;
                    GameManager.Instance.gold += 5;
                    for (int i = 0; i < location.countType; i++)
                    {
                        if (!location.lsWorking[i].isClaim && location.lsWorking[i].countPlayer >= 50)
                        {
                            count++;
                        }
                    }

                    if (count > 0)
                    {
                        UIManager.Instance.animAchievement.enabled = true;
                    }
                    else
                    {
                        UIManager.Instance.animAchievement.Rebind();
                        UIManager.Instance.animAchievement.enabled = false;
                    }
                    UIManager.Instance.txtGold.transform
                       .DOScale(new Vector3(1.25f, 1.25f, 1.25f), 0.25f)
                       .OnComplete(() => UIManager.Instance.txtGold.transform
                       .DOScale(Vector3.one, 0.25f));
                }         
            );
        location.lsWorking[IndexType].isClaim = true;
        Destroy(gameObject);
    }
}
