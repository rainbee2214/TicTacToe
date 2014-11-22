using UnityEngine;
using System.Collections;

public class GamePiece : MonoBehaviour 
{
	private bool pickedUp = false;
	private bool overGrid = false;
	private bool placed = false;
	private Vector2 newPosition;

	private string currentGridSpace;

	bool PickUp()
	{
		return true;
	}

	bool SetDown()
	{
		if (!overGrid) transform.position = 
			(this.tag == "X") ?
			transform.parent.gameObject.GetComponent<GridManager>().xStartingPosition :
				transform.parent.gameObject.GetComponent<GridManager>().oStartingPosition;
		else 
		{
			bool validSpace = SnapToGrid();
			if (validSpace) 
			{
				placed = true;
			}
			else
			{
				overGrid = false;
				SetDown();
			}
		}
		return false;
	}

	bool SnapToGrid()
	{
		bool validSpace = false;
		if (currentGridSpace != "Outside")
		{
			//Find current gridspace
			int currentSpace = int.Parse(currentGridSpace.Substring(currentGridSpace.Length-1, 1));
			string currentState = transform.parent.gameObject.GetComponent<GridManager>().GetGridState(currentSpace);
			switch(currentState)
			{
			case "X": 
			case "O": validSpace = false; break;
			case "Empty": validSpace = true; break;
			}
			//Put piece into the position of that gridspace
			if (validSpace)
			{
				transform.position = transform.parent.gameObject.GetComponent<GridManager>().positions[currentSpace];
				transform.parent.gameObject.GetComponent<GridManager>().SetGridState(currentSpace, this.name);
			}
		} 
			
		return validSpace;
	}

	void OnMouseDown() 
	{
		if (!placed)  pickedUp = pickedUp ? SetDown() : PickUp();
	}
	
	void Update()
	{
		if (!placed)
		{
			if (pickedUp)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Vector3 mousePosition = ray.origin + ray.direction;
				transform.position = mousePosition;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Outside") 
		{
			overGrid = false;
			currentGridSpace = other.tag;
		}
		else if (other.tag.Substring(0, 1) == "G") 
		{
			overGrid = true;
			currentGridSpace = other.name;
		}
	}	
}
