using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2ShieldCollisionScript : MonoBehaviour
{
    [SerializeField] Collider collider;
    static public List<PlayerCharacterScript> PlayerCharacterScripts = new List<PlayerCharacterScript>();

    private void Update()
    {
        foreach(PlayerCharacterScript p in PlayerCharacterScripts)
        {
            CharacterController controller = p.controller;
            Vector3 shieldDir = transform.forward;
            Vector3 rPos = controller.gameObject.transform.position - transform.position;
            float velDotShield = Vector3.Dot(controller.velocity, shieldDir);
            if (velDotShield > 0)
            {
                Physics.IgnoreCollision(controller, collider, true);
            }
            else
            {
                Physics.IgnoreCollision(controller, collider, false);
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == PlayerLayerMask)
    //    {
    //        Debug.Log("a");
    //        CharacterController controller = other.GetComponent<PlayerCharacterScript>().controller;
    //        Vector3 shieldDir = transform.forward;
    //        Vector3 rPos = controller.gameObject.transform.position - transform.position;
    //        float velDotShield = Vector3.Dot(controller.velocity, shieldDir);
    //        float rposDotShield = Vector3.Dot(rPos, shieldDir);

    //        if (velDotShield < 0 || rposDotShield > 0)
    //        {
    //            Physics.IgnoreCollision(controller, collider, true);
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.layer == PlayerLayerMask)
    //    {
    //        Debug.Log("b");
    //        CharacterController controller = other.GetComponent<PlayerCharacterScript>().controller;
    //        Physics.IgnoreCollision(controller, collider, false);
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == PlayerLayerMask)
    //    {
    //        Debug.Log("a");
    //        CharacterController controller = collision.gameObject.GetComponent<PlayerCharacterScript>().controller;
    //        Vector3 shieldDir = transform.forward;
    //        Vector3 rPos = controller.gameObject.transform.position - transform.position;
    //        float velDotShield = Vector3.Dot(controller.velocity, shieldDir);
    //        float rposDotShield = Vector3.Dot(rPos, shieldDir);

    //        if (velDotShield < 0 || rposDotShield < 0)
    //        {
    //            Physics.IgnoreCollision(controller, collider, true);
    //        }
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.layer == PlayerLayerMask)
    //    {
    //        Debug.Log("b");
    //        CharacterController controller = collision.gameObject.GetComponent<PlayerCharacterScript>().controller;
    //        Physics.IgnoreCollision(controller, collider, false);
    //    }
    //}
    //}
}
