using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawn : MonoBehaviour
{
    [SerializeField]
    List<Transform> spawnPoints = new List<Transform>();
    [SerializeField]
    public List<Transform> walkPonts = new List<Transform>();
    List<Transform> usedSpawnPoints = new List<Transform>();
    [SerializeField]
    GameObject enemy;
    Dictionary<string, Transform> enemys = new Dictionary<string, Transform>();
    [SerializeField]
    int numOfZomby;

    [SerializeField]

    void Awake()
    {
        if(spawnPoints.Count <= 0)
        {
            Debug.LogError("Error! You didn't assign spawn points");
        }
        if (walkPonts.Count <= 0)
        {
            Debug.LogError("Error! You didn't assign walk points");
        }
        if(!enemy)
        {
            Debug.LogError("Error! You didn't assign enemy object");
        }
    }

    void Start()
    {
        SpawnZomby();
    }

    void OnTriggerExit(Collider obj)
    {
        if (obj.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var enemy_script = obj.GetComponent<Enemy>();
            DeliteEnemyFromChunkByTag(enemy_script.GetIndex().ToString());
            Debug.Log("Deleted from " + gameObject.name + ": " + enemy_script.GetIndex().ToString());
        }
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var enemy_script = obj.GetComponent<Enemy>();
            AddEnemytoChunk(enemy_script.GetIndex().ToString(), obj.transform);
            Debug.Log("Added to " + gameObject.name + ": " + enemy_script.GetIndex().ToString());
            obj.GetComponent<FinetStateMachine>().ChunkSpawn = this;
        }
    }
    /////////////////////////////////////////////////////////////////////////////////

    void SpawnZomby()
    {
        while (numOfZomby >= 1)
        {
            numOfZomby--;
            var zomby = Instantiate(enemy) as GameObject;
            zomby.GetComponent<FinetStateMachine>().ChunkSpawn = this;
            while (true)
            {
                int key = Random.Range(0, spawnPoints.Count);
                if (!usedSpawnPoints.Contains(spawnPoints[key]))
                {
                    ZombyStartPropertes(zomby, key);
                    break;
                }
            }
        }
    }

    void ZombyStartPropertes(GameObject zomby, int key)
    {
        zomby.transform.position = spawnPoints[key].position;
        usedSpawnPoints.Add(spawnPoints[key]);
        var enemy_script = zomby.GetComponent<Enemy>();
        enemy_script.SetIndex(World.zomby_index++);
        enemys.Add(enemy_script.GetIndex().ToString(), zomby.transform);
    }

    void AddEnemytoChunk(string tag, Transform objtrans)
    {
        if (!enemys.ContainsKey(tag))
        {
            enemys.Add(tag, objtrans);
        }
        return;
    }

    void DeliteEnemyFromChunkByTag(string tag)
    {
        if (enemys.ContainsKey(tag))
        {
            enemys.Remove(tag);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////
}
