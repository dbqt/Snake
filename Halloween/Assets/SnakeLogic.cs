using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SnakeLogic : MonoBehaviour {

	public GameObject Head;
	public GameObject BodyPrefab;
	public GameObject FoodPrefab;

	public Text middleText;
	public Text score;

	public Vector2 Limit;

	public float delay = 1f;
	public float delayRate = 0.9f;
	private float internalDelay;

	private Vector3 direction;
	private Stack<GameObject> snake;
	private GameObject food;

	private int scoreValue;

	// 0 = playing
	// 1 = gameover
	// 2 = pre-start
	private int gamestate;

	// Use this for initialization
	void Start () {
		middleText.text = "Seasonal Snake\nStart";

		middleText.enabled = true;
		scoreValue = 0;
		gamestate = 2;
		internalDelay = delay;
		direction = Vector3.right;
		snake = new Stack<GameObject>();
		snake.Push(Head);
		SpawnNewFood();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(gamestate == 0)
		{
			if(Input.GetAxis("Horizontal") > 0f)
			{
				direction = Vector3.right;
			}
			else if(Input.GetAxis("Horizontal") < 0f)
			{
				direction = -Vector3.right;
			}
			else if(Input.GetAxis("Vertical") > 0f)
			{
				direction = Vector3.up;
			}
			else if(Input.GetAxis("Vertical") < 0f)
			{
				direction = -Vector3.up;
			}
		}
		else if(gamestate == 1)
		{
			if(Input.anyKey)
			{
				Application.LoadLevel("Game");
			}
		}
		else if(gamestate == 2)
		{
			if(Input.anyKey)
			{
				middleText.enabled = false;
				gamestate = 0;
				Invoke("UpdatePosition", 1f);
			}
		}
	}

	private void UpdatePosition()
	{
		
		Vector3 nextPosition = Head.transform.position + direction;
		// Am I dead?
		if(IsPositionOverSnake(nextPosition) || IsPositionOutOfBound(nextPosition))
		{
			GameOver();
			return;
		}

		// Do I add body?
		if(nextPosition == food.transform.position)
		{
			Destroy(food);
			AddBody();
			SpawnNewFood();
		}
		else
		{
			// Move Head and body.
			foreach(GameObject part in snake)
			{
				nextPosition = Move(part, nextPosition);
			}
		}
		Invoke("UpdatePosition", internalDelay);
	}

	private void AddBody()
	{
		snake.Pop();
		snake.Push(Instantiate(BodyPrefab, Head.transform.position, Quaternion.identity) as GameObject);
		snake.Push(Head);
		internalDelay *= delayRate; 
		scoreValue++;
		score.text = "Score: " + scoreValue;
		Move(Head, Head.transform.position + direction);
	}

	// Returns old position
	private Vector3 Move(GameObject partToMove, Vector3 newPosition)
	{
		Vector3 oldPosition = partToMove.transform.position;
		partToMove.transform.position = newPosition;
		return oldPosition;
	}

	private void SpawnNewFood()
	{
		Vector3 newFoodPosition = new Vector3(Mathf.Round(Random.value * Limit.x), Mathf.Round(Random.value * Limit.y), 0f);
		food = Instantiate(FoodPrefab, newFoodPosition, Quaternion.identity) as GameObject;
	}

	private bool IsPositionOverSnake(Vector3 position)
	{
		foreach(GameObject part in snake)
		{
			if(part.transform.position == position)
			{
				return true;
			}
		}
		return false;
	}

	private bool IsPositionOutOfBound(Vector3 position)
	{
		return (position.x >= Limit.x) || (position.x <= -Limit.x) || (position.y >= Limit.y) || (position.y <= -Limit.y);
	}

	private void GameOver()
	{
		//Debug.Log("GameOver");
		middleText.text = "Game Over\nTry Again";
		middleText.enabled = true;
		gamestate = 1;
	}
}
