using UnityEngine;
using UnityEngine.UI;

public class TruckManager : MonoBehaviour
{

    public Location location;
    public GameObject truck;
    public Text txtSent;
    public Text txtLevel;

    public Animator animCar;

    public int indexType;
    public bool isRun;
    public bool isOff;

    public Transform[] way;
    private int indexPos = 0;
    private bool isRetrograde;
    private bool isOnTutorial;

    public GameObject upgradeMe;

    public void LoadTruck()
    {
        if (!isRun)
        {
            isRun = true;
            indexPos = 0;
            truck.gameObject.SetActive(true);
            truck.transform.position = way[0].transform.position;
            float height = way[indexPos + 1].transform.position.y - truck.transform.position.y;
            float weight = way[indexPos + 1].transform.position.x - truck.transform.position.x;
            txtLevel.text = UIManager.Instance.ConvertNumber(location.lsWorking[indexType].levelTruck);
            if (Mathf.Abs(height) > Mathf.Abs(weight))
            {
                if (height > 0)
                {
                    animCar.SetFloat("indexRun", 1);
                }
                else
                {
                    animCar.SetFloat("indexRun", 0);
                }
            }
            else
            {
                if (weight > 0)
                {
                    animCar.SetFloat("indexRun", 3);
                }
                else
                {
                    animCar.SetFloat("indexRun", 2);
                }
            }
            SentedOutput();
        }
    }

    public void TruckRun()
    {
        if (!isOff)
        {
            if (isRun)
            {
                if (!isRetrograde)
                {
                    truck.transform.position = Vector3.MoveTowards(truck.transform.position, way[indexPos + 1].transform.position, GameConfig.Instance.TruckSpeed * Time.deltaTime);
                    if (truck.transform.position == way[indexPos + 1].transform.position)
                    {
                        indexPos++;
                        if (indexPos + 1 < way.Length)
                        {
                            float height = way[indexPos + 1].transform.position.y - truck.transform.position.y;
                            float weight = way[indexPos + 1].transform.position.x - truck.transform.position.x;
                            if (Mathf.Abs(height) > Mathf.Abs(weight))
                            {
                                if (height > 0)
                                {
                                    animCar.SetFloat("indexRun", 1);
                                }
                                else
                                {
                                    animCar.SetFloat("indexRun", 0);
                                }
                            }
                            else
                            {
                                if (weight > 0)
                                {
                                    animCar.SetFloat("indexRun", 3);
                                }
                                else
                                {
                                    animCar.SetFloat("indexRun", 2);
                                }
                            }
                        }
                        else
                        {
                            float height = way[indexPos - 1].transform.position.y - truck.transform.position.y;
                            float weight = way[indexPos - 1].transform.position.x - truck.transform.position.x;
                            if (Mathf.Abs(height) > Mathf.Abs(weight))
                            {
                                if (height > 0)
                                {
                                    animCar.SetFloat("indexRun", 1);
                                }
                                else
                                {
                                    animCar.SetFloat("indexRun", 0);
                                }
                            }
                            else
                            {
                                if (weight > 0)
                                {
                                    animCar.SetFloat("indexRun", 3);
                                }
                                else
                                {
                                    animCar.SetFloat("indexRun", 2);
                                }
                            }
                            isRetrograde = true;
                            ReceivedOutput();
                        }
                    }
                }
                else
                {
                    truck.transform.position = Vector3.MoveTowards(truck.transform.position, way[indexPos - 1].transform.position, GameConfig.Instance.TruckSpeed * Time.deltaTime);
                    if (truck.transform.position == way[indexPos - 1].transform.position)
                    {
                        indexPos--;
                        if (indexPos > 0)
                        {
                            //truck.transform.right = way[indexPos - 1].transform.position - truck.transform.position;
                            float height = way[indexPos - 1].transform.position.y - truck.transform.position.y;
                            float weight = way[indexPos - 1].transform.position.x - truck.transform.position.x;
                            if (Mathf.Abs(height) > Mathf.Abs(weight))
                            {
                                if (height > 0)
                                {
                                    animCar.SetFloat("indexRun", 1);
                                }
                                else
                                {
                                    animCar.SetFloat("indexRun", 0);
                                }
                            }
                            else
                            {
                                if (weight > 0)
                                {
                                    animCar.SetFloat("indexRun", 3);
                                }
                                else
                                {
                                    animCar.SetFloat("indexRun", 2);
                                }
                            }
                        }
                        else
                        {
                            float height = way[indexPos + 1].transform.position.y - truck.transform.position.y;
                            float weight = way[indexPos + 1].transform.position.x - truck.transform.position.x;
                            if (Mathf.Abs(height) > Mathf.Abs(weight))
                            {
                                if (height > 0)
                                {
                                    animCar.SetFloat("indexRun", 1);
                                }
                                else
                                {
                                    animCar.SetFloat("indexRun", 0);
                                }
                            }
                            else
                            {
                                if (weight > 0)
                                {
                                    animCar.SetFloat("indexRun", 3);
                                }
                                else
                                {
                                    animCar.SetFloat("indexRun", 2);
                                }
                            }
                            isRetrograde = false;
                            SentedOutput();
                        }
                    }
                }
                if (indexType == 0 && indexPos == 3)
                {
                    if (PlayerPrefs.GetInt("isTutorial") == 0)
                    {
                        if (UIManager.Instance.isOnClickTrunk)
                        {
                            if (!isOnTutorial)
                            {
                                GameConfig.Instance.TruckSpeed = 0;
                                if (!UIManager.Instance.popupTutorial.activeInHierarchy)
                                {
                                    UIManager.Instance.popupTutorial.SetActive(true);
                                }
                                if (UIManager.Instance.objTutorial != null)
                                {
                                    Destroy(UIManager.Instance.objTutorial);
                                }
                                UIManager.Instance.ControlHandTutorial(location.lsWorking[0].truckManager.truck.transform);
                                UIManager.Instance.isClickTrunk = true;
                                UIManager.Instance.isOnClickTrunk = false;
                                UIManager.Instance.txtWait.text = "Tap the Truck";
                                isOnTutorial = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (location.lsWorking[indexType].output > 0
                    && GameManager.Instance.dollar >= location.lsWorking[indexType].priceTruckSent)
                {
                    LoadTruck();
                }
            }
        }
    }

    public void LateUpdate()
    {
        TruckRun();
    }

    public void SentedOutput()
    {
        //if (GameManager.Instance.dollar >= location.lsWorking[indexType].priceTruckSent)
        //{
        //GameManager.Instance.dollar -= location.lsWorking[indexType].priceTruckSent;
        if (location.id == UIManager.Instance.lsItem[1].idLocation && UIManager.Instance.lsItem[1].isOnItem && indexType == UIManager.Instance.lsItem[1].indexType)
        {
            if (location.lsWorking[indexType].output >= location.lsWorking[indexType].maxSent * 2f)
            {
                txtSent.text = UIManager.Instance.ConvertNumber(location.lsWorking[indexType].maxSent * 2f);
                location.lsWorking[indexType].output -= location.lsWorking[indexType].maxSent * 2f;
                location.lsWorking[indexType].currentSent = location.lsWorking[indexType].maxSent *2f;
                location.lsWorking[indexType].textOutput.text = UIManager.Instance.ConvertNumber(location.lsWorking[indexType].output);
            }
            else if (location.lsWorking[indexType].output > 0)
            {
                double outputSent = location.lsWorking[indexType].output;
                txtSent.text = UIManager.Instance.ConvertNumber(outputSent);
                location.lsWorking[indexType].currentSent = outputSent;
                location.lsWorking[indexType].output -= outputSent;
                location.lsWorking[indexType].textOutput.text = UIManager.Instance.ConvertNumber(location.lsWorking[indexType].output);
            }
            else
            {
                isRun = false;
            }
        }
        else
        {
            if (location.lsWorking[indexType].output >= location.lsWorking[indexType].maxSent)
            {
                txtSent.text = UIManager.Instance.ConvertNumber(location.lsWorking[indexType].maxSent);
                location.lsWorking[indexType].output -= location.lsWorking[indexType].maxSent;
                location.lsWorking[indexType].currentSent = location.lsWorking[indexType].maxSent;
                location.lsWorking[indexType].textOutput.text = UIManager.Instance.ConvertNumber(location.lsWorking[indexType].output);
            }
            else if (location.lsWorking[indexType].output > 0)
            {
                double outputSent = location.lsWorking[indexType].output;
                txtSent.text = UIManager.Instance.ConvertNumber(outputSent);
                location.lsWorking[indexType].currentSent = outputSent;
                location.lsWorking[indexType].output -= outputSent;
                location.lsWorking[indexType].textOutput.text = UIManager.Instance.ConvertNumber(location.lsWorking[indexType].output);
            }
            else
            {
                isRun = false;
            }
        }
        //}
        //else
        //{
        //    isRun = false;
        //}
    }

    public void ReceivedOutput()
    {
        txtSent.text = "";
        if (location.countType <= indexType)
        {
            GameManager.Instance.AddDollar(+location.lsWorking[indexType].currentSent * location.lsWorking[indexType].priceOutput);
            location.lsWorking[indexType].currentSent = 0;
        }
        else
        {
            location.lsWorking[indexType + 1].input += location.lsWorking[indexType].currentSent;
            location.lsWorking[indexType + 1].textInput.text = UIManager.Instance.ConvertNumber(location.lsWorking[indexType + 1].input);
            location.lsWorking[indexType].currentSent = 0;
        }
    }

}
