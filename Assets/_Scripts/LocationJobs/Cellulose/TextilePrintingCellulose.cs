using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TextilePrintingCellulose : MonoBehaviour
{

    public bool isInput;
    public Transform cart;
    public Transform tree;
    public GameObject notification;
    public Animator anim;
    public GameObject tutorialHand;
    public Image imgBG;
    public SpriteRenderer inputSpPaper;
    public SpriteRenderer spPaper;
    public Sprite[] inputarrspPaper;
    public Sprite[] arrspPaper;
    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private bool isTutorial;
    private bool isStop;
    private int randomPaper;

    public Transform tfStart;
    public Transform tfEnd;

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
            cart.localPosition = new Vector3(-4f, 0f, 0f);
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
                    Vector3 current = cart.position;
                    current.y += dis * 0.01f * Time.deltaTime;
                    current.y = Mathf.Clamp(current.y, -5f, posCheck.y + 0.01f);
                    cart.position = current;
                }
                if (cart.position.y >= posCheck.y)
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
            AudioManager.Instance.Play("Polish");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        anim.enabled = false;
        AudioManager.Instance.Stop("Polish");
        isRun = false;
    }

    public void LoadInput()
    {
        randomPaper = Random.Range(0, arrspPaper.Length);
        inputSpPaper.sprite = inputarrspPaper[randomPaper];
        spPaper.sprite = arrspPaper[randomPaper];
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

    public IEnumerator CompleteJob()
    {
        isRun = false;
        isInput = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        yield return new WaitForSeconds(0.5f);
        cart.DOLocalMoveY(6f, 0.5f).OnComplete(() =>
        {
            anim.enabled = false;
            
            tree.DOScale(Vector3.zero, 0.5f);
            tree.DOMove(tfEnd.position, 0.5f).OnComplete(() =>
            {
                GameManager.Instance.lsLocation[ID].JobComplete(IndexType);

                GameManager.Instance.AddOutPutMiniGame(IndexType);

                cart.localPosition = new Vector3(-4f, 0f, 0f);
                tree.localPosition = Vector3.zero;
                tree.localScale = Vector3.one;
                tutorialHand.SetActive(false);
                if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
                {
                    LoadInput();
                }
                else
                {
                    isStop = true;
                    tree.gameObject.SetActive(false);
                    notification.SetActive(true);
                }
            });
        });

    }

       public void Help()
    {
        tutorialHand.SetActive(true);
    }
}
