using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planets : MonoBehaviour
{
    public float rotationPlanets;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //вращение
        Quaternion rotationX = Quaternion.AngleAxis(rotationPlanets, Vector3.up);
        transform.rotation *= rotationX;
    }
}
