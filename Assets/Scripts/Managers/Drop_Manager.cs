using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop_Manager : MonoBehaviour
{
    [SerializeField] private List<Drops> _itemDrops;

    public static Drop_Manager Instance;
    void Start()
    {
        if (Instance != null) // singleton since I only need one spawner. 
        {
            Debug.Log("Can only have one meteor spawner at a time!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void DropItem()
    {

    }
}
