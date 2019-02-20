using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //This is a Singleton of the BoidSpawner. Thre is only one instance of BoidSpawner
    //, so we can store it in astatic variable named S.
    static public Spawner S;
    static public List<Boid> boids; //holds a reference to all boids instantiated by the spanwer

    //These fields allow you to adjust the spawning behaviour of the Boids
    [Header("Set in Inspector: Spawning")]
    public GameObject boidPrefab;
    public Transform boidAnchor;
    public int numBoids = 100;
    public float spawnRadius = 100f;
    public float spawnDelay = 0.1f;

    //These fields allow you adjust the flocking behaviour of the Boids
    [Header("Set In Inspector: Boids")]
    public float velocity = 30f;
    public float neighbourDist = 30f;
    public float collDist = 4f;
    public float velMatching = 0.25f;
    public float flockCentering = 0.2f;
    public float collAvoid = 2f;
    public float attractPull = 2f;
    public float attractPush = 2f;
    public float attractPushDist = 5f;

    private void Awake()
    {
        //Set the Singleton S to be THIS instance of the Spanwer class, as this class is attached to the camera
        S = this;
        // Start instantiation of the Boids
        boids = new List<Boid>();
        InstanatiateBoid();
    }

    public void InstanatiateBoid()
    {
        GameObject go = Instantiate(boidPrefab);
        Boid boid = go.GetComponent<Boid>();
        boid.transform.SetParent(boidAnchor); //ensures all the boids have a single parent for ease of reference within the hierarchy
        boids.Add(boid);
        if (boids.Count < numBoids)
        {
            Invoke("InstanatiateBoid", spawnDelay); //continues to call the method if the amount hasn't been reached with a spawn time delay
        }
    }

}
