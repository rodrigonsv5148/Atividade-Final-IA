using System.Collections;
using UnityEngine;

namespace Holder
{
    /// <summary>
    /// Responsável por segurar a referência do singleton LevelLoaderHolder. Use em botões, events, triggers e etc.
    /// </summary>
    public class LevelLoaderHolder : MonoBehaviour
    {
        IEnumerator Start()
        {
            while (LevelLoader.Instance == null) yield return null;
            //Debug.Log("Carregou");
        }

        public float Progress
        {
            get
            {
                if (LevelLoader.Instance == null)
                {
#if DEBUGLOG
                    Debug.Log("LevelLoaderHolder :: LevelLoader equals null");
#endif
                    return -1;
                }

                return LevelLoader.Instance.Progress;
            }
        }

        public void LoadLevelSync(int level)
        {

            LevelLoader.Instance.LoadLevelSync(level);
        }

        public void LoadMainMenu()
        {

            LevelLoader.Instance.LoadMainMenu();
        }

        public void LoadLevelAsync(int level) //Chamada por botões ou demais scripts
        {

            LevelLoader.Instance.LoadLevelAsync(level);
        }

        public void ResetCurrentLevel()
        {

            LevelLoader.Instance.ResetCurrentLevel();
        }

        public void LoadNextLevelAsync() //Chamada por botões ou demais scripts
        {

            LevelLoader.Instance.LoadNextLevelAsync();
        }
    }
}