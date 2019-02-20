using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{

    [Header("Set Dynamically")]
    public Rigidbody rigid;
    private Neighborhood neighborhood;

    // Start is called before the first frame update
    void Awake()
    {
        neighborhood = GetComponent<Neighborhood>();
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

    //Called once per physics updated \ie (50x/second)
    private void FixedUpdate()
    {
        Vector3 velocity = rigid.velocity;
        Spawner spawner = Spawner.S;

        //COLLISION AVOIDANCE - Avoid neighbors who are too close
        Vector3 velAvoid = Vector3.zero;
        Vector3 tooClosePos = neighborhood.avgClosePos;
        //if the response is Vector3.zero, then no need to react
        if (tooClosePos != Vector3.zero)
        {
            velAvoid = pos - tooClosePos;
            velAvoid.Normalize();
            velAvoid *= spawner.velocity;
        }

        //Velcoity MATCHING - Try to match velocity with neighbors
        Vector3 velAlign = neighborhood.avgVel;
        // only do more if the velAlign is not VEctor.zero
        if (velAlign != Vector3.zero)
        {
            //we're really intereseted in direction, so nomalise the velcoity
            velAlign.Normalize();
            // and then set it to the speed we choose
            velAlign *= spawner.velocity;
        }

        //FLOCK CENTERING - Move towards the centre of local neighbors;
        Vector3 velCentre = neighborhood.avgPos;
        if (velCentre != Vector3.zero)
        {
            velCentre -= transform.position;
            velCentre.Normalize();
            velCentre *= spawner.velocity;
        }


        //ATTRACTION - move towards the ATTractor
        //This gives us the distance as to how close the Boid is to the attractor, neeeded to check whether to push or pull it
        Vector3 delta = Attractor.POS - pos; 
        //check whether we're attracted or avoiding the attractor
        bool attracted = (delta.magnitude > spawner.attractPushDist);
        Vector3 velocityAttract = delta.normalized * spawner.velocity; //This is to ensure that VelcoityAttract is of the same length as velocity var 

        //Apply all the veclocites
        float floatDeltaTime = Time.fixedDeltaTime;
        if (velAvoid != Vector3.zero)
        {
            velocity = Vector3.Lerp(velocity, velAvoid, spawner.collAvoid * floatDeltaTime);
        }
        else
        {
            if (velAlign != Vector3.zero)
            {
                velocity = Vector3.Lerp(velocity, velAlign, spawner.velMatching * floatDeltaTime);
            }
            if (velCentre != Vector3.zero)
            {
                velocity = Vector3.Lerp(velocity, velCentre, spawner.flockCentering * floatDeltaTime);
            }
            if (velocityAttract != Vector3.zero)
            {
                //Both vectors Velocity and VelocityAtrtact are of the same magnitude (length), makaing the interpolation weight the same, rememebr that interpolation is bcasilly guaging/guessing the distance btween two points
                // if the 3rd parameter is 0 then it heads towards velicty pos and if its 1 then it heads to VelocityAtrtact
                if (attracted)
                { //Lerp is used to give a weieghting in which direction the Boid should be pushed/pulled
                    velocity = Vector3.Lerp(velocity, velocityAttract, spawner.attractPull * floatDeltaTime);
                }
                else
                {
                    velocity = Vector3.Lerp(velocity, -velocityAttract, spawner.attractPush * floatDeltaTime);
                }
            }
        }

        //Since we've been using vectors of the same magnitude, we need to normalised the veclotity and multiply to get the final velcoity of the Boid
        //Set velocity to the velocity set on the spanwer singlteon
        velocity = velocity.normalized * spawner.velocity;
        //finally assing this to the rigidbody
        rigid.velocity = velocity;
        //look in the drrection of the new velocity
        LookAhead();
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
