// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private int itemCode;
    [SerializeField] private string itemName;
    private PlayerStats playerStats;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(GameManager.instance.coin >= 25)
        {
            Destroy(gameObject);
                playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if(itemCode == 1)
            {
                Item1();
                playerStats.AllStatUp();
            }
            else if(itemCode == 2)
            {
                Item2();
            }
            else if(itemCode == 4)
            {
                collision.gameObject.GetComponent<WeaponManager>().ReroadBullet();
            }
            else if (itemCode == 5)
            {
                Item5();
            }
            else if (itemCode == 6)
            {
                Item6();
            }
            else if (itemCode == 7)
            {
                Item7();
            }
            else if (itemCode == 8)
            {
                Item8();
            }
            else if (itemCode == 9)
            {
                Item9();
            }
            else if (itemCode == 10)
            {
                Item10();
            }
        }
    }


    /// <summary>
    /// 네크로노미콘
    /// </summary>
    private void Item1()
    {
        itemName = "네크로노미콘";
    }

    /// <summary>
    /// 살아있는심장
    /// </summary>
    private void Item2()
    {
        itemName = "살아있는심장";
        playerStats.armorDurability++;
    }
    
    /// <summary>
    /// 푸른 가시관
    /// </summary>
    private void Item3()
    {
        itemName = "푸른 가시관";
        playerStats.item1 = 1;
    }

    /// <summary>
    /// 맛있는 사과
    /// </summary>
    private void Item4()
    {
        itemName = "맛있는 사과";
    }

    /// <summary>
    /// 악마의 뿔
    /// </summary>
    private void Item5()
    {
        itemName = "악마의 뿔";
        playerStats.speed -= 1f;
        PlayerStats.attackPower += 2;
    }

    /// <summary>
    /// 십자가
    /// </summary>
    private void Item6()
    {
        itemName = "십자가";
        PlayerStats.attackPower += 1;
    }

    /// <summary>
    /// 멈춰버린 회중시계
    /// </summary>
    private void Item7()
    {
        playerStats.item2 = 1;
    }

    /// <summary>
    /// 누군가의 날개
    /// </summary>
    private void Item8()
    {
        playerStats.jumpPoawer += 0.5f;
    }

    /// <summary>
    /// 원숭이 손
    /// </summary>
    private void Item9()
    {
        playerStats.RandomStat();
    }

    /// <summary>
    /// 지니의 램프
    /// </summary>
    private void Item10()
    {

    }
}
