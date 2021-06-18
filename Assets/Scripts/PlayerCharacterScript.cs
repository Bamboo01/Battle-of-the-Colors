using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterScript : MonoBehaviour
{
    [SerializeField] Transform gun;
    [SerializeField] Transform followpoint;
    public CharacterController controller { get; private set; }

    Vector3 dir;
    float speed = 5.0f;
    int ShieldLayerMask;
    public bool isStealth;

    private void Start()
    {
        A2ShieldCollisionScript.PlayerCharacterScripts.Add(this);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller = GetComponent<CharacterController>();
        ShieldLayerMask = LayerMask.NameToLayer("A2Shield");
        EventManager.Instance.Listen("ForwardInput", onForwardInput);
        EventManager.Instance.Listen("BackInput", onBackInput);
        EventManager.Instance.Listen("LeftInput", onLeftInput);
        EventManager.Instance.Listen("RightInput", onRightInput);
        EventManager.Instance.Listen("MouseInput", onMouseInput);
    }

    private void OnDestroy()
    {
        A2ShieldCollisionScript.PlayerCharacterScripts.Remove(this);
    }

    void onLeftInput(IEventRequestInfo info)
    {
        dir -= transform.right;
    }

    void onRightInput(IEventRequestInfo info)
    {
        dir += transform.right;
    }

    void onForwardInput(IEventRequestInfo info)
    {
        dir += transform.forward;
    }

    void onBackInput(IEventRequestInfo info)
    {
        dir -= transform.forward;
    }

    void onMouseInput(IEventRequestInfo info)
    {
        if (info is EventRequestInfo<MouseData>)
        {
            EventRequestInfo<MouseData> mouseInfo = (EventRequestInfo <MouseData>)info;
            gun.transform.Rotate(-Vector3.right * mouseInfo.body.X);
            followpoint.transform.Rotate(-Vector3.right * mouseInfo.body.Y);
            transform.Rotate(-Vector3.up * mouseInfo.body.X);
        }
    }

    private void Update()
    {
        if (dir.sqrMagnitude != 0)
        {
            
            if (isStealth)
            {
                controller.Move(2*speed * dir.normalized * Time.deltaTime);
            }
            if (isStealth == false)
            {
                controller.Move(speed * dir.normalized * Time.deltaTime);
            }
        }

        //Ǳ��
        stealth();
        //����
        controller.SimpleMove(Physics.gravity);
        dir = Vector3.zero;
    }
    public void stealth()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) &&isStealth == false)
        {         
            controller.center = new Vector3(controller.center.x,controller.center.y + 1, controller.center.z);
            isStealth = true;
        }

        else if (Input.GetKeyDown(KeyCode.LeftShift) && isStealth == true)
        {
            transform.position += new Vector3(0, 2, 0);
            controller.center = new Vector3(controller.center.x, controller.center.y - 1, controller.center.z);
            isStealth = false;
        }
    }
}
