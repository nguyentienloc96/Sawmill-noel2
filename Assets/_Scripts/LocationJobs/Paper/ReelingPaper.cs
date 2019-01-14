using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ReelingPaper : MonoBehaviour
{

    public bool isInput;
    public Transform cart;
    public GameObject notification;
    public Animator anim;
    public Animator animGear;
    public ParticleSystem particleEmissions;
    public GameObject tutorialHand;
    public Image imgBG;
    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
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
        cart.localScale = new Vector3(0.5f, 0.5f, 1f);

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
            animGear.speed = 2f;
            anim.enabled = true;
            particleEmissions.Play();
            AudioManager.Instance.Play("Debarking");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        animGear.speed = 1f;
        anim.enabled = false;
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
        particleEmissions.Stop();
        isRun = false;
        isInput = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        yield return new WaitForSeconds(0.5f);
        
        cart.DOScale(Vector3.zero, 0.5f);
        cart.DOMove(tfEnd.position, 0.5f).OnComplete(() =>
        {
            GameManager.Instance.lsLocation[ID].JobComplete(IndexType);

            GameManager.Instance.AddOutPutMiniGame(IndexType);

            cart.localPosition = new Vector3(0f, -1.25f, 0f);
            cart.localScale = new Vector3(0.5f, 0.5f, 1f);
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
    }
}
