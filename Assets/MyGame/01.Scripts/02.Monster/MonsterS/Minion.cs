using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Minion : Monster
{
    [Header("animation")]
    private readonly int hashMove = Animator.StringToHash("Move");

    [Header("SpriteLibrary")]
    [SerializeField] SpriteLibrary library;
    [SerializeField] SpriteLibraryAsset[] spriteLibraries;

    protected override void Awake()
    {
        base.Awake();
        library = GetComponent<SpriteLibrary>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        int randomColor = Random.Range(0, spriteLibraries.Length);
        library.spriteLibraryAsset = spriteLibraries[randomColor];
    }

    protected override void Update()
    {
        base.Update();
        if (nextMove != 0)
        {
            animator.SetBool(hashMove, true);
        }
        else
        {
            animator.SetBool(hashMove, false);
        }
    }
}
