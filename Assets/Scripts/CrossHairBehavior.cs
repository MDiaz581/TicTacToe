using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class CrossHairBehavior : MonoBehaviour
{
    [Header("References")]
    private CharacterController cc;
    private Player player;
    [SerializeField] private int playerID;
    public GameObject bullet;
    public Transform firePoint;

    [Header("Controls")]
    private Vector3 moveVector;
    private bool b_fire;
    
    [Header("Player Variables")]
    public float moveSpeed;
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

    //Basic movement using Unity's Character Controller
    private void Movement(Vector3 input)
    {
        input.Normalize();

        Vector3 motion = input * moveSpeed * Time.deltaTime;

        cc.Move(motion);
    }

    private void Fire()
    {
        if(canFire == false) return;

        GameObject bulletInstance = Instantiate(bullet, firePoint.position, firePoint.rotation);

        CannonballBehavior ballBehavior = bulletInstance.GetComponent<CannonballBehavior>();
        if(ballBehavior != null)
        {
           ballBehavior.OnFire(bulletSpeed); //Apply speed to the bullet.
           ballBehavior.playerID = playerID; //Give ID to the bullet, important for the panel when it checks who hit it.
        }
        Invoke(nameof(ResetFire), fireCooldown); // Call function after cooldown

        canFire = false; // Prevents refiring before cooldown ends
    }

    private void ResetFire()
    {
        canFire = true;
    }

}
