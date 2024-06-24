using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Scriptable Objects/GameManager")]

public class GameManagerSO : ScriptableObject
{
    private Player player;
    private InventorySystem inventory;

    public InventorySystem Inventory
    {
        get => inventory;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += NewSceneLoaded;
    }

    private void NewSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = FindObjectOfType<Player>();
        inventory = FindObjectOfType<InventorySystem>();
    }

    public void SetPlayerState(bool state)
    {
        player.Interacting = !state;
    }
}