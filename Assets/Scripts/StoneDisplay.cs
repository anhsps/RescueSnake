using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneDisplay : MonoBehaviour
{
    public static StoneDisplay instance { get; private set; }

    [SerializeField] private StonePrefab[] displayStones;
    [SerializeField] private int spriteCount = 4;// so sprite dc random cua lv hien tai
    [SerializeField] private Sprite[] stoneSprites;
    private Sprite[] selectedSprites = new Sprite[4];// 4 sprite được random để hiển thị

    private void Start()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else instance = this;

        RandomDisplaySprites();
    }
    
    public Sprite GetNextSprite()
    {
        Sprite nextSprite = selectedSprites[0];

        // thay đổi sprite của 3 stone trên màn hình
        for (int i = 0; i < selectedSprites.Length - 1; i++)
        {
            selectedSprites[i] = selectedSprites[i + 1];
            displayStones[i].SpriteDisplay(selectedSprites[i]);
        }

        // Random sprite mới cho stone cuối cùng (stone4)
        selectedSprites[selectedSprites.Length - 1] = GetRandomSprite();
        displayStones[selectedSprites.Length - 1].SpriteDisplay(selectedSprites[selectedSprites.Length - 1]);

        return nextSprite;
    }

    private void RandomDisplaySprites()
    {
        for (int i = 0; i < selectedSprites.Length; i++)
        {
            selectedSprites[i] = GetRandomSprite();
            displayStones[i].SpriteDisplay(selectedSprites[i]);
        }
    }

    private Sprite GetRandomSprite()
    {
        if (spriteCount > stoneSprites.Length)
            spriteCount = stoneSprites.Length;
        return stoneSprites[Random.Range(0, spriteCount)];
    }
}
