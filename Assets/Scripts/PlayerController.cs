using System;
using System.Collections;
using System.Collections.Generic;
using TorcheyeUtility;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Speed of player movement
    public Transform flashlight; // Reference to the flashlight child
    public Vector3 respawn;

    private Rigidbody2D rb;

    private void Start()
    {
        respawn = transform.position;
        rb = GetComponent<Rigidbody2D>();
        if (flashlight == null)
        {
            Debug.LogWarning("Flashlight not assigned in PlayerController.");
        }
    }

    private void Update()
    {
        MovePlayer();
        PointFlashlightToMouse();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
        rb.velocity = moveDirection * moveSpeed;
    }

    void PointFlashlightToMouse()
    {
        if (flashlight != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            flashlight.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}