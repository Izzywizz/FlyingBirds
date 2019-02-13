using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This makes the main camera follow the atractors movement
public class LookAtAttractor : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(Attractor.POS); //now the main cmaera willl constantly look at the attractor
    }
}
