using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(InputCont))]
public class GameCont : CoreCont<GameCont>
{
    /* editable game parasms */
    public string DataFile = "data";
    public Vector2 BlockSize = new Vector2(1, 1);
    public List<Color> BlockColors;
    public int BlockColumn = 4;
    public int BlockRow = 5;
    public GameObject PrefabLetter;
    public GameObject PrefabKeyBlock;
    public GameObject PrefabDraggableBlock;
    public GameObject PrefabGridBlock;
    public Grid BackgroundGrid;
    public Grid DraggablePlaceGrid;
    public int DebugLevelIndex = -1;
    public float LetterSpaceX = 10;
    public float BlockItemSpace = 1;
    
    /* references */
    private InputCont inputCont;
    public LevelCont levelCont;
    public UICont uiCont;
    public ParticleSystem fxGameOver;

  

    void Awake()
    {
        inputCont = GetComponent<InputCont>();
        DataCont.Load(DataFile);
    }

    void Start()
    {
        uiCont.ShowPage(0);
        StartCoroutine(startLevel());
    }

    private IEnumerator startLevel()
    {
        yield return new WaitForSeconds(0.1f);
        levelCont.StartLevel();
        UICont.Instance.Init();
    }

    public void StartGame()
    {
        uiCont.ShowPage(1);
    }

    public void GameOver()
    {
        levelCont.SaveLevel();
        if (fxGameOver) fxGameOver.Play();
        StartCoroutine(showGameOverUI(1));
    }

    private IEnumerator showGameOverUI(float delay)
    {
        yield return new WaitForSeconds(delay);
        uiCont.ShowGameOver(100);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public CoreObj SpawnObj(GameObject prefab, Transform parent)
    {
        GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        go.transform.parent = parent;
        CoreObj obj = go.GetComponent<CoreObj>();
        return obj;
    }
}