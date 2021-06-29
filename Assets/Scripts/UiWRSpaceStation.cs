using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class UiWRSpaceStation : MonoBehaviour
{
    bool disordered = false; // Por algún motivo la página, a veces, desordena los datos.
    private const string pathISS = "http://api.open-notify.org/iss-now.json";

    public class ISS
    {
        public string latitude = "";
        public string longitude = "";
    }

    public ISS iss = new ISS();

    public TextMeshProUGUI text;
    private float reconnectTime = 3;
    private float onTime;
    private float timeToRequest = 0.8f;

    private void Start()
    {
        StartCoroutine(GetRequestISS(pathISS));
    }

    void Update()
    {
        onTime += Time.deltaTime;
        if (onTime > timeToRequest)
        {
            onTime = 0;
            StartCoroutine(GetRequestISS(pathISS));
        }
    }

    IEnumerator GetRequestISS(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.isHttpError || request.isNetworkError)
            {
                Debug.LogWarning("Error con el Servidor de Consulta");
            }
            else
            {
                string data = request.downloadHandler.text;

                if (data == null)
                {
                    onTime = -reconnectTime;
                    yield break;
                }

                iss.latitude = "";
                iss.longitude = "";

                int indexlat = 0;
                int i = 0;

                if (disordered)
                {
                    while (indexlat != 2)
                    {
                        if (data[i] == ':')
                        {
                            indexlat++;
                        }

                        i++;
                    }

                    indexlat = 0;
                }

                int aux = 0;
                aux = disordered ? 3 : 4;

                while (indexlat != aux)
                {
                    if (data[i] == '"')
                    {
                        indexlat++;
                    }

                    i++;
                }

                i++;
                while (data[i] != '"')
                {
                    iss.latitude += data[i];
                    i++;
                }
                //Debug.Log("Latitude: " + iss.latitude);

                indexlat = 0;
                i++;
                while (indexlat != 3)
                {
                    if (data[i] == '"')
                    {
                        indexlat++;
                    }

                    i++;
                }

                while (data[i] != '"')
                {
                    iss.longitude += data[i];
                    i++;
                }
                //Debug.Log("Longitude: " + iss.longitude);
                float lat = 0;
                float lon = 0;
                try
                {
                    lat = float.Parse(iss.longitude);
                }
                catch (Exception e)
                {
                    disordered = !disordered;
                    onTime = 1;
                    yield break;
                }
                text.text = "International\nSpace\nStation:\n\nNow on:\nLatitude: " + iss.latitude + "\nLongitude: " + iss.longitude;
            }
        }
    }
}