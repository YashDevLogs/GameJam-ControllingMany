using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    public List<Transform> Enemies { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Enemies = new List<Transform>();
            DontDestroyOnLoad(gameObject); // Keep the EnemyManager object between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        PopulateEnemiesList();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PopulateEnemiesList();
    }

    public void PopulateEnemiesList()
    {
        // Clear the existing list
        Enemies.Clear();

        // Find all child objects and add them to the list
        foreach (Transform child in transform)
        {
            Enemies.Add(child);
        }
    }
}
