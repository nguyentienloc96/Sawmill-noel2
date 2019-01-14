using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class Debarking : MonoBehaviour
{
    public bool isInput;
    public Transform cart;
    public Transform[] tree;
    public GameObject notification;
    public Animator anim;
    public ParticleSystem particleDebarking;
    public ParticleSystem particleEmissions;
    public GameObject tutorialHand;
    public Image imgBG;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private int random;
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
        random = Random.Range(0, tree.Length);
        ResetTree();
        tree[random].localScale = Vector3.one;

        cart.localPosition = new Vector3(-4f, 0f, 0f);
        if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
        {
            tree[random].gameObject.SetActive(true);
            notification.SetActive(false);
            LoadInput();
        }
        else
        {
            isStop = true;
            tree[random].gameObject.SetActive(false);
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
            }
        }
        else
        {
            if (GameManager.Instance.lsLocation[GameManager.Instance.IDLocation]
               .lsWorking[GameManager.Instance.lsLocation[GameManager.Instance.IDLocation].indexType].input > 0)
            {
                random = Random.Range(0, tree.Length);
                ResetTree();
                tree[random].gameObject.SetActive(true);
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
            anim.enabled = true;
            particleDebarking.Play();
            particleEmissions.Play();
            AudioManager.Instance.Play("Debarking");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        anim.enabled = false;
        particleDebarking.Stop();
        particleEmissions.Stop();
        AudioManager.Instance.Stop("Debarking");
        isRun = false;
    }

    public void LoadInput()
    {
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
        particleDebarking.Stop();
        particleEmissions.Stop();
        isRun = false;
        isInput = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        yield return new WaitForSeconds(0.5f);
        
        tree[random].DOScale(Vector3.zero, 0.5f);
        tree[random].DOMove(tfEnd.position, 0.5f).OnComplete(() =>
        {
            GameManager.Instance.lsLocation[ID].JobComplete(IndexType);

            GameManager.Instance.AddOutPutMiniGame(IndexType);

            cart.localPosition = new Vector3(-4f, 0f, 0f);
            tree[random].localPosition = Vector3.zero;
            tree[random].localScale = Vector3.one;
            ResetTree();
            tutorialHand.SetActive(false);
            if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
            {
                random = Random.Range(0, tree.Length);
                tree[random].gameObject.SetActive(true);
                LoadInput();
            }
            else
            {
                isStop = true;
                notification.SetActive(true);
            }
        });
    }

    public void Help()
    {
        tutorialHand.SetActive(true);
    }

    public void ResetTree()
    {
        foreach (Transform obj in tree)
        {
            obj.gameObject.SetActive(false);
        }
    }
}
