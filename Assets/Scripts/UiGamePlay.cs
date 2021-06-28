using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiGamePlay : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject[] uiArrowLeftRight;
    public GameObject[] uiArrowUpDown;
    public GameObject uiClock;

    public TextMeshProUGUI textVelH;
    public TextMeshProUGUI textVelV;
    public TextMeshProUGUI textAltitude;
    public TextMeshProUGUI textFuel;
    public TextMeshProUGUI textClock;

    public Image uiFuelPlayer;
    public float timer;

    public Ship player;
    private Rigidbody2D rbPlayer;

    void Start()
    {
        rbPlayer = player.GetComponent<Rigidbody2D>();
        Pause(false);
    }
    void Update()
    {
        timer += Time.deltaTime;
        textClock.text = ConvertTimerToString(timer);
        uiFuelPlayer.fillAmount = player.fuel / player.maxFuel;
        textFuel.text = player.fuel.ToString("F0");
        Vector2 playerVel = rbPlayer.velocity;

        ActiveVelocityUI(playerVel.x, ref uiArrowLeftRight, textVelH);
        ActiveVelocityUI(playerVel.y, ref uiArrowUpDown, textVelV);

        textAltitude.text = (player.AltitudeShip(false) * 10).ToString("F0");
    }
    void ActiveVelocityUI(float playerVelocity, ref GameObject[] go, TextMeshProUGUI text)
    {
        int velocity = (int) (playerVelocity * 10);
        if (velocity < 0)
        {
            go[0].SetActive(true);
            go[1].SetActive(false);
            go[2].SetActive(false);
        }
        else if (velocity > 0)
        {
            go[0].SetActive(false);
            go[1].SetActive(true);
            go[2].SetActive(false);
        }
        else
        {
            go[0].SetActive(false);
            go[1].SetActive(false);
            go[2].SetActive(true);
        }
        text.text = velocity.ToString();
    }
    string ConvertTimerToString(float timer)
    {
        int seconds = (int) timer;
        int minutes = 0;
        while (seconds > 59)
        {
            seconds -= 60;
            minutes++;
        }
        float newAngle = (360 * seconds) / 60.0f;
        uiClock.transform.rotation = Quaternion.Euler(0, 0, -newAngle);
        string text = (minutes < 10) ? "0" + minutes : minutes.ToString();
        text += ":";
        text += (seconds < 10) ? "0" + seconds : seconds.ToString();
        return text;
    }
    public void Pause(bool onPause)
    {
        Time.timeScale = onPause ? 0 : 1;
        panels[0].SetActive(!onPause);
        panels[1].SetActive(onPause);
    }
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}