using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseLanding : MonoBehaviour
{
    public int indexDistance;
    public TextMeshProUGUI text;
    void Start()
    {
        text.text = "X " + indexDistance;
    }
}