using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scene_scripts : MonoBehaviour
{
  public  string sceneName;
    void Start()
    {
        
    }
    public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.N)){LoadScene(sceneName);}
    }
}
