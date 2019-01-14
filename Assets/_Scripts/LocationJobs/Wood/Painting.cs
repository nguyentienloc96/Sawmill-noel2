using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public struct Cart
{
    public GameObject[] gTree;
    public SpriteRenderer[] spTree;
}

public class Painting : MonoBehaviour
{

    public bool isInput;
    public Transform cart;
    public Transform pen;
    public Animator anim;
    public GameObject notification;

    public List<Transform> way;
    public List<Cart> lsCart;
    public GameObject tutorialHand;
    public Image imgBG;

    private bool isRun;
    private Vector3 posDown;
    private int indexPos;
    private int indexTree;
    private bool isTutorial;
    private bool isStop;

    public Transform tfStart;
    public Transform tfEnd;
    public Sprite iconOutPut;

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
                if ((indexPos + 1) < way.Count)
                {
                    if (way[indexPos].localPosition.y == way[indexPos + 1].localPosition.y)
                    {
                        if (Input.mousePosition.x > posDown.x)
                        {
                            float dis = Input.mousePosition.x - posDown.x;
                            pen.localPosition += new Vector3(dis * 0.01f * Time.deltaTime, 0f, 0f);
                            if (pen.localPosition.x >= way[indexPos + 1].localPosition.x)
                            {
                                pen.localPosition = way[indexPos + 1].localPosition;
                                if (indexPos == 1 || indexPos == 2 || indexPos == 5 || indexPos == 6 || indexPos == 9 || indexPos == 10)
                                {
                                    lsCart[indexTree].spTree[0].color = new Color32(255, 255, 255, 255);
                                    indexTree++;
                                }
                                indexPos++;
                            }
                        }
                        pen.localEulerAngles = new Vector3(0f, 0f, -90f);
                    }
                    else
                    {
                        if (way[indexPos].localPosition.y > way[indexPos + 1].localPosition.y)
                        {
                            if (Input.mousePosition.y < posDown.y)
                            {
                                float dis = Input.mousePosition.y - posDown.y;
                                pen.localPosition += new Vector3(0f, dis * 0.01f * Time.deltaTime, 0f);
                                if (pen.localPosition.y <= way[indexPos + 1].localPosition.y)
                                {
                                    pen.localPosition = way[indexPos + 1].localPosition;
                                    if (indexPos == 1 || indexPos == 2 || indexPos == 5 || indexPos == 6 || indexPos == 9 || indexPos == 10)
                                    {
                                        lsCart[indexTree].spTree[0].color = new Color32(255, 255, 255, 255);
                                        indexTree++;
                                    }
                                    indexPos++;
                                }
                            }
                            pen.localEulerAngles = new Vector3(0f, 0f, -180f);
                        }
                        else if (way[indexPos].localPosition.y < way[indexPos + 1].localPosition.y)
                        {
                            if (Input.mousePosition.y > posDown.y)
                            {
                                float dis = Input.mousePosition.y - posDown.y;
                                pen.localPosition += new Vector3(0f, dis * 0.01f * Time.deltaTime, 0f);
                                if (pen.localPosition.y >= way[indexPos + 1].localPosition.y)
                                {
                                    pen.localPosition = way[indexPos + 1].localPosition;
                                    if (indexPos == 1 || indexPos == 2 || indexPos == 5 || indexPos == 6 || indexPos == 9 || indexPos == 10)
                                    {
                                        lsCart[indexTree].spTree[0].color = new Color32(255, 255, 255, 255);
                                        indexTree++;
                                    }
                                    indexPos++;
                                }
                            }
                            pen.localEulerAngles = new Vector3(0f, 0f, 0f);
                        }
                    }
                }
                else
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
            anim.enabled = true;
            AudioManager.Instance.Play("Painting");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        anim.enabled = false;
        AudioManager.Instance.Stop("Painting");
        isRun = false;
    }

    public void LoadInput()
    {
        for (int i = 0; i < lsCart.Count; i++)
        {
            lsCart[i].gTree[0].SetActive(true);
        }
        indexTree = 0;
        indexPos = 0;
        cart.localPosition = new Vector3(-10f, 0f, 0f);
        pen.localPosition = way[indexPos].localPosition;
        cart.DOLocalMove(Vector3.zero, 0.5f).OnComplete(() =>
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

        for (int i = 0; i < lsCart.Count; i++)
        {
            lsCart[i].gTree[0].SetActive(false);
            lsCart[i].spTree[0].color = new Color32(255, 255, 255, 0);
        }
        anim.enabled = false;
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        double valueOutput = 0;
        for (int i = 0; i < 3; i++) {
            valueOutput += GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
        }
        
        GameManager.Instance.AddOutPut(valueOutput, 
            iconOutPut, 
            tfStart.position, 
            tfEnd.position,
            ()=> 
            {
                GameManager.Instance.AddOutPutMiniGame(IndexType);
            });
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
    }

    public void Help()
    {
        tutorialHand.SetActive(true);
    }
}
