using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressingDryingPaper : MonoBehaviour
{

    public bool isInput;
    public Transform paper;
    public GameObject notification;
    public Transform gasPusher;
    public Transform posFoam;
    public Transform foam;
    public Transform paperInput;
    public Animator paperOutput;

    public SpriteRenderer spPaper;
    public Transform[] posMoveTree;
    public Sprite[] spTree;
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
        posCheck = transform.GetChild(0).position;
    }

    private void OnEnable()
    {
        int randomBG = Random.Range(0, UIManager.Instance.spBG.Length);
        imgBG.sprite = UIManager.Instance.spBG[randomBG];
        isTutorial = true;
        paperOutput.transform.localScale = new Vector3(1f, 1f, 1f);

        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
        {
            notification.SetActive(false);
            paper.gameObject.SetActive(true);
            StartCoroutine(LoadInput());
        }
        else
        {
            isStop = true;
            paper.gameObject.SetActive(false);
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
                    float scaleYGasPusher = gasPusher.localScale.y;
                    scaleYGasPusher -= 0.01f * dis * Time.deltaTime;
                    if (scaleYGasPusher >= 1.57f)
                    {
                        scaleYGasPusher = 1.57f;
                    }
                    gasPusher.localScale = new Vector3(gasPusher.localScale.x, scaleYGasPusher, gasPusher.localScale.z);
                }
                if (gasPusher.localScale.y >= 1.57f)
                {
                    StartCoroutine(CompleteJob());
                }
            }
        }
        else
        {
            if (GameManager.Instance.lsLocation[GameManager.Instance.IDLocation]
               .lsWorking[GameManager.Instance.lsLocation[GameManager.Instance.IDLocation].indexType].input > 0)
            {
                isStop = false;
                notification.SetActive(false);
                paper.gameObject.SetActive(true);
                StartCoroutine(LoadInput());
                Debug.Log("1");
            }
        }

        foam.position = posFoam.position;
    }

    public void TapDown()
    {
        if (isInput)
        {
            AudioManager.Instance.Play("Water");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        AudioManager.Instance.Stop("Water");
        isRun = false;
    }

    public IEnumerator LoadInput()
    {
        int indexPos = 0;
        bool endNextPos = false;
        for (int i = 0; i < posMoveTree.Length; i++)
        {
            endNextPos = false;
            paper.DOLocalMove(Vector3.zero, 0.15f).OnComplete(() =>
            {
                indexPos++;
                if (indexPos < posMoveTree.Length)
                {
                    spPaper.sprite = spTree[indexPos];
                    paper.SetParent(posMoveTree[indexPos]);
                }
                endNextPos = true;
            });
            yield return new WaitUntil(() => endNextPos == true);
        }
        yield return new WaitUntil(() => indexPos == posMoveTree.Length);
        paperInput.DOScaleX(1f, 0.25f).OnComplete(() =>
        {
            if (isTutorial)
            {
                tutorialHand.SetActive(true);
                isTutorial = false;
            }
            isInput = true;
        });

    }

    public IEnumerator CompleteJob()
    {
        isInput = false;

        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;

        paperOutput.enabled = true;
        yield return new WaitForSeconds(0.6f);
        paperInput.localScale = new Vector3(0f, 1f, 1f);
        gasPusher.DOScaleY(1f, 0.5f).OnComplete(() =>
        {
            
            paperOutput.transform.DOScale(Vector3.zero, 0.5f);
            paperOutput.transform.DOMove(tfEnd.position, 0.5f).OnComplete(() =>
            {
                GameManager.Instance.lsLocation[ID].JobComplete(IndexType);

                GameManager.Instance.AddOutPutMiniGame(IndexType);

                paperOutput.transform.localPosition = new Vector3(1f, 0f, 0f);
                paperOutput.transform.localScale = new Vector3(1f, 1f, 1f);
                paperOutput.Rebind();
                paperOutput.enabled = false;
                spPaper.sprite = spTree[0];
                paper.SetParent(posMoveTree[0]);
                paper.localPosition = new Vector3(-2f, 0f, 0f);
                gasPusher.localScale = Vector3.one;
                tutorialHand.SetActive(false);
                if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
                {
                    StartCoroutine(LoadInput());
                }
                else
                {
                    paper.gameObject.SetActive(false);
                    notification.SetActive(true);
                    isStop = true;
                }
            });
        });
    }
}
