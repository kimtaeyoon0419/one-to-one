using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float Speed;

    public float dirVec;

    void OnEnable()
    {   
        AudioManager.instance.PlaySFX("Shot");
        transform.localScale = Vector2.one;
        //if (PlayerBulletDir.instance.playerMovement.playerDir == Direction.Right) // 만약 플레이어의 방향이 오른쪽이라면
        //{
        //    DirVec((int)Direction.Right);
        //}
        //else // 왼쪽이라면
        //{
        //    DirVec((int)Direction.Left);
        //}
        //if(dirVec == -1)
        //{
        //    transform.localScale = transform.localScale * -1;
        //}
    }

    void Update()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }

    private void DirVec(float dir)
    {
        dirVec = dir;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Monster"))
        {
            if (collision.CompareTag("Monster")) // 만약 몬스터와 부딪혔다면
            {
                collision.GetComponent<Slime>().TakeDmg(PlayerStatManager.instance.AttackPower); // 플레이어스텟매니저의 어택파워만큼 피해를 입힘
            }
            gameObject.SetActive(false);
        }
    }
}
