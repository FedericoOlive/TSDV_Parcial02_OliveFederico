using System;
using UnityEditorInternal;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private const float velocityToExplode = 0.1f;
    public enum Status {Fly, Land, Explode}

    public Status status = Status.Fly;

    public GameObject[] propulsor;
    public LayerMask terrainLM;
    private Animator anim;
    [Serializable]
    private class PlayerPhysics
    {
        public float mass;
        public float linearDrag;
        public float angularDrag;
        public float gravityScale;
    }

    [Serializable]
    public class PlayerInputs
    {
        public bool defaultValue = true;
        public KeyCode propulsor = KeyCode.A;
        public KeyCode rotateLeft = KeyCode.A;
        public KeyCode rotateRight = KeyCode.A;

        public void SetDefaultValues()
        {
            propulsor = KeyCode.W;
            rotateLeft = KeyCode.A;
            rotateRight = KeyCode.D;
        }
    }

    [SerializeField] private PlayerPhysics playerPhisics;
    [SerializeField] private PlayerInputs playerInputs;

    private Rigidbody2D rb;
    private float playerHeight = 3.83f;
    private float onTimeFuel;
    private float timeRateFuel = 0.25f;
    public float maxFuel = 1000;
    public float fuelConsumption = 0.5f;

    public float fuel = 1000;

    public enum Dificult
    {
        Easy,
        Normal,
        Hard,
        Extreme
    }
    public Dificult dificult;
    public float propulsorForce;
    public float rotateForce;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        if (playerInputs.defaultValue)
        {
            playerInputs.SetDefaultValues();
        }

        playerHeight = GetComponent<BoxCollider2D>().size.y;
        TransferPhysicsProperties();
    }
    void FixedUpdate()
    {
        PlayerInput();
    }
    
    public float CheckDistanceTerrain()
    {
        Vector2[] directionRay = new Vector2[3];
        directionRay[0] = new Vector2(-1, -1);
        directionRay[1] = new Vector2(0, -1);
        directionRay[2] = new Vector2(1, -1);
        float[] distanceRay = new float[3];

        for (int i = 0; i < directionRay.Length; i++)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, directionRay[i], 100);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.tag == "Terrain")
                {
                    distanceRay[i] = hit.distance;
                    Debug.Log("Distancia: " + hit.distance);
                    Debug.DrawRay(transform.position, directionRay[i], Color.red);
                }
            }
        }

        float minDistance = Mathf.Min(distanceRay);
        return minDistance;
    }
    void PlayerInput()
    {
        if (Time.timeScale > 0.5f)
        {
            if (Input.GetKey(playerInputs.propulsor))
            {
                if (fuel > 0)
                {
                    onTimeFuel += Time.deltaTime;
                    rb.AddForce(transform.up * propulsorForce);
                    if (onTimeFuel > timeRateFuel)
                    {
                        fuel -= fuelConsumption;
                        if (fuel < 0)
                            fuel = 0;
                    }
                    propulsor[0].SetActive(true);
                }
            }
            else
            {
                propulsor[0].SetActive(false);
            }

            if (Input.GetKey(playerInputs.rotateLeft))
            {
                rb.AddTorque(rotateForce);
            }

            if (Input.GetKey(playerInputs.rotateRight))
            {
                rb.AddTorque(-rotateForce);
            }
        }
    }
    private void TransferPhysicsProperties()
    {
        rb.mass = playerPhisics.mass;
        rb.drag = playerPhisics.linearDrag;
        rb.angularDrag = playerPhisics.angularDrag;
        rb.gravityScale = playerPhisics.gravityScale;
    }
    public float AltitudeShip(bool modeCheckAltitude)
    {
        if (modeCheckAltitude)  // Referencia al nivel del mar
        {
            return transform.position.y;
        }
        else    // Referencia al terreno
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000, terrainLM);
            if (hit)
            {
                return hit.distance - playerHeight / 2;
            }
            Debug.LogWarning("Altitud Ship no conecta con el Terreno.");
            return -1;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 velocity = rb.velocity;
        Debug.Log("Collision: Velx:" + velocity.x + ", VelY: " + velocity.y);

        if (other.gameObject.CompareTag("Terrain"))
        {
            if (Mathf.Abs(velocity.x) < velocityToExplode && Mathf.Abs(velocity.y) < velocityToExplode)
            {
                status = Status.Land;
                Debug.Log("Land succesful.");

            }
            else
            {
                Debug.Log("Explode.");
                status = Status.Explode;
                anim.SetTrigger("Explode");
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                rb.isKinematic = true;
            }
        }
    }
}