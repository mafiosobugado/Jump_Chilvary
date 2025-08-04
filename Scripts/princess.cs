using UnityEngine;
using UnityEngine.SceneManagement;

public class princess : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Algo tocou na Princess: " + other.name + " com tag: " + other.tag);
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player tocou na Princess! Carregando cena youwin...");
            SceneManager.LoadScene("youwin");
        }
    }
}