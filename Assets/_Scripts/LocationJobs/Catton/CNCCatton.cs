using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CNCCatton : MonoBehaviour
{

    public bool isInput;
    public Transform cart;
    public SpriteRenderer paper1;
    public SpriteRenderer paper2;
    public GameObject notification;
    public Transform gear;
    public GameObject tutorialHand;
    public Image imgBG;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private Vector3 posCheckHand;
    private bool isTutorial;
    private bool isStop;

    public Transform tfStart;
    public Transform tfEnd;
    public Sprite iconOutPut;

    public void Start()
    {
        posCheck = transform.GetChild(0).position;
        posCheckHand = transform.GetChild(1).position;
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
            cart.gameObject.SetActive(true);
            LoadInput();
        }
        else
        {
            isStop = true;
            cart.gameObject.SetActive(false);
            notification.SetActive(true);
        }
    }

    public void Update()
    {
        if (!isStop)
        {
            if (isRun)
            {
                if (Input.mousePosition.y < posDown.y)
                {
                    float dis = Input.mousePosition.y - posDown.y;
                    Vector3 current = gear.localPosition;
                    current.y += dis * 0.1f * Time.deltaTime;
                    current.y = Mathf.Clamp(current.y, 0.2f, 2f);
                    gear.localPosition = current;
                }
                if (gear.position.y <= posCheck.y)
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
                cart.gameObject.SetActive(true);
                LoadInput();
                isStop = false;
            }
        }
    }

    public void TapDown()
    {
        if (isInput)
        {
            AudioManager.Instance.Play("Debarking");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        AudioManager.Instance.Stop("Debarking");
        isRun = false;
    }

    public void LoadInput()
    {

        cart.DOLocalMove(Vector3.zero, 0.5f).OnComplete(() =>
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
        cart.position = posCheck;
        isRun = false;
        isInput = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        paper1.enabled = false;
        paper2.enabled = true;
        gear.DOLocalMoveY(1f, 0.5f).OnComplete(() =>
        {
            cart.DOLocalMove(new Vector3(4f, 0f, 0f), 0.5f).OnComplete(() =>
            {
                cart.DOScale(Vector3.zero, 0.5f);
                cart.DOMove(tfEnd.position, 0.5f).OnComplete(() =>
                {
                    GameManager.Instance.lsLocation[ID].JobComplete(IndexType);

                    GameManager.Instance.AddOutPutMiniGame(IndexType);

                    cart.localPosition = new Vector3(-4f, 0f, 0f);
                    cart.localScale = new Vector3(1.5f, 1.5f, 1.5f);

                    paper1.enabled = true;
                    paper2.enabled = false;
                    tutorialHand.SetActive(false);
                    if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
                    {
                        LoadInput();
                    }
                    else
                    {
                        isStop = true;
                        cart.gameObject.SetActive(false);
                        notification.SetActive(true);
                    }
                });
            });
        });

    }

    public void Help()
    {
        tutorialHand.SetActive(true);
    }
}
