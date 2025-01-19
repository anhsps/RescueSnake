using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    public float speed = 100f;
    public SnakeHead snakeHead;
    public Transform upperSnake;

    private Quaternion lastRotation;

    private bool move = true;
    private Vector3 savePos;

    void Start()
    {
        speed = snakeHead.speed;
        lastRotation = upperSnake.rotation;// Lưu góc xoay ban đầu của upperSnake
    }

    void Update()
    {
        if (move)
            MoveTarget(upperSnake.position);

        if (upperSnake.rotation != lastRotation)
        {
            move = false;
            savePos = upperSnake.position;
            lastRotation = upperSnake.rotation;
        }

        if (!move)
        {
            MoveTarget(savePos);

            if (Vector2.Distance(transform.position, savePos) < 0.5f)
            {
                transform.rotation = lastRotation;
                move = true;
            }
        }

        if (!snakeHead.move) speed = 0;
        else speed = snakeHead.speed;
    }

    private void MoveTarget(Vector3 pos) => transform.position =
        Vector2.MoveTowards(transform.position, pos, speed * Time.deltaTime);
}
