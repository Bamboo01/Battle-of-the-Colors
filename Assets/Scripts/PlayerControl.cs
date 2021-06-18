using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;


public class PlayerControl : MonoBehaviour
{
    public static PlayerControl S;
    private void Awake()
    {

        S = this;

    }
    private FSMManager fsmManager;
    public GameObject Target;
    //开火点
    public Transform FirPoint;
    //开火效果
    public GameObject FireEffect;
    public GameObject bullet;
    float bulletCD;
    bool isbulletOK;

    //private AimIK aimIK;
    public  CharacterController controller;
    public Transform cam;
    public bool isStealth;
    private float vertical;
    private float horizontal;
    public Vector3 dir;

    //攀爬
    public float wallRayLength = 1f;
    public bool onWall, isClimbing;
    public float walloffset = 0.5f;
    public Vector3 targetPos;


    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    // Start is called before the first frame update
    void Start()
    {
        isbulletOK = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //aimIK = GetComponent<AimIK>();
        controller = GetComponent<CharacterController>();
        fsmManager = new FSMManager();
        IdleState idle = new IdleState(0, this, fsmManager);
        RunState run = new RunState(1, this, fsmManager);
       
        climbState climb = new climbState(3, this, fsmManager);
        //StealthState stealth = new StealthState(4, this, fsmManager);
        fsmManager.stateList.Add(idle);
        fsmManager.stateList.Add(run);
       
        fsmManager.stateList.Add(climb);
        //fsmManager.stateList.Add(stealth);
        fsmManager.ChangeState((int)PlayerState.idle);
        

    }

    
    // Update is called once per frame
    void Update()
    {
        if (EControl.swit)       
        GetComponent<Animator>().SetTrigger("Switch");
        //float mouseX = Input.GetAxis("Mouse X");
        //float mouseY = Input.GetAxis("Mouse Y");
        //transform.Rotate(Vector3.up, mouseX * Time.deltaTime * 90);
        // Camera.main.transform.parent.Rotate(Vector3.right, mouseY * Time.deltaTime * 60);
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        dir = new Vector3(horizontal, 0, vertical).normalized;
        controller.SimpleMove(Physics.gravity);

        //射线
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        bool res = Physics.Raycast(ray,out hit);

        if (res)
        {

            //枪有瞄准的物体;
            Target.transform.position = hit.point;
            //aimIK.enabled = true;

        }
        else
        {
            //瞄准的是天空
            //aimIK.enabled = false;
        }
        fsmManager.Update();
        //CheckClimb();
        ShootCD();
       
        //监听攻击状态
        if (Input.GetMouseButtonDown(0)&& isbulletOK)
        {
            transform.rotation = cam.rotation;

        }
    }
    public bool CheckClimb()
    {
        
        Vector3 origin = transform.position;
        Vector3 dir = transform.forward;
        RaycastHit hit;
        Debug.DrawRay(origin, dir * wallRayLength, Color.green); ;
        if (Physics.Raycast(origin, dir, out hit, wallRayLength))
        {
            InitClimb(hit);
            return true;
        }
        else
        {
            fsmManager.ChangeState((int)PlayerState.idle);
            isClimbing = false;
            return false;

        }

    }
    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0) && isbulletOK)
        {
            Shoot();

        }
    }
    public void InitClimb(RaycastHit hit)
    {
       
        isClimbing = true;
        onWall = false;
        targetPos = hit.point + hit.normal * walloffset;
    }
    public void ShootCD()
    {
        if (!isbulletOK)
        {
            bulletCD += Time.deltaTime;
        }

        if (bulletCD >= 0.5f)
        {
            isbulletOK = true;
        }
    }
    public void Shoot() {
        {
            
            GetComponent<Animator>().SetTrigger("Attack");
            
            GameObject go = Instantiate(FireEffect,FirPoint.position,FirPoint.rotation);

            GameObject bullet1=Instantiate(bullet, FirPoint.position, FirPoint.rotation);

            go.transform.parent = FirPoint.transform;
            Destroy(go, 0.2f);
            isbulletOK = false;
            bulletCD = 0f;
           

        }
    }

}
