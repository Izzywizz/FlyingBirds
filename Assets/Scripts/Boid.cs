using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{

    [Header("Set Dynamically")]
    public Rigidbody rigid;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        //Set a random intial position within a sphere radius and our spawn radius from origin (0,0,0)
        pos = Random.insideUnitSphere * Spawner.S.spawnRadius;

        //Set a random intial velocity, essnetially makes a vector3 that points a random direction
        Vector3 vel = Random.onUnitSphere * Spawner.S.velocity;
        rigid.velocity = vel;

        LookAhead();

        //Give the Boid a random colour, but make sure its not too dark
        Color randomColour = Color.black;
        while (randomColour.r + randomColour.g + randomColour.b < 1.0f)
        {
            randomColour = new Color(Random.value, Random.value, Random.value);
        }
        //reason why getChildren is used is because you have colour in the wings and fusalage which are children of the boid
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>(); 
        foreach (Renderer rend in renderers)
        {
            rend.material.color = randomColour;
        }
        TrailRenderer trail = GetComponent<TrailRenderer>();
        trail.material.SetColor("_TintColor", randomColour);
    }

    //methods
    private void LookAhead()
    {
        //Orients the Boid to look at the direction it's flying/ where its rigid.velocity is facing 
        transform.LookAt(pos + rigid.velocity);
    }

    //properties
    public Vector3 pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
}
