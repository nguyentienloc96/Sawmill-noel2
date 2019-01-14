using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Drying : MonoBehaviour
{
    public bool isInput;
    public Transform cart;
    public Transform tree;
    public Transform treeMask;
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
        treeMask.localScale = Vector3.one;

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
                    CompleteJob();
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

    public void CompleteJob()
    {
        isInput = false;

        anim.enabled = false;
        particleEmissions.Stop();
        needle.DOLocalRotate(new Vector3(0f, 0f, 90f), 0.5f);
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        treeMask.DOLocalMove(new Vector3(0f, 1.8f, 0f), 1f).OnComplete(() =>
        {
            treeMask.DOLocalRotate(new Vector3(0f, 0f, -45f), 0.25f).OnComplete(() =>
            {
               
                treeMask.DOScale(Vector3.zero, 0.5f);
                treeMask.DOMove(tfEnd.position, 0.5f).OnComplete(() => 
                {
                    GameManager.Instance.lsLocation[ID].JobComplete(IndexType);

                    GameManager.Instance.AddOutPutMiniGame(IndexType);

                    treeMask.localPosition = Vector3.zero;
                    treeMask.localEulerAngles = Vector3.zero;
                    treeMask.localScale = Vector3.one;
                    cart.localPosition = new Vector3(-4f, 0f, 0f);
                    treeMask.localPosition = Vector3.zero;
                    needle.localEulerAngles = new Vector3(0f, 0f, 90f);
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
        });
        
    }

    public void Help()
    {
        tutorialHand.SetActive(true);
    }
}
