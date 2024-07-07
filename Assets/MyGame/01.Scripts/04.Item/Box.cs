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

        Instantiate(items[itemIndex], transform.position, Quaternion.identity);
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
