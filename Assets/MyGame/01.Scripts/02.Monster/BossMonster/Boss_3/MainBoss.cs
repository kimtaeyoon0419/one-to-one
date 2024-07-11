using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainBoss : MonoBehaviour
{
    [Header("Transforms")]
    public Transform leftHand;
    public Transform rightHand;
    public Transform body;
    public Transform leftMapEdge; // 맵의 왼쪽 끝
    public Transform rightMapEdge; // 맵의 오른쪽 끝
    public Transform leftOriginalPosition; // 왼손의 원래 위치
    public Transform rightOriginalPosition; // 오른손의 원래 위치
    public Transform player; // 플레이어 위치

    [Header("Clap Attack Settings")]
    public float initialClapSpeed = 1.0f; // 초기 박수 속도
    public float speedIncreaseRate = 0.5f; // 속도 증가율
    public float clapRange = 1.0f; // 박수 범위
    public float clapCooldown = 3.0f; // 박수 공격 쿨타임

    [Header("Slam Attack Settings")]
    public float moveSpeedToPlayer = 5.0f; // 플레이어 위치로 이동하는 속도
    public float slamSpeed = 0.2f; // 내려치는 속도
    public float slamRange = 1.0f; // 내려치기 범위
    public float slamDamage = 10f; // 내려치기 데미지

    [Header("General Settings")]
    public float stunDuration = 1.0f; // 스턴 지속 시간
    public float patternInterval = 5.0f; // 패턴 간격
    public float maxHp = 1000.0f; // 보스의 최대 체력

    public float curHp; // 현재 체력
    private float nextPatternTime;
    private Animator leftHandAnimator;
    private Animator rightHandAnimator;

    private float originalPatternInterval;
    private float originalClapSpeed;
    private float originalSlamSpeed;

    public string bossName;


    void Start()
    {
        curHp = maxHp; // 현재 체력을 최대 체력으로 초기화
        nextPatternTime = Time.time + patternInterval;
        leftHandAnimator = leftHand.GetComponent<Animator>();
        rightHandAnimator = rightHand.GetComponent<Animator>();

        originalPatternInterval = patternInterval;
        originalClapSpeed = initialClapSpeed;
        originalSlamSpeed = slamSpeed;
    }

    void Update()
    {
        if (Time.time >= nextPatternTime)
        {
            StartCoroutine(ExecuteRandomPattern());
            nextPatternTime = Time.time + patternInterval + (2 * stunDuration); // 두 손이 각각 스턴 후 다음 패턴 시작
        }

        // 현재 체력이 최대 체력의 3분의 1 이하로 떨어지면 패턴 주기와 속도 증가
        if (curHp <= maxHp / 3.0f)
        {
            patternInterval = originalPatternInterval / 2;
            initialClapSpeed = originalClapSpeed * 1.5f;
            slamSpeed = originalSlamSpeed / 1.5f;
        }
    }

    IEnumerator ExecuteRandomPattern()
    {
        int pattern = Random.Range(0, 2); // 0 또는 1의 랜덤한 패턴 선택

        if (pattern == 0)
        {
            yield return ClapAttack();
        }
        else if (pattern == 1)
        {
            yield return SlamAttack();
        }

        // 스턴 상태
        yield return new WaitForSeconds(stunDuration);

        // 손을 원래 위치로 이동
        yield return MoveHandsToOriginalPosition();
    }

    IEnumerator ClapAttack()
    {
        // 맵의 끝에서 시작
        Vector3 leftStartPos = leftMapEdge.position;
        Vector3 rightStartPos = rightMapEdge.position;
        Vector3 clapPosition = (leftStartPos + rightStartPos) / 2; // 중간 지점

        // 손을 중간 지점으로 이동시키며 점점 속도를 증가
        float elapsedTime = 0;
        float currentSpeed = initialClapSpeed;
        float duration = 1.0f; // 전체 이동 시간

        // 박수 애니메이션 실행
        leftHandAnimator.SetTrigger("Clap");
        rightHandAnimator.SetTrigger("Clap");
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            leftHand.position = Vector3.Lerp(leftStartPos, clapPosition, t * t); // 가속을 위해 t^2 사용
            rightHand.position = Vector3.Lerp(rightStartPos, clapPosition, t * t);
            elapsedTime += Time.deltaTime * currentSpeed;
            currentSpeed += speedIncreaseRate * Time.deltaTime; // 속도 증가
            yield return null;
        }
    }

    IEnumerator SlamAttack()
    {
        // 양손을 동시에 내려치기 시작
        StartCoroutine(SlamHand(leftHand));
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(SlamHand(rightHand));
    }

    IEnumerator SlamHand(Transform hand)
    {
        Vector3 startPos = hand.position;
        Vector3 targetPosition = new Vector3(player.position.x, startPos.y, startPos.z);

        // 손을 플레이어 x 위치로 빠르게 이동
        float elapsedTime = 0;
        while (elapsedTime < 1 / moveSpeedToPlayer)
        {
            hand.position = Vector3.Lerp(startPos, targetPosition, elapsedTime * moveSpeedToPlayer);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 내려치기 애니메이션 실행
        hand.GetComponent<Animator>().SetTrigger("Slam");

        Vector3 slamPosition = new Vector3(targetPosition.x, player.position.y - 2.0f, targetPosition.z); // 플레이어 위치까지 내려침

        elapsedTime = 0;
        float totalSlamDuration = 1.0f; // 전체 내려치는 시간
        while (elapsedTime < totalSlamDuration)
        {
            float t = elapsedTime / totalSlamDuration;
            hand.position = Vector3.Lerp(targetPosition, slamPosition, t * t); // t^2 사용하여 가속
            elapsedTime += Time.deltaTime;
            yield return null;
        }


        // 손을 다시 제자리로 이동
        elapsedTime = 0;
        while (elapsedTime < slamSpeed)
        {
            hand.position = Vector3.Lerp(slamPosition, startPos, elapsedTime / slamSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator MoveHandsToOriginalPosition()
    {
        Vector3 leftStartPos = leftHand.position;
        Vector3 rightStartPos = rightHand.position;
        Vector3 leftTargetPos = leftOriginalPosition.position;
        Vector3 rightTargetPos = rightOriginalPosition.position;

        float elapsedTime = 0;
        float duration = 1.0f; // 전체 이동 시간

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            leftHand.position = Vector3.Lerp(leftStartPos, leftTargetPos, t);
            rightHand.position = Vector3.Lerp(rightStartPos, rightTargetPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void TakeDamage(float damage)
    {
        curHp -= damage;

        if(curHp < 0)
        {
            GameManager.instance.curGameState = CurGameState.gameClear;
            SceneManager.LoadScene("GameClear");
        }
    }
}
