using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public abstract class StoryBlock { }

public class StoryText : StoryBlock
{
    public string Content;
    public string Speaker;

    public StoryText(string content, string speaker)
    {
        Content = content;
        Speaker = speaker;
    }
}

public class StoryImage : StoryBlock
{
    public string ImageName;

    public StoryImage(string imageName)
    {
        ImageName = imageName;
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
                blocks.Add(new StoryText(content, speaker));
            }
            else if (node.Name == "image")
            {
                string imageName = (string)node.Attribute("name") ?? "";
                blocks.Add(new StoryImage(imageName));
            }
        }

        return blocks;
    }
}
