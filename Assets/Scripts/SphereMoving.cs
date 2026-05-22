using UnityEngine;

public class SphereMoving : MonoBehaviour
{
   Vector3 Pos;
   float Size;
   Vector3 dir = new Vector3(1,1,1);
   float Vel = 0.01f;
    void Start()
    {
        
    }
    public void SetData(Vector3 pos, float size)
    {
        Pos = pos;
        Size = size;
    }
   
    void Update()
    {
        if (transform.position.x > Pos.x + Size || transform.position.x < Pos.x + -Size)
            {
                dir.x *= -1;
            }
            if(transform.position.y > Pos.y + Size || transform.position.y < Pos.y + -Size)
            {
                dir.y *= -1;
            }
            if (transform.position.z > Pos.z + Size || transform.position.z < Pos.z + -Size)
            {
                dir.z *= -1;
            }
                
            transform.position += dir * Vel;
    }
}
