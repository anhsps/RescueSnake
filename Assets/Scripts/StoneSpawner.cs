using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner : MonoBehaviour
{
    [SerializeField] private bool up, down, left, right;
    [SerializeField] private StonePrefab stonePrefab;
    [SerializeField] private float posUp = (147 + 16) / 3f;

    private void Start()
    {
        
    }

    private void Update()
    {

    }

    public void SpawnStone()
    {
        Vector3 nextPos = transform.position + transform.up * posUp;
        Collider2D hit = Physics2D.OverlapCircle(nextPos, 1f);
        if (hit && (hit.CompareTag("Stone") || hit.CompareTag("Snake")))
            return;

        StonePrefab newStone = Instantiate(stonePrefab, transform.position, Quaternion.identity, transform.parent);
        newStone.SetSprite();

        Vector3 moveDirection = up ? Vector3.up : down ? Vector3.down :
            left ? Vector3.left : right ? Vector3.right : Vector3.zero;
        newStone.GetComponent<StoneController>().SetDirection(moveDirection);
    }
}
