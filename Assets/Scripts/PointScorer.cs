using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UIElements;

public class PointScorer : MonoBehaviour
{
    [Header("Vital References")]
    private GameManager gameManager;
    public TMP_Text ui_player1Text;
    public TMP_Text ui_player2Text;

    [Header("PlayerScores")]
    [SerializeField] private int player1Score;
    [SerializeField] private int player2Score;

    [Header("Lists")]
    [SerializeField] private List<GameObject> individualPanels = new List<GameObject>();
    [SerializeField] private List<GameObject> coloumn1 = new List<GameObject>();
    [SerializeField] private List<GameObject> coloumn2 = new List<GameObject>();
    [SerializeField] private List<GameObject> coloumn3 = new List<GameObject>();
    [SerializeField] private List<GameObject> row1 = new List<GameObject>();
    [SerializeField] private List<GameObject> row2 = new List<GameObject>();
    [SerializeField] private List<GameObject> row3 = new List<GameObject>();
    [SerializeField] private List<GameObject> diagonal1 = new List<GameObject>();
    [SerializeField] private List<GameObject> diagonal2 = new List<GameObject>();
    [SerializeField] private List<List<GameObject>> gridCollection = new List<List<GameObject>>(); // This is the collection of each list this is important to check for complete rows, columns, and diagonals.


    void OnEnable()
    {
        GridCreator.OnGridMade += SortList;
    }

    void OnDisable()
    {
        GridCreator.OnGridMade -= SortList;
    }

    void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    //Bit messy but adds each list to the collection to be able to parse later.
    void Start()
    {
        gridCollection.Add(coloumn1);
        gridCollection.Add(coloumn2);
        gridCollection.Add(coloumn3);
        gridCollection.Add(row1);
        gridCollection.Add(row2);
        gridCollection.Add(row3);
        gridCollection.Add(diagonal1);
        gridCollection.Add(diagonal2);
    }

    //This calculates the position of each object in the list to determine if they're in a column, row, or diagonal. Set up specifically for modularity of bigger boards.
    public void SortList(List<GameObject> ts, int size)
    {
        individualPanels = ts;
        for (int i = 0; i < size; i++)
        {
            coloumn1.Add(ts[0 + (i * size)]);
            coloumn2.Add(ts[1 + (i * size)]);
            coloumn3.Add(ts[2 + (i * size)]);

            row1.Add(ts[0 + i]);
            row2.Add(ts[size + i]);
            row3.Add(ts[(size * 2) + i]);

            //Note: math changes for even number grid sizes
            diagonal1.Add(ts[0 + (i * size) + i]);
            diagonal2.Add(ts[2 + (i * size) - i]);
        }
    }

    //This function scores each individual panel first as a single point.
    private void CheckIndividual(GameObject panel)
    {
        PanelBehavior pb = panel.GetComponent<PanelBehavior>();

        if (pb.panelType == PanelBehavior.PanelType.X)
        {
            player1Score++;
            ui_player1Text.text = "+1";
        }
        else if (pb.panelType == PanelBehavior.PanelType.O)
        {
            player2Score++;
            ui_player2Text.text = "+1";
        }
    }

    //This checks each list we have to see if they're completed, if they are give the player an additional 2 bonus points for completing a line.
    private void CheckList(List<GameObject> list)
    {
        int checkForComplete = 0;

        foreach (GameObject panel in list)
        {
            PanelBehavior pb = panel.GetComponent<PanelBehavior>();

            if (pb.panelType == PanelBehavior.PanelType.X)
            {
                checkForComplete++;
            }
            else if (pb.panelType == PanelBehavior.PanelType.O)
            {
                checkForComplete--;
            }
        }

        if (Mathf.Abs(checkForComplete) == list.Count)
        {
            foreach (GameObject panel in list)
            {
                panel.transform.localScale = panel.transform.localScale * 1.2f;
            }
        }

        if (checkForComplete == list.Count)
        {
            player1Score += 3;
            ui_player1Text.text = "+3";
        }
        else if (checkForComplete == -list.Count)
        {
            player2Score += 3;
            ui_player2Text.text = "+3";
        }
    }

    private void GetWinner()
    {
        string result = "";

        if (player1Score > player2Score)
        {
            result = "Player 1 Wins!";
        }
        else if (player1Score < player2Score)
        {
            result = "Player 2 Wins!";
        }
        else if (player1Score == player2Score)
        {
            result = "Draw!";
        }
        gameManager.ui_countDown.text = result;
    }

    public void CheckCollection()
    {
        StartCoroutine(StartScoring());
    }

    //Timing is all based on this coroutine to apply all the effects without it instantly being calculated. Allows for the small value popups.
    public IEnumerator StartScoring()
    {
        foreach (GameObject panel in individualPanels)
        {
            panel.transform.localScale = panel.transform.localScale * 1.2f;
            CheckIndividual(panel);
            yield return new WaitForSeconds(.5f);

            ui_player1Text.text = "";
            ui_player2Text.text = "";

            yield return new WaitForSeconds(.25f);
            panel.transform.localScale = new Vector3(3, 3, 0.2f);

        }

        for (int i = 0; i < gridCollection.Count; i++)
        {
            CheckList(gridCollection[i]);

            yield return new WaitForSeconds(.5f);

            foreach (GameObject panel in gridCollection[i])
            {
                panel.transform.localScale = new Vector3(3, 3, 0.2f);
            }

            ui_player1Text.text = "";
            ui_player2Text.text = "";

            yield return new WaitForSeconds(.25f);
        }

        GetWinner();

        yield return new WaitForSeconds(10f);

        gameManager.RestartGame();

        yield return null;
    }
}
