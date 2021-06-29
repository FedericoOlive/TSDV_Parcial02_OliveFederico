using UnityEngine;

public class UiButtonEffect : MonoBehaviour
{
    [SerializeField] private float scaleMultiply = 3;
    [SerializeField] private float limit = 1.2f;
    private bool increment = false;
    private Vector3 initialScale;
    private Vector3 scale;

    private void Awake()
    {
        increment = false;
        initialScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = initialScale;
        increment = false;
    }

    private void Update()
    {
        ChangeScale();
    }

    public void OnMouseEnterButton()
    {
        increment = true;
    }
    public void OnMouseExitButton()
    {
        increment = false;
    }
    private void ChangeScale()
    {
        float timeStep = scaleMultiply * Time.deltaTime;
        scale = transform.localScale;
        if (increment)
        {
            if (transform.localScale.x < limit)
            {
                scale = new Vector3(scale.x + timeStep, scale.y + timeStep, scale.z + timeStep);
                transform.localScale = scale;
            }
            else
            {
                transform.localScale = new Vector3(limit, limit, limit);
            }
        }
        else
        {
            if (transform.localScale.x > initialScale.x)
            {
                scale = new Vector3(scale.x - timeStep, scale.y - timeStep, scale.z - timeStep);
                transform.localScale = scale;
            }
            else
            {
                transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
            }
        }
    }
}