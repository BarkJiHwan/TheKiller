using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPool : MonoBehaviour
{
    public GameObject[] npcPrefab;
    private Queue<GameObject> objPool = new Queue<GameObject>();
    public static NPCPool Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetObject()
    {
        if(objPool.Count > 0)
        {
            GameObject obj = objPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int randomNpcIndex = Random.Range(0, npcPrefab.Length);
            GameObject obj = Instantiate(npcPrefab[randomNpcIndex]);
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        objPool.Enqueue(obj);            
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
