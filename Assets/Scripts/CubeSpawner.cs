using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    
    [Header("Root Octree Node Params")]
    [SerializeField] float Size;
    [SerializeField] Vector3 Pos;

    [Header("Spawnable Objects")]
    [SerializeField] GameObject StaticObject;
   // [SerializeField] int StaticObjectNum = 0;   
   

   // Private varibles
    Octreenode rootNode;
       
    void Start()
    {
        rootNode = new Octreenode(Pos,new Vector3(Size, Size, Size));
        
        
       
    }
    void Update()
    {
        
         if( Keyboard.current.eKey.wasPressedThisFrame)
        {
            Vector3 posPrefabs = Pos + 
                new Vector3(Random.Range(-Size*0.5f, Size*0.5f),
                 Random.Range(-Size*0.5f, Size*0.5f),
                 Random.Range(-Size*0.5f, Size*0.5f));
            
            

            rootNode.AddEntity(Instantiate(StaticObject, posPrefabs, Quaternion.identity));
            
           
        }
        
        
    }
    void OnDrawGizmos()
    {
        rootNode?.DrawGizmos();

        
    }
}
