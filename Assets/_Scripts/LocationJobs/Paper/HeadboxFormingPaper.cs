using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadboxFormingPaper : MonoBehaviour
{

    public bool isInput;
    public Transform cart;
    public Transform tree;
    public Transform paperRoll;
    public GameObject notification;
    public Animator anim;

    public ParticleSystem[] particleLimbing;
    public GameObject tutorialHand;
    public Image imgBG;

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
        posCheck = transform.GetChild(0).GetChild(0).position;
    }

    private void OnEnable()
    {
        int randomBG = Random.Range(0, UIManager.Instance.spBG.Length);
        imgBG.sprite = UIManager.Instance.spBG[randomBG];
        isTutorial = true;
        paperRoll.localScale = new Vector3(60f, 60f, 1f);

        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        tree.localPosition = new Vector3(-1f, 0f, 0f);
        cart.localPosition = new Vector3(0f, 0f, 0f);
        paperRoll.localPosition = Vector3.zero;
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
                if (Input.mousePosition.x > posDown.x)
                {
                    float dis = Input.mousePosition.x - posDown.x;
                    cart.position += new Vector3(dis * 0.005f * Time.deltaTime, 0f, 0f);
                }
                if (cart.position.x > posCheck.x)
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
            anim.enabled = true;
            foreach (ParticleSystem ps in particleLimbing)
            {
                ps.Play();
            }
            AudioManager.Instance.Play("Water");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        anim.enabled = false;
        foreach (ParticleSystem ps in particleLimbing)
        {
            ps.Stop();
        }
        AudioManager.Instance.Stop("Water");
        isRun = false;
    }

    public void LoadInput()
    {
        tree.DOLocalMove(Vector3.zero, 1f).OnComplete(() =>
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
        anim.enabled = false;
        foreach (ParticleSystem ps in particleLimbing)
        {
            ps.Stop();
        }
        isRun = false;
        isInput = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        paperRoll.DOLocalMove(new Vector3(200, 0f, 0f), 1.5f).OnComplete(() =>
        {
           
            paperRoll.DOScale(Vector3.zero, 0.5f);
            paperRoll.DOMove(tfEnd.position, 0.5f).OnComplete(() =>
            {
                GameManager.Instance.lsLocation[ID].JobComplete(IndexType);

                GameManager.Instance.AddOutPutMiniGame(IndexType);

                tree.localPosition = new Vector3(-1f, 0f, 0f);
                cart.localPosition = new Vector3(0f, 0f, 0f);
                paperRoll.localPosition = Vector3.zero;
                paperRoll.localScale = new Vector3(60f,60f,1f);
                tutorialHand.SetActive(false);

                if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
                {
                    paperRoll.transform.localPosition = Vector3.zero;
                    LoadInput();
                }
                else
                {
                    tree.gameObject.SetActive(false);
                    notification.SetActive(true);
                    isStop = true;
                }
            });
        });

    }
}
