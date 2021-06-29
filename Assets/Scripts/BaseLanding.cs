using TMPro;
using UnityEngine;

public class BaseLanding : MonoBehaviour
{
    private Ship player;
    public int indexDistance;
    public TextMeshProUGUI text;
    private int baseScore = 50;

    void Start()
    {
        text.text = "X " + indexDistance;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        player = other.gameObject.GetComponent<Ship>();
        player.scoreOnWin = baseScore * indexDistance;
    }
}