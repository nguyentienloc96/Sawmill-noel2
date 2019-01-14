using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGiftDay : MonoBehaviour {
    public Text txtNameDay;
    public Image imgClaim;
    public Image imgClaimNot;
    public Image imgTick;
    public bool isClaim = false;
	// Use this for initialization
	void Start () {
		
	}

    public void Set_ClaimNow()
    {
        isClaim = true;
        imgClaim.enabled = true;
        imgClaimNot.enabled = false;
        imgTick.enabled = false;
    }

    public void Set_Claimed()
    {
        isClaim = false;
        imgClaim.enabled = false;
        imgClaimNot.enabled = true;
        imgTick.enabled = true;
    }

    public void Set_ClaimNot()
    {
        isClaim = false;
        gameObject.GetComponent<Button>().interactable = false;
        imgClaim.enabled = false;
        imgClaimNot.enabled = true;
        imgTick.enabled = false;
    }

    public void btnClick()
    {
        if (!isClaim)
        {
            return;
        }
        else{
            UIManager.Instance.btnGiftDay();
        }
            
    }
}
