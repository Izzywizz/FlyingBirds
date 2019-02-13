using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    static public Vector3 POS = Vector3.zero;   // class variable that allows the BOID's access to it location

    [Header("Set in Inspector")]
    public float radius = 10;
    public float xPhase = 0.5f;
    public float yPhase = 0.4f;
    public float zPhase = 0.1f;

    //FixedUpdated is called once per physics updated (ie 50x/ second)
    private void FixedUpdate()
    {
        Vector3 tPos = Vector3.zero;
        Vector3 scale = transform.localScale;
        tPos.x = Mathf.Sin(xPhase * Time.time) * radius * scale.x;
        tPos.y = Mathf.Sin(yPhase * Time.time) * radius * scale.y;
        tPos.z = Mathf.Sin(zPhase * Time.time) * radius * scale.z;
        transform.position = tPos; //remmeber you can't update the postion directly
        POS = tPos;
    }

}
