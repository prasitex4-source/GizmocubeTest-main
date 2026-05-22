
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Octreenode
{
    public enum Figures
    {
        Cubo,
        Esfera
    }
    Vector3 basePosition = Vector3.zero; 
    Vector3 baseSize = Vector3.zero;
    Color gizmoColor = Color.white;
    List<Octreenode> octreeNodes = new List<Octreenode>();
    List<GameObject>Cubes = new List<GameObject>();
    List<GameObject>Spheres = new List<GameObject>();

    int Level = 0;
   
    public Octreenode(int NewLevel, Vector3 aPos, Vector3 aSize, Color? aGizmoColor = null)
    {
        Level = NewLevel;
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
        if (Level == 2)
        {
            return;
        }
        Color ColorRandom = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        float testSize = baseSize.x * 0.5f;
        Vector3 testSizeV3 = new Vector3(testSize, testSize, testSize);
        octreeNodes.Add(new Octreenode(Level + 1, basePosition + new Vector3(-testSize*0.5f, -testSize*0.5f, -testSize*0.5f), testSizeV3, ColorRandom));
        octreeNodes.Add(new Octreenode(Level + 1, basePosition + new Vector3(-testSize*0.5f, -testSize*0.5f, testSize*0.5f), testSizeV3, ColorRandom));
        octreeNodes.Add(new Octreenode(Level + 1, basePosition + new Vector3(-testSize*0.5f, testSize*0.5f, -testSize*0.5f), testSizeV3, ColorRandom));
        octreeNodes.Add(new Octreenode(Level + 1, basePosition + new Vector3(-testSize*0.5f, testSize*0.5f, testSize*0.5f), testSizeV3, ColorRandom));
        octreeNodes.Add(new Octreenode(Level + 1, basePosition + new Vector3(testSize*0.5f, -testSize*0.5f, -testSize*0.5f), testSizeV3, ColorRandom));
        octreeNodes.Add(new Octreenode(Level + 1, basePosition + new Vector3(testSize*0.5f, -testSize*0.5f, testSize*0.5f), testSizeV3, ColorRandom));
        octreeNodes.Add(new Octreenode(Level + 1, basePosition + new Vector3(testSize*0.5f, testSize*0.5f, -testSize*0.5f), testSizeV3, ColorRandom));
        octreeNodes.Add(new Octreenode(Level + 1, basePosition + new Vector3(testSize*0.5f, testSize*0.5f, testSize*0.5f), testSizeV3, ColorRandom));   
          
        foreach(GameObject iter in Cubes)
        {
            foreach(Octreenode Cubito in octreeNodes)
            {

                if(Cubito.AddEntity(iter,Figures.Cubo))
                {
                    break;
                }
            }
        }

        foreach(GameObject iter in Spheres)
        {
            foreach(Octreenode Cubito in octreeNodes)
            {

                if(Cubito.AddEntity(iter,Figures.Esfera))
                {
                    break;
                }
            }
        }      
    }

    public int GetNumEntities()
    {
        int TotalEntities = 0;

            foreach(Octreenode Iter in octreeNodes)
            {
                TotalEntities += Iter.GetNumEntities(); 
            }

      return Cubes.Count + Spheres.Count + TotalEntities; 
    } 

    public bool AddEntity(GameObject aCubo,Figures Type)
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
                switch(Type)
                {
                    case Figures.Cubo:
                         Cubes.Add(aCubo);

                    break;
                    case Figures.Esfera:
                        Spheres.Add(aCubo);
                    break;
                    default: 
                    break;
                }
                

                if(Cubes.Count + Spheres.Count >= 4)
                {
                    
                    Subdivide();
                    
                }
                

            }

            else
            {
                foreach(Octreenode Cubito in octreeNodes)
                {

                    if(Cubito.AddEntity(aCubo,Type))
                    {
                        break;
                    }
                } 
            }
            
            return true;
        } 
                
        return false;

    }

    public List<GameObject> GetAllCubes()
    {
        return Cubes;
    }

    public List<GameObject> GetAllSpheres()
    {
       foreach (Octreenode Iter in octreeNodes)
        {
            foreach(GameObject Iter2 in Iter.GetAllSpheres())
            {
                Spheres.Add(Iter2);
            }
        }
        return Spheres;
    }
    public void SubdivideUpdater()
    {
        foreach (Octreenode Iter in octreeNodes)
        {
            Iter.SubdivideUpdater();
        }

        List<GameObject> SphereToDelete = new List<GameObject>();
        foreach(GameObject Sphere in Spheres)
        {
            Vector3 EntityPosition = Sphere.transform.position;
            Vector3 MinNode = basePosition - (baseSize *0.5f);
            Vector3 MaxNode = basePosition + (baseSize *0.5f);

            if(EntityPosition.x <= MinNode.x || EntityPosition.x >= MaxNode.x || 
            EntityPosition.y <= MinNode.y || EntityPosition.y >= MaxNode.y ||
            EntityPosition.z <= MinNode.z || EntityPosition.z >= MaxNode.z)
            {
                SphereToDelete.Add(Sphere);
            }
            
        }

        foreach(GameObject Sphere in SphereToDelete)
        {            
            Spheres.Remove(Sphere);
            CubeSpawner.instance.AddEntity(Sphere, Figures.Esfera);
        }
  
        if (octreeNodes.Count > 0)
        {
            int TotalEntities = 0;

            foreach(Octreenode Iter in octreeNodes)
            {
                TotalEntities += Iter.GetNumEntities(); 
            }

            if(TotalEntities < 4 && TotalEntities != 0)
            {
                foreach(Octreenode Iter in octreeNodes)
                {
                    foreach (GameObject Cubos in Iter.GetAllCubes().ToList())
                    {
                        CubeSpawner.instance.AddEntity(Cubos,Figures.Cubo); 
                    }
                    
                    foreach (GameObject Sfera in Iter.GetAllSpheres().ToList())
                    {
                        CubeSpawner.instance.AddEntity(Sfera,Figures.Esfera); 
                    }
                }
                octreeNodes.Clear();
                
            }
            }

    }   

}
