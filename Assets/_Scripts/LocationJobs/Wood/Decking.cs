using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Decking : MonoBehaviour
{
    public bool isInput;
    public Transform cart;
    public Transform[] tree;
    public GameObject notification;
    public Animator anim;
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
            particleEmissions.Play();
            AudioManager.Instance.Play("Debarking");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
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

    public void CompleteJob()
    {
        isInput = false;
        anim.enabled = false;
        particleEmissions.Stop();
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        tree[random].position = posCheck;
        tutorialHand.SetActive(false);
        if (random == 0)
        {
            tree[random].DOLocalMove(new Vector3(0f, 2.5f, 0f), 0.5f).OnComplete(() =>
            {
                tree[random].DOLocalRotate(new Vector3(0f, 0f, -45f), 0.5f).OnComplete(() =>
                {
                    
                    tree[random].DOScale(Vector3.zero, 0.5f);
                    tree[random].DOMove(tfEnd.position, 0.5f).OnComplete(() =>
                        CallBackDG(ID, IndexType)
                    );
                });
            });
        }
        else
        {
            tree[random].DOLocalMove(new Vector3(1.8f, 0f, 0f), 0.5f).OnComplete(() =>
            {
                tree[random].DOLocalRotate(new Vector3(0f, 0f, -45f), 0.5f).OnComplete(() =>
                {
                    
                    tree[random].DOScale(Vector3.zero, 0.5f);
                    tree[random].DOMove(tfEnd.position, 0.5f).OnComplete(() =>
                        CallBackDG(ID, IndexType)
                    );
                });
            });
        }

    }

    public void CallBackDG(int ID, int IndexType)
    {
        GameManager.Instance.lsLocation[ID].JobComplete(IndexType);

        GameManager.Instance.AddOutPutMiniGame(IndexType);

        cart.localPosition = new Vector3(-4f, 0f, 0f);
        random = Random.Range(0, tree.Length);
        ResetTree();
        if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
        {
            tree[random].gameObject.SetActive(true);
            LoadInput();
        }
        else
        {
            isStop = true;
            notification.SetActive(true);
        }
    }

    public void Help()
    {
        tutorialHand.SetActive(true);
    }

    public void ResetTree()
    {
        foreach (Transform obj in tree)
        {
            obj.localPosition = Vector3.zero;
            obj.localEulerAngles = Vector3.zero;
            obj.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            obj.gameObject.SetActive(false);
        }
    }
}
