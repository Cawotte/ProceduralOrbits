using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrbitManager : MonoBehaviour
{

    [SerializeField] private GameObject orbitPrefab;
    [SerializeField] private GameObject trailPrefab;
    
    [SerializeField] private int maxOrbitals = 8;
    
    [SerializeField]
    private OrbitalRandomRanges[] parameters = new OrbitalRandomRanges[3];
    
    [SerializeField]
    private List<Color> orbitColors = new List<Color>();
    [SerializeField]
    private List<Color> trailColors = new List<Color>();

    private List<Orbital>[] orbitLayers = new List<Orbital>[3];

    private int totalOrbitsGenerated = 0;
    
    void Start()
    {
        InitOrbitals();
    }
    
    public void ResetAllOrbits()
    {
        DestroyAllOrbits();
        InitOrbitals();
    }

    private void InitOrbitals()
    {
        totalOrbitsGenerated = 0;
        //Depth 3
        for (int i = 0; i < 3; i++)
        {
            GenerateOrbitalLayer(i);
        }
    }

    private void GenerateOrbitalLayer(int depth)
    {
        List<Orbital> orbits = new List<Orbital>();
        orbitLayers[depth] = orbits;
        OrbitalRandomRanges param = parameters[depth];

        int nbOrbits = UnityEngine.Random.Range(param.MinOrbitals, param.MaxOrbitals + 1);
        totalOrbitsGenerated += nbOrbits;

        while (totalOrbitsGenerated > maxOrbitals)
        {
            nbOrbits--;
            totalOrbitsGenerated--;
        }

        for (int i = 0; i < nbOrbits; i++)
        {
            Transform centerOfGravity;
            if (depth == 0)
            {
                centerOfGravity = transform;
            }
            else
            {
                List<Orbital> previousLayer = orbitLayers[depth - 1];
                centerOfGravity = previousLayer[Random.Range(0, previousLayer.Count)].transform;
            }

            //Generate an Orbit of that Layer
            Orbital orbit = Instantiate(orbitPrefab, centerOfGravity).GetComponent<Orbital>();


            orbit.InitializeOrbital(
                centerOfGravity,
                Random.Range(param.MinSpeed, param.MaxSpeed),
                Random.Range(param.MinRadius, param.MaxRadius),
                (Random.Range(0f, 1f) < param.ClockwiseProbability),
                Random.Range(param.MinSize, param.MaxSize));
            orbit.SetColor(orbitColors[Random.Range(0, orbitColors.Count)]);

            bool hasTrail = (Random.Range(0f, 1f) < param.TrailsProbability);
            if (hasTrail)
            {
                TrailSpawner spawner = orbit.gameObject.AddComponent<TrailSpawner>();
                spawner.InitializeTrail(
                    trailPrefab,
                    Random.Range(param.TParam.MinSpawnTime, param.TParam.MaxSpawnTime),
                    Random.Range(param.TParam.MinFadingTime, param.TParam.MaxFadingTime),
                    trailColors[Random.Range(0, trailColors.Count)],
                    Random.Range(param.TParam.MinSize, param.TParam.MaxSize)
                );
            }
            
            orbits.Add(orbit);
                
        }
    }

    
    private void DestroyAllOrbits()
    {
        for (int i = 0; i < orbitLayers.Length; i++)
        {
            List<Orbital> orbits = orbitLayers[i];
            foreach (Orbital orbit in orbits)
            {
                Destroy(orbit.gameObject);
            }
            orbitLayers[i] = new List<Orbital>();
        }
    }

    [Serializable]
    private struct OrbitalRandomRanges
    {
        public int MinOrbitals;
        public int MaxOrbitals;
        
        public float MinSpeed;
        public float MaxSpeed;

        public float MinRadius;
        public float MaxRadius;

        public float MinSize;
        public float MaxSize;
        
        public float ClockwiseProbability;
        public float TrailsProbability;

        public TrailRandomRanges TParam;

        /*
        public OrbitalRandomRanges(int depth)
        {
            switch (depth)
            {
                case 0:
                    this.MinOrbitals = 2;
                    this.MaxOrbitals = 4;
                    this.MinSpeed = 0.2f;
                    this.MaxSpeed = 6f;
                    this.MinRadius = 0.3f;
                    this.MaxRadius = 2f;
                    this.MinSize = 0.5f;
                    this.MaxSize = 1.8f;
                    this.ClockwiseProbability = 0.66f;
                    this.TrailsProbability = 1f;
                    break;
                case 1:
                    this.MinOrbitals = 1;
                    this.MaxOrbitals = 3;
                    this.MinSpeed = 0.2f;
                    this.MaxSpeed = 6f;
                    this.MinRadius = 0.3f;
                    this.MaxRadius = 2f;
                    this.MinSize = 0.5f;
                    this.MaxSize = 1.8f;
                    this.ClockwiseProbability = 0.66f;
                    this.TrailsProbability = 0.5f;
                    break;
                case 2:
                default:
                    this.MinOrbitals = 0;
                    this.MaxOrbitals = 2;
                    this.MinSpeed = 0.2f;
                    this.MaxSpeed = 6f;
                    this.MinRadius = 0.3f;
                    this.MaxRadius = 2f;
                    this.MinSize = 0.5f;
                    this.MaxSize = 1.8f;
                    this.ClockwiseProbability = 0.66f;
                    this.TrailsProbability = 0.25f;
                    break;

            }
        } */
        
    }

    [Serializable]
    private struct TrailRandomRanges
    {
        
        public float MinSpawnTime;
        public float MaxSpawnTime;

        public float MinFadingTime;
        public float MaxFadingTime;

        public float MinSize;
        public float MaxSize;
    }
}
