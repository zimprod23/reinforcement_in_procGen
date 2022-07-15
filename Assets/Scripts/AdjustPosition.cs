using UnityEngine;

public class AdjustPosition : MonoBehaviour
{
   void Start(){
    Adjust();
   }

   void Adjust(){
    if(this.transform.position.y < 50)Destroy(this.gameObject);
   }
}
