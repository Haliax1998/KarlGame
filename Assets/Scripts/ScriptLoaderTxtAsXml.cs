using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public abstract class StoryBlock { }

public class StoryText : StoryBlock
{
    public string Content;
    public string Speaker;
    public string AudioName;

    public StoryText(string content, string speaker, string audioName = null)
    {
        Content = content;
        Speaker = speaker;
        AudioName = audioName;
    }
}

public class StoryImage : StoryBlock
{
    public string ImageName;
    public string AudioName;

    public StoryImage(string imageName, string audioName = null)
    {
        ImageName = imageName;
        AudioName = audioName;
    }
}

public static class ScriptLoaderTxtAsXml
{
    public static List<StoryBlock> LoadStoryBlocks()
    {
        var asset = Resources.Load<TextAsset>("Story/story");
        if (asset == null)
        {
            Debug.LogError("No se encontró Resources/Story/story.txt");
            return new List<StoryBlock>();
        }

        var doc = XDocument.Parse(asset.text);
        var blocks = new List<StoryBlock>();

        foreach (var node in doc.Root.Elements())
        {
            if (node.Name == "text")
            {
                string content = node.Value.Trim();
                string speaker = (string)node.Attribute("speaker") ?? "";
                string audio = (string)node.Attribute("audio");
                blocks.Add(new StoryText(content, speaker, audio));
            }
            else if (node.Name == "image")
            {
                string name = (string)node.Attribute("name") ?? "";
                string audio = (string)node.Attribute("audio");
                blocks.Add(new StoryImage(name, audio));
            }
        }

        return blocks;
    }
}