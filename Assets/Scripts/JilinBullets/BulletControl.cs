using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public float speed;
    bool istrigger;
    float destryCD;
    public GameObject boom;
    // Start is called before the first frame update
    private void Start()
    {
        istrigger = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (istrigger)
            Destroy(this.gameObject);

    }
    private void FixedUpdate()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (!istrigger)
        {
            destryCD += Time.deltaTime;
        }
        if (destryCD >= 10)
        {
            istrigger = true;
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        istrigger = true;
        GameObject go = Instantiate(boom, transform.position, transform.rotation);
        Destroy(go, 2f);
    }
}
