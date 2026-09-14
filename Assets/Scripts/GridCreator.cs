using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    [Header("Vital References")]
    public GameObject panel;
    public GameObject backboard;

    [Header("Raycast Check")]
    public LayerMask groundLayer;

    [Header("Grid Variables")]
    public int gridSize;
    public float gridGap = 4;
    public List<GameObject> panels; //This is the collection of panels, sent to the scorer.

    [Header("Events")]
    public static Action<List<GameObject>, int> OnGridMade;



    void Awake()
    {
        backboard.transform.localScale = new Vector3(((3*gridSize) - 1f) / 8f, ((3*gridSize) - 1f) / 8f, 1f); //Sets the backboard to be able to contain much bigger boards. Note: Math isn't perfect, but works.
        CheckGroundDistance();
    }

    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        //Procedurely generates the grid to allow for any size gameplay.
        for (int i = 0; i < gridSize; i++) //This creates the first column
        {
            GameObject panelTemp = Instantiate(panel, transform.position + new Vector3(0, -i * gridGap, 0), transform.rotation, transform);

            panelTemp.gameObject.name = $"Panel R{i + 1} C1"; //Label each game object
            panels.Add(panelTemp);
            for (int j = 0; j < gridSize - 1; j++) //Then generates the rows that belong to the column panel
            {
                GameObject panelTemp2 = Instantiate(panel, panelTemp.transform.position + new Vector3(j * gridGap + gridGap, 0, 0), transform.rotation, transform);
                panelTemp2.gameObject.name = $"Panel R{i + 1} C{j + 2}";
                panels.Add(panelTemp2);
            }
        }

        OnGridMade?.Invoke(panels, gridSize); //Sends generated list to the point scorer script to be sorted
    }

    //Positions the Grid so that it doesn't go under the world terrain.
    private void CheckGroundDistance()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 40, groundLayer))
        {
            transform.position = new Vector3(hit.point.x, gridGap * gridSize, hit.point.z);
        }
    }
}
