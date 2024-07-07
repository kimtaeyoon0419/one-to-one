using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[System.Serializable]
public class ItemType
{
    public SpriteLibraryAsset library;
    public float speed;
    public int armor;
    public int attack;
}

/// <summary>
/// 클리어 아이템 스텟
/// </summary>
public class clearItem : MonoBehaviour
{
    [SerializeField] private List<ItemType> items;
    private SpriteLibrary spriteLibrary;
    private int itemIdex;

    public int attack;
    public int armor;
    public float speed;

    private void Awake()
    {
        spriteLibrary = GetComponent<SpriteLibrary>();
    }

    private void OnEnable()
    {
        itemIdex = Random.Range(0, items.Count);
        spriteLibrary.spriteLibraryAsset = items[itemIdex].library;
        attack = items[itemIdex].attack;
        armor = items[itemIdex].armor;
        speed = items[itemIdex].speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RamdomMapSpawner.ItemCollected();
            Destroy(gameObject);
        }
    }
}
