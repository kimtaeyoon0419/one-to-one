using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public int poolCount; // 오브젝트를 생성 할 개수
    public string poolName; // 풀링할 오브젝트의 이름

    public int poolLength => pool.Count;

    public GameObject poolObject; // 풀링할 오브젝트의 프리펩
    public Transform parentObject;

    private Queue<GameObject> pool = new Queue<GameObject>(); // 풀링한 오브젝트를 담을 큐

    public void Enqueue(GameObject _object) => pool.Enqueue(_object);
    public GameObject Dequeue() => pool.Dequeue();
}


public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance = null;

    public Dictionary<string, Pool> poolDictionary = new Dictionary<string, Pool>();

    public List<Pool> poolList = new List<Pool>();

    #region Unity_Function
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start() => _Init();
    #endregion

    #region Private_Fucntion
    private void _Init()
    {
        foreach (Pool pool in poolList) // pool List의 값들은 pool Dictionary에 추가하는 과정
            poolDictionary.Add(pool.poolName, pool);

        foreach (Pool pool in poolDictionary.Values) // pool Dictionary의 갯수만큼 오브젝트를 생성하고 비활화하는 과정
        {
            GameObject parent = new GameObject(); // 빈 오브젝트를 부모로써 사용하기 위해 생성

            pool.parentObject = parent.transform; // 풀의 부모 오브젝트의 위치를 지정

            parent.transform.SetParent(transform); // 생성한 빈 오브젝트의 위치를 자기 자신으로
            parent.name = pool.poolName; // 생성한 오브젝트의 이름을 풀 오브젝트 이름으로

            for (int i = 0; i < pool.poolCount; i++) // 현재 풀의 생성 개수 만큼 반복
            {
                GameObject currentObject = Instantiate(pool.poolObject, parent.transform); // 현재 풀의 프리펩을 생성함
                currentObject.SetActive(false); // 비활성화

                pool.Enqueue(currentObject); // 선택된 풀에 추가
            }
        }
    }

    private GameObject _SpawnFromPool(string name, Vector3 position)
    {
        Pool currentPool = poolDictionary[name]; // 딕셔너리에서 입력받은 이름을 찾아서 초기화

        if (currentPool.poolLength <= 0)
        {
            GameObject obj = Instantiate(currentPool.poolObject, currentPool.parentObject);
            obj.SetActive(false);
            currentPool.Enqueue(obj);
        }

        GameObject currentObject = currentPool.Dequeue(); // 선택된 풀에서 Dequeue 함수로 오브젝트 가져오기
        currentObject.transform.position = position; // 입력받은 위치로 변경

        currentObject.SetActive(true); // 활성화

        return currentObject;
    }

    private GameObject _SpawnFromPool(string name, Vector3 position, Quaternion rotate)
    {
        Pool currentPool = poolDictionary[name]; // 딕셔너리에서 입력받은 이름을 찾아서 초기화

        if (currentPool.poolLength <= 0)
        {
            GameObject obj = Instantiate(currentPool.poolObject, currentPool.parentObject);
            obj.SetActive(false);
            currentPool.Enqueue(obj);
        }

        GameObject currentObject = currentPool.Dequeue(); // 선택된 풀에서 Dequeue 함수로 오브젝트 가져오기
        currentObject.transform.position = position; // 입력받은 위치로 변경
        currentObject.transform.rotation = rotate; // 입력받은 각도로 변경

        currentObject.SetActive(true); // 활성화

        return currentObject;
    }

    private void _ReturnToPool(string name, GameObject currentObject)
    {
        Pool pool = poolDictionary[name]; // 이름에 맞는 풀을 찾아서 지정

        currentObject.SetActive(false); // 비활성화
        currentObject.transform.SetParent(pool.parentObject); // 사용이 끝난 오브젝트의 부모를 다시 설정

        pool.Enqueue(currentObject);
    }
    #endregion

    #region Public_Function
    /// <summary>
    /// 풀에서 오브젝트 가져와 생성
    /// </summary>
    /// <param name="name">풀링할 오브젝트의 이름</param> 
    /// <param name="position">풀링할 위치</param>
    /// <returns></returns>
    public static GameObject SpawnFromPool(string name, Vector3 position) => instance._SpawnFromPool(name, position);
    /// <summary>
    ///  풀에서 오브젝트 가져와 생성
    /// </summary>
    /// <param name="name">풀링할 오브젝트 이름</param>
    /// <param name="position">풀링할 위치</param>
    /// <param name="rotate">풀링할 각도</param>
    /// <returns></returns>
    public static GameObject SpawnFromPool(string name, Vector3 position, Quaternion rotate) => instance._SpawnFromPool(name, position, rotate);

    /// <summary>
    /// 풀로 되돌림
    /// </summary>
    /// <param name="name">리턴할 오브젝트 이름</param>
    /// <param name="currentObejct">되돌릴 오브젝트</param>
    public static void ReturnToPool(string name, GameObject currentObejct) => instance._ReturnToPool(name, currentObejct);
    #endregion
}
