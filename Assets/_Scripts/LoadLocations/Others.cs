using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Others : MonoBehaviour
{

    public Location location;

    public List<Animator> arrAnim;

    public List<Transform> lsPoint;

    public void LoadOtherRandom()
    {
        location.lsOther = new List<int>();
        for (int i = 0; i < lsPoint.Count; i++)
        {
            int random = Random.Range(0, GameManager.Instance.arrPrefabOther.Length);
            Animator anim = Instantiate(GameManager.Instance.arrPrefabOther[random],lsPoint[i]).GetComponent<Animator>();
            location.lsOther.Add(random);
            arrAnim.Add(anim);
        }
        location.isLoaded = true;
    }

    public void LoadOtherJson()
    {
        for (int i = 0; i < lsPoint.Count; i++)
        {
            Animator anim = Instantiate(GameManager.Instance.arrPrefabOther[location.lsOther[i]],lsPoint[i]).GetComponent<Animator>();
            arrAnim.Add(anim);
        }
        location.isLoaded = true;
    }
}
