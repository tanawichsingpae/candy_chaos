using System.Collections.Generic;
using UnityEngine;

namespace CandyChaos
{
    public sealed partial class CandyGame : MonoBehaviour
    {
        private enum Mode { Title, Story, CharacterSelect, Playing, Paused, Upgrade, ChapterClear, BonusResult, Defeat, Victory, EndingStory }
        private enum HeroType { Marshmallow, Strawberry, Blueberry, Mage, Sniper }
        private enum ShotKind { Standard, Marshmallow, Jam, BlueberryIce, EnemyJam, EnemyCaramel, EnemyFrost, EnemyMint }
        private enum EnemyType { Pink, Purple, Cupcake, Tank, Mint, Lollipop, Donut, Cookie, Macaron, IceCream, Gummy,
            BossJelly, BossCake, BossKing }
        private enum DropType { Sugar, Coin, Heart, Crystal }
        private enum UpgradeType { Damage, FireRate, Health, Speed, DoubleShot, Pierce, Magnet, Heal, Ice, Fire, Poison, Ricochet,
            BlastRadius, JamDuration, FrostPower, ProjectileSpeed }

        private sealed class Enemy
        {
            public SpriteRenderer sprite;
            public EnemyType type;
            public Vector2 position;
            public float hp, maxHp, speed, radius, attackTimer, dashTimer, contactTimer, age, flashTimer;
            public float slowTimer, burnTimer, poisonTimer, burnDps, poisonDps;
            public float slowFactor;
            public Color baseColor;
            public SpriteRenderer aura;
            public bool boss, elite, child;
        }

        private sealed class Bullet
        {
            public SpriteRenderer sprite;
            public Vector2 position, velocity;
            public float damage, life, radius;
            public int pierce;
            public int bounces;
            public bool ice, fire, poison;
            public ShotKind kind;
            public float blastRadius, chillDuration, chillFactor;
            public bool special;
            public readonly List<Enemy> hitEnemies = new List<Enemy>();
            public bool hostile;
            public bool slowsPlayer;
        }

        private sealed class Drop
        {
            public SpriteRenderer sprite;
            public Vector2 position;
            public DropType type;
            public int value;
            public float age;
        }

        private sealed class Spark
        {
            public SpriteRenderer sprite;
            public Vector2 velocity;
            public float life, total, grow, startScale;
        }

        private sealed class Puddle
        {
            public SpriteRenderer sprite;
            public Vector2 position;
            public float life, total, radius;
        }

        private static readonly string[] ChapterNames = { "สวนเยลลี่", "ทะเลทรายคาราเมล", "ทุ่งหิมะน้ำตาล" };
        private static readonly string[] BossNames = { "ราชาเยลลี่", "ราชินีคัพเค้ก", "จักรพรรดิลูกกวาด" };
        private static readonly EnemyType[] EliteTypes = { EnemyType.Pink, EnemyType.Cupcake, EnemyType.Donut,
            EnemyType.Cookie, EnemyType.Macaron, EnemyType.IceCream, EnemyType.Gummy };
        private static readonly EnemyType[] GardenOpening = { EnemyType.Pink, EnemyType.Pink, EnemyType.Pink, EnemyType.Cupcake };
        private static readonly EnemyType[] GardenMid = { EnemyType.Pink, EnemyType.Purple, EnemyType.Cupcake,
            EnemyType.Cupcake, EnemyType.Donut, EnemyType.Cookie };
        private static readonly EnemyType[] GardenLate = { EnemyType.Pink, EnemyType.Purple, EnemyType.Cupcake,
            EnemyType.Donut, EnemyType.Cookie, EnemyType.Mint, EnemyType.Tank };
        private static readonly EnemyType[] DesertOpening = { EnemyType.Pink, EnemyType.Purple, EnemyType.Cupcake,
            EnemyType.Donut, EnemyType.Cookie, EnemyType.Mint, EnemyType.Tank };
        private static readonly EnemyType[] DesertMid = { EnemyType.Pink, EnemyType.Cupcake, EnemyType.Donut,
            EnemyType.Cookie, EnemyType.Mint, EnemyType.Tank, EnemyType.Lollipop, EnemyType.Macaron, EnemyType.Gummy };
        private static readonly EnemyType[] DesertLate = { EnemyType.Pink, EnemyType.Cupcake, EnemyType.Donut,
            EnemyType.Cookie, EnemyType.Mint, EnemyType.Tank, EnemyType.Lollipop, EnemyType.Macaron,
            EnemyType.Gummy, EnemyType.IceCream };
        private static readonly EnemyType[] SnowOpening = { EnemyType.Pink, EnemyType.Purple, EnemyType.Cupcake,
            EnemyType.Donut, EnemyType.Cookie, EnemyType.Mint };
        private static readonly EnemyType[] SnowMid = { EnemyType.Pink, EnemyType.Purple, EnemyType.Cupcake,
            EnemyType.Donut, EnemyType.Cookie, EnemyType.Mint, EnemyType.Tank, EnemyType.Lollipop, EnemyType.Macaron };
        private static readonly EnemyType[] SnowEnemies = { EnemyType.Pink, EnemyType.Purple, EnemyType.Cupcake,
            EnemyType.Tank, EnemyType.Mint, EnemyType.Lollipop, EnemyType.Donut, EnemyType.Cookie,
            EnemyType.Macaron, EnemyType.IceCream, EnemyType.Gummy };

        private sealed class StageCheckpoint
        {
            public HeroType hero;
            public bool endless, bonus, ice, fire, poison;
            public int chapter, level, sugar, sugarGoal, runCoins, score, kills;
            public int spread, pierce, ricochet, blastRadius, jamDuration, frostPower, projectileSpeed;
            public float hp, maxHp, damage, fireInterval, moveSpeed, magnetRange, abilityCooldown;
        }

        private readonly List<Enemy> enemies = new List<Enemy>();
        private readonly List<Bullet> bullets = new List<Bullet>();
        private readonly List<Drop> drops = new List<Drop>();
        private readonly List<Spark> sparks = new List<Spark>();
        private readonly List<Puddle> puddles = new List<Puddle>();
        private readonly List<SpriteRenderer> bonusProps = new List<SpriteRenderer>();
        private readonly List<SpriteRenderer> stageDecor = new List<SpriteRenderer>();
        private Camera gameCamera;
        private SpriteRenderer floorRenderer, heroRenderer, abilityAura;
        private Transform worldRoot;
        private CandyAudio audioPlayer;
        private Mode mode = Mode.Title;
        private HeroType selectedHero = HeroType.Marshmallow;
        private bool endless, pendingEndless, bossSpawned, chapterClearPending, manualAim, movingTouch, bonusStage, bonusWon;
        private int chapter, level, sugar, sugarGoal, runCoins, score, kills, wallet, bestScore;
        private int eliteWave;
        private int bonusCrystals;
        private int permanentHealth, permanentDamage, permanentSpeed;
        private int spread, pierce, ricochet;
        private int blastRadiusLevel, jamDurationLevel, frostPowerLevel, projectileSpeedLevel;
        private bool iceShots, fireShots, poisonShots;
        private float stageClock, spawnClock, eliteClock, fireClock, invincibleClock, playerSlowTimer, bannerClock, abilityCooldown, abilityTime;
        private string eliteBanner = "";
        private float hp, maxHp, damage, fireInterval, moveSpeed, magnetRange;
        private Vector2 playerPosition, moveInput, aimDirection, leftAnchor, leftCurrent, rightAnchor, rightCurrent;
        private int leftFinger = -1, rightFinger = -1;
        private UpgradeType[] choices = new UpgradeType[3];
        private StageCheckpoint checkpoint;

        private const float ArenaHalfWidth = 10.45f;
        private const float ArenaHalfHeight = 5.75f;
        private const int BonusGoal = 10;
        private const float BonusTime = 60f;
        private const float BossArrivalTime = 48f;
        private const float PreBossBreakTime = 40f;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Launch()
        {
            if (FindAnyObjectByType<CandyGame>() != null) return;
            new GameObject("Candy Chaos Game").AddComponent<CandyGame>();
        }

        private void Awake()
        {
            Application.targetFrameRate = 60;
            if (Application.isMobilePlatform && Application.platform != RuntimePlatform.WebGLPlayer)
            {
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.orientation = ScreenOrientation.AutoRotation;
            }
            CandyVisuals.Load();
            GameObject root = new GameObject("Candy World");
            worldRoot = root.transform;
            gameCamera = Camera.main;
            if (gameCamera == null)
            {
                GameObject cameraObject = new GameObject("Main Camera");
                cameraObject.tag = "MainCamera";
                gameCamera = cameraObject.AddComponent<Camera>();
                cameraObject.AddComponent<AudioListener>();
            }
            gameCamera.orthographic = true;
            gameCamera.orthographicSize = 5.9f;
            gameCamera.backgroundColor = new Color32(242, 210, 225, 255);
            gameCamera.transform.position = new Vector3(0, 0, -10);
            floorRenderer = CandyVisuals.Create("Frosting floor", CandyVisuals.Floor, Vector2.zero, 1f, -20, worldRoot);
            CandyVisuals.Create("Floor shadow", CandyVisuals.Dot, new Vector2(0, -5.98f), 24f, -21, worldRoot).color = new Color(0.74f, 0.54f, 0.68f, 0.28f);
            heroRenderer = CandyVisuals.Create("Candy hero", CandyVisuals.Hero, Vector2.zero, 0.92f, 10, worldRoot);
            heroRenderer.gameObject.SetActive(false);
            abilityAura = CandyVisuals.Create("Ability aura", CandyVisuals.Ring, Vector2.zero, 1f, 9, worldRoot);
            abilityAura.gameObject.SetActive(false);
            audioPlayer = new CandyAudio(gameObject);
            LoadSave();
            BuildDecorations(0);
        }

        private void BuildDecorations(int theme)
        {
            foreach (SpriteRenderer item in stageDecor) if (item != null) Destroy(item.gameObject);
            stageDecor.Clear();
            Random.State previous = Random.state;
            Random.InitState(14851 + theme * 571);
            Sprite main = theme == 0 ? CandyVisuals.GardenTree : theme == 1 ? CandyVisuals.DesertRock : CandyVisuals.SnowTree;
            Sprite secondary = theme == 0 ? CandyVisuals.GardenMushroom : theme == 1 ? CandyVisuals.DesertCactus : CandyVisuals.SnowDrift;
            for (int i = 0; i < 38; i++)
            {
                Vector2 at = new Vector2(Random.Range(-ArenaHalfWidth + 0.6f, ArenaHalfWidth - 0.6f), Random.Range(-ArenaHalfHeight + 0.5f, ArenaHalfHeight - 0.5f));
                if (at.sqrMagnitude < 5f) continue;
                Sprite sprite = i % 3 == 0 ? secondary : main;
                if (sprite == null) continue;
                SpriteRenderer prop = CandyVisuals.Create("Candy scenery", sprite, at, Random.Range(0.54f, 0.92f), -9, worldRoot);
                prop.color = new Color(1f, 1f, 1f, 0.84f);
                stageDecor.Add(prop);
            }
            for (int i = 0; i < 115; i++)
            {
                Vector2 at = new Vector2(Random.Range(-ArenaHalfWidth, ArenaHalfWidth), Random.Range(-ArenaHalfHeight, ArenaHalfHeight));
                float size = Random.Range(0.06f, 0.15f);
                SpriteRenderer sprinkle = CandyVisuals.Create("Ground sparkle", i % 5 == 0 ? CandyVisuals.Star : CandyVisuals.Dot, at, size, -10, worldRoot);
                sprinkle.color = theme == 0 ? new[] { new Color32(255, 145, 186, 255), new Color32(255, 225, 124, 255), new Color32(126, 212, 175, 255) }[i % 3] :
                                 theme == 1 ? new[] { new Color32(255, 248, 189, 255), new Color32(226, 156, 113, 255), new Color32(255, 203, 135, 255) }[i % 3] :
                                 new[] { new Color32(255, 255, 255, 255), new Color32(143, 209, 255, 255), new Color32(193, 178, 242, 255) }[i % 3];
                sprinkle.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                stageDecor.Add(sprinkle);
            }
            Random.state = previous;
        }

        private void LoadSave()
        {
            wallet = PlayerPrefs.GetInt("cc_wallet", 0);
            bestScore = PlayerPrefs.GetInt("cc_best", 0);
            permanentHealth = PlayerPrefs.GetInt("cc_health", 0);
            permanentDamage = PlayerPrefs.GetInt("cc_damage", 0);
            permanentSpeed = PlayerPrefs.GetInt("cc_speed", 0);
        }

        private void Save()
        {
            PlayerPrefs.SetInt("cc_wallet", wallet);
            PlayerPrefs.SetInt("cc_best", bestScore);
            PlayerPrefs.SetInt("cc_health", permanentHealth);
            PlayerPrefs.SetInt("cc_damage", permanentDamage);
            PlayerPrefs.SetInt("cc_speed", permanentSpeed);
            PlayerPrefs.Save();
        }

        private void StartRun(bool endlessMode)
        {
            endless = endlessMode;
            bonusStage = false;
            chapter = 0;
            level = 1;
            sugar = 0;
            sugarGoal = 10;
            score = 0;
            kills = 0;
            runCoins = 0;
            maxHp = (selectedHero == HeroType.Blueberry ? 130 : selectedHero == HeroType.Strawberry ? 90 :
                     selectedHero == HeroType.Mage ? 85 : selectedHero == HeroType.Sniper ? 95 : 100) + permanentHealth * 18;
            hp = maxHp;
            damage = (selectedHero == HeroType.Mage ? 8 : selectedHero == HeroType.Sniper ? 42 :
                     selectedHero == HeroType.Marshmallow ? 18 : 12) + permanentDamage * 2.5f;
            fireInterval = selectedHero == HeroType.Strawberry ? 0.33f : selectedHero == HeroType.Marshmallow ? 0.46f :
                           selectedHero == HeroType.Mage ? 0.47f : selectedHero == HeroType.Sniper ? 1.05f : 0.32f;
            moveSpeed = (selectedHero == HeroType.Blueberry ? 4.8f : selectedHero == HeroType.Sniper ? 4.65f : 5.1f) + permanentSpeed * 0.25f;
            magnetRange = 1.35f;
            spread = 1;
            pierce = 0;
            ricochet = 0;
            blastRadiusLevel = jamDurationLevel = frostPowerLevel = projectileSpeedLevel = 0;
            iceShots = fireShots = poisonShots = false;
            abilityCooldown = 0;
            abilityTime = 0;
            playerSlowTimer = 0;
            heroRenderer.sprite = selectedHero == HeroType.Strawberry ? CandyVisuals.StrawberryHero :
                                  selectedHero == HeroType.Blueberry ? CandyVisuals.BlueberryHero :
                                  selectedHero == HeroType.Mage ? CandyVisuals.MageHero :
                                  selectedHero == HeroType.Sniper ? CandyVisuals.SniperHero : CandyVisuals.Hero;
            heroRenderer.gameObject.SetActive(true);
            BeginChapter();
        }

        private void BeginChapter(bool retry = false)
        {
            bonusStage = false;
            ClearEntities();
            playerPosition = Vector2.zero;
            heroRenderer.transform.position = playerPosition;
            if (!retry) hp = Mathf.Min(maxHp, hp + maxHp * 0.45f);
            stageClock = 0;
            spawnClock = chapter == 0 ? 3f : 2.6f;
            eliteClock = chapter == 0 ? 30f : chapter == 1 ? 25f : 19f;
            eliteWave = 0;
            eliteBanner = "";
            fireClock = 0;
            invincibleClock = 2f;
            bannerClock = 3.5f;
            bossSpawned = false;
            chapterClearPending = false;
            floorRenderer.sprite = CandyVisuals.FloorForChapter(chapter);
            floorRenderer.color = Color.white;
            BuildDecorations(chapter % 3);
            mode = Mode.Playing;
            gameCamera.transform.position = new Vector3(0, 0, -10);
            CaptureStageCheckpoint();
        }

        private void BeginBonus()
        {
            bonusStage = true;
            bonusWon = false;
            bonusCrystals = 0;
            ClearEntities();
            playerPosition = Vector2.zero;
            heroRenderer.transform.position = playerPosition;
            stageClock = 0;
            spawnClock = 1.2f;
            eliteClock = 100f;
            eliteBanner = "";
            fireClock = 0;
            invincibleClock = 2f;
            bannerClock = 4f;
            bossSpawned = false;
            chapterClearPending = false;
            floorRenderer.sprite = CandyVisuals.FloorForChapter(2);
            floorRenderer.color = new Color(0.94f, 0.88f, 1f);
            BuildDecorations(2);
            if (CandyVisuals.PortalProp != null)
            {
                bonusProps.Add(CandyVisuals.Create("Frosting portal", CandyVisuals.PortalProp, new Vector2(0, 4.2f), 2.0f, -8, worldRoot));
                bonusProps.Add(CandyVisuals.Create("Sugar crystals", CandyVisuals.CrystalProp, new Vector2(-8.8f, 3.8f), 1.45f, -8, worldRoot));
                bonusProps.Add(CandyVisuals.Create("Candy chest", CandyVisuals.ChestProp, new Vector2(8.6f, -4.2f), 1.6f, -8, worldRoot));
                bonusProps.Add(CandyVisuals.Create("Strawberry heart", CandyVisuals.HeartProp, new Vector2(-8.8f, -4.1f), 1.25f, -8, worldRoot));
            }
            mode = Mode.Playing;
            audioPlayer.BonusStart();
            gameCamera.transform.position = new Vector3(0, 0, -10);
            CaptureStageCheckpoint();
        }

        private void CaptureStageCheckpoint()
        {
            checkpoint = new StageCheckpoint
            {
                hero = selectedHero, endless = endless, bonus = bonusStage, ice = iceShots, fire = fireShots, poison = poisonShots,
                chapter = chapter, level = level, sugar = sugar, sugarGoal = sugarGoal, runCoins = runCoins,
                score = score, kills = kills, spread = spread, pierce = pierce, ricochet = ricochet,
                blastRadius = blastRadiusLevel, jamDuration = jamDurationLevel, frostPower = frostPowerLevel,
                projectileSpeed = projectileSpeedLevel, hp = hp, maxHp = maxHp, damage = damage,
                fireInterval = fireInterval, moveSpeed = moveSpeed, magnetRange = magnetRange,
                abilityCooldown = abilityCooldown
            };
        }

        private void RetryStage()
        {
            if (checkpoint == null) { StartRun(endless); return; }
            StageCheckpoint saved = checkpoint;
            selectedHero = saved.hero;
            endless = saved.endless;
            chapter = saved.chapter;
            level = saved.level;
            sugar = saved.sugar;
            sugarGoal = saved.sugarGoal;
            runCoins = saved.runCoins;
            score = saved.score;
            kills = saved.kills;
            spread = saved.spread;
            pierce = saved.pierce;
            ricochet = saved.ricochet;
            blastRadiusLevel = saved.blastRadius;
            jamDurationLevel = saved.jamDuration;
            frostPowerLevel = saved.frostPower;
            projectileSpeedLevel = saved.projectileSpeed;
            hp = saved.hp;
            maxHp = saved.maxHp;
            damage = saved.damage;
            fireInterval = saved.fireInterval;
            moveSpeed = saved.moveSpeed;
            magnetRange = saved.magnetRange;
            abilityCooldown = saved.abilityCooldown;
            iceShots = saved.ice;
            fireShots = saved.fire;
            poisonShots = saved.poison;
            abilityTime = 0;
            playerSlowTimer = 0;
            heroRenderer.sprite = selectedHero == HeroType.Strawberry ? CandyVisuals.StrawberryHero :
                                  selectedHero == HeroType.Blueberry ? CandyVisuals.BlueberryHero :
                                  selectedHero == HeroType.Mage ? CandyVisuals.MageHero :
                                  selectedHero == HeroType.Sniper ? CandyVisuals.SniperHero : CandyVisuals.Hero;
            heroRenderer.gameObject.SetActive(true);
            if (saved.bonus) BeginBonus(); else BeginChapter(true);
        }

        private void ClearEntities()
        {
            foreach (Enemy item in enemies) if (item.sprite != null) Destroy(item.sprite.gameObject);
            foreach (Enemy item in enemies) if (item.aura != null) Destroy(item.aura.gameObject);
            foreach (Bullet item in bullets) if (item.sprite != null) Destroy(item.sprite.gameObject);
            foreach (Drop item in drops) if (item.sprite != null) Destroy(item.sprite.gameObject);
            foreach (Spark item in sparks) if (item.sprite != null) Destroy(item.sprite.gameObject);
            foreach (Puddle item in puddles) if (item.sprite != null) Destroy(item.sprite.gameObject);
            foreach (SpriteRenderer item in bonusProps) if (item != null) Destroy(item.gameObject);
            enemies.Clear(); bullets.Clear(); drops.Clear(); sparks.Clear(); puddles.Clear();
            bonusProps.Clear();
            abilityAura.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (mode == Mode.Story || mode == Mode.EndingStory)
            {
                UpdateStory();
                return;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (mode == Mode.Playing) mode = Mode.Paused;
                else if (mode == Mode.Paused) mode = Mode.Playing;
            }
            if (mode != Mode.Playing) return;
            float dt = Mathf.Min(Time.deltaTime, 0.05f);
            stageClock += dt;
            spawnClock -= dt;
            eliteClock -= dt;
            fireClock -= dt;
            invincibleClock -= dt;
            playerSlowTimer -= dt;
            bannerClock -= dt;
            abilityCooldown -= dt;
            abilityTime -= dt;
            if (Input.GetKeyDown(KeyCode.Space)) TriggerAbility();
            ReadControls();
            UpdatePlayer(dt);
            UpdateSpawning(dt);
            UpdateEnemies(dt);
            if (mode != Mode.Playing) return;
            UpdateBullets(dt);
            if (mode != Mode.Playing) return;
            UpdatePuddles(dt);
            UpdateDrops(dt);
            UpdateSparks(dt);
            UpdateCamera(dt);
            if (chapterClearPending) CompleteChapter();
            if (bonusStage && mode == Mode.Playing && (bonusCrystals >= BonusGoal || stageClock >= BonusTime)) CompleteBonus();
        }

        private void ReadControls()
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            manualAim = false;
            movingTouch = false;
            float uiScale = Mathf.Min(Screen.width / 1280f, Screen.height / 720f);
            float uiTop = (Screen.height - 720f * uiScale) * 0.5f + 114f * uiScale;
            if (Input.touchCount > 0)
            {
                bool leftSeen = false;
                bool rightSeen = false;
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Began && touch.position.y <= Screen.height - uiTop && !PointerOverAbility(touch.position))
                    {
                        if (touch.position.x < Screen.width * 0.46f && leftFinger < 0)
                        {
                            leftFinger = touch.fingerId;
                            leftAnchor = touch.position;
                        }
                        else if (touch.position.x >= Screen.width * 0.46f && rightFinger < 0)
                        {
                            rightFinger = touch.fingerId;
                            rightAnchor = touch.position;
                        }
                    }
                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) continue;
                    if (touch.fingerId == leftFinger)
                    {
                        leftSeen = true;
                        movingTouch = true;
                        leftCurrent = touch.position;
                        moveInput = Vector2.ClampMagnitude((leftCurrent - leftAnchor) / (Screen.height * 0.15f), 1f);
                    }
                    else if (touch.fingerId == rightFinger)
                    {
                        rightSeen = true;
                        rightCurrent = touch.position;
                        Vector2 drag = rightCurrent - rightAnchor;
                        if (drag.sqrMagnitude > Screen.height * Screen.height * 0.0004f) aimDirection = drag.normalized;
                        else
                        {
                            Vector2 target = gameCamera.ScreenToWorldPoint(touch.position);
                            aimDirection = (target - playerPosition).normalized;
                        }
                        manualAim = aimDirection.sqrMagnitude > 0.01f;
                    }
                }
                if (!leftSeen) { leftFinger = -1; leftAnchor = Vector2.zero; }
                if (!rightSeen) { rightFinger = -1; rightAnchor = Vector2.zero; }
            }
            else
            {
                leftFinger = -1;
                rightFinger = -1;
                leftAnchor = Vector2.zero;
                rightAnchor = Vector2.zero;
                if (Input.GetMouseButton(0) && Input.mousePosition.y < Screen.height - uiTop && !PointerOverAbility(Input.mousePosition))
                {
                    Vector2 target = gameCamera.ScreenToWorldPoint(Input.mousePosition);
                    aimDirection = (target - playerPosition).normalized;
                    manualAim = aimDirection.sqrMagnitude > 0.01f;
                }
            }
            moveInput = Vector2.ClampMagnitude(moveInput, 1f);
        }

        private static bool PointerOverAbility(Vector2 screenPosition)
        {
            float scale = Mathf.Min(Screen.width / 1280f, Screen.height / 720f);
            float offsetX = (Screen.width - 1280f * scale) * 0.5f;
            float offsetY = (Screen.height - 720f * scale) * 0.5f;
            float x = (screenPosition.x - offsetX) / scale;
            float y = (Screen.height - screenPosition.y - offsetY) / scale;
            return x >= 970 && x <= 1228 && y >= 116 && y <= 174;
        }

        private void UpdatePlayer(float dt)
        {
            playerPosition += moveInput * moveSpeed * (playerSlowTimer > 0 ? 0.7f : 1f) * dt;
            playerPosition.x = Mathf.Clamp(playerPosition.x, -ArenaHalfWidth + 0.5f, ArenaHalfWidth - 0.5f);
            playerPosition.y = Mathf.Clamp(playerPosition.y, -ArenaHalfHeight + 0.5f, ArenaHalfHeight - 0.5f);
            heroRenderer.transform.position = new Vector3(playerPosition.x, playerPosition.y, 0);
            heroRenderer.flipX = manualAim ? aimDirection.x < 0 : moveInput.x < -0.05f;
            heroRenderer.color = invincibleClock > 0 && Mathf.Sin(Time.time * 24) > 0 ? new Color(1, 1, 1, 0.55f) : Color.white;
            UpdateAbilityVisual();
            if (fireClock > 0) return;
            Vector2 direction = aimDirection;
            if (!manualAim)
            {
                Enemy nearest = FindNearestEnemy();
                if (nearest == null) return;
                direction = (nearest.position - playerPosition).normalized;
            }
            Fire(direction);
            fireClock = fireInterval;
        }

        private void TriggerAbility()
        {
            if (mode != Mode.Playing || abilityCooldown > 0) return;
            if (selectedHero == HeroType.Marshmallow)
            {
                Enemy target = FindNearestEnemy();
                Vector2 direction = manualAim ? aimDirection : target != null ? (target.position - playerPosition).normalized : Vector2.up;
                SpawnBullet(playerPosition + direction * 0.5f, direction * 9.5f, damage * 2.5f, false,
                    new Color32(255, 220, 156, 255), 0, ShotKind.Marshmallow, true);
                abilityCooldown = 17f;
            }
            else if (selectedHero == HeroType.Strawberry)
            {
                for (int i = 0; i < 12; i++)
                {
                    float angle = 2f * Mathf.PI * i / 12f;
                    Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                    SpawnBullet(playerPosition + direction * 0.42f, direction * 8.4f, damage * 1.3f, false,
                        new Color32(255, 97, 146, 255), 0, ShotKind.Jam, true);
                }
                for (int i = 0; i < 6; i++)
                {
                    float angle = 2f * Mathf.PI * i / 6f;
                    SpawnJamPuddle(playerPosition + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 2.1f, true);
                }
                abilityCooldown = 17f;
            }
            else if (selectedHero == HeroType.Blueberry)
            {
                invincibleClock = Mathf.Max(invincibleClock, 3.5f);
                abilityTime = 3.5f;
                abilityCooldown = 21f;
                for (int i = enemies.Count - 1; i >= 0; i--)
                {
                    Enemy enemy = enemies[i];
                    if ((enemy.position - playerPosition).sqrMagnitude > 4.5f * 4.5f) continue;
                    ApplySlow(enemy, 3.4f + frostPowerLevel * 0.7f, enemy.boss ? 0.62f : 0.25f);
                    enemy.hp -= damage * 1.2f;
                    if (enemy.hp <= 0) KillEnemy(i);
                }
            }
            else if (selectedHero == HeroType.Mage)
            {
                abilityTime = 6f;
                abilityCooldown = 19f;
                for (int i = 0; i < 24; i++)
                {
                    float angle = i * Mathf.PI * 2f / 24f;
                    Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                    SpawnBullet(playerPosition + direction * 0.4f, direction * 8f, damage * 1.1f, false, new Color32(206, 143, 255, 255), 0);
                }
            }
            else
            {
                abilityTime = 6f;
                abilityCooldown = 18f;
                fireClock = 0;
            }
            Color effectColor = selectedHero == HeroType.Blueberry ? new Color32(119, 201, 255, 255) :
                                selectedHero == HeroType.Mage ? new Color32(210, 147, 255, 255) :
                                selectedHero == HeroType.Sniper ? new Color32(255, 220, 108, 255) :
                                new Color32(255, 139, 186, 255);
            AbilityWave(effectColor);
            audioPlayer.Ability((int)selectedHero);
        }

        private void UpdateAbilityVisual()
        {
            bool active = abilityTime > 0 && selectedHero != HeroType.Strawberry && selectedHero != HeroType.Marshmallow;
            abilityAura.gameObject.SetActive(active);
            if (!active) return;
            abilityAura.transform.position = playerPosition;
            abilityAura.transform.localScale = Vector3.one * (selectedHero == HeroType.Blueberry ? 2.45f : 2.05f) * (1f + Mathf.Sin(Time.time * 7f) * 0.08f);
            abilityAura.transform.Rotate(0, 0, 90f * Time.deltaTime);
            abilityAura.color = selectedHero == HeroType.Mage ? new Color(0.79f, 0.49f, 1f, 0.75f) :
                                selectedHero == HeroType.Sniper ? new Color(1f, 0.84f, 0.37f, 0.8f) :
                                selectedHero == HeroType.Blueberry ? new Color(0.39f, 0.8f, 1f, 0.8f) :
                                new Color(1f, 0.82f, 0.35f, 0.8f);
        }

        private void AbilityWave(Color color)
        {
            SpawnRing(playerPosition, color, 3.3f);
            for (int i = 0; i < 10; i++) HitSpark(playerPosition + Random.insideUnitCircle * 0.35f, color);
        }

        private void SpawnRing(Vector2 position, Color color, float size)
        {
            SpriteRenderer visual = CandyVisuals.Create("Sugar shockwave", CandyVisuals.Ring, position, 0.35f, 11, worldRoot);
            visual.color = color;
            sparks.Add(new Spark { sprite = visual, velocity = Vector2.zero, life = 0.65f, total = 0.65f, grow = size, startScale = 0.35f });
        }

        private Enemy FindNearestEnemy()
        {
            Enemy best = null;
            float bestDistance = 75f;
            foreach (Enemy enemy in enemies)
            {
                float sqr = (enemy.position - playerPosition).sqrMagnitude;
                if (sqr < bestDistance) { best = enemy; bestDistance = sqr; }
            }
            return best;
        }

        private void Fire(Vector2 direction)
        {
            if (direction.sqrMagnitude < 0.01f) return;
            ShotKind kind = selectedHero == HeroType.Marshmallow ? ShotKind.Marshmallow :
                            selectedHero == HeroType.Strawberry ? ShotKind.Jam :
                            selectedHero == HeroType.Blueberry ? ShotKind.BlueberryIce : ShotKind.Standard;
            Color32 color = iceShots ? new Color32(109, 214, 255, 255) : fireShots ? new Color32(255, 134, 67, 255) :
                          poisonShots ? new Color32(181, 106, 228, 255) : selectedHero == HeroType.Strawberry ? new Color32(255, 96, 153, 255) :
                          selectedHero == HeroType.Blueberry ? new Color32(116, 165, 255, 255) :
                          selectedHero == HeroType.Mage ? new Color32(198, 126, 255, 255) :
                          selectedHero == HeroType.Sniper ? new Color32(255, 215, 96, 255) : new Color32(80, 187, 252, 255);
            int count = spread + (selectedHero == HeroType.Mage ? 4 : 0);
            for (int i = 0; i < count; i++)
            {
                float angle = (i - (count - 1) * 0.5f) * (selectedHero == HeroType.Mage ? 12f : selectedHero == HeroType.Sniper ? 3f : 11f);
                Vector2 rotated = Quaternion.Euler(0, 0, angle) * direction;
                float speed = selectedHero == HeroType.Sniper ? 20f : selectedHero == HeroType.Mage ? 10f :
                              selectedHero == HeroType.Strawberry ? 8.6f : selectedHero == HeroType.Marshmallow ? 9.8f : 11.5f;
                speed *= 1f + projectileSpeedLevel * 0.18f;
                float power = damage * (selectedHero == HeroType.Sniper && abilityTime > 0 ? 1.85f : 1f);
                SpawnBullet(playerPosition + rotated * 0.48f, rotated * speed, power, false, color,
                    pierce + (selectedHero == HeroType.Sniper ? 1 : 0), kind);
            }
            audioPlayer.Shot((int)selectedHero, iceShots || selectedHero == HeroType.Blueberry, fireShots, poisonShots);
        }

        private void SpawnBullet(Vector2 position, Vector2 velocity, float power, bool hostile, Color color,
            int bulletPierce = 0, ShotKind kind = ShotKind.Standard, bool special = false, bool slowsPlayer = false)
        {
            if (bullets.Count > 190) return;
            Sprite sprite = kind == ShotKind.Marshmallow ? CandyVisuals.MarshmallowShot :
                            kind == ShotKind.Jam ? CandyVisuals.JamShot :
                            kind == ShotKind.BlueberryIce ? CandyVisuals.BlueberryShot :
                            kind == ShotKind.EnemyJam ? CandyVisuals.EnemyJamBolt :
                            kind == ShotKind.EnemyCaramel ? CandyVisuals.EnemyCaramelBolt :
                            kind == ShotKind.EnemyFrost ? CandyVisuals.EnemyFrostBolt :
                            kind == ShotKind.EnemyMint ? CandyVisuals.EnemyMintBolt : CandyVisuals.Dot;
            if (sprite == null) sprite = CandyVisuals.Dot;
            float scale = hostile ? (kind == ShotKind.EnemyFrost ? 0.54f : 0.48f) : kind == ShotKind.Marshmallow ? (special ? 0.9f : 0.56f) :
                          kind == ShotKind.Jam ? 0.43f : kind == ShotKind.BlueberryIce ? 0.42f : 0.28f;
            SpriteRenderer visual = CandyVisuals.Create(hostile ? "Enemy candy shot" : "Candy shot", sprite, position, scale, 8, worldRoot);
            visual.color = kind == ShotKind.Standard ? color : Color.white;
            if (kind != ShotKind.Standard)
                visual.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg);
            bullets.Add(new Bullet { sprite = visual, position = position, velocity = velocity, damage = power, hostile = hostile,
                life = 2.2f, radius = hostile ? 0.18f : kind == ShotKind.Marshmallow ? (special ? 0.43f : 0.3f) :
                    selectedHero == HeroType.Sniper ? 0.2f : 0.18f,
                pierce = bulletPierce, bounces = hostile || kind == ShotKind.Marshmallow ? 0 : ricochet,
                ice = !hostile && (iceShots || kind == ShotKind.BlueberryIce),
                fire = !hostile && fireShots, poison = !hostile && poisonShots,
                kind = kind, special = special,
                slowsPlayer = slowsPlayer,
                blastRadius = kind == ShotKind.Marshmallow ? (special ? 2.2f : 1.1f) * (1f + blastRadiusLevel * 0.22f) : 0,
                chillDuration = 2.5f + frostPowerLevel * 0.6f,
                chillFactor = kind == ShotKind.BlueberryIce ? Mathf.Max(0.25f, 0.45f - frostPowerLevel * 0.07f) : 0.5f });
        }

        private void UpdateSpawning(float dt)
        {
            int eliteLimit = chapter == 0 ? 1 : chapter == 1 ? 1 : 2;
            bool eliteAlive = false;
            foreach (Enemy enemy in enemies) if (enemy.elite) { eliteAlive = true; break; }
            if (!bonusStage && !bossSpawned && stageClock < 37f && eliteWave < eliteLimit &&
                eliteClock <= 0 && !eliteAlive && enemies.Count < 18)
            {
                EnemyType type = EliteTypes[(chapter * 3 + eliteWave) % EliteTypes.Length];
                SpawnEnemyVariant(type, EdgePoint(), true, false);
                eliteWave++;
                eliteClock = chapter == 2 ? 16f : 18f;
                eliteBanner = "ตัวแสบมาแล้ว! " + EliteName(type);
                bannerClock = 2.8f;
                audioPlayer.EliteAlert();
            }
            if (!bonusStage && !bossSpawned && stageClock >= BossArrivalTime)
            {
                SpawnBoss();
                bossSpawned = true;
                spawnClock = 5f;
                bannerClock = 3f;
            }
            if (!bonusStage && !bossSpawned && stageClock >= PreBossBreakTime) return;
            if (!bonusStage && bossSpawned && chapter == 0) return;
            int enemyLimit = bonusStage ? 6 : bossSpawned ? chapter == 1 ? 6 : 8 :
                chapter == 0 ? 14 : chapter == 1 ? 19 : 24;
            if (enemies.Count >= enemyLimit)
            {
                spawnClock = Mathf.Max(spawnClock, 0.75f);
                return;
            }
            if (spawnClock > 0) return;
            spawnClock = bonusStage ? 1.8f : bossSpawned ? chapter == 1 ? 5.4f : 4.8f :
                Mathf.Max(1.85f, 2.65f - stageClock * 0.009f - chapter * 0.1f);
            int count = bonusStage || bossSpawned ? 1 :
                1 + (stageClock >= (chapter == 0 ? 32f : chapter == 1 ? 29f : 24f) ? 1 : 0);
            count = Mathf.Min(count, enemyLimit - enemies.Count);
            for (int i = 0; i < count; i++)
            {
                EnemyType kind = bonusStage ? (Random.value < 0.55f ? EnemyType.Mint : EnemyType.Lollipop) : ChooseEnemyType();
                SpawnEnemy(kind, EdgePoint());
            }
        }

        private EnemyType ChooseEnemyType()
        {
            EnemyType[] pool = chapter == 0 ? stageClock < 17f ? GardenOpening :
                stageClock < 32f ? GardenMid : GardenLate :
                chapter == 1 ? stageClock < 20f ? DesertOpening :
                stageClock < 34f ? DesertMid : DesertLate :
                stageClock < 14f ? SnowOpening : stageClock < 30f ? SnowMid : SnowEnemies;
            return pool[Random.Range(0, pool.Length)];
        }

        private Vector2 EdgePoint()
        {
            for (int attempt = 0; attempt < 8; attempt++)
            {
                Vector2 point;
                int side = Random.Range(0, 4);
                if (side == 0) point = new Vector2(-ArenaHalfWidth + 0.35f, Random.Range(-ArenaHalfHeight, ArenaHalfHeight));
                else if (side == 1) point = new Vector2(ArenaHalfWidth - 0.35f, Random.Range(-ArenaHalfHeight, ArenaHalfHeight));
                else if (side == 2) point = new Vector2(Random.Range(-ArenaHalfWidth, ArenaHalfWidth), -ArenaHalfHeight + 0.35f);
                else point = new Vector2(Random.Range(-ArenaHalfWidth, ArenaHalfWidth), ArenaHalfHeight - 0.35f);
                if ((point - playerPosition).sqrMagnitude > 28f) return point;
            }
            return new Vector2(-ArenaHalfWidth + 0.35f, 0);
        }

        private void SpawnEnemy(EnemyType kind, Vector2 position)
        {
            SpawnEnemyVariant(kind, position, false, false);
        }

        private void SpawnEnemyVariant(EnemyType kind, Vector2 position, bool elite, bool child)
        {
            bool isBoss = kind >= EnemyType.BossJelly;
            Sprite sprite = elite && kind == EnemyType.Pink ? CandyVisuals.EliteJelly :
                            elite && kind == EnemyType.Cupcake ? CandyVisuals.EliteCupcake :
                            child && kind == EnemyType.Pink ? CandyVisuals.MiniJelly :
                            kind == EnemyType.Purple || kind == EnemyType.BossKing ? CandyVisuals.PurpleJelly :
                            kind == EnemyType.Cupcake || kind == EnemyType.BossCake ? CandyVisuals.Cupcake :
                            kind == EnemyType.Mint ? CandyVisuals.MintMonster :
                            kind == EnemyType.Lollipop ? CandyVisuals.LollipopMonster :
                            kind == EnemyType.Donut ? CandyVisuals.DonutMonster :
                            kind == EnemyType.Cookie ? CandyVisuals.CookieMonster :
                            kind == EnemyType.Macaron ? CandyVisuals.MacaronMonster :
                            kind == EnemyType.IceCream ? CandyVisuals.IceCreamMonster :
                            kind == EnemyType.Gummy ? CandyVisuals.GummyMonster : CandyVisuals.PinkJelly;
            if (sprite == null) sprite = CandyVisuals.PinkJelly;
            float size = isBoss ? 2.8f : elite ? 1.75f : child ? 0.5f :
                         kind == EnemyType.Tank ? 1.45f : kind == EnemyType.Cupcake || kind == EnemyType.Mint ? 1.08f : 0.88f;
            SpriteRenderer visual = CandyVisuals.Create(kind.ToString(), sprite, position, size, 5, worldRoot);
            Color baseColor = kind == EnemyType.Tank ? (Color)new Color32(255, 220, 119, 255) : Color.white;
            visual.color = baseColor;
            float health = kind == EnemyType.Pink ? 23 : kind == EnemyType.Purple ? 29 : kind == EnemyType.Cupcake ? 27 :
                           kind == EnemyType.Mint ? 36 : kind == EnemyType.Lollipop ? 30 : kind == EnemyType.Tank ? 65 :
                           kind == EnemyType.Donut ? 34 : kind == EnemyType.Cookie ? 38 : kind == EnemyType.Macaron ? 32 :
                           kind == EnemyType.IceCream ? 36 : kind == EnemyType.Gummy ? 42 : 320 + chapter * 90;
            health *= 1f + chapter * 0.14f;
            if (elite) health = health * 2.15f + 45f;
            if (child) health *= 0.48f;
            float speed = kind == EnemyType.Purple ? 3f : kind == EnemyType.Lollipop ? 2.45f :
                          kind == EnemyType.Tank ? 1.2f : kind == EnemyType.Mint ? 1.7f :
                          kind == EnemyType.Macaron ? 2.6f : kind == EnemyType.IceCream ? 1.55f :
                          kind == EnemyType.Donut ? 2.2f : kind == EnemyType.Gummy ? 1.85f : isBoss ? 1.25f : 1.9f;
            if (elite) speed *= 0.82f;
            if (child) speed *= 1.28f;
            SpriteRenderer aura = null;
            if (elite)
            {
                aura = CandyVisuals.Create("Elite glow", CandyVisuals.Ring, position, 2.35f, 4, worldRoot);
                aura.color = new Color(1f, 0.83f, 0.35f, 0.8f);
            }
            enemies.Add(new Enemy { sprite = visual, type = kind, position = position, hp = health, maxHp = health, speed = speed,
                radius = isBoss ? 1.05f : elite ? 0.85f : child ? 0.29f : kind == EnemyType.Tank ? 0.62f : 0.43f,
                boss = isBoss, elite = elite, child = child, aura = aura,
                attackTimer = Random.Range(1.5f, 2.8f), dashTimer = Random.Range(2f, 3.5f), baseColor = baseColor });
        }

        private static string EliteName(EnemyType kind)
        {
            switch (kind)
            {
                case EnemyType.Pink: return "เยลลี่ราชาแตกตัว";
                case EnemyType.Cupcake: return "คัพเค้กระเบิด";
                case EnemyType.Donut: return "โดนัทพายุ";
                case EnemyType.Cookie: return "คุกกี้เศษกระสุน";
                case EnemyType.Macaron: return "มาการองพุ่งชน";
                case EnemyType.IceCream: return "ไอศกรีมเยือกแข็ง";
                default: return "กัมมีเรียกลูกน้อง";
            }
        }

        private void SpawnBoss()
        {
            EnemyType type = chapter % 3 == 0 ? EnemyType.BossJelly : chapter % 3 == 1 ? EnemyType.BossCake : EnemyType.BossKing;
            SpawnEnemy(type, EdgePoint());
            audioPlayer.BossAlert();
        }

        private void UpdateEnemies(float dt)
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                Enemy enemy = enemies[i];
                enemy.age += dt;
                enemy.attackTimer -= dt;
                enemy.dashTimer -= dt;
                enemy.contactTimer -= dt;
                enemy.flashTimer -= dt;
                enemy.slowTimer -= dt;
                if (enemy.burnTimer > 0) { enemy.burnTimer -= dt; enemy.hp -= enemy.burnDps * dt; }
                else enemy.burnDps = 0;
                if (enemy.poisonTimer > 0) { enemy.poisonTimer -= dt; enemy.hp -= enemy.poisonDps * dt; }
                else enemy.poisonDps = 0;
                if (enemy.hp <= 0) { KillEnemy(i); continue; }
                enemy.sprite.color = enemy.flashTimer > 0 ? new Color(1f, 0.66f, 0.79f) :
                                     enemy.burnTimer > 0 ? new Color(1f, 0.64f, 0.39f) :
                                     enemy.poisonTimer > 0 ? new Color(0.77f, 0.6f, 0.94f) :
                                     enemy.slowTimer > 0 ? new Color(0.62f, 0.87f, 1f) : enemy.baseColor;
                Vector2 toPlayer = playerPosition - enemy.position;
                float distance = toPlayer.magnitude;
                Vector2 direction = distance > 0.01f ? toPlayer / distance : Vector2.right;
                float speed = enemy.speed * (enemy.slowTimer > 0 ? Mathf.Clamp(enemy.slowFactor, 0.2f, 1f) : 1f);
                if ((enemy.type == EnemyType.Purple || enemy.type == EnemyType.Macaron || enemy.type == EnemyType.Gummy) && enemy.dashTimer < 0)
                {
                    speed *= enemy.type == EnemyType.Macaron ? (enemy.elite ? 3.3f : 2.5f) : 2.1f;
                    if (enemy.dashTimer < -0.28f) enemy.dashTimer = Random.Range(2.2f, 3.4f);
                }
                if (enemy.type == EnemyType.Lollipop || enemy.type == EnemyType.Donut)
                    direction = (direction + Vector2.Perpendicular(direction) * Mathf.Sin(enemy.age * 8f) * 0.95f).normalized;
                if ((enemy.type == EnemyType.Cupcake || enemy.boss) && distance < 4f) direction = Vector2.Perpendicular(direction) * Mathf.Sign(Mathf.Sin(enemy.age));
                enemy.position += direction * speed * dt;
                enemy.position.x = Mathf.Clamp(enemy.position.x, -ArenaHalfWidth + 0.4f, ArenaHalfWidth - 0.4f);
                enemy.position.y = Mathf.Clamp(enemy.position.y, -ArenaHalfHeight + 0.4f, ArenaHalfHeight - 0.4f);
                enemy.sprite.transform.position = enemy.position;
                enemy.sprite.transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(enemy.age * 5f) * 4f);
                if (enemy.aura != null)
                {
                    enemy.aura.transform.position = enemy.position;
                    enemy.aura.transform.localScale = Vector3.one * (2.35f + Mathf.Sin(enemy.age * 5f) * 0.16f);
                }
                if (enemy.attackTimer <= 0)
                {
                    if (enemy.boss)
                    {
                        int count = enemy.type == EnemyType.BossKing ? 9 : 7;
                        for (int j = 0; j < count; j++)
                        {
                            float a = Mathf.PI * 2f * j / count + enemy.age * 0.27f;
                            SpawnBullet(enemy.position, new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * 4f, 9f + chapter * 1.5f, true,
                                Color.white, 0, enemy.type == EnemyType.BossKing ? ShotKind.EnemyCaramel : ShotKind.EnemyJam);
                        }
                        audioPlayer.EnemyVolley(false);
                        enemy.attackTimer = enemy.type == EnemyType.BossCake ? 2.5f : 2.9f;
                    }
                    else if (enemy.elite)
                    {
                        if (enemy.type == EnemyType.Donut) EnemyShootRing(enemy, 6, 4.3f, false);
                        else if (enemy.type == EnemyType.Gummy)
                        {
                            if (enemies.Count < 24) SpawnEnemyVariant(EnemyType.Gummy, enemy.position + Random.insideUnitCircle * 0.8f, false, true);
                            EnemyShootFan(enemy, 2, 4.7f, false);
                        }
                        else EnemyShootFan(enemy, enemy.type == EnemyType.Cupcake ? 3 : 2,
                            enemy.type == EnemyType.IceCream ? 5.1f : 4.6f,
                            enemy.type == EnemyType.IceCream);
                        audioPlayer.EnemyVolley(enemy.type == EnemyType.IceCream);
                        enemy.attackTimer = enemy.type == EnemyType.Gummy ? 4.8f : 3.3f;
                    }
                    else if (enemy.type == EnemyType.Cupcake || enemy.type == EnemyType.Lollipop ||
                             enemy.type == EnemyType.Cookie || enemy.type == EnemyType.IceCream || enemy.type == EnemyType.Donut)
                    {
                        if (enemy.type == EnemyType.Donut) EnemyShootRing(enemy, 3, 3.7f, false);
                        else EnemyShootFan(enemy, 1,
                            enemy.type == EnemyType.Lollipop ? 5.7f : 4.5f, enemy.type == EnemyType.IceCream);
                        audioPlayer.EnemyVolley(enemy.type == EnemyType.IceCream);
                        enemy.attackTimer = enemy.type == EnemyType.Donut ? 4.4f : Random.Range(3.2f, 4.2f);
                    }
                    else enemy.attackTimer = 2f;
                }
                if (distance < enemy.radius + 0.34f && enemy.contactTimer <= 0)
                {
                    DamagePlayer(enemy.boss ? 15f : enemy.elite ? 12f : enemy.type == EnemyType.Tank ? 11f : 7f);
                    enemy.contactTimer = 1f;
                }
            }
        }

        private void EnemyShootFan(Enemy enemy, int count, float speed, bool frost)
        {
            Vector2 direction = (playerPosition - enemy.position).normalized;
            if (direction.sqrMagnitude < 0.01f) direction = Vector2.down;
            for (int i = 0; i < count; i++)
            {
                float angle = (i - (count - 1) * 0.5f) * 19f;
                Vector2 shot = Quaternion.Euler(0, 0, angle) * direction;
                SpawnBullet(enemy.position, shot * speed, enemy.elite ? 10f : 7f, true,
                    Color.white, 0,
                    frost ? ShotKind.EnemyFrost : enemy.type == EnemyType.Cookie ? ShotKind.EnemyCaramel :
                    enemy.type == EnemyType.Gummy ? ShotKind.EnemyMint : ShotKind.EnemyJam, false, frost);
            }
        }

        private void EnemyShootRing(Enemy enemy, int count, float speed, bool frost)
        {
            for (int i = 0; i < count; i++)
            {
                float angle = Mathf.PI * 2f * i / count + enemy.age * 0.24f;
                Vector2 shot = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                SpawnBullet(enemy.position, shot * speed, enemy.elite ? 10f : 7f, true,
                    Color.white, 0, ShotKind.EnemyCaramel, false, frost);
            }
        }

        private void UpdateBullets(float dt)
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                Bullet bullet = bullets[i];
                bullet.position += bullet.velocity * dt;
                bullet.life -= dt;
                bullet.sprite.transform.position = bullet.position;
                if (bullet.kind != ShotKind.Standard)
                    bullet.sprite.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(bullet.velocity.y, bullet.velocity.x) * Mathf.Rad2Deg);
                bool remove = bullet.life <= 0 || Mathf.Abs(bullet.position.x) > ArenaHalfWidth + 1 || Mathf.Abs(bullet.position.y) > ArenaHalfHeight + 1;
                if (!remove && bullet.hostile)
                {
                    if ((bullet.position - playerPosition).sqrMagnitude < (bullet.radius + 0.31f) * (bullet.radius + 0.31f))
                    {
                        bool vulnerable = invincibleClock <= 0;
                        DamagePlayer(bullet.damage);
                        if (vulnerable && bullet.slowsPlayer) playerSlowTimer = Mathf.Max(playerSlowTimer, 1.3f);
                        remove = true;
                    }
                }
                else if (!remove)
                {
                    for (int e = enemies.Count - 1; e >= 0; e--)
                    {
                        Enemy enemy = enemies[e];
                        float radius = bullet.radius + enemy.radius;
                        if ((bullet.position - enemy.position).sqrMagnitude >= radius * radius) continue;
                        if (bullet.hitEnemies.Contains(enemy)) continue;
                        bullet.hitEnemies.Add(enemy);
                        if (bullet.kind == ShotKind.Marshmallow)
                        {
                            ExplodeMarshmallow(bullet);
                            remove = true;
                            break;
                        }
                        enemy.hp -= bullet.damage;
                        ApplyBulletEffects(enemy, bullet);
                        HitSpark(bullet.position, bullet.ice ? new Color32(133, 223, 255, 255) :
                            bullet.fire ? new Color32(255, 153, 83, 255) : bullet.poison ? new Color32(185, 121, 232, 255) : new Color32(255, 209, 110, 255));
                        if (enemy.hp <= 0) KillEnemy(e);
                        else enemy.flashTimer = 0.12f;
                        Enemy next = bullet.bounces > 0 ? FindRicochetTarget(bullet) : null;
                        if (next != null)
                        {
                            bullet.velocity = (next.position - bullet.position).normalized * bullet.velocity.magnitude;
                            bullet.bounces--;
                            audioPlayer.Ricochet();
                        }
                        else if (bullet.pierce-- <= 0) remove = true;
                        break;
                    }
                }
                if (remove)
                {
                    if (!bullet.hostile && bullet.kind == ShotKind.Jam) SpawnJamPuddle(bullet.position, bullet.special);
                    Destroy(bullet.sprite.gameObject);
                    bullets.RemoveAt(i);
                }
            }
        }

        private void ApplySlow(Enemy enemy, float duration, float factor)
        {
            enemy.slowFactor = enemy.slowTimer > 0 ? Mathf.Min(enemy.slowFactor, factor) : factor;
            enemy.slowTimer = Mathf.Max(enemy.slowTimer, duration);
        }

        private void ApplyBulletEffects(Enemy enemy, Bullet bullet)
        {
            if (bullet.ice)
                ApplySlow(enemy, bullet.chillDuration, enemy.boss ? Mathf.Max(0.68f, bullet.chillFactor + 0.31f) : bullet.chillFactor);
            if (bullet.fire) { enemy.burnTimer = 3.5f; enemy.burnDps = Mathf.Max(enemy.burnDps, bullet.damage * 0.22f); }
            if (bullet.poison) { enemy.poisonTimer = 6f; enemy.poisonDps = Mathf.Min(24f, enemy.poisonDps + bullet.damage * 0.085f); }
        }

        private void ExplodeMarshmallow(Bullet bullet)
        {
            float radius = bullet.blastRadius;
            for (int e = enemies.Count - 1; e >= 0; e--)
            {
                Enemy enemy = enemies[e];
                float distance = Vector2.Distance(enemy.position, bullet.position);
                if (distance > radius + enemy.radius) continue;
                float falloff = Mathf.Lerp(1f, 0.42f, Mathf.Clamp01(distance / (radius + enemy.radius)));
                enemy.hp -= bullet.damage * falloff;
                ApplyBulletEffects(enemy, bullet);
                if (enemy.hp <= 0) KillEnemy(e);
                else enemy.flashTimer = 0.16f;
            }
            SpawnRing(bullet.position, new Color32(255, 214, 143, 255), radius * 2.1f);
            for (int i = 0; i < (bullet.special ? 16 : 7); i++)
                HitSpark(bullet.position + Random.insideUnitCircle * radius * 0.5f, new Color32(255, 222, 151, 255));
            audioPlayer.Explosion();
        }

        private void SpawnJamPuddle(Vector2 position, bool special)
        {
            if (puddles.Count >= 24)
            {
                Destroy(puddles[0].sprite.gameObject);
                puddles.RemoveAt(0);
            }
            position.x = Mathf.Clamp(position.x, -ArenaHalfWidth + 0.5f, ArenaHalfWidth - 0.5f);
            position.y = Mathf.Clamp(position.y, -ArenaHalfHeight + 0.5f, ArenaHalfHeight - 0.5f);
            float radius = (special ? 1.18f : 0.78f) + jamDurationLevel * 0.1f;
            float duration = (special ? 6f : 4.2f) + jamDurationLevel * 1.4f;
            Sprite sprite = CandyVisuals.JamPuddle != null ? CandyVisuals.JamPuddle : CandyVisuals.Dot;
            SpriteRenderer visual = CandyVisuals.Create("Sticky strawberry jam", sprite, position, radius * 1.38f, 2, worldRoot);
            visual.color = new Color(1f, 1f, 1f, 0.68f);
            puddles.Add(new Puddle { sprite = visual, position = position, radius = radius, life = duration, total = duration });
            audioPlayer.Splat();
        }

        private void UpdatePuddles(float dt)
        {
            for (int i = puddles.Count - 1; i >= 0; i--)
            {
                Puddle puddle = puddles[i];
                puddle.life -= dt;
                if (puddle.life <= 0)
                {
                    Destroy(puddle.sprite.gameObject);
                    puddles.RemoveAt(i);
                    continue;
                }
                Color tint = puddle.sprite.color;
                tint.a = 0.68f * Mathf.Clamp01(puddle.life / 0.7f);
                puddle.sprite.color = tint;
                foreach (Enemy enemy in enemies)
                    if ((enemy.position - puddle.position).sqrMagnitude < (puddle.radius + enemy.radius * 0.35f) * (puddle.radius + enemy.radius * 0.35f))
                        ApplySlow(enemy, 0.42f, enemy.boss ? 0.82f : 0.56f);
            }
        }

        private Enemy FindRicochetTarget(Bullet bullet)
        {
            Enemy best = null;
            float bestDistance = 16f;
            foreach (Enemy enemy in enemies)
            {
                if (bullet.hitEnemies.Contains(enemy)) continue;
                float distance = (enemy.position - bullet.position).sqrMagnitude;
                if (distance < bestDistance) { best = enemy; bestDistance = distance; }
            }
            return best;
        }

        private void KillEnemy(int index)
        {
            Enemy enemy = enemies[index];
            Vector2 position = enemy.position;
            bool boss = enemy.boss;
            Destroy(enemy.sprite.gameObject);
            if (enemy.aura != null) Destroy(enemy.aura.gameObject);
            enemies.RemoveAt(index);
            kills++;
            score += boss ? 500 : enemy.elite ? 125 : enemy.child ? 10 : 25;
            if (bonusStage) SpawnDrop(position, DropType.Crystal, 1);
            else SpawnDrop(position, DropType.Sugar, boss ? 16 : enemy.elite ? 7 : enemy.child ? 1 : enemy.type == EnemyType.Tank ? 5 : 2);
            if (Random.value < (boss ? 1f : enemy.elite ? 0.9f : 0.55f)) SpawnDrop(position + Random.insideUnitCircle * 0.28f, DropType.Coin, boss ? 20 : enemy.elite ? 6 : 1);
            if (Random.value < 0.035f && !boss) SpawnDrop(position + Vector2.right * 0.22f, DropType.Heart, 18);
            if (enemy.type == EnemyType.Mint)
            {
                for (int i = 0; i < 6; i++)
                {
                    float a = 2f * Mathf.PI * i / 6f;
                    SpawnBullet(position, new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * 3.3f, 7f, true,
                        Color.white, 0, ShotKind.EnemyMint);
                }
            }
            if (enemy.elite && enemy.type == EnemyType.Pink)
            {
                audioPlayer.JellySplit();
                for (int i = 0; i < 3; i++)
                {
                    float angle = Mathf.PI * 2f * i / 3f;
                    Vector2 childPosition = position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 0.7f;
                    SpawnEnemyVariant(EnemyType.Pink, childPosition, false, true);
                }
            }
            if (enemy.elite && enemy.type == EnemyType.Cupcake)
            {
                SpawnRing(position, new Color32(255, 177, 205, 255), 3.5f);
                if ((playerPosition - position).sqrMagnitude < 2.1f * 2.1f) DamagePlayer(14f);
                for (int i = 0; i < 12; i++) HitSpark(position + Random.insideUnitCircle * 0.9f, new Color32(255, 205, 228, 255));
                audioPlayer.CupcakeBlast();
            }
            for (int i = 0; i < (boss ? 18 : enemy.elite ? 12 : 5); i++)
                HitSpark(position + Random.insideUnitCircle * 0.35f, boss || enemy.elite ? new Color32(255, 205, 84, 255) : new Color32(255, 142, 192, 255));
            audioPlayer.Hit();
            if (boss) chapterClearPending = true;
        }

        private void DamagePlayer(float amount)
        {
            if (invincibleClock > 0 || mode != Mode.Playing) return;
            hp -= amount;
            invincibleClock = 1f;
            audioPlayer.Hurt();
            for (int i = 0; i < 7; i++) HitSpark(playerPosition, new Color32(115, 201, 255, 255));
            if (hp <= 0) EndRun(false);
        }

        private void SpawnDrop(Vector2 position, DropType type, int value)
        {
            Sprite sprite = type == DropType.Crystal && CandyVisuals.CrystalProp != null ? CandyVisuals.CrystalProp :
                            type == DropType.Heart && CandyVisuals.HeartProp != null ? CandyVisuals.HeartProp :
                            type == DropType.Coin && CandyVisuals.CandyCoin != null ? CandyVisuals.CandyCoin :
                            type == DropType.Sugar ? CandyVisuals.Star : CandyVisuals.Dot;
            SpriteRenderer visual = CandyVisuals.Create(type.ToString(), sprite, position, 0.52f, 6, worldRoot);
            visual.color = type == DropType.Sugar ? new Color32(255, 214, 97, 255) : type == DropType.Crystal ? new Color32(255, 238, 135, 255) :
                           type == DropType.Coin ? (CandyVisuals.CandyCoin != null ? Color.white : new Color32(255, 176, 58, 255)) :
                           new Color32(255, 93, 133, 255);
            drops.Add(new Drop { sprite = visual, position = position, type = type, value = value });
        }

        private void UpdateDrops(float dt)
        {
            for (int i = drops.Count - 1; i >= 0; i--)
            {
                Drop drop = drops[i];
                drop.age += dt;
                Vector2 delta = playerPosition - drop.position;
                float distance = delta.magnitude;
                if (distance < magnetRange) drop.position += delta.normalized * (4.8f + (magnetRange - distance) * 2f) * dt;
                drop.sprite.transform.position = drop.position + Vector2.up * (Mathf.Sin(drop.age * 5f) * 0.07f);
                drop.sprite.transform.Rotate(0, 0, 60f * dt);
                if (distance < 0.42f)
                {
                    if (drop.type == DropType.Sugar) { sugar += drop.value; score += 5 * drop.value; }
                    else if (drop.type == DropType.Coin) runCoins += drop.value;
                    else if (drop.type == DropType.Crystal) { bonusCrystals += drop.value; score += 35 * drop.value; audioPlayer.Crystal(); }
                    else hp = Mathf.Min(maxHp, hp + drop.value);
                    audioPlayer.Pickup();
                    Destroy(drop.sprite.gameObject);
                    drops.RemoveAt(i);
                }
                else if (drop.age > 35f)
                {
                    Destroy(drop.sprite.gameObject);
                    drops.RemoveAt(i);
                }
            }
            if (!bonusStage && sugar >= sugarGoal && mode == Mode.Playing)
            {
                sugar -= sugarGoal;
                level++;
                sugarGoal = 9 + level * 5;
                PickChoices();
                mode = Mode.Upgrade;
                audioPlayer.Upgrade();
            }
        }

        private void HitSpark(Vector2 position, Color color)
        {
            if (sparks.Count > 90) return;
            SpriteRenderer visual = CandyVisuals.Create("Candy sparkle", CandyVisuals.Star, position, Random.Range(0.12f, 0.26f), 12, worldRoot);
            visual.color = color;
            sparks.Add(new Spark { sprite = visual, velocity = Random.insideUnitCircle * Random.Range(1.5f, 4.2f), life = 0.42f, total = 0.42f, startScale = visual.transform.localScale.x });
        }

        private void UpdateSparks(float dt)
        {
            for (int i = sparks.Count - 1; i >= 0; i--)
            {
                Spark spark = sparks[i];
                spark.life -= dt;
                spark.sprite.transform.position += (Vector3)(spark.velocity * dt);
                if (spark.grow > 0) spark.sprite.transform.localScale = Vector3.one * (spark.startScale + spark.grow * (1f - spark.life / spark.total));
                Color c = spark.sprite.color;
                c.a = Mathf.Clamp01(spark.life / spark.total);
                spark.sprite.color = c;
                if (spark.life <= 0) { Destroy(spark.sprite.gameObject); sparks.RemoveAt(i); }
            }
        }

        private void UpdateCamera(float dt)
        {
            float visibleHalfWidth = gameCamera.orthographicSize * gameCamera.aspect;
            float xLimit = Mathf.Max(0, ArenaHalfWidth - visibleHalfWidth + 0.25f);
            float targetX = Mathf.Clamp(playerPosition.x, -xLimit, xLimit);
            Vector3 target = new Vector3(targetX, 0, -10);
            gameCamera.transform.position = Vector3.Lerp(gameCamera.transform.position, target, dt * 3f);
        }

        private void PickChoices()
        {
            List<UpgradeType> pool = new List<UpgradeType> { UpgradeType.Damage, UpgradeType.FireRate, UpgradeType.Health, UpgradeType.Speed,
                UpgradeType.DoubleShot, UpgradeType.Pierce, UpgradeType.Magnet, UpgradeType.Heal,
                UpgradeType.Ice, UpgradeType.Fire, UpgradeType.Poison, UpgradeType.Ricochet, UpgradeType.ProjectileSpeed };
            if (selectedHero == HeroType.Marshmallow && blastRadiusLevel < 3) pool.Add(UpgradeType.BlastRadius);
            if (selectedHero == HeroType.Strawberry && jamDurationLevel < 3) pool.Add(UpgradeType.JamDuration);
            if (selectedHero == HeroType.Blueberry && frostPowerLevel < 3) pool.Add(UpgradeType.FrostPower);
            if (spread >= 5) pool.Remove(UpgradeType.DoubleShot);
            if (pierce >= 4 || selectedHero == HeroType.Marshmallow) pool.Remove(UpgradeType.Pierce);
            if (iceShots || selectedHero == HeroType.Blueberry) pool.Remove(UpgradeType.Ice);
            if (fireShots) pool.Remove(UpgradeType.Fire);
            if (poisonShots) pool.Remove(UpgradeType.Poison);
            if (ricochet >= 2 || selectedHero == HeroType.Marshmallow) pool.Remove(UpgradeType.Ricochet);
            if (projectileSpeedLevel >= 3) pool.Remove(UpgradeType.ProjectileSpeed);
            List<UpgradeType> special = new List<UpgradeType>();
            foreach (UpgradeType item in pool)
                if (item == UpgradeType.DoubleShot || item == UpgradeType.Pierce || item == UpgradeType.Ice ||
                    item == UpgradeType.Fire || item == UpgradeType.Poison || item == UpgradeType.Ricochet ||
                    item == UpgradeType.BlastRadius || item == UpgradeType.JamDuration ||
                    item == UpgradeType.FrostPower || item == UpgradeType.ProjectileSpeed) special.Add(item);
            if (special.Count > 0)
            {
                choices[0] = special[Random.Range(0, special.Count)];
                pool.Remove(choices[0]);
            }
            for (int i = 0; i < 3; i++)
            {
                if (i == 0 && special.Count > 0) continue;
                int choice = Random.Range(0, pool.Count);
                choices[i] = pool[choice];
                pool.RemoveAt(choice);
            }
        }

        private void ApplyChoice(int index)
        {
            if (index < 0 || index >= choices.Length || mode != Mode.Upgrade) return;
            switch (choices[index])
            {
                case UpgradeType.Damage: damage *= 1.23f; break;
                case UpgradeType.FireRate: fireInterval = Mathf.Max(0.10f, fireInterval * 0.83f); break;
                case UpgradeType.Health: maxHp += 25; hp = Mathf.Min(maxHp, hp + 40); break;
                case UpgradeType.Speed: moveSpeed += 0.55f; break;
                case UpgradeType.DoubleShot: spread++; break;
                case UpgradeType.Pierce: pierce++; break;
                case UpgradeType.Magnet: magnetRange += 0.7f; break;
                case UpgradeType.Heal: hp = Mathf.Min(maxHp, hp + maxHp * 0.55f); break;
                case UpgradeType.Ice: iceShots = true; break;
                case UpgradeType.Fire: fireShots = true; break;
                case UpgradeType.Poison: poisonShots = true; break;
                case UpgradeType.Ricochet: ricochet++; break;
                case UpgradeType.BlastRadius: blastRadiusLevel++; break;
                case UpgradeType.JamDuration: jamDurationLevel++; break;
                case UpgradeType.FrostPower: frostPowerLevel++; break;
                case UpgradeType.ProjectileSpeed: projectileSpeedLevel++; break;
            }
            audioPlayer.Upgrade();
            mode = Mode.Playing;
        }

        private void CompleteChapter()
        {
            chapterClearPending = false;
            runCoins += 35 + chapter * 12;
            wallet += runCoins;
            runCoins = 0;
            Save();
            ClearEntities();
            if (!endless && chapter >= 2) BeginEndingStory();
            else mode = Mode.ChapterClear;
        }

        private void CompleteBonus()
        {
            bonusWon = bonusCrystals >= BonusGoal;
            int reward = bonusWon ? 70 : 15;
            wallet += runCoins + reward;
            runCoins = 0;
            if (bonusWon) hp = Mathf.Min(maxHp, hp + maxHp * 0.3f);
            bestScore = Mathf.Max(bestScore, score);
            Save();
            ClearEntities();
            mode = Mode.BonusResult;
            if (bonusWon) audioPlayer.StageClear();
        }

        private void NextChapter()
        {
            chapter++;
            BeginChapter();
        }

        private void EndRun(bool win)
        {
            wallet += runCoins;
            runCoins = 0;
            bestScore = Mathf.Max(bestScore, score);
            if (win) PlayerPrefs.SetInt("cc_won", 1);
            Save();
            mode = win ? Mode.Victory : Mode.Defeat;
            abilityAura.gameObject.SetActive(false);
        }

        private int UpgradeCost(int rank) { return 35 + rank * 35; }

        private void BuyPermanent(int type)
        {
            int rank = type == 0 ? permanentHealth : type == 1 ? permanentDamage : permanentSpeed;
            if (rank >= 5 || wallet < UpgradeCost(rank)) return;
            wallet -= UpgradeCost(rank);
            if (type == 0) permanentHealth++;
            else if (type == 1) permanentDamage++;
            else permanentSpeed++;
            audioPlayer.Upgrade();
            Save();
        }
    }
}
