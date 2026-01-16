using UnityEngine;

/// <summary>
/// 게임 전체 관리 (싱글톤)
/// </summary>
public class GameManager : MonoBehaviour
{
    // 싱글톤
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    instance = go.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    [Header("Core Systems")]
    [SerializeField] private GridSystem gridSystem;
    [SerializeField] private GridStateManager stateManager;
    [SerializeField] private BlockSpawner blockSpawner;
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private StageManager stageManager;

    [Header("Audio")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip clickSfx;
    [SerializeField] private AudioClip successSfx;
    [SerializeField] private AudioClip failSfx;

    [Header("Game State")]
    [SerializeField] private bool isGameActive;
    [SerializeField] private bool isPaused;

    public bool IsGameActive => isGameActive;
    public bool IsPaused => isPaused;

    // 이벤트
    public System.Action OnGameStarted;
    public System.Action OnGamePaused;
    public System.Action OnGameResumed;
    public System.Action OnGameOver;

    private void Awake()
    {
        // 싱글톤 설정
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // 컴포넌트 자동 찾기
        FindComponents();

        // 오디오 소스 생성
        SetupAudio();
    }

    private void Start()
    {
        // 이벤트 연결
        ConnectEvents();

        // BGM 시작
        PlayBGM();

        // 게임 시작
        StartGame();
    }

    /// <summary>
    /// 컴포넌트 자동 찾기
    /// </summary>
    private void FindComponents()
    {
        if (gridSystem == null)
            gridSystem = FindObjectOfType<GridSystem>();
        
        if (stateManager == null)
            stateManager = FindObjectOfType<GridStateManager>();
        
        if (blockSpawner == null)
            blockSpawner = FindObjectOfType<BlockSpawner>();
        
        if (placementSystem == null)
            placementSystem = FindObjectOfType<PlacementSystem>();
        
        if (inputManager == null)
            inputManager = FindObjectOfType<InputManager>();
        
        if (stageManager == null)
            stageManager = FindObjectOfType<StageManager>();
    }

    /// <summary>
    /// 오디오 설정
    /// </summary>
    private void SetupAudio()
    {
        // BGM 소스
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
            bgmSource.volume = 0.5f;
        }

        // SFX 소스
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
            sfxSource.volume = 0.7f;
        }
    }

    /// <summary>
    /// 이벤트 연결
    /// </summary>
    private void ConnectEvents()
    {
        if (inputManager != null)
        {
            inputManager.OnBlockSelected += OnBlockSelected;
            inputManager.OnGridCellClicked += OnGridCellClicked;
        }

        if (placementSystem != null)
        {
            placementSystem.OnBlockPlaced += OnBlockPlaced;
            placementSystem.OnPlacementFailed += OnPlacementFailed;
        }

        if (stageManager != null)
        {
            stageManager.OnStageLoaded += OnStageLoaded;
            stageManager.OnStageCompleted += OnStageCompleted;
            stageManager.OnAllStagesCompleted += OnAllStagesCompleted;
            stageManager.OnStageFailed += OnStageFailed;
        }
    }

    /// <summary>
    /// 게임 시작
    /// </summary>
    public void StartGame()
    {
        isGameActive = true;
        isPaused = false;

        Debug.Log("Game Started!");
        OnGameStarted?.Invoke();
    }

    /// <summary>
    /// 게임 일시정지
    /// </summary>
    public void PauseGame()
    {
        if (!isGameActive) return;

        isPaused = true;
        Time.timeScale = 0f;

        Debug.Log("Game Paused");
        OnGamePaused?.Invoke();
    }

    /// <summary>
    /// 게임 재개
    /// </summary>
    public void ResumeGame()
    {
        if (!isGameActive) return;

        isPaused = false;
        Time.timeScale = 1f;

        Debug.Log("Game Resumed");
        OnGameResumed?.Invoke();
    }

    /// <summary>
    /// 게임 종료
    /// </summary>
    public void GameOver()
    {
        isGameActive = false;

        Debug.Log("Game Over");
        OnGameOver?.Invoke();
    }

    #region Audio Methods

    /// <summary>
    /// BGM 재생
    /// </summary>
    public void PlayBGM()
    {
        if (bgmSource != null && bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.Play();
        }
    }

    /// <summary>
    /// BGM 정지
    /// </summary>
    public void StopBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }

    /// <summary>
    /// SFX 재생
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// BGM 볼륨 설정
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = Mathf.Clamp01(volume);
        }
    }

    /// <summary>
    /// SFX 볼륨 설정
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
        }
    }

    #endregion

    #region Event Handlers

    private void OnBlockSelected(BlockObject block)
    {
        PlaySFX(clickSfx);
    }

    private void OnGridCellClicked(GridCell cell)
    {
        PlaySFX(clickSfx);
    }

    private void OnBlockPlaced(BlockObject block, Vector2Int gridPos)
    {
        PlaySFX(successSfx);
    }

    private void OnPlacementFailed()
    {
        PlaySFX(failSfx);
    }

    private void OnStageLoaded(StageData stage)
    {
        Debug.Log($"GameManager: Stage loaded - {stage.stageName}");
    }

    private void OnStageCompleted(StageData stage)
    {
        Debug.Log($"GameManager: Stage completed - {stage.stageName}");
        PlaySFX(successSfx);
    }

    private void OnAllStagesCompleted()
    {
        Debug.Log("GameManager: All stages completed!");
        GameOver();
    }

    private void OnStageFailed(StageData stage)
    {
        Debug.Log($"GameManager: Stage failed - {stage.stageName}");
        PlaySFX(failSfx);
    }

    #endregion

    #region Public Helper Methods

    /// <summary>
    /// 현재 스테이지 재시작
    /// </summary>
    public void RestartCurrentStage()
    {
        if (stageManager != null)
        {
            stageManager.RestartStage();
        }
    }

    /// <summary>
    /// 다음 스테이지로
    /// </summary>
    public void GoToNextStage()
    {
        if (stageManager != null)
        {
            stageManager.LoadNextStage();
        }
    }

    /// <summary>
    /// 특정 스테이지로
    /// </summary>
    public void LoadStage(int index)
    {
        if (stageManager != null)
        {
            stageManager.LoadStage(index);
        }
    }

    #endregion

    private void OnDestroy()
    {
        // 이벤트 해제
        if (inputManager != null)
        {
            inputManager.OnBlockSelected -= OnBlockSelected;
            inputManager.OnGridCellClicked -= OnGridCellClicked;
        }

        if (placementSystem != null)
        {
            placementSystem.OnBlockPlaced -= OnBlockPlaced;
            placementSystem.OnPlacementFailed -= OnPlacementFailed;
        }

        if (stageManager != null)
        {
            stageManager.OnStageLoaded -= OnStageLoaded;
            stageManager.OnStageCompleted -= OnStageCompleted;
            stageManager.OnAllStagesCompleted -= OnAllStagesCompleted;
            stageManager.OnStageFailed -= OnStageFailed;
        }
    }
}