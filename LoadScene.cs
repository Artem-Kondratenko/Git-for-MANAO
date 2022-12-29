
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{


   /// <summary>
   /// Перезапуск активной сцены
   /// </summary>
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



/// <summary>
/// Загрузка сцены по ее имени
/// </summary>
/// <param name="name"></param>
    public void SceneLoad (string name)
    {

        SceneManager.LoadScene(name);
    }

  


}
