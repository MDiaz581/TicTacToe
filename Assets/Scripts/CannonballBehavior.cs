using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballBehavior : MonoBehaviour
{
    private Rigidbody rb;
    public int playerID;

    private void OnEnable()
    {
        GameManager.TimerEnded += DestroySelf;
    }
    private void OnDisable()
    {
        GameManager.TimerEnded -= DestroySelf;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        //Destroy after delay;
        Invoke("DestroySelf", 2f);
    }

    public void OnFire(float speed)
    {
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    public void DestroySelf()
    {
        Destroy(transform.gameObject);
    }
}
