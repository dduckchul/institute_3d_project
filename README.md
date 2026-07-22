# Institute 3D Project

Unity 기반의 3D 실험 프로젝트입니다. 현재는 기본 씬에서 플레이어 점프 동작과 점수 시스템의 기초 스크립트를 구현한 상태입니다.

## 개발 환경

- Unity: 6000.4.1f1
- Render Pipeline: Universal Render Pipeline
- Input: Unity Input System

## 현재 구성

- `Assets/Scenes/SampleScene.unity`
  - 현재 빌드에 등록된 메인 씬입니다.
  - 기본 카메라, 방향 조명, 글로벌 볼륨, 플레이어, 바닥, 큐브 오브젝트가 배치되어 있습니다.

- `Assets/JumpScript.cs`
  - 플레이어 점프 기능을 담당합니다.
  - 스페이스바를 누르면 `Rigidbody` 속도를 이용해 위로 점프합니다.
  - `Ground` 태그가 붙은 오브젝트와 충돌하면 다시 점프할 수 있습니다.

- `Assets/ScoreManager.cs`
  - 전체 점수를 관리하는 싱글톤 스크립트입니다.
  - 점수 추가, 점수 초기화, 점수 변경 이벤트 호출 기능이 있습니다.

- `Assets/EnemyScoreTarget.cs`
  - 특정 태그의 오브젝트와 충돌하거나 트리거에 들어오면 점수를 추가하는 타겟 스크립트입니다.
  - 기본 충돌 태그는 `Bullet`입니다.
  - 설정에 따라 타겟 또는 충돌한 오브젝트를 제거할 수 있습니다.

- `Assets/NewMonoBehaviourScript.cs`
  - 시작 시 `Hello, World!` 로그를 출력하는 테스트용 기본 스크립트입니다.

## 현재 동작

1. `SampleScene`을 실행합니다.
2. 플레이어 오브젝트에서 스페이스바 입력으로 점프할 수 있습니다.
3. 바닥 오브젝트는 `Ground` 태그를 가지고 있어 착지 판정에 사용됩니다.

## 아직 연결이 필요한 부분

- `ScoreManager`와 `EnemyScoreTarget` 스크립트는 구현되어 있지만, 현재 씬에 완전히 연결된 상태는 아닙니다.
- 점수 UI, 적 오브젝트, 총알 또는 충돌 오브젝트 구성이 추가되면 점수 시스템을 실제 플레이 흐름에 붙일 수 있습니다.
- `JumpScript.cs`의 한글 주석은 인코딩이 깨져 있어 정리가 필요합니다.

## 실행 방법

1. Unity Hub에서 이 프로젝트 폴더를 엽니다.
2. Unity `6000.4.1f1` 또는 호환 가능한 Unity 6 버전으로 프로젝트를 실행합니다.
3. `Assets/Scenes/SampleScene.unity`를 엽니다.
4. Play 버튼을 누른 뒤 스페이스바로 점프 동작을 확인합니다.
