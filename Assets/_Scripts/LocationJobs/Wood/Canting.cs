using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Canting : MonoBehaviour
{
    public bool isInput;
    public Transform cart;
    public Transform tree;
    public GameObject notification;
    public Animator anim;
    public ParticleSystem particleEmissions;
    public ParticleSystem particleCanting1;
    public ParticleSystem particleCanting2;

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
        tree.localScale = new Vector3(1.5f, 1.5f, 1f);

        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
        {
            tree.gameObject.SetActive(true);
            notification.SetActive(false);
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
            }
        }
        else
        {
            if (GameManager.Instance.lsLocation[GameManager.Instance.IDLocation]
               .lsWorking[GameManager.Instance.lsLocation[GameManager.Instance.IDLocation].indexType].input > 0)
            {
                tree.gameObject.SetActive(true);
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
            particleCanting1.Play();
            particleCanting2.Play();

            AudioManager.Instance.Play("Saw");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        anim.enabled = false;
        particleEmissions.Stop();
        particleCanting1.Stop();
        particleCanting2.Stop();

        AudioManager.Instance.Stop("Saw");
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
        particleCanting1.Stop();
        particleCanting2.Stop();
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        
        tree.GetChild(0).DOLocalRotate(new Vector3(0f, 0f, -45f), 0.5f);
        tree.GetChild(1).DOLocalRotate(new Vector3(0f, 0f, -45f), 0.5f).OnComplete(() =>
        {
            tree.DOScale(Vector3.zero, 0.5f);
            tree.DOMove(tfEnd.position, 0.5f).OnComplete(() => CallBackDG(ID, IndexType));
        });

    }

    public void CallBackDG(int ID, int IndexType)
    {

        GameManager.Instance.lsLocation[ID].JobComplete(IndexType);

        GameManager.Instance.AddOutPutMiniGame(IndexType);

        tree.localPosition = Vector3.zero;
        tree.GetChild(0).localEulerAngles = Vector3.zero;
        tree.GetChild(1).localEulerAngles = Vector3.zero;
        tree.localPosition = Vector3.zero;
        tree.localScale = new Vector3(1.5f, 1.5f, 1f);
        cart.localPosition = new Vector3(-4f, 0f, 0f);
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
    }

    public void Help()
    {
        tutorialHand.SetActive(true);
    }
}
