using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[System.Serializable]
public class UserData
{
    [XmlElement(DataType = "int", ElementName = "ZustimmungABG")]
    public int agreeConsentForm { get; set; }

    public UserData() { }

    [XmlElement(DataType = "string", ElementName = "Startzeit")]
    public string startTime { get; set; }

    [XmlElement(DataType = "string", ElementName = "Endzeit")]
    public string endTime { get; set; }

    [XmlElement(DataType = "int", ElementName = "SekundenSeitSpielOeffnung")]
    public int playedTimeSinceStart { get; set; }

    [XmlElement(DataType = "int", ElementName = "SekundenFuerRunde")]
    public int secondsForThisRound { get; set; }

    [XmlIgnoreAttribute]
    [XmlElement(DataType = "int", ElementName = "MatheUebungUebersprungen")]
    public int skipMathExercise { get; set; }

    [XmlElement(DataType = "int", ElementName = "MatheUebungFalsch")]
    public int mathExerciseWrong { get; set; }

    [XmlElement(DataType = "int", ElementName = "ObjektSucheUebersprungen")]
    public int skipSearchObject { get; set; }

    [XmlElement(DataType = "int", ElementName = "RundenSeitAppOeffnung")]
    public int roundsInARow { get; set; }

    [XmlElement(DataType = "int", ElementName = "UnvollstaendigeRunden")]
    public int unfinishedRounds { get; set; }

    [XmlElement(DataType = "int", ElementName = "RundeAbgeschlossen")]
    public int roindIsComplete { get; set; }

    [XmlElement(DataType = "int", ElementName = "Schwierigkeitsgrad")]
    public int difficultyLevel { get; set; }

    [XmlIgnoreAttribute]
    [XmlElement(DataType = "string", ElementName = "Land")]
    public string selectedCountry { get; set; }
}

//[XmlRoot("Benutzerinformationen")]
public class UserDataContainer
{
    public List<UserData> userDataContainers = new List<UserData>();
}

//[XmlIgnoreAttribute]
//[XmlArrayItem("PersonalUserData")]
//[XmlArray("UserData")]
