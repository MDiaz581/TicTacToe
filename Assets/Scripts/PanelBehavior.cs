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

    [Header("Panel Materials")]
    public Material mat_x;
    public Material mat_o;

    public void Awake()
    {
        panelRenderer = GetComponent<Renderer>();
    }

    private void PanelHit(int ID) //Change to x or o depending on who's player ID hit it.
    {
        switch (ID)
        {
            case 0:
                panelType = PanelType.X;
                panelRenderer.material = mat_x;
                break;
            case 1:
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
