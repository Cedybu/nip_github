using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MathController : MonoBehaviour
{
    private int mathNr1;
    private int mathNr2;
    private int mathNr3;
    private int solution;
    private int arithmeticcharacters;
    public int attemptsMathExercise = 0;

    public TextMeshProUGUI mathExercise;
    public TextMeshProUGUI mathSuggestionPro1;
    public TextMeshProUGUI mathSuggestionPro2;
    public TextMeshProUGUI mathSuggestionPro3;
    public TextMeshProUGUI mathSuggestionPro4;

    void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }
    
    public void CreateMathExercise()
    {
        mathNr1 = Random.Range(3, 11);
        mathNr2 = Random.Range(3, 11);
        mathNr3 = Random.Range(1, 11);
        arithmeticcharacters = Random.Range(1, 3);

        List<string> mathFormula = new List<string>();
        mathFormula.Add((mathNr1 + mathNr2 + mathNr3).ToString());
        mathFormula.Add((mathNr1 + mathNr2 * mathNr3).ToString());
        mathFormula.Add((mathNr1 * mathNr2 - mathNr3).ToString());
        mathFormula.Add((mathNr1 * mathNr2 + mathNr3).ToString());

        Shuffle(mathFormula);
        
        mathSuggestionPro1.GetComponent<TMPro.TextMeshProUGUI>().text = mathFormula[0];
        mathSuggestionPro2.GetComponent<TMPro.TextMeshProUGUI>().text = mathFormula[1];
        mathSuggestionPro3.GetComponent<TMPro.TextMeshProUGUI>().text = mathFormula[2];
        mathSuggestionPro4.GetComponent<TMPro.TextMeshProUGUI>().text = mathFormula[3];

    }

    // https://forum.unity.com/threads/clever-way-to-shuffle-a-list-t-in-one-line-of-c-code.241052/
    public void Shuffle<String>(List<String> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public void CreateMathText()
    {
        string basicText = "Was ist " + mathNr1 + " x " + mathNr2 + FindArithmeticCharacters(arithmeticcharacters) + mathNr3 + "?";
        if (attemptsMathExercise == 0)
        {
            mathExercise.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = basicText;
        }
        else
        {
            mathExercise.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Lieder falsch. Neuer Versuch: " + basicText;
        }
        attemptsMathExercise++;
    }

    public int CalculateNumber()
    {
        if (arithmeticcharacters == 1)
        {
            solution = mathNr1 * mathNr2 + mathNr3;
        }
        else
        {
            solution = mathNr1 * mathNr2 - mathNr3;
        }
        return solution;
    }

    private string FindArithmeticCharacters(int arithmeticcharacters)
    {
        string symbol;
        if (arithmeticcharacters == 1)
        {
            symbol = " + ";
        }
        else
        {
            symbol = " - ";
        }
        return symbol;
    }
}
