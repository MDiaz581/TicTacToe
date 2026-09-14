using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{

    public GameObject panel;

    public int gridSize;

    public float gridGap = 4;

    public List<GameObject> panels;

    public static Action<List<GameObject>, int> OnGridMade;

    public LayerMask groundLayer;

    void Awake()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        CheckGroundDistance();

        for (int i = 0; i < gridSize; i++)
        {
            GameObject panelTemp = Instantiate(panel, transform.position + new Vector3(0, -i * gridGap, 0), transform.rotation, transform);

            panelTemp.gameObject.name = $"Panel R{i+1} C1";
            panels.Add(panelTemp);
            for (int j = 0; j < gridSize - 1; j++)
            {
                GameObject panelTemp2 = Instantiate(panel, panelTemp.transform.position + new Vector3(j * gridGap + gridGap, 0, 0), transform.rotation, transform);
                panelTemp2.gameObject.name = $"Panel R{i+1} C{j + 2}";
                panels.Add(panelTemp2);
            }
        }

        OnGridMade?.Invoke(panels, gridSize);
    }


    private void CheckGroundDistance()
    {
        
        RaycastHit hit;

        if(Physics.Raycast(transform.position, -Vector3.up, out hit, 40, groundLayer))
        {
           
            transform.position = new Vector3(hit.point.x, gridGap*gridSize, hit.point.z);

             Debug.Log(new Vector3(hit.point.x, gridGap*gridSize, hit.point.z));
        }
    }
}
