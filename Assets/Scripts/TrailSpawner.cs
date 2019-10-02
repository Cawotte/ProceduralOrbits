using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailSpawner : MonoBehaviour
{
    [Header("Gameobjects")]
    [SerializeField] private GameObject trailPrefab;

    [Header("Trail settings")]
    [SerializeField] private float spawnTime = 0.5f;

    [SerializeField] private float fadingTime = 1f;
    [SerializeField] private Color color;
    [SerializeField] private float relativeSize = 0.3f;

    private float timerTrail = 0f;
    private Orbital orbital;

    // Start is called before the first frame update
    void Start()
    {
        timerTrail = 0f;
        orbital = GetComponent<Orbital>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Spawn Trails
        if (timerTrail >= spawnTime)
        {
            SpawnTrail();
            timerTrail -= spawnTime;
        }

        timerTrail += Time.deltaTime;
    }
    
    public void InitializeTrail(
        GameObject trailPrefab,
        float trailSpawnTime,
        float trailFadingTime,
        Color trailColor,
        float trailRelativeSize)
    {
        this.trailPrefab = trailPrefab;
        this.spawnTime = trailSpawnTime;
        this.fadingTime = trailFadingTime;
        this.color = trailColor;
        this.relativeSize = trailRelativeSize;
    }
    
    private void SpawnTrail()
    {
        FadingTrail trail = Instantiate(trailPrefab, transform.position, Quaternion.identity, orbital.transform).GetComponent<FadingTrail>();
        
        trail.SetColor(color);
        trail.SetRelativeSize(relativeSize);
        trail.InitializeTrail(orbital, fadingTime);
    }
}
