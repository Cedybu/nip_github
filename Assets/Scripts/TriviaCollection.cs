/*
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;


//Source of code: https://unity3d.college/2017/07/14/using-unity3d-xml-files-game-data-quiz-game-example/
public class TriviaCollection : MonoBehaviour
{
    private Trivia[] allTrivia;

    private void Awake()
    {
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "Trivia.xml")))
        {
            WriteSampleQuestionsToXml();
        }

        LoadAllQuestions();
    }

    public void Start()
    {
        print(GetUnaskedQuestion().triviaSentences[0]); 
        //print(GetUnaskedQuestion().wasShown);
        //string triviapiece = GetUnaskedQuestion().triviaSentences[0];
        //print(triviapiece);
    }

    private void LoadAllQuestions()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Trivia[]));
        using (StreamReader streamReader = new StreamReader(Path.Combine(Application.persistentDataPath, "Trivia.xml")))
        {
            allTrivia = (Trivia[])serializer.Deserialize(streamReader);
        }
    }

    public Trivia GetUnaskedQuestion()
    {
        ResetQuestionsIfAllHaveBeenAsked();

        var trivia = allTrivia
            .Where(t => t.wasShown == false)
            .OrderBy(t => UnityEngine.Random.Range(0, int.MaxValue))
            .FirstOrDefault();

        trivia.wasShown = true;
        return trivia;
    }

    private void ResetQuestionsIfAllHaveBeenAsked()
    {
        if (allTrivia.Any(t => t.wasShown == false) == false)
        {
            ResetQuestions();
        }
    }

    private void ResetQuestions()
    {
        foreach (var trivia in allTrivia)
            trivia.wasShown = false;
    }

    /// <summary>
    /// This method is used to generate a starting sample xml file if none exists
    /// </summary>
    private void WriteSampleQuestionsToXml()
    {
        allTrivia = new Trivia[] {
            new Trivia { modelName = "EiffelTower",
                triviaSentences = new string[] { "Der Eiffelturm ist gross", "ist schön", "ist französisch", "ist in Paris" } },
            new Trivia { modelName = "plant" ,
                triviaSentences = new string[] { "1PM", "2PM", "Noon", "11AM" } },
        };

        XmlSerializer serializer = new XmlSerializer(typeof(Trivia[]));
        using (StreamWriter streamWriter = new StreamWriter(Path.Combine(Application.persistentDataPath, "Trivia.xml")))
        {
            serializer.Serialize(streamWriter, allTrivia);
        }
    }
}
public class Trivia
{
    public string modelName { get; set; }
    public string[] triviaSentences { get; set; }
    public bool wasShown { get; set; }
}
*/

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Serialization;
using UnityEngine;

public class TriviaCollection : MonoBehaviour
{
    private Trivia[] allTrivia;
    private string fileName = "triviaFile2.xml";

    private void Awake()
    {
        if (!File.Exists(Path.Combine(Application.persistentDataPath, fileName)))
        {
            WriteTriviaToXml();
        }

        LoadAllQuestions();
    }

    private void Start()
    {
        //print(Path.Combine(Application.dataPath, "trivia.xml"));
        //print(GetTriviaNotShownYet(allTrivia[1]));
        //print(GetTriviaNotShownYet(allTrivia[1]));
        //print(GetTriviaNotShownYet(allTrivia[1]));
        //print(GetTriviaNotShownYet(allTrivia[1]));
        //print(GetTriviaNotShownYet(allTrivia[1]));
    }

    private void LoadAllQuestions()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Trivia[]));
        using (StreamReader streamReader = new StreamReader(Path.Combine(Application.persistentDataPath, fileName)))
        {
            allTrivia = (Trivia[])serializer.Deserialize(streamReader);
        }
    }

    public Trivia getTriviaBasedOnObjectName(string objectName)
    {
        return allTrivia.First(x => x.modelName == objectName);
    }

    public string GetTriviaNotShownYet(Trivia trivia)
    {
        ResetTriviaIfAllHaveBeenShown(trivia);

        System.Random rnd = new System.Random();
        int randomnumber = rnd.Next(0, 4);

        while(trivia.shown[randomnumber] == true)
        {
            randomnumber = rnd.Next(0, 4);
        }

        trivia.shown[randomnumber] = true;
        return trivia.triviaList[randomnumber];
    }

    private void ResetTriviaIfAllHaveBeenShown(Trivia trivia)
    {
        //foreach(bool triviaShown in trivia.shown)
        if (trivia.shown.Any(t => t == false) == false)
        {
            ResetQuestions(trivia);
        }
    }

    private void ResetQuestions(Trivia trivia)
    {
        for (int i = 0; i < 4; i++)
        {
            trivia.shown[i] = false;
        }
    }

    /// <summary>
    /// This method is used to generate a starting sample xml file if none exists
    /// </summary>
    private void WriteTriviaToXml()
    {
        allTrivia = new Trivia[] {
            new Trivia { modelName = "Eiffelturm",
                triviaList = new string[] { "1PM", "2PM", "Noon", "11AM" },
                shown = new bool[] {false,false,false,false } },
            new Trivia { modelName = "frenchberet",
                triviaList = new string[] { "Donkey", "Spider", "Dog", "Pig" },
                shown = new bool[] {false,false,false,false } },
            new Trivia { modelName = "louvre",
                triviaList = new string[] { "1PM", "2PM", "Noon", "11AM" },
                shown = new bool[] {false,false,false,false } },
            new Trivia { modelName = "croissant",
                triviaList = new string[] { "1PM", "2PM", "Noon", "11AM" },
                shown = new bool[] {false,false,false,false } },
            new Trivia { modelName = "Notre Dame",
                triviaList = new string[] { "1PM", "2PM", "Noon", "11AM" },
                shown = new bool[] {false,false,false,false } },
            new Trivia { modelName = "Arc de Triomphe",
                triviaList = new string[] { "1PM", "2PM", "Noon", "11AM" },
                shown = new bool[] {false,false,false,false } }
        };

        /*
        theme1NameList.Add("Eiffelturm");
        theme1NameList.Add("frenchberet");
        theme1NameList.Add("louvre");
        theme1NameList.Add("croissant");
        theme1NameList.Add("Notre Dame");
        theme1NameList.Add("arcdetriomphe");
        */

        XmlSerializer serializer = new XmlSerializer(typeof(Trivia[]));
        using (StreamWriter streamWriter = new StreamWriter(Path.Combine(Application.persistentDataPath, fileName)))
        {
            serializer.Serialize(streamWriter, allTrivia);
        }
    }
}

public class Trivia
{
    public string modelName { get; set; }
    public string[] triviaList { get; set; }
    public bool[] shown { get; set; }

}
