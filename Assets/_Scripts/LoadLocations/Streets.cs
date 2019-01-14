using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Streets : MonoBehaviour
{
    public Location location;

    public List<Transform> lsPoint;

    public void LoadStreetRandom()
    {
        location.lsStreet = new List<int>();
        for (int i = 0; i < lsPoint.Count; i++)
        {
            int random;
            if (i % 2 == 0)
            {
                random = Random.Range(0, GameManager.Instance.arrPrefabsStreet.Length / 2);
            }
            else
            {
                random = Random.Range(GameManager.Instance.arrPrefabsStreet.Length / 2, GameManager.Instance.arrPrefabsStreet.Length);
            }
            Transform tf = Instantiate(GameManager.Instance.arrPrefabsStreet[random], lsPoint[i]).transform;
            tf.SetAsFirstSibling();
            location.lsWorking[i].truckManager.way = tf.GetComponent<WayPoint>().way;
            location.lsWorking[i].truckManager.truck.transform.position = location.lsWorking[i].truckManager.way[0].position;
            location.lsStreet.Add(random);
        }
        location.isLoaded = true;
    }

    public void LoadStreetJson()
    {
        for (int i = 0; i < lsPoint.Count; i++)
        {
            Transform tf = Instantiate(GameManager.Instance.arrPrefabsStreet[location.lsStreet[i]], lsPoint[i]).transform;
            tf.SetAsFirstSibling();
            location.lsWorking[i].truckManager.way = tf.GetComponent<WayPoint>().way;
            location.lsWorking[i].truckManager.truck.transform.position = location.lsWorking[i].truckManager.way[0].position;
        }
        location.isLoaded = true;
    }
}
