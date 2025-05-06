using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public static class QuizLoaderTxtAsXml
{
    public static List<QuestionData> LoadAllQuestions()
    {
        var all = new List<QuestionData>();

        // Lee todos los .txt en Resources/Questions
        foreach (var file in Resources.LoadAll<TextAsset>("Questions"))
        {
            var doc = XDocument.Parse(file.text);
            var topic = (string)doc.Root.Attribute("topic") ?? "General";

            foreach (var qElem in doc.Root.Elements("question"))
            {
                var qd = new QuestionData
                {
                    topic = topic,
                    question = qElem.Element("q")?.Value.Trim() ?? "",
                    options = new string[4],
                    correctAnswerIndex = 0,
                    epicHint = qElem.Element("epicHint")?.Value.Trim() ?? "",
                    legendaryHint = qElem.Element("legendaryHint")?.Value.Trim() ?? ""
                };

                // Rellena opciones a1–a4
                for (int i = 1; i <= 4; i++)
                    qd.options[i - 1] = qElem.Element($"a{i}")?.Value.Trim() ?? "";

                all.Add(qd);
            }
        }

        return all;
    }
}
