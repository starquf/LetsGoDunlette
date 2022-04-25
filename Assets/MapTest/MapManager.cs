using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    [SerializeField] MapGenerator mapGenerator;

    public List<Map> tiles;
    [SerializeField] Transform playerTrm;

    void Awake()
    {
        GameManager.Instance.mapManager = this;
        tiles = new List<Map>();
    }

    void Start()
    {
        mapGenerator.GenerateGrid();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MovePlayer(tiles[Random.Range(0, tiles.Count)].transform);
        }
    }

    public void MovePlayer(Transform trm, Action onComplete = null)
    {
        //playerTrm.SetParent(trm);


        //playerTrm.DOLocalJump(Vector3.zero, 80f, 1, 0.5f).SetDelay(0.5f).OnComplete(()=>
        //{
        //    onComplete?.Invoke();
        //});


        playerTrm.DOJump(trm.position, 0.5f, 1, 0.5f).SetEase(Ease.OutCubic).SetDelay(0.5f).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void SetPlayerPos(Transform trm)
    {
        //playerTrm.SetParent(trm);

        //playerTrm.localPosition = Vector3.zero;

        playerTrm.position = trm.position;
    }
}
