using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FloorPoolSystem : MonoBehaviour
{
    public GameObject Train;
    public GameObject FloorPrefab;

    public int poolSize = 7;
    public float scaleX;

    public float furthest;

    public Queue<GameObject> floorPool = new Queue<GameObject>();

    public List<Biome> biomes;
    public int biomeCount = 0;

    // Initialize the object pool
    void Start()
    {
        scaleX = FloorPrefab.transform.localScale.x;

        InitializePool();
        float xOffset = -poolSize/2; // Initialize xOffset
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newFloor = GetObjectFromPool();
            newFloor.transform.position = new Vector3(Train.transform.position.x + xOffset * scaleX, Train.transform.position.y - 1, Train.transform.position.z);
            xOffset += 1; // Increment xOffset by 1 for the next floor
            furthest = newFloor.transform.position.x;
        }       
    }



    // Create and enqueue objects in the pool
    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(FloorPrefab);
            obj.SetActive(false);
            floorPool.Enqueue(obj);
            //floorList.Remove(obj);
        }
    }

    // Retrieve an object from the pool
    public GameObject GetObjectFromPool()
    {
        if (floorPool.Count == 0)
        {
            Debug.LogWarning("Object pool is empty. Consider resizing it.");
            return null;
        }

        GameObject obj = floorPool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    // Return an object to the pool
    // Return an object to the pool
    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.position = new Vector3(furthest+1, transform.position.y, transform.position.z); // Reset the position of the object
        floorPool.Enqueue(obj);
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void PutForward(GameObject floor)
    {
        ReturnObjectToPool(floor);

        // Find the furthest floor to determine the placement position
        float newFloorXPosition = furthest + scaleX;
        furthest = newFloorXPosition;
        GameObject newFloor = GetObjectFromPool();
        newFloor.transform.position = new Vector3(newFloorXPosition, Train.transform.position.y - 1, Train.transform.position.z);
    }



    //public GameObject FindFurthest()
    //{
    //    GameObject furthest = null;
    //    float max = float.MinValue;

    //    foreach (GameObject obj in floorList)
    //    {
    //        if (obj.transform.position.x > max)
    //        {
    //            max = obj.transform.position.x;
    //            furthest = obj;
    //        }
    //    }

    //    return furthest;
    //}

}

[System.Serializable]
public class Biome
{
    public string Name;
    public GameObject[] Sections;
}
