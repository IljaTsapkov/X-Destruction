using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{

    public long lastUpdated;

    public Vector3 playerPosition;

    public float playerHealth;

    public int playerArmor;

    public List<string> collectedUsables = new List<string>();

    public List<string> touchedCheckpoints = new List<string>();

    public List<string> killedEnemies = new List<string>();

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData() {
        playerPosition = new Vector3(115,1,58);
        playerHealth = 100f;
    }

}
