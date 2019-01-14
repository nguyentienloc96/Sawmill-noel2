using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LigninDissolvesCellulose : MonoBehaviour
{

    public bool isInput;
    public Transform cart;
    public Transform tree;
    public GameObject notification;
    public Animator anim;
    public Transform needle;
    public ParticleSystem particleEmissions;

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
            timeNeedle = 0;
            needle.DOLocalRotate(new Vector3(0f, 0f, Random.Range(-90f, 45f)), 1f);
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
        particleEmissions.Stop();
        AudioManager.Instance.Stop("Water");
        isRun = false;
        needle.DOLocalRotate(new Vector3(0f, 0f, 90f), 0.5f);
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
        particleEmissions.Stop();
        isRun = false;
        isInput = false;
        needle.DOLocalRotate(new Vector3(0f, 0f, 90f), 0.5f);
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        yield return new WaitForSeconds(0.5f);
        tree.DOScale(Vector3.zero, 0.5f);
        tree.DOMove(tfEnd.position, 0.5f).OnComplete(() =>
        {
            GameManager.Instance.lsLocation[ID].JobComplete(IndexType);

            GameManager.Instance.AddOutPutMiniGame(IndexType);

            cart.localPosition = new Vector3(-4f, 0f, 0f);
            needle.localEulerAngles = new Vector3(0f, 0f, 90f);
            tree.localPosition = Vector3.zero;
            tree.localScale = new Vector3(1.25f, 1.25f, 1f);
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
    }

    public void Help()
    {
        tutorialHand.SetActive(true);
    }
}
