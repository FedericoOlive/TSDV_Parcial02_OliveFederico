using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiMainMenu : MonoBehaviour
{
    private String[] menues = { "Main Menu", "Options", "Credits", "Exit" };

    [Serializable]
    public class PanelsList
    {
        public List<Image> images = new List<Image>();
        public List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    }

    public List<PanelsList> menu = new List<PanelsList>();
    public GameObject panelBlockRaycast;
    bool buttonPressed;
    int currentMenu;
    [SerializeField] private float durationTransition = 2;
    private float colortime;

    private void Start()
    {
        GameObject[] panels = GameObject.FindGameObjectsWithTag("Panels");

        for (int i = 0; i < panels.Length; i++)
        {
            menu.Add(new PanelsList());

            foreach (Image child in panels[i].GetComponentsInChildren<Image>(true))
            {
                menu[i].images.Add(child);
                child.gameObject.SetActive(false);
            }

            foreach (TextMeshProUGUI child in panels[i].GetComponentsInChildren<TextMeshProUGUI>(true))
            {
                menu[i].texts.Add(child);
                child.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < menu.Count; i++)
        {
            if (i == (int)currentMenu)
            {
                foreach (var btn in menu[i].images)
                {
                    btn.gameObject.SetActive(true);
                }

                foreach (var txt in menu[i].texts)
                {
                    txt.gameObject.SetActive(true);
                }
            }
        }

        try
        {
            Destroy(FindObjectOfType<DataPersistant>().gameObject);
        }
        catch (Exception e)
        {
            // ignored
        }
    }

    private void Update()
    {
        if (buttonPressed)
        {
            OnMenu();
            if (panelBlockRaycast)
            {
                panelBlockRaycast.SetActive(buttonPressed);
            }
        }
    }

    public void ButtonPressed(int _currentMenu)
    {
        buttonPressed = true;
        currentMenu = _currentMenu;

        for (int i = 0; i < menu.Count; i++)
        {
            if (i == (int)currentMenu)
            {
                foreach (var btn in menu[i].images)
                {
                    btn.gameObject.SetActive(true);
                    Color color = btn.color;
                    btn.color = new Color(color.r, color.g, color.b, 0);
                }

                foreach (var txt in menu[i].texts)
                {
                    txt.gameObject.SetActive(true);
                    Color color = txt.color;
                    txt.color = new Color(color.r, color.g, color.b, 0);
                }
            }
        }
    }

    private void OnMenu()
    {
        float deltaT = Time.deltaTime;
        colortime += deltaT;

        for (int i = 0; i < menu.Count; i++)
        {
            if (i != (int)currentMenu)
            {
                foreach (var btn in menu[i].images)
                {
                    Color color = btn.color;
                    color.a = 1;
                    Color colorTransparent = new Color(color.r, color.g, color.b, 0);
                    btn.color = Color.Lerp(color, colorTransparent, colortime / durationTransition);

                    if (colortime > durationTransition || !buttonPressed)
                    {
                        btn.color = new Color(color.r, color.g, color.b, 0);
                        btn.gameObject.SetActive(false);
                        buttonPressed = false;
                        colortime = 0;
                    }
                }

                foreach (var txt in menu[i].texts)
                {
                    Color color = txt.color;
                    color.a = 1;
                    Color colorTransparent = new Color(color.r, color.g, color.b, 0);

                    txt.color = Color.Lerp(color, colorTransparent, colortime / durationTransition);

                    if (colortime > durationTransition || !buttonPressed)
                    {
                        txt.color = new Color(color.r, color.g, color.b, 0);
                        txt.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                foreach (var btn in menu[i].images)
                {
                    Color color = btn.color;
                    color.a = 0;
                    Color colorOpaque = new Color(color.r, color.g, color.b, 1);
                    btn.color = Color.Lerp(color, colorOpaque, colortime);

                    if (colortime > durationTransition || !buttonPressed)
                    {
                        btn.color = new Color(color.r, color.g, color.b, 1);
                        buttonPressed = false;
                    }
                }

                foreach (var txt in menu[i].texts)
                {
                    Color color = txt.color;
                    color.a = 0;
                    Color colorOpaque = new Color(color.r, color.g, color.b, 1);

                    txt.color = Color.Lerp(color, colorOpaque, colortime / durationTransition);

                    if (colortime > durationTransition || !buttonPressed)
                    {
                        txt.color = new Color(color.r, color.g, color.b, 1);
                    }
                }
            }
        }
    }

    public void ChangeScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}