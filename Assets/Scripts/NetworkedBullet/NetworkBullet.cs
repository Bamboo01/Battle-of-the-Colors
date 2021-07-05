using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using CSZZGame.Networking;
using CSZZGame.Refactor;

public class NetworkBullet : NetworkBehaviour
{
    [SerializeField] public float bulletSpeed = 10.0f;
    [SerializeField] Renderer renderer;
    [SerializeField] Collider collider;
    [SerializeField] Rigidbody rb;

    // Properties for the particles
    public ParticlePainterProperties properties;
    // The ACTUAL color of the particle
    private Color color;
    // Particles that will only be spawned on the server
    public GameObject brushParticlesPrefab;


    // Bullet properties
    private NetworkConnection shooterConnection;
    private float gravityModifier;
    private Vector3 lastPosition;
    private float distanceTravelled = 0.0f;
    public float maxDistanceTravelled = 300.0f;
    public float paintingScale = 1.0f;
    private int damage;
    public ServerCharacterData.CHARACTER_TEAM bulletTeam;

    [Server]
    public void ServerSetup(NetworkConnection conn, ServerCharacterData.CHARACTER_TEAM team, float speed, float paintscale = 1.0f, float maxDistance = 300.0f, float gravity = 9.8f, int dmg = 1)
    {
        shooterConnection = conn;
        bulletTeam = team;
        color = ServerCharacterData.teamToColor(bulletTeam);
        bulletSpeed = speed;
        gravityModifier = gravity;
        maxDistanceTravelled = maxDistance;
        paintingScale = paintscale;
        rb.useGravity = false;
        damage = dmg;
    }

    [Server]
    void ServerUpdate()
    {
        distanceTravelled += (lastPosition - transform.position).magnitude;
        lastPosition = transform.position;

        if (distanceTravelled > maxDistanceTravelled)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    public override void OnStartServer()
    {
        lastPosition = transform.position;
        rb.velocity = transform.forward * bulletSpeed;
    }

    public override void OnStartClient()
    {
        if (!isServer)
        {
            Destroy(collider);
            Destroy(rb);
        }
    }

    void Update()
    {
        if (isServer)
        {
            ServerUpdate();
        }
    }

    void FixedUpdate()
    {
        if (isServer && rb)
        {
            rb.AddForce(new Vector3(0, -gravityModifier, 0) * rb.mass);
        }
    }

    [Server]
    void OnCollisionEnter(Collision collision)
    {
        renderer.enabled = false;
        Destroy(collider);
        Destroy(rb);

        Vector3 dir = -collision.contacts[0].normal;
        Vector3 newForwardDir = Vector3.Cross(transform.right, dir);
        Vector3 lookPos = transform.position + newForwardDir.normalized;
        SpawnParticlesRPC(transform.position, lookPos, -dir, color, paintingScale);

        if (collision.gameObject.layer == LayerMask.NameToLayer("player"))
        {
            collision.gameObject.GetComponent<NetworkCharacter>().takeDamage(shooterConnection, damage, bulletTeam);
            return;
        }

        // Spawn brush particles locally (server)
        GameObject brushParticles = Instantiate(brushParticlesPrefab, transform.position, transform.rotation);
        var brushparticles = brushParticles.GetComponent<BulletBrushParticlesManager>();
        brushparticles.SetupServerParticles(properties, color, paintingScale);
        brushParticles.transform.LookAt(lookPos, -dir);
        Destroy(brushParticles, 3.0f);
        // Radially Paint
        RadiallyPaint(transform.position, transform.localScale.x);

        if ((NetworkManager.singleton as NetworkRoomManagerScript).isDedicatedServer)
        {
            Destroy(gameObject);
        }
    }

    [Server]
    void RadiallyPaint(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius * 2.5f);
        foreach (var hitCollider in hitColliders)
        {
            Paintable p = hitCollider.GetComponent<Paintable>();
            if (p != null)
            {
                int hitPaintableID = NetworkPainterManager.Instance.GetPaintableID(p);
                CSZZNetworkInterface.Instance.SendNetworkEvent(EventChannels.OnPaintPaintableEvent, new PaintParticlesNetworkData(properties, transform.position, hitPaintableID, color, paintingScale * 5.0f));
            }
        }
    }


    [ClientRpc]
    void SpawnParticlesRPC(Vector3 contactPoint, Vector3 target, Vector3 up, Vector4 color, float scale)
    {
        Debug.Log("Particles spawned");

        GameObject particlesGameObject = ObjectPool.Instance.spawnFromPool("PaintBulletParticles");
        particlesGameObject.transform.position = contactPoint;
        particlesGameObject.transform.rotation = transform.rotation;
        particlesGameObject.transform.LookAt(target, up);
        particlesGameObject.GetComponent<BulletPaintParticlesManager>().SetupPaintParticles(color, scale);
        particlesGameObject.SetActiveDelayed(false, 3.0f);


        AudioSource pointSound = SoundManager.Instance.PlaySoundAtPointByName("Splash" + Random.Range(0, 4).ToString(), contactPoint);

        Destroy(gameObject);
    }
}
