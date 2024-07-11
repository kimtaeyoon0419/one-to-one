using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Box : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private GameObject[] items;

    [Header("Animation")]
    private Animator animator;
    private readonly int hashOpen = Animator.StringToHash("Open");
    private readonly int hashOutLine = Animator.StringToHash("OutLine");

    [Header("ItemPos")]
    [SerializeField] private Transform itemPos;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenBoxAnim()
    {
        animator.SetTrigger(hashOpen);
    }

    public void OpenBox()
    {
        int itemIndex = Random.Range(0, items.Length);
        Debug.Log("아이템 소환함 아이템 번호 : " + itemIndex);

        GameObject item =  Instantiate(items[itemIndex], transform.position, Quaternion.identity);
        item.transform.position = new Vector2(Mathf.Lerp(item.transform.position.x, itemPos.position.x, 1f), Mathf.Lerp(item.transform.position.y, itemPos.position.y, 1f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetBool(hashOutLine, true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetBool(hashOutLine, false);
        }
    }
}
