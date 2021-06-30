using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class WebsRequests : MonoBehaviour
{
    // Por algún motivo cambian de orden: Posibilidades:
    // Order 1.1: {"iss_position": {"longitude": "-0.5100", "latitude": "-26.6301"}, "message": "success", "timestamp": 1624974597}
    // Order 1.2: {"iss_position": {"latitude": "-26.6301", "longitude": "-0.5100"}, "message": "success", "timestamp": 1624974597}
    // Order 2.1: {"message": "success", "iss_position": {"longitude": "-0.5100", "latitude": "-26.6301"}, "timestamp": 1624974597}
    // Order 2.2: {"message": "success", "iss_position": {"latitude": "-26.6301", "longitude": "-0.5100"}, "timestamp": 1624974597}
    // Order 3.1: {"message": "success", "timestamp": 1624974597, "iss_position": {"longitude": "-0.5100", "latitude": "-26.6301"}}
    // Order 3.2: {"message": "success", "timestamp": 1624974597, "iss_position": {"latitude": "-26.6301", "longitude": "-0.5100"}}

    private const string pathISS = "http://api.open-notify.org/iss-now.json";
    private string longLat = "";
    private int indexAperturaLlave;
    private int indexCierreLlave;
    private int indexComa;
    private string varLat;
    private string varLon;
    private int charO_lOngitude;
    private int charI_latItude;

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
        StartCoroutine(GetRequest(pathISS));
    }

    void Update()
    {
        onTime += Time.deltaTime;
        if (onTime > timeToRequest)
        {
            onTime = 0;
            StartCoroutine(GetRequest(pathISS));
        }
    }

    IEnumerator GetRequest(string url)
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

                indexAperturaLlave = data.IndexOf("{", 2);
                indexCierreLlave = data.IndexOf("}", 2);
                longLat = data.Substring(indexAperturaLlave, indexCierreLlave - indexAperturaLlave); // {"longitude": "-0.5100", "latitude": "-26.6301"}
                indexComa = longLat.IndexOf(",");

                charO_lOngitude = longLat.IndexOf('o');
                charI_latItude = longLat.IndexOf('i');

                if (charI_latItude < charO_lOngitude)
                {
                    varLon = longLat.Substring(14, indexComa - 15);
                    varLat = longLat.Substring(indexComa + 15);
                    varLat = varLat.Substring(1, varLat.Length - 2);
                }
                else
                {
                    varLon = longLat.Substring(15, indexComa - 16);
                    varLat = longLat.Substring(indexComa + 14);
                    varLat = varLat.Substring(1, varLat.Length - 2);
                }
                iss.longitude = varLon;
                iss.latitude = varLat;
                
                text.text = "International\nSpace\nStation:\n\nNow on:\nLatitude: " + iss.latitude + "\nLongitude: " + iss.longitude;
            }
        }
    }
}