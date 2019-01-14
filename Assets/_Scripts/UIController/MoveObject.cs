using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MoveObject : MonoBehaviour
{
    public float timeStart;
    public float timeWaiting;
    public float speed;

    public Transform[] poitStart;
    public Transform[] poitEnd;

    public bool isOneWay;
    public bool isNotHide;
    public bool isLocation;
    public bool isInLocation;
    public bool isPlaneGive;

    public Location location;

    private Transform posTo;
    private int indexPoit;
    private bool isWaiting;
    private float timeWaitingUpdate;
    private bool isBack;
    private bool isStarted;
    private float timeStartPlaneGive;

    public IEnumerator Start()
    {
        if (isInLocation)
        {
            timeWaiting = timeWaiting * 5f;
        }
        yield return new WaitForSeconds(timeStart);
        if (!isPlaneGive)
        {
            transform.position = poitStart[indexPoit].position;
            posTo = poitEnd[indexPoit];
            isStarted = true;
        }

    }

    public void Update()
    {
        if (isStarted)
        {
            if (!isLocation)
            {
                if (UIManager.Instance.scene == TypeScene.WOLRD)
                {
                    MoveUpdate();
                }
            }
            else
            {
                if (UIManager.Instance.scene == TypeScene.LOCATION || UIManager.Instance.scene == TypeScene.MINIGAME)
                {
                    if (isInLocation)
                    {
                        if (location.id == GameManager.Instance.IDLocation)
                        {
                            MoveUpdate();
                        }
                    }
                    else
                    {
                        MoveUpdate();
                    }
                }
            }
        }
        else
        {
            if (GameManager.Instance.lsLocation.Count > 0 && isPlaneGive && GameManager.Instance.lsLocation[0].countType >= 0)
            {
                timeStartPlaneGive += Time.deltaTime;
                if (timeStartPlaneGive >= timeWaiting)
                {
                    transform.position = poitStart[indexPoit].position;
                    posTo = poitEnd[indexPoit];
                    isStarted = true;
                    timeStartPlaneGive = 0;
                }
            }
        }
    }

    public void MoveUpdate()
    {
        if (isWaiting)
        {
            timeWaitingUpdate += Time.deltaTime;
            if (timeWaitingUpdate >= timeWaiting)
            {
                if (!isNotHide) transform.localScale = Vector3.one;
                if (isOneWay)
                {
                    posTo = poitEnd[indexPoit];
                    transform.position = poitStart[indexPoit].position;
                }
                else
                {
                    if (isBack)
                    {
                        posTo = poitStart[indexPoit];
                        transform.position = poitEnd[indexPoit].position;
                    }
                    else
                    {

                        posTo = poitEnd[indexPoit];
                        transform.position = poitStart[indexPoit].position;
                    }
                }

                Vector3 check = posTo.position - transform.position;
                if (check.x > 0)
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }

                timeWaitingUpdate = 0;
                isWaiting = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, posTo.position, speed * Time.deltaTime);
            if (transform.position == posTo.position)
            {
                indexPoit = Random.Range(0, poitStart.Length);
                if (!isNotHide) transform.localScale = Vector3.zero;
                timeWaitingUpdate = 0;
                isWaiting = true;
                isBack = !isBack;
            }
        }
    }
}
