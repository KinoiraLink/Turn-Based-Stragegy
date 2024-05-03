using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }
    public event EventHandler OnTurnChanged; 

    private int turnNumber;

    private bool isPlayerTurn = true;

    private void Awake()
    {
        isPlayerTurn = true;
        if (Instance != null) 
        {
            Debug.LogError("There's more than one TurnSystem! + " + transform + "-" + Instance);
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;
        OnTurnChanged?.Invoke(this,EventArgs.Empty);
    }
    
    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
