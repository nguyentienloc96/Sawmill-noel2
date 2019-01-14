using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Maker : MonoBehaviour
{

    public bool isInput;
    public Transform cart;
    public Transform tree;
    public Transform treeMask;
    public Transform screw;
    public SpriteRenderer spTreeMask;
    public GameObject notification;
    public ParticleSystem particleEmissions;

    public GameObject tutorialHand;
    public Image imgBG;
    public Sprite[] spOutput;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private bool isTutorial;
    private bool isStop;

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
        treeMask.localScale = new Vector3(0.82f, 0.82f, 1f);

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
                    screw.localPosition -= new Vector3(0f, dis * 0.005f * Time.deltaTime, 0f);
                    tree.localPosition += new Vector3(0f, dis * 0.01f * Time.deltaTime, 0f);
                    treeMask.localPosition += new Vector3(dis * 0.03f * Time.deltaTime, 0f, 0f);
                }
                if (tree.position.y > posCheck.y)
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
            particleEmissions.Play();
            AudioManager.Instance.Play("Drill");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        particleEmissions.Stop();
        AudioManager.Instance.Stop("Drill");
        isRun = false;
    }

    public void LoadInput()
    {
        int ID = GameManager.Instance.IDLocation;
        spTreeMask.sprite = spOutput[GameManager.Instance.lsLocation[ID].makerType];
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
        isInput = false;

        particleEmissions.Stop();
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        yield return new WaitForSeconds(0.5f);
       
        treeMask.DOScale(Vector3.zero, 0.5f);
        treeMask.DOMove(tfEnd.position, 0.5f).OnComplete(() =>
        {
            GameManager.Instance.lsLocation[ID].JobComplete(IndexType);

            GameManager.Instance.AddOutPutMiniGame(IndexType);

            screw.localPosition = Vector3.zero;
            tree.localPosition = Vector3.zero;
            treeMask.localPosition = Vector3.zero;
            treeMask.localScale = new Vector3(0.82f,0.82f,1f);
            cart.localPosition = new Vector3(-1.5f, 0f, 0f);
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
