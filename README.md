# 🧙‍♂️ TextRPG - 무한의 탑 오르기

정말 간단하게 만든 콘솔 기반 턴제 텍스트 RPG 게임입니다.  
플레이어는 몬스터를 쓰러뜨리며 무한히 탑을 올라가고, 전투 후에는 **랜덤 상자** 또는 **휴식** 중 하나를 선택할 수 있습니다.  

---

## 🎮 게임 내용

1. 게임 시작 시 플레이어가 생성됩니다.
2. 매 층마다 몬스터와 1:1 전투를 진행합니다.
3. 전투가 끝나면 다음 행동을 선택할 수 있습니다:
   - **상자 열기**: 일정 확률로 무기 / 방어구 획득
   - **휴식하기**: 체력 전부 회복
4. 레벨은 처치한 몬스터 수에 따라 자동으로 상승하며, 능력치가 증가합니다.
5. 죽으면 게임은 종료됩니다.
6. 
---

## 📜 주요 기능

- **전투 시스템**  
  - 공격 or 방어(확률 기반 방어 및 반격 가능)

- **레벨업 시스템**  
  - 일정 킬 수 도달 시 레벨업 + 능력치 상승

- **랜덤 보상 시스템**  
  - 전투 후 무기/방어구 or 아무것도 없음!

---

## 🛠️ 사용 기술

- C# (.NET Core / Console Application)
- 객체지향 프로그래밍
  - `Player`, `Enemy`, `Game`, `InputHandler` 클래스로 구성

---
