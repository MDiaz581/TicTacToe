using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class CrossHairBehavior : MonoBehaviour
{
    private CharacterController cc;
    private Player player;
    [SerializeField] private int playerID;
    public GameObject bullet;
    private Vector3 moveVector;
    public float speed;
    public Transform firePoint;
    private bool b_fire;
    public float bulletSpeed;
    public float fireCooldown;
    private bool canFire = false;
    private bool b_masterStopControl = false;

    private void OnEnable()
    {
        GameManager.TimerEnded += EnableMasterStop;
        GameManager.GameStart += ResetFire;
    }
    private void OnDisable() 
    {
        GameManager.TimerEnded -= EnableMasterStop;
        GameManager.GameStart -= ResetFire;
    }


    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerID);
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(b_masterStopControl) return;
        GetInput();
        ProcessInput();
    }

    private void GetInput()
    {
        moveVector.x = player.GetAxis("Move Horizontal");
        moveVector.y = player.GetAxis("Move Vertical");
        b_fire = player.GetButtonDown("Fire");
    }
    private void ProcessInput()
    {
        if(moveVector.magnitude > 0) Movement(moveVector);

        if(b_fire) Fire();
    }

    private void EnableMasterStop()
    {
        b_masterStopControl = true;
    }

    private void Movement(Vector3 input)
    {
        input.Normalize();

        Vector3 motion = input * speed * Time.deltaTime;

        cc.Move(motion);
    }

    private void Fire()
    {
        if(canFire == false) return;

        GameObject bulletInstance = Instantiate(bullet, firePoint.position, firePoint.rotation);
        CannonballBehavior ballBehavior = bulletInstance.GetComponent<CannonballBehavior>();
        if(ballBehavior != null)
        {
           ballBehavior.OnFire(bulletSpeed);
           ballBehavior.playerID = playerID; 
        }
        Invoke(nameof(ResetFire), fireCooldown);
        canFire = false;
    }

    private void ResetFire()
    {
        canFire = true;
    }

}
