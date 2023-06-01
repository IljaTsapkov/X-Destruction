using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMotor : MonoBehaviour, IDataPersistence
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public Accelerometer accelerometer;
    private bool isGrounded;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 1f;
    public Transform orientation;
    [SerializeField] private DataPersistence dataPersistence;
    public float speedIncreaseAmount = 6f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

  
        accelerometer.IncreaseAcceleration();

 

        float currentAcceleration = accelerometer.GetCurrentAcceleration();

       
        float currentSpeed = speed + (currentAcceleration * speedIncreaseAmount);

        
        controller.Move(transform.TransformDirection(moveDirection) * currentSpeed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;

        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -1.0f * gravity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Checkpoint")
        {
            // Check if the checkpoint is already in the list of touched checkpoints
            if (!DataPersistence.instance.gameDataPublic.touchedCheckpoints.Contains(other.gameObject.name))
            {
                // Add the checkpoint to the list of touched checkpoints
                DataPersistence.instance.gameDataPublic.touchedCheckpoints.Add(other.gameObject.name);
                // Save the game
                DataPersistence.instance.SaveGame();
                Debug.Log("Saving game using checkpoint.");
                // Disable the checkpoint
                other.gameObject.SetActive(false);
            }
        }

        if (other.gameObject.tag == "Finish")
        {
            SceneManager.LoadSceneAsync(2);
        }
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
        // Disable any checkpoints that are in the list of touched checkpoints
        foreach (string checkpointName in data.touchedCheckpoints)
        {
            GameObject checkpoint = GameObject.Find(checkpointName);
            if (checkpoint != null)
            {
                checkpoint.SetActive(false);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }
}
