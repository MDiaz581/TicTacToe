using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBehavior : MonoBehaviour
{
    public enum PanelType
    {
        Empty,
        X,
        O
    }

    [SerializeField] public PanelType panelType;
    private Renderer panelRenderer;

    public Material mat_x;
    public Material mat_o;

    public void Awake()
    {
        panelRenderer = GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PanelHit(int ID)
    {
        switch (ID)
        {
            case 0:
                Debug.Log($"Player 1 hit");
                panelType = PanelType.X;
                panelRenderer.material = mat_x;
                break;
            case 1:
                Debug.Log("Player 2 hit");
                panelType = PanelType.O;
                panelRenderer.material = mat_o;
                break;
            default:
                Debug.LogError($"ID Error! Player ID: {ID}");
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        CannonballBehavior cb = other.gameObject.GetComponent<CannonballBehavior>();
        if (cb != null)
        {
            PanelHit(cb.playerID);
            Destroy(other.gameObject);
        }

        
    }
}
