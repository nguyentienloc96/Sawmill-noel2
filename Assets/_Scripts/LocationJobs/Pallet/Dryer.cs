using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class Dryer : MonoBehaviour
{
    public bool isInput;
    public Transform cart;
    public GameObject tree;
    public GameObject notification;
    public Animator anim;
    public Transform needle;
    public GameObject tutorialHand;
    public Image imgBG;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private float timeNeedle;
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
        tree.transform.localScale = new Vector3(1f, 1f, 1f);

        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
        {
            notification.SetActive(false);
            LoadInput();
        }
        else
        {
            isStop = true;
            tree.SetActive(false);
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
                }
                if (cart.position.y > posCheck.y)
                {
                    StartCoroutine(CompleteJob());
                }
                timeNeedle += Time.deltaTime;
                if (timeNeedle >= 2f)
                {
                    needle.DOLocalRotate(new Vector3(0f, 0f, Random.Range(-90f, 45f)), 1.5f);
                    timeNeedle = 0;
                }
            }
        }
        else
        {
            if (GameManager.Instance.lsLocation[GameManager.Instance.IDLocation]
               .lsWorking[GameManager.Instance.lsLocation[GameManager.Instance.IDLocation].indexType].input > 0)
            {
                notification.SetActive(false);
                LoadInput();
                isStop = false;
            }
        }
    }

    public void TapDown()
    {
        if (isInput)
        {
            timeNeedle = 0;
            needle.DOLocalRotate(new Vector3(0f, 0f, Random.Range(-90f, 45f)), 1f);
            anim.enabled = true;
            AudioManager.Instance.Play("Water");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        anim.enabled = false;
        AudioManager.Instance.Stop("Water");
        isRun = false;
        needle.DOLocalRotate(new Vector3(0f, 0f, 90f), 0.5f);
    }

    public void LoadInput()
    {
        tree.SetActive(true);
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
        anim.enabled = false;
        isRun = false;
        isInput = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        yield return new WaitForSeconds(0.5f);
       
        tree.transform.DOScale(Vector3.zero, 0.5f);
        tree.transform.DOMove(tfEnd.position, 0.5f).OnComplete(() =>
        {
            GameManager.Instance.lsLocation[ID].JobComplete(IndexType);

            GameManager.Instance.AddOutPutMiniGame(IndexType);

            tree.transform.localPosition = Vector3.zero;
            tree.transform.localScale = new Vector3(1f, 1f, 1f);
            cart.localPosition = new Vector3(-4f, 0f, 0f);
            needle.DOLocalRotate(new Vector3(0f, 0f, 90f), 0.5f);
            tutorialHand.SetActive(false);
            if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
            {
                tree.SetActive(false);
                LoadInput();
            }
            else
            {
                isStop = true;
                tree.SetActive(false);
                notification.SetActive(true);
            }
        });
    }

    public void Help()
    {
        tutorialHand.SetActive(true);
    }
}
