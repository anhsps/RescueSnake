using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoneController : MonoBehaviour
{
    private Vector2 direction, nextPos;
    private Sprite sprite;
    private bool isMoving = false;
    private float moveStep = 163 / 3f; // S move 1 step
    private float moveSpeed = 1000f;
    [SerializeField] private Sprite destroy, destroy2;
    AudioSource explosion_audio;

    private void Start()
    {
        sprite = GetComponent<Image>().sprite;
        explosion_audio = GetComponent<AudioSource>();

        moveStep = GameManager.instance.AdjustSize(moveStep);
        moveSpeed = GameManager.instance.AdjustSize(moveSpeed);
    }

    private void Update()
    {

    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
        StartCoroutine(Move(0));
    }

    private IEnumerator Move(float t)
    {
        yield return new WaitForSeconds(t);
        transform.position = new Vector3(Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y), transform.position.z);

        isMoving = true;
        while (isMoving)
        {
            nextPos = (Vector2)transform.position + direction * moveStep;

            // check nếu va chạm
            while (CheckCollision(nextPos))
            {
                isMoving = false;
                CheckDestroyStones();
                //yield break;
                yield return StartCoroutine(Move(0.5f));// !CheckCollision(nextPos) -> isMoving
            }

            // move -> nextPos
            float elapsedTime = 0f;
            float duration = moveStep / moveSpeed;
            Vector2 startPos = transform.position;

            while (elapsedTime < duration)
            {
                transform.position = Vector2.Lerp(startPos, nextPos, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = nextPos;
        }
    }

    private bool CheckCollision(Vector2 targetPos)
    {
        Collider2D hit = Physics2D.OverlapCircle(targetPos, 1f);
        if (hit && (hit.CompareTag("Stone") || hit.CompareTag("Snake")))
            return true;
        return false;
    }

    private void CheckDestroyStones()
    {
        // Kiểm tra hàng ngang và hàng dọc
        List<GameObject> horizontal = CheckLine(Vector2.left, Vector2.right);
        List<GameObject> vertical = CheckLine(Vector2.up, Vector2.down);

        if (horizontal.Count >= 3) StartCoroutine(DestroyStones(horizontal));
        if (vertical.Count >= 3) StartCoroutine(DestroyStones(vertical));
    }

    private List<GameObject> CheckLine(Vector2 dir1, Vector2 dir2)
    {
        List<GameObject> result = new List<GameObject> { gameObject };
        result.AddRange(GetStonesInDirection(dir1));
        result.AddRange(GetStonesInDirection(dir2));
        return result;
    }

    private List<GameObject> GetStonesInDirection(Vector2 dir)
    {
        List<GameObject> stones = new List<GameObject>();
        Vector2 checkPos = (Vector2)transform.position + dir * moveStep;

        while (true)
        {
            Collider2D hit = Physics2D.OverlapCircle(checkPos, 1f);
            if (hit && hit.CompareTag("Stone") && hit.GetComponent<Image>().sprite == sprite)
            {
                stones.Add(hit.gameObject);
                checkPos += dir * moveStep;
            }
            else break;
        }

        return stones;
    }

    private IEnumerator DestroyStones(List<GameObject> stones)
    {
        if (explosion_audio && GameManager.instance.soundEnabled)
            explosion_audio.Play();

        if (destroy && destroy2)
        {
            stones.ForEach(stone => stone.GetComponent<Image>().sprite = destroy);
            yield return new WaitForSeconds(0.1f);
            stones.ForEach(stone => stone.GetComponent<Image>().sprite = destroy2);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.1f);
        stones.ForEach(Destroy);
    }
}
