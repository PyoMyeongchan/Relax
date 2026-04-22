# 🎮 Relax Architecture

## 📋 프로젝트 개요
편안한 정렬의 만족감을 주는 캐주얼 퍼즐 게임
- **플랫폼**: Android
- **장르**: 캐주얼 퍼즐
- **핵심 컨셉**: 오브젝트를 정렬/배치하여 만족감 제공

---

## 📁 프로젝트 구조

```
Assets/
├── Scenes/
│   └── MainGame.unity
│
├── Scripts/
│   ├── Core/
│   │   ├── GameManager.cs           # 게임 전체 관리 (싱글톤)
│   │   └── StageManager.cs          # 스테이지 로드/진행/클리어
│   │
│   ├── Grid/
│   │   ├── GridSystem.cs            # 격자 생성/관리/상태 체크
│   │   └── GridCell.cs              # 개별 셀 (비어있음/채워짐)
│   │
│   ├── Block/
│   │   ├── BlockObject.cs           # 블록 개체 (모양, 선택 상태)
│   │   ├── BlockSpawner.cs          # 블록 생성 및 배치
│   │   └── BlockShape.cs            # 블록 모양 데이터
│   │
│   ├── Placement/
│   │   └── PlacementSystem.cs       # 배치 로직/검증/프리뷰
│   │
│   ├── Input/
│   │   └── InputManager.cs          # 클릭 입력 처리
│   │
│   └── Data/
│       └── StageData.cs             # 스테이지 데이터 (ScriptableObject)
│
├── Prefabs/
│   ├── GridCell.prefab
│   └── Block.prefab
│
├── Materials/
│   └── BlockMaterials/
│
├── Audio/
│   ├── BGM/
│   │   └── main_bgm.mp3             # 메인 배경음 (루프)
│   └── SFX/
│       ├── click.wav                # 일반 클릭
│       ├── success.wav              # 성공
│       └── fail.wav                 # 실패
│
└── Resources/
    └── Stages/
        ├── Stage_1_1.asset
        ├── Stage_1_2.asset
        └── Stage_1_3.asset
```

---

## 🏗️ 시스템 아키텍처

> 📐 **상세 아키텍처 다이어그램 (FigJam)**: [시스템별 전체 보기](https://www.figma.com/online-whiteboard/create-diagram/7b09cc90-b454-4ae8-8325-251c82e41c97?utm_source=claude&utm_content=edit_in_figjam)

<details>
<summary>개별 시스템 다이어그램 보기</summary>

| 다이어그램 | 링크 |
|-----------|------|
| ① GameManager 오케스트레이션 | [보기](https://www.figma.com/online-whiteboard/create-diagram/33e92f7e-10a1-4bde-b034-0aaf08f5bb9f?utm_source=claude&utm_content=edit_in_figjam) |
| ② 스테이지 흐름 | [보기](https://www.figma.com/online-whiteboard/create-diagram/f2edab0e-0d87-41d6-8eff-aae21035efbd?utm_source=claude&utm_content=edit_in_figjam) |
| ③ 격자 시스템 | [보기](https://www.figma.com/online-whiteboard/create-diagram/7bb8b998-8579-48c5-982f-2af5972061e3?utm_source=claude&utm_content=edit_in_figjam) |
| ④ 블록 시스템 | [보기](https://www.figma.com/online-whiteboard/create-diagram/3a93bfca-0b7f-4e7a-903f-6749f27c07da?utm_source=claude&utm_content=edit_in_figjam) |
| ⑤ 입력 & 배치 흐름 | [보기](https://www.figma.com/online-whiteboard/create-diagram/72b71ed9-fb0e-4a84-bba3-272bb1198cb2?utm_source=claude&utm_content=edit_in_figjam) |
| ⑥ 로비 씬 | [보기](https://www.figma.com/online-whiteboard/create-diagram/c6f9eb02-2993-4797-b6df-260a0b54e743?utm_source=claude&utm_content=edit_in_figjam) |

</details>

### 전체 의존성 구조

![Architecture](docs/architecture.png)

---

## 🔄 게임 플로우

### 초기화
```
GameManager.Start()
  ↓
StageManager.LoadStage(stageNumber)
  ↓
GridSystem.CreateGrid(width, height)
  ↓
BlockSpawner.SpawnBlocks(stageData.blocks)
```

### 플레이 루프
```
InputManager.OnClick()
  ↓
블록 클릭? → selectedBlock 저장
  ↓
셀 클릭? → PlacementSystem.TryPlace(block, cell)
  ↓
배치 가능?
  ├─ YES → GridSystem.UpdateCells()
  │         BlockObject 위치 이동
  │         SFX: success.wav
  │
  └─ NO  → 배치 실패
            SFX: fail.wav
```

### 클리어 체크
```
PlacementSystem.OnBlockPlaced()
  ↓
GridSystem.IsGridFull()?
  ↓
YES → StageManager.OnStageComplete()
       ↓
     다음 스테이지 로드
```

---

## 📊 클래스 책임

| 클래스 | 책임 |
|--------|------|
| **GameManager** | 전체 게임 흐름 총괄, 싱글톤 |
| **StageManager** | 스테이지 로드/진행/완료 관리 |
| **GridSystem** | 격자 생성 및 셀 상태 관리 |
| **GridCell** | 개별 셀 정보 (좌표, 상태) |
| **BlockObject** | 블록 개체의 모양과 상태 |
| **BlockSpawner** | 블록 생성 및 초기 배치 |
| **PlacementSystem** | 배치 가능 여부 검증 및 실행 |
| **InputManager** | 사용자 입력(클릭) 처리 |
| **StageData** | 스테이지 정보 (ScriptableObject) |

---

## 🎯 데이터 구조

### StageData (ScriptableObject)
```csharp
public class StageData : ScriptableObject
{
    public int stageNumber;
    public int gridWidth;
    public int gridHeight;
    public List<BlockShapeData> blocks;
}
```

### BlockShapeData
```csharp
[System.Serializable]
public class BlockShapeData
{
    public bool[,] shape;      // 2D 블록 모양
    public Vector2Int size;    // 블록 크기
}
```

---

## 🎮 미니게임 타입 (계획)

각 타입당 3개 스테이지 예정

1. **테트리스형 채우기** ⭐ (1차 구현)
   - 다양한 모양의 블록을 격자에 완벽하게 채우기

2. **높이 맞추기**
   - 다양한 높이의 큐브를 평평하게 정렬

3. **크기 순서 정렬**
   - 작은 것부터 큰 것까지 순서대로 배치

4. **대칭 맞추기**
   - 한쪽 패턴을 반대편에 똑같이 복사

5. **빈틈 메우기**
   - 구멍 난 벽에 딱 맞는 블록 끼우기

6. **정렬/정돈**
   - 흩어진 큐브를 일렬/원형으로 정렬

7. **회전 맞추기**
   - 회전된 블록을 모두 같은 방향으로 정렬

8. **스택 쌓기**
   - 블록을 안정적으로 쌓기

---

## 🔊 사운드 시스템

| 타입 | 파일 | 용도 |
|------|------|------|
| BGM | main_bgm.mp3 | 메인 배경음악 (루프) |
| SFX | click.wav | 일반 클릭 |
| SFX | success.wav | 블록 배치 성공 |
| SFX | fail.wav | 배치 실패 |

---

## 🎨 디자인 컨셉

- **색상**: 최소화 (미니멀)
- **스타일**: 깔끔한 정렬/배치의 만족감
- **피드백**: 시각/청각/촉각(햅틱) 조합
- **목표**: 편안함과 만족감 제공

---

## 📝 개발 우선순위

1. ✅ 프로젝트 구조 설계
2. ✅ 사운드 에셋 준비
3. ✅ GridSystem 구현
4. ✅ BlockObject 구현
5. ✅ InputManager 구현
6. ✅ PlacementSystem 구현
7. ✅ StageManager 구현
8. ✅ GridStateManager 구현
9. ✅ BlockSpawner 구현
10. ✅ 로비 씬 구현 (LobbyManager, GameCard, SceneLoader)
11. ⬜ 사운드 통합
12. ⬜ 첫 스테이지 테스트
13. ⬜ 스테이지 3개 완성
