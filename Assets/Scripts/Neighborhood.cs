using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighborhood : MonoBehaviour
{
    [Header("Set Dynamically")]
    public List<Boid> neighbors;
    private SphereCollider coll;

    private void Start()
    {
        //Remmeber this is a Boid GameObject, settings the neighbor hood difference to half allos them to just touch eachother
        neighbors = new List<Boid>();
        coll = GetComponent<SphereCollider>();
        coll.radius = Spawner.S.neighbourDist / 2;
    }

    private void FixedUpdate()
    {
        //if the neighborhood distance changes from the sphere collider changes then update it (PhysX calc)
        if (coll.radius != Spawner.S.neighbourDist / 2)
        {
            coll.radius = Spawner.S.neighbourDist / 2;
        }
    }

    //Recall that a trigger is a collider that allows other things to pass throught it
    //if the things we pass through is a Boid and not currently on our list, we add it
    private void OnTriggerEnter(Collider other)
    {
        Boid b = other.GetComponent<Boid>();
        if (b != null)
        {
            if (neighbors.IndexOf(b) != -1)
            {
                neighbors.Add(b);
            }
        }
    }

    //If the boid has gone, we remove it from our list
    private void OnTriggerExit(Collider other)
    {
        Boid b = GetComponent<Boid>();
        if (b != null)
        {
            if (neighbors.IndexOf(b) != -1)
            {
                neighbors.Remove(b);
            }
        }
    }

    //calcs all the Boids in the list average postion
    public Vector3 avgPos
    {
        get
        {
            Vector3 avg = Vector3.zero;
            if (neighbors.Count == 0) return avg;

            for (int i = 0; i < neighbors.Count; i++)
            {
                avg += neighbors[i].pos;
            }
            avg /= neighbors.Count;

            return avg;
        }
    }

    //calcs the all the boids velocity
    public Vector3 avgVel
    {
        get
        {
            Vector3 avg = Vector3.zero;
            if (neighbors.Count == 0) return avg;

            for (int i = 0; i < neighbors.Count; i++)
            {
                avg += neighbors[i].rigid.velocity;
            }

            avg /= neighbors.Count;
            return avg; 
        }
    }

    ///calcs and looks for neighbors that are within the collisionDist (from the spanwer singteon) and averages their postion.
    public Vector3 avgClosePos
    {
        get
        {
            Vector3 avg = Vector3.zero;
            Vector3 delta;
            int nearCount = 0;
            for (int i = 0; i < neighbors.Count; i++)
            {
                delta = neighbors[i].pos - transform.position;
                if (delta.magnitude <= Spawner.S.collDist)
                {
                    avg = neighbors[i].pos;
                    nearCount += 1;
                }
            }
            //If there were no neighbors too close. return vector 3 zero
            if (nearCount == 0) return avg;

            //Otherwise, average their lcoations
            avg /= nearCount;
            return avg;
        }
    }
}
