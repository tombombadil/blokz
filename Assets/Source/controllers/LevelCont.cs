using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelCont : CoreObj
{
    public static int LevelIndex = 0;
    public Transform ContainerDraggables;
    public Transform ContainerKeys;
    public Transform ContainerGrids;
    public static LevelCont Instance;
    private int letterIndex;
    public void StartLevel()
    {
        if (Instance == null) Instance = this;
        if (GameCont.Instance.DebugLevelIndex > -1) LevelIndex = GameCont.Instance.DebugLevelIndex;
        createGrids();
        createLetters();
    }

    private void createLetters()
    {
        LevelItem dataLevel = DataCont.Instance.GetLevelData(LevelIndex);
        Vector3 lettersSize = Vector3.zero;
        List<DraggableBlock> listDraggables = new List<DraggableBlock>(); //for placed to grid of all draggables
        foreach (LevelLetter letterData in dataLevel.Letters)
        {
            Letter letter = createLetter(letterData.Id);
            lettersSize.x += GetBoundsOfLetter().x + GameCont.Instance.LetterSpaceX;
            listDraggables.AddRange(letter.listDraggables);
        }

        Utils.PlaceToObjectsOnGrid(listDraggables.Cast<CoreObj>().ToList(), GameCont.Instance.DraggablePlaceGrid);
        lettersSize.x -= GameCont.Instance.LetterSpaceX;
        lettersSize.y = GetBoundsOfLetter().y;
        float x = 0 - (lettersSize.x / 2);
        float y = 0 + (lettersSize.y / 2);
        ContainerKeys.transform.localPosition = new Vector3(x, y, 0);

        foreach (DraggableBlock block in listDraggables)
        {
            block.InitDefaultPos();
        }
    }

    public void SaveLevel()
    {
        LevelIndex++;
        // PlayerPrefs.SetInt("Level", LevelIndex + 1);
        // PlayerPrefs.Save();
    }

    public int LoadLevel()
    {
        int level = 0;
        if (PlayerPrefs.HasKey("Level"))
        {
            level = PlayerPrefs.GetInt("Level");
        }

        return level;
    }

    private void createGrids()
    {
        List<CoreObj> list = new List<CoreObj>();
        for (int i = 0; i < GameCont.Instance.BackgroundGrid.Count; i++)
        {
            GameObject go = Instantiate(GameCont.Instance.PrefabGridBlock, Vector3.zero, Quaternion.identity);
            go.transform.parent = ContainerGrids;
            BlockItem item = go.GetComponent<BlockItem>();
            list.Add(item);
        }

        Utils.PlaceToObjectsOnGrid(list, GameCont.Instance.BackgroundGrid);
    }

    private Letter createLetter(string id)
    {
        Letter letter = GameCont.Instance.SpawnObj(GameCont.Instance.PrefabLetter, this.transform) as Letter;
        letter.ContainerDraggables = ContainerDraggables;
        letter.ContainerKeys = letter.transform;
        letter.Init(id);
        letter.transform.parent = ContainerKeys;
        letter.transform.localPosition =
            new Vector3((letterIndex * (GetBoundsOfLetter().x + GameCont.Instance.LetterSpaceX)), 0, 0);
        letter.EventResult.AddListener(letterOnResult);

        AddChild(letter);
        letterIndex++;
        return letter;
    }



    private void letterOnResult(CoreObj obj, bool isResult)
    {
        CheckResult();
    }

    public override bool CheckResult()
    {
        bool r = base.CheckResult();

        if (r)
        {
            GameCont.Instance.GameOver();
        }


        return r;
    }


    public Vector2 GetBoundsOfLetter()
    {
        Vector2 v = Vector2.zero;
        v.x = GameCont.Instance.BlockColumn * GameCont.Instance.BlockSize.x;
        v.y = GameCont.Instance.BlockRow * GameCont.Instance.BlockSize.y;
        return v;
    }






}