
using System.Collections.Generic;
using UnityEngine;

public class Octreenode
{
    Vector3 basePosition = Vector3.zero; 
    Vector3 baseSize = Vector3.zero;
    Color gizmoColor = Color.white;
    List<Octreenode> octreeNodes = new List<Octreenode>();
    List<GameObject>Cubes = new List<GameObject>();
    
   
    public Octreenode(Vector3 aPos, Vector3 aSize, Color? aGizmoColor = null)
    {
        basePosition = aPos;
        baseSize = aSize;
        gizmoColor = aGizmoColor ?? Color.white;
    }

    public void DrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(basePosition, baseSize);
        foreach(Octreenode item in octreeNodes)
        {
            item.DrawGizmos();
        }
    }
    private void Subdivide()
    {
        Color ColorRandom = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        float testSize = baseSize.x * 0.5f;
        Vector3 testSizeV3 = new Vector3(testSize, testSize, testSize);
        octreeNodes.Add(new Octreenode(basePosition + new Vector3(-testSize*0.5f, -testSize*0.5f, -testSize*0.5f), testSizeV3, ColorRandom));
        octreeNodes.Add(new Octreenode(basePosition + new Vector3(-testSize*0.5f, -testSize*0.5f, testSize*0.5f), testSizeV3, ColorRandom));
        octreeNodes.Add(new Octreenode(basePosition + new Vector3(-testSize*0.5f, testSize*0.5f, -testSize*0.5f), testSizeV3, ColorRandom));
        octreeNodes.Add(new Octreenode(basePosition + new Vector3(-testSize*0.5f, testSize*0.5f, testSize*0.5f), testSizeV3, ColorRandom));
        octreeNodes.Add(new Octreenode(basePosition + new Vector3(testSize*0.5f, -testSize*0.5f, -testSize*0.5f), testSizeV3, ColorRandom));
        octreeNodes.Add(new Octreenode(basePosition + new Vector3(testSize*0.5f, -testSize*0.5f, testSize*0.5f), testSizeV3, ColorRandom));
        octreeNodes.Add(new Octreenode(basePosition + new Vector3(testSize*0.5f, testSize*0.5f, -testSize*0.5f), testSizeV3, ColorRandom));
        octreeNodes.Add(new Octreenode(basePosition + new Vector3(testSize*0.5f, testSize*0.5f, testSize*0.5f), testSizeV3, ColorRandom));   
          
        foreach(GameObject iter in Cubes)
        {
            foreach(Octreenode Cubito in octreeNodes)
            {

                if(Cubito.AddEntity(iter))
                {
                    break;
                }
            }
        }   
    }

    public bool AddEntity(GameObject aCubo)
    {
       
        Vector3 EntityPosition = aCubo.transform.position;
        Vector3 MinNode = basePosition - (baseSize *0.5f);
        Vector3 MaxNode = basePosition + (baseSize *0.5f);

        if(EntityPosition.x >= MinNode.x && EntityPosition.x <= MaxNode.x && 
        EntityPosition.y >= MinNode.y && EntityPosition.y <= MaxNode.y &&
        EntityPosition.z >= MinNode.z && EntityPosition.z <= MaxNode.z)
        {
             if (octreeNodes.Count == 0)
            {
                Cubes.Add(aCubo);

                if(Cubes.Count >= 4)
                {
                    
                    Subdivide();
                    
                }

            }

            else
            {
                foreach(Octreenode Cubito in octreeNodes)
                {

                    if(Cubito.AddEntity(aCubo))
                    {
                        break;
                    }
                } 
            }
             
            return true;
        }

        return false;

        
    }
}
