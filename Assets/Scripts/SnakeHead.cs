using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    public float speed = 100f;
    public Transform[] waypoints;
    [HideInInspector] public bool move = true;
    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        speed = GameManager.instance.AdjustSize(speed);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!move || waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex];

        Vector2 direction = (target.position - transform.position).normalized;
        UpdateRotation(direction);

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.01f)
            currentIndex = Mathf.Clamp(currentIndex + 1, 0, waypoints.Length - 1);

        if (Vector2.Distance(transform.position, waypoints[waypoints.Length - 1].position) < 0.01f)
        {
            move = false;
            GameManager.instance.GameWin();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stone") && CheckDirection(collision.transform.position))
            move = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Stone"))
            move = true;
    }

    private bool CheckDirection(Vector3 stonePos)
    {
        Vector2 dirToStone = (stonePos - transform.position).normalized;
        Vector2 dirToWaypoint = ((Vector2)waypoints[currentIndex].position - (Vector2)transform.position).normalized;
        return Vector2.Dot(dirToStone, dirToWaypoint) > 0.8f;
    }

    // Xác định góc xoay dựa trên hướng// sprite của Snake mặc định hướng trái
    private void UpdateRotation(Vector2 direction)
    {
        if (direction == Vector2.zero) return;

        transform.rotation = Mathf.Abs(direction.x) > Mathf.Abs(direction.y)
            ? Quaternion.Euler(0, 0, direction.x > 0 ? 180f : 0) // move right or left
            : Quaternion.Euler(0, 0, direction.y > 0 ? -90f : 90f);// move up or down
    }
}
