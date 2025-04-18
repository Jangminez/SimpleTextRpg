using System;

namespace TextRpg
{
    public static class InputHandler
    {
        // 자주쓰이는 string 상수로 저장
        public const string CHOICE_ACTION = "원하시는 행동을 선택하세요.";
        public const string WRONG_INPUT = "잘못된 입력입니다. 다시 입력해주세요.";

        // 플레이어 응답 대기 메서드 (행동 선택)
        public static int InputValidator(string options, int min, int max)
        {
            while (true)
            {
                Console.Write($"\n{options}\n\n{CHOICE_ACTION}\n>> ");
                if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                    return value;

                Console.WriteLine($"{WRONG_INPUT}\n");
            }
        }

        // 플레이어 응답 대기 메서드 (이름 입력)
        public static string InputValidator(int minLength, int maxLength)
        {
            while (true)
            {
                Console.Write($"\n원하시는 이름을 입력해주세요.(길이{minLength} ~ {maxLength})\n>> ");
                string inputName = Console.ReadLine();

                // 이름 길이가 조건에 맞다면 return
                if (inputName.Length >= minLength && inputName.Length <= maxLength) return inputName;

                Console.WriteLine($"{WRONG_INPUT}");
            }
        }
    }

    public class Player
    {
        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public int Atk { get; private set; }
        public int Def { get; private set; }
        public int Level { get; private set; }
        public bool IsDefend { get; private set; }
        private int kills;
        public Player(int maxHp, int atk, int def)
        {
            MaxHp = maxHp;
            Hp = MaxHp;
            Atk = atk;
            Def = def;
            Level = 1;
        }

        // 공격
        public void Attack(Enemy enemy)
        {
            Console.WriteLine("\n플레이어의 공격!");
            enemy.TakeDamage(Atk);

            Thread.Sleep(500);
        }

        // 방어 키고 끄기
        public void ToggleDefend()
        {
            IsDefend = !IsDefend;
        }

        // 20% 확률로 보상 X 80% 확률로 무기 or 방어구 획득
        public void OpenChest(int floor)
        {
            Random rand = new Random();
            int randomNum = rand.Next(100);
            int incValue = 5 + (floor / 2);

            Console.WriteLine("\n상자를 조심스럽게 열어봅니다.");
            Thread.Sleep(1000);

            if (randomNum <= 20)
            {
                Console.WriteLine("아무 것도 발견하지 못했습니다....");
            }
            else if (randomNum <= 60)
            {
                Console.WriteLine("\n쓸만한 무기를 발견했습니다!!");
                Console.WriteLine($"공격력 증가 +{incValue}");
                Atk += incValue;
            }
            else
            {
                Console.WriteLine("\n괜찮은 방어구를 발견했습니다!!");
                Console.WriteLine($"방어력 증가 +{incValue}");
                Def += incValue;
            }
            Thread.Sleep(1000);
        }

        // 휴식하기
        public void Rest()
        {
            Console.WriteLine("\n플레이어가 지친 몸을 내려놓고 쉽니다...");
            Thread.Sleep(1000);
            Console.WriteLine("체력이 전부 회복되었습니다!");

            Hp = MaxHp; // 체력 회복

            Thread.Sleep(1000);
        }
        public void TakeDamage(int damage, Enemy enemy)
        {
            Random rand = new Random();

            // 70% 확률로 방어
            if (IsDefend && rand.Next(100) <= 70)
            {
                // 공격 방어 후 방어 해제
                Console.WriteLine("\n플레이어의 방어!");
                ToggleDefend();
                Thread.Sleep(500);

                if (rand.Next(100) <= 30)
                {
                    Console.WriteLine("\n플레이어가 반격합니다!!!");
                    Attack(enemy);
                }
            }
            else
            {
                int finalDamage = damage - Def < 0 ? 1 : damage - Def;
                Hp -= finalDamage;

                Console.WriteLine($"\n{finalDamage} 만큼의 피해를 입습니다.");
                Thread.Sleep(500);
            }
        }

        // 킬 수 지정
        public void SetKills()
        {
            kills++;

            if (Level == kills)
                LevelUp();
        }

        // 레벨 업 스탯 증가
        private void LevelUp()
        {
            Console.WriteLine($"\n레벨업 하셨습니다!!!! \n\n현재 레벨: Lv. {Level} -> {Level + 1}");
            Console.WriteLine($"공격력 {Atk} -> {Atk + 5}, 방어력 {Def} -> {Def + 3}");
            Level++;
            kills = 0;

            Atk += 5;
            Def += 3;

            Thread.Sleep(1000);
        }
    }

    // 적 클래스
    public class Enemy
    {
        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public int Atk { get; private set; }
        public int Def { get; private set; }
        private int avoidStat;
        Random rand = new Random();

        public Enemy(int floor)
        {
            MaxHp = 20 + (floor * 10);
            Hp = MaxHp;
            Atk = 15 + (floor * 8);
            Def = 8 + (floor * 3);
            avoidStat = 0 + (floor / 2);
        }

        // 공격
        public void Attack(Player player)
        {
            Console.WriteLine("\n몬스터가 공격!");
            player.TakeDamage(Atk, this);

            Thread.Sleep(500);
        }

        // 데미지 받기
        public void TakeDamage(int damage)
        {
            // (3 + 회피율) % 확률로 회피
            if (rand.Next(100) <= 3 + avoidStat)
            {
                Console.WriteLine("\n공격이 빗나갔습니다!!");
                Thread.Sleep(500);
            }

            else
            {
                int finalDamage = damage - Def < 0 ? 1 : damage - Def;
                Hp -= finalDamage;

                Console.WriteLine($"\n입힌 피해: {finalDamage}");
                Thread.Sleep(500);
            }
        }
    }

    public class Game
    {
        public int Floor { private set; get; }
        Player player;

        public void StartGame()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("무한의 탑 오르기에 오신 여러분을 환영합니다!");

                int choice = InputHandler.InputValidator("1. 시작하기\n0. 종료하기", 0, 1);

                if (choice == 1)
                    MainLoop();
                else
                {
                    ExitGame();
                    return;
                }
            }
        }

        private void ExitGame()
        {
            Console.WriteLine("\n게임이 종료됩니다...");
            Thread.Sleep(1000);
        }

        private void BattlePage(Enemy enemy)
        {
            Console.WriteLine($"현재 위치는 {Floor}층 입니다.\n");

            Console.WriteLine("┌────────────────────┐");
            Console.WriteLine("│       Player       │");
            Console.WriteLine($"│ HP : {player.Hp,-3} / {player.MaxHp,-8}│");
            Console.WriteLine($"│ ATK: {player.Atk,-3} | DEF: {player.Def,-2} │");
            Console.WriteLine("└────────────────────┘");
            Console.WriteLine();
            Console.WriteLine("         VS");
            Console.WriteLine();
            Console.WriteLine("┌────────────────────┐");
            Console.WriteLine($"│     {"Monster",-15}│");
            Console.WriteLine($"│ HP : {enemy.Hp,-3} / {enemy.MaxHp,-8}│");
            Console.WriteLine($"│ ATK: {enemy.Atk,-3} | DEF: {enemy.Def,-2} │");
            Console.WriteLine("└────────────────────┘");
        }

        private void MainLoop()
        {
            player = new Player(100, 30, 10);
            Floor = 1;

            while (true)
            {
                Enemy enemy = new Enemy(Floor);
                bool isInBattle = true;

                while (isInBattle)
                {
                    Console.Clear();
                    BattlePage(enemy);

                    int choice = InputHandler.InputValidator("1. 공격하기\n2. 방어하기(70% 확률로 방어, 방어 후 30% 확률로 반격)\n", 1, 2);

                    // 플레이어 턴
                    if (choice == 1)
                        player.Attack(enemy);
                    else
                        player.ToggleDefend();

                    Thread.Sleep(500);

                    // 적 턴, 적이 죽으면 배틀 종료
                    if (enemy.Hp <= 0)
                    {
                        Console.WriteLine("\n몬스터를 처치했습니다!!");
                        player.SetKills();
                        Floor++;
                        isInBattle = false;
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        enemy.Attack(player);

                        // 반격 당해서 죽는다면
                        if (enemy.Hp <= 0)
                        {
                            Console.WriteLine("\n몬스터를 처치했습니다!!");
                            player.SetKills();
                            Floor++;
                            isInBattle = false;
                            Thread.Sleep(1000);
                        }
                    }

                    // 플레이어 죽으면 게임오버
                    if (player.Hp <= 0)
                    {
                        Console.WriteLine("\n당신은 쓰러졌습니다... 게임 오버");
                        Thread.Sleep(1000);

                        return;
                    }
                }

                EndBattlePage();
            }
        }

        // 전투 종료 페이지, 랜덤 보상 및 휴식 선택
        private void EndBattlePage()
        {
            Console.Clear();
            Console.WriteLine("전투 종료");
            Console.WriteLine("여기서는 아이템을 얻거나 휴식을 할 수 있습니다.");

            int choice = InputHandler.InputValidator("1. 상자 열기 (랜덤 아이템)\n2. 휴식하기", 1, 2);

            if (choice == 1)
            {
                player.OpenChest(Floor);
                return;
            }

            else
            {
                player.Rest();
                return;
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            game.StartGame();
        }
    }
}