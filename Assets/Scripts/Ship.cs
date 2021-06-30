using System;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [Serializable] private class PlayerPhysics
    {
        public float mass;
        public float linearDrag;
        public float angularDrag;
        public float gravityScale;
    }

    [Serializable] public class PlayerInputs
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

    AudioSource audioPropulsor;
    public GameObject[] propulsor;
    public Action PlayerLandSuccesful;
    public Action PlayerExplode;
    public LayerMask terrainLM;
    private const float velocityToExplode = 2;
    private const int angleToExplode = 20;
    private float playerHeight = 3.83f;
    private float onTimeFuel;
    private float timeRateFuel = 0.25f;
    private Rigidbody2D rb;
    public int level = 1;
    public int score;
    public int scoreOnWin = 50;
    public float maxFuel = 1000;
    public float fuelConsumption = 0.5f;
    public float fuel;
    public enum Status {Fly, Land, Explode}
    public Status status = Status.Fly;
    private Animator anim;

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
        audioPropulsor = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        DataPersistant.Get().LoadData();
        //Debug.Log("Fuel: " + fuel);
        //Debug.Log("FuelData: " + DataPersistant.Get().data.playerFuel);
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
        if (status == Status.Fly)
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
                        PlaySound(true, audioPropulsor);
                    }
                }
                else
                {
                    PlaySound(false, audioPropulsor);
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
        //Debug.Log("Collision: Velx:" + velocity.x + ", VelY: " + velocity.y);
        string tags = other.gameObject.tag;

        switch (tags)
        {
            case "BarrelFuel":
                FullFuel();
                Destroy(other.gameObject);
                if (CheckExplode(velocity))
                    Explode();
                break;
            case "BaseInitial":
                if (CheckExplode(velocity))
                    Explode();
                break;
            default:
                if (!CheckExplode(velocity))
                {
                    status = Status.Land;
                    Invoke(nameof(CheckLand), 2);
                }
                else
                {
                    Explode();
                }
                break;
        }
    }

    bool CheckExplode(Vector2 velocity)
    {
        float rotation = transform.rotation.eulerAngles.z;
        return !((rotation < angleToExplode || rotation > 360 - angleToExplode) && Mathf.Abs(velocity.x) < velocityToExplode && Mathf.Abs(velocity.y) < velocityToExplode);
    }

    void Explode()
    {
        Debug.Log("Explode.");
        status = Status.Explode;
        anim.SetTrigger("Explode");
        fuel = 0;
        PlayerExplode?.Invoke();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.isKinematic = true;
        Destroy(DataPersistant.Get().gameObject);
    }
    void CheckLand()
    {
        Vector2 velocity = rb.velocity;
        if (!CheckExplode(velocity))
        {
            Debug.Log("Land succesful.");
            ReceiveScore();
            DataPersistant.Get().SaveData();
            PlayerLandSuccesful?.Invoke();
        }
        else
            PlayerExplode?.Invoke();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.isKinematic = true;
    }

    void FullFuel()
    {
        fuel = maxFuel;
    }
    void ReceiveScore()
    {
        score += scoreOnWin;
    }
    void PlaySound(bool soundOn, AudioSource audioTrack)
    {
        if (soundOn && !audioTrack.isPlaying)
            audioTrack.Play();
        else if (!soundOn)
            audioTrack.Stop();
    }
}