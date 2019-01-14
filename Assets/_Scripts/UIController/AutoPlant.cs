using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AutoPlant : MonoBehaviour
{

    public Text txtInfo;
    public Button btnYes;

    public void AutoPlant_Onclick(string str, UnityAction actionYes)
    {
        txtInfo.text = str;
        btnYes.onClick.RemoveAllListeners();
        btnYes.onClick.AddListener(() =>
        {
            actionYes();
            this.gameObject.SetActive(false);
        });
    }

    public void btnNo()
    {
        this.gameObject.SetActive(false);
    }
}
