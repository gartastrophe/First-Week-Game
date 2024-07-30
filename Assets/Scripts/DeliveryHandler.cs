using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryHandler : MonoBehaviour
{
    public static int deliveryCount = 0;
    public ParticleSystem deliveryEffect;
    public AudioClip deliverySoundClip;

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
        if (collision.gameObject.CompareTag("DeliveryArea"))
        {
            if (deliveryEffect != null)
            {
                ParticleSystem effect = Instantiate(deliveryEffect, collision.transform.position, Quaternion.identity);
                effect.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f); 
                effect.Play();
                Destroy(effect.gameObject, effect.main.duration); 
            }
            if (deliverySoundClip != null)
            {
                AudioSource.PlayClipAtPoint(deliverySoundClip, collision.transform.position);
            }
            Destroy(gameObject, 1);
        }
    }

    private void OnDestroy()
    {
        deliveryCount++;
    }
}
