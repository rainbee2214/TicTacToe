using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour 
{
	public bool resetGrid = false;
	public Vector2[] positions;
	public Vector2 xStartingPosition;
	public Vector2 oStartingPosition;
	public float resetDelay;

	private string[] states;
	private static int numberPerPlayer = 6;
	private GameObject[] xS = new GameObject[numberPerPlayer];
	private GameObject[] oS = new GameObject[numberPerPlayer];
	private string winningPiece;
	private float resetTime;

	public string GetGridState(int gridSpaceIndex)
	{
		return states[gridSpaceIndex];
	}
	
	public void SetGridState(int gridSpaceIndex, string state)
	{
		states[gridSpaceIndex] = state;
	}

	void Start () 
	{
		states = new string[9];
		for (int i = 0; i < states.Length; i++)
			states[i] = "Empty";

		for (int i = 0; i < numberPerPlayer; i++)
		{
			xS[i] = Instantiate(Resources.Load("X", typeof(GameObject)), xStartingPosition, Quaternion.identity) as GameObject;
			oS[i] = Instantiate(Resources.Load("O", typeof(GameObject)), oStartingPosition, Quaternion.identity) as GameObject;

			xS[i].name = "X";
			oS[i].name = "O";

			xS[i].transform.parent = transform;
			oS[i].transform.parent = transform;
		}

	}

	void Update () 
	{
		if (resetGrid) 
		{
			if (Time.time > resetTime + resetDelay) ResetGrid();
		}
		if ((IsWinningCondition() || IsGridFull()) && !resetGrid)
		{
			string result = "";
			if (IsWinningCondition()) result = (winningPiece + " won!");
			else if (IsGridFull()) result =  ("Draw!");
			GameObject showResult = Instantiate(Resources.Load("Result", typeof(GameObject))) as GameObject;
			showResult.guiText.text = result;
			resetTime = Time.time;
			resetGrid = true;
		}
	}


	void ResetGrid()
	{
		Application.LoadLevel("TicTacToe");
	}

	bool IsGridFull()
	{
		for (int i = 0; i < 9; i++)
		{
			if (states[i] == "Empty") return false;
		}
		return true;
	}

	bool IsWinningCondition()
	{
		//Check for horizontal lines
		//0,1,2; 3,4,5; 6,7,8
		if (CheckLine(0,1,2)) return true;
		if (CheckLine(3,4,5)) return true;
		if (CheckLine(6,7,8)) return true;

		//Check for vertical lines
		//0,3,6; 1,4,7; 2,5,8
		if (CheckLine(0,3,6)) return true;
		if (CheckLine(1,4,7)) return true;
		if (CheckLine(2,5,8)) return true;

		//Check for diagonal lines
		//0,4,8; 6,4,2
		if (CheckLine(0,4,8)) return true;
		if (CheckLine(6,4,2)) return true;

		return false;
	}

	bool CheckLine(int indexA, int indexB, int indexC)
	{
		if (states[indexA] == "Empty") return false;
		if (states[indexB] == "Empty") return false;
		if (states[indexC] == "Empty") return false;
		string state = states[indexA];
		if (state == states[indexB] && state == states[indexC]) 
		{
			winningPiece = state;
			return true;
		}
		return false;
	}

}
