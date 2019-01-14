using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizingCoatingPaper : MonoBehaviour
{

    public bool isInput;
    public Transform paper;
    public GameObject notification;
    public Transform pen;
    public GameObject tutorialHand;
    public Image imgBG;

    private bool isRun;
    private Vector3 posCheck;
    private bool time;
    private bool isTutorial;
    private bool isStop;
    private Texture2D texture;
    public Renderer renderPaper;

    public Transform tfStart;
    public Transform tfEnd;
    public Sprite iconOutPut;

    public void Start()
    {
        posCheck = transform.GetChild(0).position;
        texture = new Texture2D(4, 4);
        renderPaper.material.SetTexture("_SliceGuide", texture);

        for (int y = 0; y < texture.height; ++y)
        {
            for (int x = 0; x < texture.width; ++x)
            {
                texture.SetPixel(x, y, Color.white);
            }
        }
        texture.Apply();
    }

    private void OnEnable()
    {
        int randomBG = Random.Range(0, UIManager.Instance.spBG.Length);
        imgBG.sprite = UIManager.Instance.spBG[randomBG];
        isTutorial = true;
        paper.localScale = new Vector3(-0.325f, 1f, 0.375f);

        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
        {
            notification.SetActive(false);
            paper.gameObject.SetActive(true);
            LoadInput();
        }
        else
        {
            isStop = true;
            paper.gameObject.SetActive(false);
            notification.SetActive(true);
        }
    }

    public void Update()
    {
        if (!isStop)
        {
            if (isRun)
            {
                Vector2 posMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pen.position = posMouse;
                if (Input.GetMouseButton(0))
                {
                    RaycastHit hit;
                    if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) return;

                    Renderer renderer = hit.collider.GetComponent<Renderer>();
                    MeshCollider meshCollider = hit.collider as MeshCollider;
                    if (renderer == null || renderer.sharedMaterial == null || texture == null || meshCollider == null) return;

                    Texture2D tex = texture;
                    var pixelUV = hit.textureCoord;
                    pixelUV.x *= tex.width;
                    pixelUV.y *= tex.height;

                    if (tex.GetPixel((int)pixelUV.x, (int)pixelUV.y).grayscale > 0)
                    {
                        tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.black);
                        tex.Apply();
                    }
                }
                if (CheckPushPull(0.25f))
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
                paper.gameObject.SetActive(true);
                LoadInput();
                isStop = false;
            }
        }
    }

    public void TapDown()
    {
        if (isInput)
        {
            AudioManager.Instance.Play("Painting");
            isRun = true;
        }
    }

    public void TapUp()
    {
        AudioManager.Instance.Stop("Painting");
        isRun = false;
    }

    public void LoadInput()
    {
        paper.DOLocalMove(new Vector3(0f, 0f, -5f), 1f).OnComplete(() =>
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
        PaintPull();
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        isInput = false;

        paper.DOLocalMoveX(2.5f, 1f).OnComplete(() =>
        {
           
            paper.DOScale(Vector3.zero, 0.5f);
            paper.DOMove(tfEnd.position, 0.5f).OnComplete(() =>
            {
                GameManager.Instance.lsLocation[ID].JobComplete(IndexType);

                GameManager.Instance.AddOutPutMiniGame(IndexType);

                pen.localPosition = new Vector3(0f, 0f, -10f);
                paper.localPosition = new Vector3(-2.5f, 0f, -5f);
                paper.localScale = new Vector3(-0.325f, 1f, 0.375f);
                ClearPaint();
                tutorialHand.SetActive(false);
                if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
                {
                    LoadInput();
                }
                else
                {
                    paper.gameObject.SetActive(false);
                    notification.SetActive(true);
                    isStop = true;
                }
            });
        });
    }

    public void ClearPaint()
    {
        Texture2D tex = texture;
        for (int y = 0; y < tex.height; ++y)
        {
            for (int x = 0; x < tex.width; ++x)
            {
                tex.SetPixel(x, y, Color.white);
            }
        }
        tex.Apply();
    }

    public void PaintPull()
    {
        Texture2D tex = texture;
        for (int y = 0; y < tex.height; ++y)
        {
            for (int x = 0; x < tex.width; ++x)
            {
                tex.SetPixel(x, y, Color.black);
            }
        }
        tex.Apply();
    }

    public bool CheckPushPull(float percentWhite)
    {
        bool isFull = true;
        Texture2D tex = texture;
        int sum = 0;
        int sumWhite = 0;
        for (int y = 0; y < tex.height; ++y)
        {
            for (int x = 0; x < tex.width; ++x)
            {
                sum++;
                if (tex.GetPixel(x, y) != Color.black)
                {
                    sumWhite++;
                }
            }
        }
        if ((float)sumWhite / (float)sum >= percentWhite)
        {
            isFull = false;
        }
        return isFull;
    }
}
