using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCam : MonoBehaviour
{
    public float speed = 20f;
    public static float mx, my;
    public float minLimity=10f,maxLimity=-5f;
    public GameObject fire1, fire2,mrotation,bullet;
    public Transform fire1Point, fire2Point;
    bool isShoot1OK, isShoot2OK;
    public float shoot1CD=0, shoot2CD=0;
    float fireCD1, fireCD2;
    
   
    //public GameObject cam, robte;
   // public GameObject fireEffect;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {

         mx += Input.GetAxis("Mouse X")*speed ;
         my -= Input.GetAxis("Mouse Y")*speed ;

        
        my = ClampAngle(my, minLimity, maxLimity);
      
        
       mrotation.transform.localEulerAngles = new Vector3(mrotation.transform.rotation.x-mx, mrotation.transform.rotation.y,  0);
       
        fire1.transform.localRotation = Quaternion.Euler(new Vector3(-90,0,180+my-10));
        fire2.transform.localRotation = Quaternion.Euler(new Vector3(90,180,-my+10));
        ShootCD();
        shoot();
    }
   private float ClampAngle(float angle,float min,float max)
    {
        if (angle < -360) angle += -360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
    void shoot()
    {
        if (Input.GetMouseButtonDown(0)&&isShoot1OK){
           GameObject Bullet= Instantiate(bullet,fire1Point.transform.position,fire1Point.transform.rotation);
            isShoot1OK = false;
            fireCD1 = 0f;
            //GameObject go= Instantiate(fireEffect, fire1Point.transform.position, fire1Point.transform.rotation);
            //Destroy(go, 0.2f);
        }
        if (Input.GetMouseButtonDown(1)&&isShoot2OK){
            GameObject Bullet= Instantiate(bullet,fire2Point.transform.position, fire2Point.transform.rotation);
            isShoot2OK = false;
            fireCD2 = 0f;
           // GameObject go= Instantiate(fireEffect, fire2Point.transform.position,fire2Point.transform.rotation);
            //Destroy(go, 0.2f);
        }
    }
    public void ShootCD()
    {
        if (!isShoot1OK)
        {
            fireCD1 += Time.deltaTime;
        }

        if (fireCD1 >= shoot1CD)
        {
            isShoot1OK = true;
        }
        if (!isShoot2OK)
        {
            fireCD2 += Time.deltaTime;
        }

        if (fireCD2 >= shoot2CD)
        {
            isShoot2OK = true;
        }
    }
   
}
