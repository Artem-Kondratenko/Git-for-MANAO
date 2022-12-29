
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{


   /// <summary>
   /// ���������� �������� �����
   /// </summary>
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



/// <summary>
/// �������� ����� �� �� �����
/// </summary>
/// <param name="name"></param>
    public void SceneLoad (string name)
    {

        SceneManager.LoadScene(name);
    }

  


}
