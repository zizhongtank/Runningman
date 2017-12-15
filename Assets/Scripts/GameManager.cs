using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    // obstacle list
    public List<Transform> bornPosList = new List<Transform>();
    // road list
    public List<Transform> roadList = new List<Transform>();
    // arrive
    public List<Transform> arrivePosList = new List<Transform>();
    public List<GameObject> objPrefabList = new List<GameObject>();
    Dictionary<string, List<GameObject>> objDict = new Dictionary<string, List<GameObject>>();
    public int roadDistance;
    public bool isEnd = false;
	// Use this for initialization
	void Start () {
        foreach(Transform road in roadList)
        {
            List<GameObject> objList = new List<GameObject>();
            objDict.Add(road.name, objList);
        }
        initRoad(0);
        initRoad(1);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    // new road
    public void changeRoad(Transform arrivePos)
    {
        int index = arrivePosList.IndexOf(arrivePos);
        if(index >= 0)
        {
            int lastIndex = index - 1;
            if (lastIndex < 0)
                lastIndex = roadList.Count - 1;
            // move road
            roadList[index].position = roadList[lastIndex].position + new Vector3(roadDistance, 0, 0);

            initRoad(index);
        }
        else
        {
            Debug.LogError("arrivePos index is error");
            return;
        }
    }

    void initRoad(int index)
    {
        
        string roadName = roadList[index].name;
        // clear raod
        foreach(GameObject obj in objDict[roadName])
        {
            Destroy(obj);
        }
        objDict[roadName].Clear();

        // add obstacle
        foreach(Transform pos in bornPosList[index])
        {
            GameObject prefab = objPrefabList[Random.Range(0, objPrefabList.Count)];
            Vector3 eulerAngle = new Vector3(0, Random.Range(0, 360), 0);
            GameObject obj = Instantiate(prefab, pos.position, Quaternion.Euler(eulerAngle)) as GameObject;
            obj.tag = "Obstacle";
            objDict[roadName].Add(obj);
        }
    }
}
