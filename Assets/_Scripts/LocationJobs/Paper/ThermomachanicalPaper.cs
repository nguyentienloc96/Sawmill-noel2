using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThermomachanicalPaper : MonoBehaviour
{

    public bool isInput;
    public Transform cart;
    public Transform tree;
    public GameObject notification;
    public Animator anim;
    public Animator animFoam;
    public Animator animFlour;

    public ParticleSystem particleEmissions;
    public ParticleSystem particleLimbing;
    public GameObject tutorialHand;
    public Image imgBG;
    public Transform gear;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private bool time;
    private bool isTutorial;
    private bool isStop;

    public Transform tfStart;
    public Transform tfEnd;
    public Sprite iconOutPut;

    public void Start()
    {
        posCheck = transform.GetChild(0).position;
    }

    private void OnEnable()
    {
        int randomBG = Random.Range(0, UIManager.Instance.spBG.Length);
        imgBG.sprite = UIManager.Instance.spBG[randomBG];
        isTutorial = true;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
        {
            notification.SetActive(false);
            tree.gameObject.SetActive(true);
            LoadInput();
        }
        else
        {
            isStop = true;
            tree.gameObject.SetActive(false);
            notification.SetActive(true);
        }
    }

    public void Update()
    {
        if (!isStop)
        {
            if (isRun)
            {
                if (Input.mousePosition.y > posDown.y)
                {
                    float dis = Input.mousePosition.y - posDown.y;
                    cart.position += new Vector3(0f, dis * 0.01f * Time.deltaTime, 0f);
                    gear.localEulerAngles += new Vector3(0f, 0f, dis * 5f * Time.deltaTime);
                }
                if (cart.position.y > posCheck.y)
                {
                    CompleteJob();
                }
            }
        }
        else
        {
            if (GameManager.Instance.lsLocation[GameManager.Instance.IDLocation]
               .lsWorking[GameManager.Instance.lsLocation[GameManager.Instance.IDLocation].indexType].input > 0)
            {
                notification.SetActive(false);
                tree.gameObject.SetActive(true);
                LoadInput();
                isStop = false;
            }
        }
    }

    public void TapDown()
    {
        if (isInput)
        {
            animFoam.enabled = true;
            anim.enabled = true;
            particleEmissions.Play();
            AudioManager.Instance.Play("Water");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        anim.enabled = false;
        animFoam.enabled = false;
        particleEmissions.Stop();
        AudioManager.Instance.Stop("Water");
        isRun = false;
    }

    public void LoadInput()
    {
        cart.localPosition = new Vector3(-4f, 0f, 0f);
        cart.DOLocalMove(Vector3.zero, 1f).OnComplete(() =>
        {
            if (isTutorial)
            {
                tutorialHand.SetActive(true);
                isTutorial = false;
            }
            isInput = true;
        });
    }

    public void CompleteJob()
    {
        isInput = false;

        anim.enabled = false;
        animFoam.enabled = false;
        particleEmissions.Stop();
        particleLimbing.Play();
        animFlour.enabled = true;
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        gear.DOLocalRotate(gear.localEulerAngles + new Vector3(0f, 0f, 180f), 1.5f).OnComplete(() =>
        {
            particleLimbing.Stop();
            animFlour.Rebind();
            animFlour.enabled = false;
            double valueOutput = GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
            GameManager.Instance.AddOutPut(valueOutput, iconOutPut, tfStart.position, tfEnd.position);
            tutorialHand.SetActive(false);

            if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
            {
                LoadInput();
            }
            else
            {
                tree.gameObject.SetActive(false);
                notification.SetActive(true);
                isStop = true;
            }
        });
    }
}
