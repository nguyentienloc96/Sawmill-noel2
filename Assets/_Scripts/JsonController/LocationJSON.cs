using System.Collections.Generic;

[System.Serializable]
public class LocationJSON {

    public int id;
    public string nameLocation;
    public int indexTypeWork;
    public int countType = 0;
    public int indexType = 0;
    public int makerType;

    public int capIndex = 4;
    public int captruckIndex = 4;

    public ForestST forest;
    public TypeOfWorkST[] lsWorking;

    public List<int> lsOther;
    public List<int> lsRiverRight;
    public List<int> lsRiverLeft;
    public List<int> lsStreet;

}
