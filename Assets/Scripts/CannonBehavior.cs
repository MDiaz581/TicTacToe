using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBehavior : MonoBehaviour
{
    public Transform crossHair;
    
    //Just cosmetic, aims the cannon towards the crosshair.
    void Update()
    {
        if(crossHair == null) return;

        Vector3 targetPosition = crossHair.position - transform.position; //Determine the position of the cross hair based on the position of itself.

        transform.rotation = Quaternion.FromToRotation(Vector3.up, targetPosition); //Lookat wasn't used here because of the model's forward axis not aligning to cannon's barrel, making it aim improperly.
    }
}
