# 빠른 시작 가이드

## 개발자를 위해

### 프로젝트 열기

1. **Unity Hub 설치** (아직 설치하지 않았다면)
   - 다운로드: https://unity.com/download

2. **Unity 2022.3 LTS 설치**
   - Unity Hub 열기
   - Installs 탭으로 이동
   - "Install Editor" 클릭
   - Unity 2022.3 LTS 선택
   - Android Build Support 및 iOS Build Support 포함

3. **프로젝트 열기**
   - Unity Hub에서 "Add" 클릭
   - 프로젝트 폴더로 이동
   - 이 README가 포함된 폴더 선택
   - "Open" 클릭

### 프로젝트 이해하기

#### 시작할 주요 파일

1. **Assets/Scripts/Managers/GameManager.cs**
   - 게임의 메인 진입점
   - 플레이어 데이터 및 게임 상태 처리
   - 전체 흐름을 이해하기 좋은 곳

2. **Assets/Scripts/Systems/BattleSystem.cs**
   - 핵심 전투 메커니즘
   - 턴제 전투 로직
   - AI 구현

3. **Assets/StreamingAssets/Data/cards.json**
   - 샘플 카드 정의
   - 여기에 새로운 카드를 쉽게 추가할 수 있음

#### 시스템 테스트

Unity Editor에서 시스템을 테스트하려면:

1. **테스트 씬 생성**
   - 새 씬 생성: File → New Scene
   - "GameManager"라는 이름의 빈 GameObject 추가
   - GameManager.cs 컴포넌트 연결

2. **카드 시스템 테스트**
   ```csharp
   // 임의의 MonoBehaviour에서:
   void Start()
   {
       CardData card = CardSystem.Instance.CreateCard("card_001", 1);
       Debug.Log($"카드 생성: {card.name}");
   }
   ```

3. **전투 시스템 테스트**
   ```csharp
   // 간단한 전투 생성:
   BattleData battle = new BattleData();
   // ... 전투 데이터 설정
   BattleSystem.Instance.StartBattle(battle);
   ```

### 프로젝트 구조 한눈에 보기

```
Assets/
├── Scripts/
│   ├── Data/           → 데이터 구조 (먼저 읽기)
│   ├── Systems/        → 게임 로직 (두 번째로 읽기)
│   ├── Managers/       → 모든 것을 제어하는 싱글톤
│   ├── UI/             → 사용자 인터페이스 컨트롤러
│   └── Utils/          → 헬퍼 함수 및 상수
├── StreamingAssets/
│   └── Data/           → JSON 파일 (가장 쉽게 편집 가능)
├── Scenes/             → Unity 씬
└── Prefabs/            → 재사용 가능한 게임 오브젝트 (생성 예정)
```

### 일반적인 작업

#### 새로운 카드 추가

1. `Assets/StreamingAssets/Data/cards.json` 열기
2. 기존 카드 항목 복사
3. ID 및 속성 변경
4. 파일 저장
5. 이제 게임에서 카드를 사용할 수 있습니다!

#### 새로운 스킬 추가

1. `Assets/StreamingAssets/Data/skills.json` 열기
2. 새 스킬 항목 추가
3. 카드의 `skillIds` 배열에서 스킬 ID 참조

#### 새로운 퀘스트 생성

1. `Assets/StreamingAssets/Data/quests.json` 열기
2. 목표와 함께 퀘스트 추가
3. 보상 설정
4. 완료!

### 빌드 및 테스트

#### Editor에서 빠른 테스트
- Unity Editor에서 Play 버튼 누르기
- 오류가 있는지 Console 확인
- 시스템이 자동으로 초기화됨

#### Android용 빌드
1. File → Build Settings
2. Android 선택
3. "Build" 클릭
4. APK 생성 대기

#### iOS용 빌드
1. File → Build Settings
2. iOS 선택
3. "Build" 클릭
4. 생성된 Xcode 프로젝트 열기
5. Xcode에서 빌드

### 디버깅 팁

1. **Debug.Log를 많이 사용하세요**
   ```csharp
   Debug.Log("전투 시작!");
   Debug.LogWarning("체력이 낮습니다!");
   Debug.LogError("문제가 발생했습니다!");
   ```

2. **Console 창 확인**
   - Window → General → Console
   - 모든 디버그 메시지와 오류 표시

3. **Unity Profiler 사용**
   - Window → Analysis → Profiler
   - 실시간 성능 모니터링

4. **Visual Studio에서 중단점**
   - Unity 디버거 연결
   - 코드에 중단점 설정
   - 실행 단계별로 진행

### 구현된 것 vs UI가 필요한 것

#### ✅ 완전히 구현됨 (백엔드)
- 모든 메커니즘을 가진 카드 시스템
- AI가 있는 전투 시스템
- 가챠 시스템
- 퀘스트 시스템
- 방치 시스템
- 탐험
- 이벤트
- 저장/로드

#### 🎨 구현 필요 (프론트엔드)
- UI 화면
- 비주얼 카드 디스플레이
- 전투 애니메이션
- 터치 컨트롤
- 메뉴 및 네비게이션
- 효과 및 파티클
- 효과음
- 음악

### 개발을 위한 다음 단계

1. **UI 화면 생성**
   - 메인 메뉴
   - 카드 컬렉션 뷰
   - 전투 인터페이스
   - 가챠 화면
   - 퀘스트 로그

2. **비주얼 에셋 추가**
   - 카드 아트워크
   - 스킬 효과
   - 전투 그리드 비주얼
   - 캐릭터 스프라이트

3. **터치 컨트롤 구현**
   - 카드를 위한 드래그 앤 드롭
   - 선택을 위한 탭
   - 네비게이션을 위한 스와이프

4. **다듬기 추가**
   - 애니메이션
   - 전환
   - 파티클 효과
   - 사운드 디자인

### 도움 받기

- **문서**: README.md 및 SETUP_GUIDE.md 참조
- **구현 세부사항**: IMPLEMENTATION_SUMMARY.md 참조
- **코드 주석**: 대부분의 클래스에 인라인 문서 있음
- **Unity 매뉴얼**: https://docs.unity3d.com/Manual/index.html

### 중요 사항

⚠️ **코딩을 시작하기 전에**
- 아키텍처를 이해하기 위해 기존 코드를 읽어보세요
- 기존 패턴을 따르세요 (관리자를 위한 싱글톤 등)
- 데이터를 로직과 분리하세요 (JSON 파일 사용)
- Unity Editor에서 자주 테스트하세요

⚠️ **모바일 개발**
- 항상 실제 기기에서 테스트하세요
- Profiler로 성능 모니터링
- 60 FPS 목표를 염두에 두세요
- 터치 입력에 최적화하세요

⚠️ **버전 관리**
- 자주 커밋하세요
- 설명적인 커밋 메시지를 작성하세요
- 생성된 파일 커밋 안 함 (Library, Temp 등)
- .gitignore가 이미 설정되어 있음

### 빠른 참조

#### 싱글톤 액세스 패턴
```csharp
GameManager.Instance.GetPlayerData();
CardSystem.Instance.CreateCard("card_001", 1);
BattleSystem.Instance.StartBattle(battleData);
```

#### 데이터 로딩
```csharp
// 데이터는 게임 시작 시 자동으로 로드됨
CardData template = DataManager.Instance.GetCardTemplate("card_001");
```

#### 저장/로드
```csharp
// 일시정지/종료 시 자동 저장
// 수동 저장:
GameManager.Instance.SavePlayerData();

// 수동 로드:
GameManager.Instance.LoadPlayerData();
```

### 문제 해결

**문제**: 스크립트가 컴파일되지 않음
- **해결**: Console에서 오류 확인, 구문 문제 수정

**문제**: 데이터가 로드되지 않음
- **해결**: JSON 파일이 StreamingAssets/Data 폴더에 있는지 확인

**문제**: 씬이 로드되지 않음
- **해결**: Build Settings → Scenes in Build 확인

**문제**: 빌드 실패
- **해결**: 빌드 캐시 정리, Unity 재시작, 다시 시도

## 즐거운 코딩! 🎮

코드를 탐색하고, Unity Editor에서 프로젝트를 실행하고, JSON 데이터 파일을 실험하면서 시작하세요. 모든 시스템이 작동하며 그 위에 UI를 구축할 준비가 되어 있습니다!
