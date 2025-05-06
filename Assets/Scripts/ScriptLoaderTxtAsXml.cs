using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class StoryLineData
{
    public string Content;
    public string Speaker;
    public StoryLineData(string content, string speaker)
    {
        Content = content;
        Speaker = speaker;
    }
}

public static class ScriptLoaderTxtAsXml
{
    public static List<StoryLineData> LoadStoryLines()
    {
        // Ajusta aquí "story" si tu .txt se llama story.txt
        var asset = Resources.Load<TextAsset>("Story/story");
        if (asset == null)
        {
            Debug.LogError("No se encontró Resources/Story/story.txt");
            return new List<StoryLineData>();
        }

        var doc = XDocument.Parse(asset.text);
        return doc.Root
                  .Elements("text")
                  .Select(x => new StoryLineData(
                      x.Value.Trim(),
                      (string)x.Attribute("speaker") ?? ""
                  ))
                  .ToList();
    }
}
