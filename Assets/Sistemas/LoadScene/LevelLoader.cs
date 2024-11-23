using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ScenesIndex
{
    MENU = 0,
    FASE1 = 1,
    FASE2 = 2,
    FASE3 = 3,
    GAMEOVER = 4,
    ASYNC_LOAD = 6,
}

/// <summary>
/// LevelLoader Async. Da maneira que está aqui feito, é preciso que algum GameObject com o script exista em algum momento.
/// </summary>
[DisallowMultipleComponent]
public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;
    private int levelToLoad; //Utilizado no editor
    private AsyncOperation asyncLoad;
    private static WaitForSeconds waitBeforeLoad;

    public float Progress
    {
        get
        {
            if (asyncLoad == null)
            {
#if DEBUGLOG
                Debug.Log("AsyncLoad null");
#endif
                return 0f;
            }

            return asyncLoad.progress;
        }
    }


    public int LevelToLoad
    {
        get
        {
            return levelToLoad;
        }

        set
        {
            if (value >= 0)
            {
                levelToLoad = value;
                return;
            }
            else
            {
#if DEBUGLOG
                Debug.Log("Invalid level to load");
#endif
            }

            levelToLoad = -1;
        }
    }

    private LevelLoader()
    { }
    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);


        waitBeforeLoad = new WaitForSeconds(1f);
    }

    public void LoadLevelSync(int level)
    {
#if DEBUGLOG
        Debug.Log("LevelLoader :: LoadLevelSync :: " + System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(level)));
#endif

        SceneManager.LoadScene(level);
    }

    public void LoadLevelSync(ScenesIndex level)
    {
#if DEBUGLOG
        Debug.Log("LevelLoader :: LoadLevelSync :: " + System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(level)));
#endif

        SceneManager.LoadScene((int)level);
    }

    public void LoadMainMenu()
    {
        LoadLevelAsync((int)ScenesIndex.MENU); //Cena 0 é mainMenu
    }

    public void LoadLevelAsync(int level) //Chamada por botões ou demais scripts
    {
        StartCoroutine(LoadAsync(level));
    }
    public void LoadLevelAsync(ScenesIndex level) //Chamada por botões ou demais scripts
    {
#if DEBUGLOG
        Debug.Log("LevelLoader :: LoadLevelAsync :: " + System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(level)));
#endif
        StartCoroutine(LoadAsync((int)level));
    }

    public void LoadNextLevelAsync() //Chamada por botões ou demais scripts
    {
#if DEBUGLOG
        Debug.Log("LevelLoader :: LoadLevelAsync :: " + System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex+1)));
#endif
        StartCoroutine(LoadAsync(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void ResetCurrentLevel()
    {
#if DEBUGLOG
        Debug.Log("LevelLoader :: Reset Current Level :: " + SceneManager.GetActiveScene().name);
#endif

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator LoadAsync(int nextLevel)
    {
        //Colocar a async load no Build Settings
        //Sync load da cena de Async Load, pois é pequena
        SceneManager.LoadScene("Async Load");

        yield return waitBeforeLoad;

        asyncLoad = SceneManager.LoadSceneAsync(nextLevel);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        yield return waitBeforeLoad;

        ActiveScene(LevelLoader.Instance.asyncLoad);
        levelToLoad = -1; //Sem cena para carregar

        yield return waitBeforeLoad;
    }

    private void ActiveScene(AsyncOperation asyncLoad)
    {
        //Carrega a nova
        asyncLoad.allowSceneActivation = true;
        //Desativa a cena atual, de maneira ASync
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
     
    }

}