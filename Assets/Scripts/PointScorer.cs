using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PointScorer : MonoBehaviour
{
    private GameManager gameManager;

    public int player1Score;
    public int player2Score;

    public TMP_Text ui_player1Text;
    public TMP_Text ui_player2Text;

    public List<GameObject> individualPanels = new List<GameObject>();

    public List<GameObject> coloumn1 = new List<GameObject>();
    public List<GameObject> coloumn2 = new List<GameObject>();
    public List<GameObject> coloumn3 = new List<GameObject>();
    public List<GameObject> row1 = new List<GameObject>();
    public List<GameObject> row2 = new List<GameObject>();
    public List<GameObject> row3 = new List<GameObject>();
    public List<GameObject> diagnol1 = new List<GameObject>();
    public List<GameObject> diagnol2 = new List<GameObject>();

    public List<List<GameObject>> gridCollection = new List<List<GameObject>>();


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

    void Start()
    {
        gridCollection.Add(coloumn1);
        gridCollection.Add(coloumn2);
        gridCollection.Add(coloumn3);
        gridCollection.Add(row1);
        gridCollection.Add(row2);
        gridCollection.Add(row3);
        gridCollection.Add(diagnol1);
        gridCollection.Add(diagnol2);
    }

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

            diagnol1.Add(ts[0 + (i * size) + i]);
            diagnol2.Add(ts[2 + (i * size) - i]);
        }
    }

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

        if (Mathf.Abs(checkForComplete) == 3)
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

    public void CheckCollection()
    {
        StartCoroutine(StartScoring());
    }


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

        string result = (player1Score >= player2Score) ? "Player 1 Wins!" : "Player 2 Wins!";
        gameManager.ui_countDown.text = result;

        yield return new WaitForSeconds(10f);

        gameManager.RestartGame();

        yield return null;
    }
}
