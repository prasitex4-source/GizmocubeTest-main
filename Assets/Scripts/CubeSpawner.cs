using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    
    [Header("Root Octree Node Params")]
    [SerializeField] float Size;
    [SerializeField]  Vector3 Pos;

    [Header("Spawnable Objects")]
    [SerializeField] GameObject StaticObject;
   // [SerializeField] int StaticObjectNum = 0;   
    [SerializeField] GameObject DynamicObject;

    [SerializeField] float Vel;
    [SerializeField] Vector3 dir;

    GameObject sphere;

   // Private varibles
    Octreenode rootNode;
    public static CubeSpawner instance;
       
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            
        }
        else
        {
            return;
        }

        rootNode = new Octreenode(0,Pos,new Vector3(Size, Size, Size));
        
        dir.Normalize();
       
    }
    void Update()
    {
        
        if( Keyboard.current.eKey.wasPressedThisFrame)
        {
            Vector3 posPrefabs = Pos + 
                new Vector3(Random.Range(-Size*0.5f, Size*0.5f),
                 Random.Range(-Size*0.5f, Size*0.5f),
                 Random.Range(-Size*0.5f, Size*0.5f));
        
            rootNode.AddEntity(Instantiate(StaticObject, posPrefabs, Quaternion.identity),Octreenode.Figures.Cubo);
           
        }
        
        if( Keyboard.current.sKey.wasPressedThisFrame && sphere == null)
        {
            Vector3 posPrefabs = Pos + 
                new Vector3(Random.Range(-Size*0.5f, Size*0.5f),
                 Random.Range(-Size*0.5f, Size*0.5f),
                 Random.Range(-Size*0.5f, Size*0.5f));
                 GameObject esfera = Instantiate(DynamicObject, posPrefabs, Quaternion.identity);
                 esfera.GetComponent<SphereMoving>().SetData(Pos,Size*0.5f);
            rootNode.AddEntity(esfera,Octreenode.Figures.Esfera);

        }
        rootNode.SubdivideUpdater();

    }
    void OnDrawGizmos()
    {
        rootNode?.DrawGizmos();

        
    }

    public void AddEntity(GameObject aCubo,Octreenode.Figures Type)
    {
        rootNode.AddEntity(aCubo,Type);
    }
}
