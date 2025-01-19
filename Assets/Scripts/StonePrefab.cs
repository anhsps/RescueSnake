using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StonePrefab : MonoBehaviour
{
    private Image image;
    [SerializeField] private bool up, down, left, right, originalStone;
    [SerializeField] private Sprite[] stoneSprites;

    void Start()
    {
        image = GetComponent<Image>();
        SetSprite();

        Vector3 moveDirection = up ? Vector3.up : down ? Vector3.down :
            left ? Vector3.left : right ? Vector3.right : Vector3.zero;
        if (originalStone)
            GetComponent<StoneController>().SetDirection(moveDirection);
    }

    void Update()
    {
        if (transform.localPosition.y > Screen.height * 2f)
            Destroy(gameObject);
    }

    public void SetSprite()
    {
        if (image != null && stoneSprites.Length > 0)
        {
            Sprite nextSprite = StoneDisplay.instance.GetNextSprite();
            image.sprite = nextSprite;
        }
    }

    public void SpriteDisplay(Sprite sprite) => image.sprite = sprite;
}
