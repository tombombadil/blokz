using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;


public class BlockDataItem
{
    [XmlAttribute("id")] public string Id;

    [XmlAttribute("pattern")] public string Pattern;
}

public class LetterBlock
{
    [XmlAttribute("id")] public string Id;
    [XmlAttribute("pos")] public int PositionIndex;
}

public class LetterItem
{
    [XmlAttribute("id")] public string Id;

    [XmlArray("BlockRefs")] [XmlArrayItem("Block")]
    public List<LetterBlock> blocks = new List<LetterBlock>();
}


public class LevelLetter
{
    [XmlAttribute("id")] public string Id;
}

public class LevelItem
{
    [XmlAttribute("index")] public int Index;

    [XmlArray("Letters")] [XmlArrayItem("Letter")]
    public List<LevelLetter> Letters = new List<LevelLetter>();
}


[XmlRoot("data")]
public class DataCont
{
    public static DataCont Instance;

    [XmlArray("Blocks")] [XmlArrayItem("Item")]
    public List<BlockDataItem> blocks = new List<BlockDataItem>();

    [XmlArray("Letters")] [XmlArrayItem("Letter")]
    public List<LetterItem> letters = new List<LetterItem>();

    [XmlArray("Levels")] [XmlArrayItem("Level")]
    public List<LevelItem> Levels = new List<LevelItem>();


    public static DataCont Load(string _path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(_path);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataCont));
        StringReader sr = new StringReader(_xml.text);
        DataCont datacont = xmlSerializer.Deserialize(sr) as DataCont;
        sr.Close();
        if (Instance == null) Instance = datacont;
        return datacont;
    }

    public BlockDataItem GetBlock(string id)
    {
        return blocks.Find(e => e.Id == id);
    }

    public LetterItem GetLetter(string id)
    {
        return letters.Find(e => e.Id == id);
    }

    public LevelItem GetLevelData(int level)
    {
        if (level >= Levels.Count)
            level = Levels.Count - 1;
        LevelItem dataLevelItem = Levels[level];
        return dataLevelItem;
    }
}