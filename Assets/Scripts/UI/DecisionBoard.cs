using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DecisionBoard : MonoBehaviour
{
    [SerializeField] private GameObject decisionButton1;
    [SerializeField] private GameObject decisionButton2;
    [SerializeField] private GameObject decisionButton3;
    [SerializeField] private GameObject decisionButton4;
    [SerializeField] private GameObject backGround;

    private bool decisionWasMade = false;
    private int decision = 0;

    private void Awake()
    {

    }

    public void InitDecision(int numberDecisions, string firstChoice = null, string secondChoice = null, string thirdChoice = null, string fourthChoice = null)
    {
        Debug.Log("Number of Decisions: " + numberDecisions +  "\n"+firstChoice + "\n"+secondChoice + "\n"+thirdChoice + "\n"+fourthChoice);
        decisionWasMade = false;
        if (numberDecisions > 0)
        {
            decisionButton1.SetActive(true);
            if (!string.IsNullOrEmpty(firstChoice))
                decisionButton1.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = firstChoice;
        }
        if (numberDecisions > 1)
        {
            decisionButton2.SetActive(true);
            if (!string.IsNullOrEmpty(secondChoice)) 
                decisionButton2.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = secondChoice;
        }
        if (numberDecisions > 2) {
            decisionButton3.SetActive(true);
            if (!string.IsNullOrEmpty(thirdChoice))
                decisionButton3.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = thirdChoice;
        }
        if (numberDecisions > 3)
        {
            decisionButton4.SetActive(true);
            if (!string.IsNullOrEmpty(fourthChoice))
                decisionButton4.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = fourthChoice;
        }
        backGround.SetActive(true);

        Debug.Log("Successfully activated the DecisionBorad.");
    }

    public bool AwaitDecision()
    {
        return decisionWasMade;
    }

    public int GetDecision()
    {
        return (decision);
    }

    public void SetDecision(int index)
    {
        decision = index;
        decisionWasMade = true;
        Debug.Log("Setting Decision to " + index);
    }

    public void UnInitDecision()
    {
        decisionButton2.SetActive (false);
        decisionButton1.SetActive (false);
        decisionButton3.SetActive (false);
        decisionButton4.SetActive (false);
        backGround.SetActive (false);
        decisionWasMade = false;
    }
}
