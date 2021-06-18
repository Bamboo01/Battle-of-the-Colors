using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EControl : MonoBehaviour
{
    public GameObject E, cam, 角色, PlayerParent, 机甲;
    public Collider collider;
    bool isPlayer;
    public static bool swit;
    private void Awake()
    {
        GameObject go = Instantiate(角色);
        go.transform.parent = PlayerParent.transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        swit = false;
        collider = GetComponent<Collider>();
        E.SetActive(false);
        isPlayer = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (E.activeSelf)
        {
            if (isPlayer && Input.GetKeyDown(KeyCode.E))
            {
                swit = true;
                Invoke("panduan", 0.5f);
            }
        }
    }
    private void LateUpdate()
    {
        if (!isPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                swit = false;
                cam.SetActive(false);
                GameObject.Find("GameObject (1)").GetComponent<TankCam>().enabled = false;
                GameObject.Find("GameObject (1)").GetComponent<tankecontrol>().enabled = false;
                GameObject go = Instantiate(角色, transform.position + new Vector3(10, 0, 10), transform.rotation);
                go.transform.parent = PlayerParent.transform;

                isPlayer = true;
            }

        }
    }
    void panduan()
    {
        isPlayer = false;
        cam.SetActive(true);
        GameObject.Find("GameObject (1)").GetComponent<TankCam>().enabled = true;
        GameObject.Find("GameObject (1)").GetComponent<tankecontrol>().enabled = true;
        E.SetActive(false);
        swit = false;
        Destroy(PlayerParent.transform.GetChild(0).gameObject);

    }
    private void OnTriggerStay(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                
                E.SetActive(true);
                break;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                
                E.SetActive(false);
                break;

        }
    }
}
