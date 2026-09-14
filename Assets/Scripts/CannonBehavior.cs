using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBehavior : MonoBehaviour
{
    public Transform crossHair;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(crossHair == null) return;

        Vector3 targetPosition = crossHair.position - transform.position;

        transform.rotation = Quaternion.FromToRotation(Vector3.up, targetPosition);
    }
}
