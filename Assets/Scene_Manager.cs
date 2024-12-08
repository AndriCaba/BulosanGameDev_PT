using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scene_Manager : MonoBehaviour
{
    public string scenename;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object you hit has a specific tag (like "Enemy" or "Obstacle")
        if (collision.gameObject.CompareTag("Player")) // Or any other logic you need for the collision
        {
            // Change the scene to "GameScene1" (change to your scene name)
            SceneManager.LoadScene(scenename);
        }
    }
}
