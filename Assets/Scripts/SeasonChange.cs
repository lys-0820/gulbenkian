using UnityEngine;
using Vuforia;
using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using System;
public class SeasonChange : DefaultObserverEventHandler
{
    public GameObject myModelPrefab;

    GameObject mMyModelObject;

    public List<SpriteRenderer> seasonImgs;
    public float fadeInTime;
    public float fadeOutTime;
    //4 season objs
    [SerializeField] private GameObject springObj;
    [SerializeField] private GameObject summerObj;
    [SerializeField] private GameObject fallObj;
    [SerializeField] private GameObject winterObj;
    //animator
    [SerializeField] private Animator animator;
    [SerializeField] private string[] triggers;
    /*
    private void Start()
    {
        springObj.SetActive(true);
        summerObj.SetActive(false);
        fallObj.SetActive(false);
        winterObj.SetActive(false);
    }
    */
    protected override void OnTrackingFound()
    {
        // Instantiate the model prefab only if it hasn't been instantiated yet
        if (mMyModelObject == null)
            InstantiatePrefab();

        base.OnTrackingFound();
    }

    protected override void OnTrackingLost()
    {
        // Instantiate the model prefab only if it hasn't been instantiated yet
        if (mMyModelObject != null)
            DestroyPrefab();
        base.OnTrackingLost();
    }

    void InstantiatePrefab()
    {
        if (myModelPrefab != null)
        {
            mMyModelObject = Instantiate(myModelPrefab, mObserverBehaviour.transform);
            //mMyModelObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            mMyModelObject.SetActive(true);
            PlaySeasonChange(0);
        }
    }
    void DestroyPrefab()
    {
        if (myModelPrefab != null)
        {
            Destroy(mMyModelObject);
        }
    }
    private void PlaySeasonChange(int index)
    {
        // 假设 seasons 是一个包含四个季节的列表
        if (index >= seasonImgs.Count) index = 0; // 确保索引在范围内


         // 下一个季节的索引
        int nextIndex = (index + 1) % seasonImgs.Count;

        // 当前季节淡出
        seasonImgs[index].DOFade(0, fadeOutTime);
        // 下一个季节直接淡入
        seasonImgs[nextIndex].DOFade(1, fadeInTime).OnComplete(() =>
        {
            // 启动对应季节的游戏
            StartCoroutine(StartSeasonGame(nextIndex,()=>
            {
                PlaySeasonChange(nextIndex);
            }));

            // 如果需要在协程完成后执行某些操作，可以在协程内部调用
            // 例如：

            // 递归调用以继续循环
            
        });
    }
    private IEnumerator StartSeasonGame(int seasonIndex, Action onComplete)
    {

        switch (seasonIndex)
        {
            case 0:
                //春天
                springObj.SetActive(true);
                summerObj.SetActive(false);
                fallObj.SetActive(false);
                winterObj.SetActive(false);
                SetRandomTrigger();
                break;
            case 1:
                //夏天
                springObj.SetActive(false);
                summerObj.SetActive(true);
                fallObj.SetActive(false);
                winterObj.SetActive(false);
                break;
            case 2:
                //秋天
                springObj.SetActive(false);
                summerObj.SetActive(false);
                fallObj.SetActive(true);
                winterObj.SetActive(false);
                break;
            case 3:
                // 冬天
                springObj.SetActive(false);
                summerObj.SetActive(false);
                fallObj.SetActive(false);
                winterObj.SetActive(true);
                break;
        }
    // 在这里启动对应季节的游戏逻辑
    // 例如：加载场景、初始化游戏状态等
    Debug.Log($"启动季节 {seasonIndex} 的游戏");

    // 模拟一些延迟，例如加载时间
    yield return new WaitForSeconds(5f);

        Debug.Log($"结束季节 {seasonIndex} 的游戏");
        // 继续游戏逻辑
        // ...

        // 调用回调
        onComplete?.Invoke();
    }

    private void SetRandomTrigger(){
        if (triggers.Length == 0) return; // 确保触发器数组不为空

        // 随机选择一个触发器的索引
        int randomIndex = UnityEngine.Random.Range(0, triggers.Length);
        string selectedTrigger = triggers[randomIndex];

        // 设置 Animator 的触发器
        animator.SetTrigger(selectedTrigger);
        Debug.Log($"设置触发器: {selectedTrigger}");
    }
}
