using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MillPellet : MonoBehaviour
{
    public bool isInput;
    public Transform tray;
    public GameObject notification;
    public Transform lever;
    public Transform flour;
    public MeshRenderer scroll;
    public GameObject tutorialHand;
    public Image imgBG;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private bool isTutorial;

    private float ScrollSpeed = 0.5f;
    private float Offset;
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
        tray.localScale = Vector3.one;

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
                    lever.localPosition += new Vector3(0f, dis * 0.018f * Time.deltaTime, 0f);
                    flour.localPosition -= new Vector3(dis * 0.015f * Time.deltaTime, 0f, 0f);
                    tray.localPosition -= new Vector3(dis * 0.05f * Time.deltaTime, 0f, 0f);
                    scroll.material.mainTextureOffset += new Vector2(dis * 0.005f * Time.deltaTime, 0f);
                }
                if (tray.position.x > posCheck.x)
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
            AudioManager.Instance.Play("Debarking");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        AudioManager.Instance.Stop("Debarking");
        isRun = false;
    }

    public void LoadInput()
    {
        if (isTutorial)
        {
            tutorialHand.SetActive(true);
            isTutorial = false;
        }
        isInput = true;
    }

    public void CompleteJob()
    {
        isInput = false;

        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        tray.position = posCheck;
        tray.GetChild(0).gameObject.SetActive(true);
        lever.DOLocalMove(new Vector3(2.5f, 1f, 0f), 1.5f).OnComplete(() =>
        {
            
            tray.DOScale(Vector3.zero, 0.5f);
            tray.DOMove(tfEnd.position, 0.5f).OnComplete(() =>
            {
                GameManager.Instance.lsLocation[ID].JobComplete(IndexType);

                GameManager.Instance.AddOutPutMiniGame(IndexType);

                tray.GetChild(0).gameObject.SetActive(false);
                
                tray.localPosition = Vector3.zero;
                tray.localScale = Vector3.one;
                flour.localPosition = Vector3.zero;
                tutorialHand.SetActive(false);

                if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
                {
                    LoadInput();
                }
                else
                {
                    isStop = true;
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
