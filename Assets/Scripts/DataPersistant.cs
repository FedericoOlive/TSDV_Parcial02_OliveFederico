using UnityEngine;
public class DataPersistant : MonoBehaviourSingleton<DataPersistant>
{
    public struct Data
    {
        public float playerFuel;
        public int playerLevel;
        public int playerScore;
    }
    public Data data;
    
    private Ship player;
    
    void Start()
    {
        player = FindObjectOfType<Ship>();
        data.playerScore = 0;
        data.playerFuel = 1000;
        data.playerLevel = 1;
        LoadData();
    }
    public void SaveData()
    {
        player = FindObjectOfType<Ship>();
        Debug.Log("Guardando: player.fuel:" + player.fuel);
        data.playerScore = player.score;
        data.playerFuel = player.fuel;
        data.playerLevel = player.level;
    }
    public void DestroyDataPersistant()
    {
        if (gameObject)
            Destroy(gameObject);
    }
    public void LoadData()
    {
        player = FindObjectOfType<Ship>();
        player.fuel = data.playerFuel;
        Debug.Log("Cargando: player.fuel:" + player.fuel);
        player.level = data.playerLevel;
        player.score = data.playerScore;
    }
}