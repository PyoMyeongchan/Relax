using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 스테이지 로드, 진행, 클리어 관리
/// </summary>
public class StageManager : MonoBehaviour
{
    [Header("Stage Data")]
    [SerializeField] private List<StageData> allStages = new List<StageData>();
    [SerializeField] private int currentStageIndex = 0;
    [SerializeField] private StageData currentStage;

    [Header("References")]
    [SerializeField] private GridSystem gridSystem;
    [SerializeField] private GridStateManager stateManager;
    [SerializeField] private BlockSpawner blockSpawner;
    [SerializeField] private PlacementSystem placementSystem;

    [Header("Stage Progress")]
    [SerializeField] private bool isStageActive;
    [SerializeField] private float stageStartTime;

    // 이벤트
    public System.Action<StageData> OnStageLoaded;
    public System.Action<StageData> OnStageCompleted;
    public System.Action OnAllStagesCompleted;
    public System.Action<StageData> OnStageFailed;

    public StageData CurrentStage => currentStage;
    public int CurrentStageIndex => currentStageIndex;
    public int TotalStages => allStages.Count;
    public bool IsStageActive => isStageActive;

    private void Awake()
    {
        // 자동으로 컴포넌트 찾기
        if (gridSystem == null)
            gridSystem = GetComponent<GridSystem>();
        
        if (stateManager == null)
            stateManager = GetComponent<GridStateManager>();
        
        if (blockSpawner == null)
            blockSpawner = GetComponent<BlockSpawner>();
        
        if (placementSystem == null)
            placementSystem = GetComponent<PlacementSystem>();
    }

    private void OnEnable()
    {
        if (placementSystem != null)
        {
            placementSystem.OnBlockPlaced += OnBlockPlaced;
            placementSystem.OnAllBlocksPlaced += OnAllBlocksPlaced;
        }
    }

    private void OnDisable()
    {
        if (placementSystem != null)
        {
            placementSystem.OnBlockPlaced -= OnBlockPlaced;
            placementSystem.OnAllBlocksPlaced -= OnAllBlocksPlaced;
        }
    }

    private void Start()
    {
        // 자동으로 첫 스테이지 시작 (원하면 주석 처리)
        if (allStages.Count > 0)
        {
            LoadStage(0);
        }
    }

    /// <summary>
    /// 스테이지 로드
    /// </summary>
    public void LoadStage(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= allStages.Count)
        {
            Debug.LogError($"Invalid stage index: {stageIndex}");
            return;
        }

        currentStageIndex = stageIndex;
        currentStage = allStages[stageIndex];

        if (currentStage == null)
        {
            Debug.LogError($"Stage at index {stageIndex} is null!");
            return;
        }

        // 스테이지 검증
        if (!currentStage.Validate())
        {
            Debug.LogError($"Stage validation failed: {currentStage.stageName}");
            return;
        }

        SetupStage();
    }

    /// <summary>
    /// StageData로 직접 로드
    /// </summary>
    public void LoadStage(StageData stage)
    {
        if (stage == null)
        {
            Debug.LogError("Stage data is null!");
            return;
        }

        currentStage = stage;
        
        // allStages에서 인덱스 찾기
        currentStageIndex = allStages.IndexOf(stage);
        if (currentStageIndex < 0)
        {
            currentStageIndex = 0;
        }

        if (!currentStage.Validate())
        {
            Debug.LogError($"Stage validation failed: {currentStage.stageName}");
            return;
        }

        SetupStage();
    }

    /// <summary>
    /// 스테이지 설정
    /// </summary>
    private void SetupStage()
    {
        Debug.Log($"Loading stage: {currentStage.stageName}");

        // 기존 것들 정리
        CleanupStage();

        // Null 체크
        if (gridSystem == null || stateManager == null || blockSpawner == null)
        {
            Debug.LogError("Required components are null!");
            return;
        }

        // 격자 생성
        gridSystem.CreateGrid(currentStage.gridWidth, currentStage.gridHeight);
        
        // StateManager 초기화 (중요!)
        stateManager.Initialize(currentStage.gridWidth, currentStage.gridHeight);
        Debug.Log($"StateManager initialized: {currentStage.gridWidth}x{currentStage.gridHeight}");

        // 블록 생성
        blockSpawner.SpawnBlocks(currentStage.blocks);

        // 스테이지 활성화
        isStageActive = true;
        stageStartTime = Time.time;

        // 이벤트 발생
        OnStageLoaded?.Invoke(currentStage);

        Debug.Log($"Stage loaded successfully: {currentStage.stageName}");
    }

    /// <summary>
    /// 스테이지 정리
    /// </summary>
    private void CleanupStage()
    {
        if (gridSystem != null)
        {
            gridSystem.ClearGrid();
        }

        if (blockSpawner != null)
        {
            blockSpawner.ClearBlocks();
        }

        isStageActive = false;
    }

    /// <summary>
    /// 블록 배치될 때마다 호출
    /// </summary>
    private void OnBlockPlaced(BlockObject block, Vector2Int gridPos)
    {
        Debug.Log($"Block placed. Checking stage completion...");
        
        // 격자 채워짐 체크는 OnAllBlocksPlaced에서 함
    }

    /// <summary>
    /// 모든 블록 배치 완료 시 호출
    /// </summary>
    private void OnAllBlocksPlaced()
    {
        Debug.Log("All blocks placed! Checking grid completion...");

        // 격자가 완전히 채워졌는지 확인
        if (placementSystem.IsGridComplete())
        {
            CompleteStage();
        }
        else
        {
            // 블록은 다 썼지만 격자가 안 채워짐 = 실패
            FailStage();
        }
    }

    /// <summary>
    /// 스테이지 클리어
    /// </summary>
    private void CompleteStage()
    {
        if (!isStageActive) return;

        isStageActive = false;
        float clearTime = Time.time - stageStartTime;

        Debug.Log($"Stage completed: {currentStage.stageName} in {clearTime:F2} seconds");

        OnStageCompleted?.Invoke(currentStage);

        // 다음 스테이지 자동 로드 (딜레이 추가 가능)
        Invoke(nameof(LoadNextStage), 2f);
    }

    /// <summary>
    /// 스테이지 실패
    /// </summary>
    private void FailStage()
    {
        if (!isStageActive) return;

        Debug.Log($"Stage failed: {currentStage.stageName}");

        OnStageFailed?.Invoke(currentStage);

        // 실패 시 재시작 옵션 제공
    }

    /// <summary>
    /// 다음 스테이지 로드
    /// </summary>
    public void LoadNextStage()
    {
        int nextIndex = currentStageIndex + 1;

        if (nextIndex < allStages.Count)
        {
            LoadStage(nextIndex);
        }
        else
        {
            // 모든 스테이지 완료
            OnAllStagesCompleted?.Invoke();
            Debug.Log("All stages completed!");
        }
    }

    /// <summary>
    /// 이전 스테이지 로드
    /// </summary>
    public void LoadPreviousStage()
    {
        int prevIndex = currentStageIndex - 1;

        if (prevIndex >= 0)
        {
            LoadStage(prevIndex);
        }
        else
        {
            Debug.Log("Already at first stage");
        }
    }

    /// <summary>
    /// 현재 스테이지 재시작
    /// </summary>
    public void RestartStage()
    {
        if (currentStage != null)
        {
            LoadStage(currentStage);
        }
    }

    /// <summary>
    /// 스테이지 추가 (런타임)
    /// </summary>
    public void AddStage(StageData stage)
    {
        if (stage != null && !allStages.Contains(stage))
        {
            allStages.Add(stage);
        }
    }

    /// <summary>
    /// 진행도 퍼센트
    /// </summary>
    public float GetProgressPercentage()
    {
        if (allStages.Count == 0) return 0f;
        return (float)(currentStageIndex + 1) / allStages.Count * 100f;
    }

    /// <summary>
    /// 현재 스테이지 플레이 시간
    /// </summary>
    public float GetCurrentStagePlayTime()
    {
        if (!isStageActive) return 0f;
        return Time.time - stageStartTime;
    }
}