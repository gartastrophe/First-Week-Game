using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryHandler : MonoBehaviour
{
    public static int deliveryCount = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("DeliveryArea")) 
        {
            Destroy(gameObject, 1);
        }
    }

    private void OnDestroy() 
    {
        deliveryCount++;
    }
            

}
