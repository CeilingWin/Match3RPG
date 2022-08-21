using System;
using System.Collections;
using System.Collections.Generic;
using core;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Item : MonoBehaviour, IItem
{
    private int contentId;
    private Vector3 defaultScale;

    void Awake()
    {
        defaultScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform Transform => transform;

    public void SetContentId(int id)
    {
        contentId = id;
        var texture = (Texture) Resources.Load("Textures/Items/tile_" + id);
        GetComponent<MeshRenderer>().material.mainTexture = texture;
    }

    public int GetContentId()
    {
        return this.contentId;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public async UniTask Appear(float delayTime = 0)
    {
        transform.localScale = Vector3.zero;
        Debug.Log(defaultScale);
        await transform.DOScale(defaultScale, 0.7f)
            .SetEase(Ease.OutBack)
            .SetDelay(delayTime)
            .WithCancellation(this.GetCancellationTokenOnDestroy());
    }

    public async UniTask Disappear(float delayTime = 0)
    {
        transform.localScale = defaultScale;
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
        await transform.DOScale(Vector3.zero, 0.5f).WithCancellation(this.GetCancellationTokenOnDestroy());
    }
}
