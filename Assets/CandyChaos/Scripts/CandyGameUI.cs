using System.Collections.Generic;
using UnityEngine;

namespace CandyChaos
{
    public sealed partial class CandyGame
    {
        private static readonly Color Ink = new Color32(255, 247, 255, 255);
        private static readonly Color MutedInk = new Color32(224, 193, 246, 255);
        private static readonly Color Pink = new Color32(255, 87, 218, 255);
        private static readonly Color Blue = new Color32(59, 205, 255, 255);
        private static readonly Color Yellow = new Color32(255, 204, 57, 255);
        private static readonly Color Green = new Color32(146, 230, 67, 255);
        private GUIStyle titleStyle, headingStyle, bodyStyle, smallStyle, buttonStyle, softButtonStyle;
        private GUIStyle successButtonStyle, goldButtonStyle;
        private GUIStyle cardStyle, panelStyle, centeredStyle, cardTitleStyle, hintStyle;
        private GUIStyle buttonGlowStyle, buttonShadowStyle, barTrackStyle, barFillStyle, sparkleStyle;
        private GUIStyle storyFrameStyle, storyHeadingStyle;
        private Texture2D panelTexture, cardTexture, buttonTexture, buttonHoverTexture, buttonPressedTexture;
        private Texture2D softButtonTexture, softButtonHoverTexture, buttonGlowTexture, buttonShadowTexture;
        private Texture2D successButtonTexture, successButtonHoverTexture, goldButtonTexture, goldButtonHoverTexture;
        private Texture2D storyFrameTexture, barTrackTexture, barFillTexture, barTexture;
        private readonly Dictionary<int, float> buttonGlow = new Dictionary<int, float>();
        private HeroType previewHero = HeroType.Marshmallow;

        private void PrepareGui()
        {
            Font font = Resources.Load<Font>("Fonts/NotoSansThai-Regular");
            if (font == null)
            {
                Debug.LogError("Candy Chaos could not load its bundled Thai UI font.");
                font = Font.CreateDynamicFontFromOSFont(new[] { "Noto Sans Thai", "Tahoma", "Arial" }, 28);
            }
            panelTexture = CandyTexture(new Color32(103, 48, 150, 248), new Color32(44, 19, 83, 248), new Color32(155, 83, 205, 255));
            cardTexture = CandyTexture(new Color32(121, 55, 168, 251), new Color32(69, 28, 116, 251), new Color32(176, 87, 221, 255));
            buttonTexture = CandyTexture(new Color32(255, 121, 228, 255), new Color32(206, 48, 190, 255), new Color32(111, 28, 151, 255));
            buttonHoverTexture = CandyTexture(new Color32(255, 162, 236, 255), new Color32(229, 66, 211, 255), new Color32(151, 47, 185, 255));
            buttonPressedTexture = CandyTexture(new Color32(214, 54, 195, 255), new Color32(149, 27, 157, 255), new Color32(88, 22, 126, 255));
            softButtonTexture = CandyTexture(new Color32(95, 224, 255, 255), new Color32(43, 120, 223, 255), new Color32(78, 39, 150, 255));
            softButtonHoverTexture = CandyTexture(new Color32(144, 241, 255, 255), new Color32(60, 154, 240, 255), new Color32(103, 58, 183, 255));
            successButtonTexture = CandyTexture(new Color32(177, 248, 92, 255), new Color32(74, 175, 68, 255), new Color32(54, 83, 122, 255));
            successButtonHoverTexture = CandyTexture(new Color32(210, 255, 128, 255), new Color32(101, 203, 80, 255), new Color32(69, 111, 135, 255));
            goldButtonTexture = CandyTexture(new Color32(255, 218, 90, 255), new Color32(249, 139, 43, 255), new Color32(125, 59, 134, 255));
            goldButtonHoverTexture = CandyTexture(new Color32(255, 235, 130, 255), new Color32(255, 164, 61, 255), new Color32(148, 80, 147, 255));
            buttonGlowTexture = RoundedTexture(Color.white);
            buttonShadowTexture = RoundedTexture(new Color32(28, 11, 58, 255));
            storyFrameTexture = CandyTexture(new Color32(127, 57, 172, 255), new Color32(56, 22, 98, 255), new Color32(199, 106, 231, 255));
            barTrackTexture = CandyTexture(new Color32(54, 17, 91, 255), new Color32(32, 11, 68, 255), new Color32(115, 48, 154, 255));
            barFillTexture = CandyTexture(new Color32(255, 255, 255, 255), new Color32(190, 190, 190, 255), new Color32(255, 255, 255, 255));
            barTexture = Texture2D.whiteTexture;
            titleStyle = Style(font, 68, FontStyle.Bold, TextAnchor.MiddleCenter, Ink);
            headingStyle = Style(font, 38, FontStyle.Bold, TextAnchor.MiddleCenter, Ink);
            bodyStyle = Style(font, 26, FontStyle.Normal, TextAnchor.MiddleLeft, Ink);
            smallStyle = Style(font, 20, FontStyle.Normal, TextAnchor.MiddleLeft, MutedInk);
            centeredStyle = Style(font, 25, FontStyle.Normal, TextAnchor.MiddleCenter, Ink);
            cardTitleStyle = Style(font, 27, FontStyle.Bold, TextAnchor.MiddleCenter, Ink);
            hintStyle = Style(font, 18, FontStyle.Normal, TextAnchor.MiddleCenter, MutedInk);
            panelStyle = new GUIStyle(GUI.skin.box) { normal = { background = panelTexture }, border = new RectOffset(12, 12, 12, 12) };
            cardStyle = new GUIStyle(GUI.skin.box) { normal = { background = cardTexture }, border = new RectOffset(12, 12, 12, 12) };
            Color buttonInk = new Color32(48, 18, 78, 255);
            buttonStyle = Style(font, 25, FontStyle.Bold, TextAnchor.MiddleCenter, buttonInk);
            buttonStyle.normal.background = buttonTexture;
            buttonStyle.hover.background = buttonHoverTexture;
            buttonStyle.active.background = buttonPressedTexture;
            buttonStyle.hover.textColor = buttonInk;
            buttonStyle.active.textColor = buttonInk;
            buttonStyle.border = new RectOffset(12, 12, 12, 12);
            softButtonStyle = new GUIStyle(buttonStyle);
            softButtonStyle.normal.background = softButtonTexture;
            softButtonStyle.hover.background = softButtonHoverTexture;
            softButtonStyle.active.background = softButtonTexture;
            successButtonStyle = new GUIStyle(buttonStyle);
            successButtonStyle.normal.background = successButtonTexture;
            successButtonStyle.hover.background = successButtonHoverTexture;
            successButtonStyle.active.background = successButtonTexture;
            goldButtonStyle = new GUIStyle(buttonStyle);
            goldButtonStyle.normal.background = goldButtonTexture;
            goldButtonStyle.hover.background = goldButtonHoverTexture;
            goldButtonStyle.active.background = goldButtonTexture;
            buttonGlowStyle = new GUIStyle(GUI.skin.box) { normal = { background = buttonGlowTexture }, border = new RectOffset(12, 12, 12, 12) };
            buttonShadowStyle = new GUIStyle(GUI.skin.box) { normal = { background = buttonShadowTexture }, border = new RectOffset(12, 12, 12, 12) };
            barTrackStyle = new GUIStyle(GUI.skin.box) { normal = { background = barTrackTexture }, border = new RectOffset(10, 10, 10, 10) };
            barFillStyle = new GUIStyle(GUI.skin.box) { normal = { background = barFillTexture }, border = new RectOffset(10, 10, 10, 10) };
            sparkleStyle = Style(font, 21, FontStyle.Bold, TextAnchor.MiddleCenter, Color.white);
            storyFrameStyle = new GUIStyle(GUI.skin.box) { normal = { background = storyFrameTexture }, border = new RectOffset(12, 12, 12, 12) };
            storyHeadingStyle = Style(font, 34, FontStyle.Bold, TextAnchor.MiddleLeft, Ink);
        }

        private static GUIStyle Style(Font font, int size, FontStyle weight, TextAnchor anchor, Color color)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.font = font;
            style.fontSize = size;
            style.fontStyle = weight;
            style.alignment = anchor;
            style.normal.textColor = color;
            style.richText = true;
            style.wordWrap = true;
            return style;
        }

        private static Texture2D RoundedTexture(Color color)
        {
            const int size = 32;
            Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            Color[] pixels = new Color[size * size];
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                Vector2 q = new Vector2(Mathf.Abs(x - 15.5f), Mathf.Abs(y - 15.5f)) - new Vector2(9, 9);
                float edge = new Vector2(Mathf.Max(q.x, 0), Mathf.Max(q.y, 0)).magnitude + Mathf.Min(Mathf.Max(q.x, q.y), 0) - 6f;
                Color c = color;
                c.a *= Mathf.Clamp01(0.5f - edge);
                pixels[y * size + x] = c;
            }
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }

        private static Texture2D CandyTexture(Color top, Color bottom, Color rim)
        {
            const int size = 48;
            Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            Color[] pixels = new Color[size * size];
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                Vector2 q = new Vector2(Mathf.Abs(x - 23.5f), Mathf.Abs(y - 23.5f)) - new Vector2(10f, 10f);
                float edge = new Vector2(Mathf.Max(q.x, 0f), Mathf.Max(q.y, 0f)).magnitude +
                    Mathf.Min(Mathf.Max(q.x, q.y), 0f) - 12f;
                float t = y / (size - 1f);
                Color fill = Color.Lerp(bottom, top, t);
                fill = Color.Lerp(fill, Color.white, Mathf.Max(0f, t - 0.64f) * 0.28f);
                float interior = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((-edge - 1f) / 2.5f));
                Color color = Color.Lerp(rim, fill, interior);
                color.a *= Mathf.Clamp01(0.5f - edge);
                pixels[y * size + x] = color;
            }
            texture.SetPixels(pixels);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();
            return texture;
        }

        private void OnGUI()
        {
            if (titleStyle == null) PrepareGui();
            if ((mode == Mode.Title || mode == Mode.Story || mode == Mode.CharacterSelect) && CandyVisuals.MenuArt != null)
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), CandyVisuals.MenuArt, ScaleMode.ScaleAndCrop);
            if (mode == Mode.Title)
            {
                GUI.color = new Color(0.16f, 0.04f, 0.28f, 0.78f);
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
                GUI.color = Color.white;
            }
            float scale = Mathf.Min(Screen.width / 1280f, Screen.height / 720f);
            float x = (Screen.width - 1280f * scale) * 0.5f;
            float y = (Screen.height - 720f * scale) * 0.5f;
            Matrix4x4 previous = GUI.matrix;
            GUI.matrix = Matrix4x4.TRS(new Vector3(x, y, 0), Quaternion.identity, new Vector3(scale, scale, 1));
            switch (mode)
            {
                case Mode.Title: DrawTitle(); break;
                case Mode.Story: DrawStory(); break;
                case Mode.EndingStory: DrawEndingStory(); break;
                case Mode.CharacterSelect: DrawCharacterSelect(); break;
                case Mode.Playing: DrawGameplay(); break;
                case Mode.Paused: DrawGameplay(); DrawPause(); break;
                case Mode.Upgrade: DrawGameplay(); DrawUpgrade(); break;
                case Mode.ChapterClear: DrawChapterClear(); break;
                case Mode.BonusResult: DrawBonusResult(); break;
                case Mode.Defeat: DrawEnding(false); break;
                case Mode.Victory: DrawEnding(true); break;
            }
            GUI.matrix = previous;
        }

        private void DrawTitle()
        {
            GUI.Box(new Rect(315, 24, 650, 356), "", panelStyle);
            DrawAccent(new Rect(356, 43, 568, 5), Pink);
            GUI.color = new Color(0.93f, 0.25f, 0.76f, 0.72f);
            GUI.Label(new Rect(344, 52, 600, 90), "CANDY CHAOS", titleStyle);
            GUI.color = Color.white;
            GUI.Label(new Rect(340, 48, 600, 90), "CANDY CHAOS", titleStyle);
            GUI.Label(new Rect(355, 132, 570, 49), "มาช่วยกันปกป้องเมืองขนมแสนหวาน!", centeredStyle);
            if (ActionButton(new Rect(455, 197, 370, 65), "ออกผจญภัยกัน!")) { pendingEndless = false; BeginStory(); }
            if (PlayerPrefs.GetInt("cc_won", 0) == 1 && SoftButton(new Rect(455, 273, 370, 54), "ผจญภัยไม่รู้จบ"))
            { pendingEndless = true; mode = Mode.CharacterSelect; }
            GUI.Label(new Rect(440, 334, 400, 32), "★  คะแนนที่ดีที่สุด  " + bestScore, hintStyle);
            GUI.Box(new Rect(190, 391, 900, 282), "", panelStyle);
            GUI.Label(new Rect(222, 400, 550, 54), "เสริมพลังติดตัว", headingStyle);
            GUI.Box(new Rect(790, 402, 264, 47), "", cardStyle);
            DrawCoinIcon(new Rect(805, 410, 31, 31));
            GUI.Label(new Rect(836, 407, 210, 37), "ลูกอมของเรา  " + wallet, centeredStyle);
            DrawShopCard(220, 465, 0, "พลังชีวิต", permanentHealth, "+18 HP");
            DrawShopCard(505, 465, 1, "พลังโจมตี", permanentDamage, "+2.5");
            DrawShopCard(790, 465, 2, "ความเร็ว", permanentSpeed, "+0.25");
            GUI.Label(new Rect(190, 681, 900, 26), "เดินด้วย WASD/ลูกศร  •  เล็งด้วยเมาส์  •  ปล่อยเพื่อยิงอัตโนมัติ", hintStyle);
        }

        private void DrawCharacterSelect()
        {
            Overlay();
            GUI.Box(new Rect(20, 38, 1240, 634), "", panelStyle);
            GUI.Label(new Rect(64, 53, 680, 58), "เลือกเพื่อนร่วมผจญภัย", headingStyle);
            GUI.Label(new Rect(75, 109, 635, 32), "แตะภาพเพื่อดูพลังพิเศษของแต่ละคน", smallStyle);
            if (SoftButton(new Rect(1053, 68, 168, 51), "ย้อนกลับ")) mode = Mode.Title;
            DrawHeroCard(36, HeroType.Marshmallow, "Marlo", "กระสุนระเบิด", "ลูกอมยักษ์", CandyVisuals.Hero);
            DrawHeroCard(282, HeroType.Strawberry, "berrybliss", "แยมชะลอ", "พายุแยม", CandyVisuals.StrawberryHero);
            DrawHeroCard(528, HeroType.Blueberry, "Bluebelle", "กระสุนน้ำแข็ง", "เกราะหิมะ", CandyVisuals.BlueberryHero);
            DrawHeroCard(774, HeroType.Mage, "Cottony", "ยิงวงกว้าง", "ดอกเวทมนตร์", CandyVisuals.MageHero);
            DrawHeroCard(1020, HeroType.Sniper, "Citra tart", "ยิงแรงแต่ช้า", "เพ่งเล็งทองคำ", CandyVisuals.SniperHero);
            GUI.Box(new Rect(60, 604, 1160, 51), "", cardStyle);
            GUI.Label(new Rect(78, 607, 1124, 45), HeroPreview(previewHero), centeredStyle);
        }

        private static string HeroPreview(HeroType hero)
        {
            switch (hero)
            {
                case HeroType.Strawberry: return "berrybliss  ·  โปรยแยมรอบตัว ทิ้งบ่อหนืดชะลอศัตรู";
                case HeroType.Blueberry: return "Bluebelle  ·  กางเกราะน้ำแข็ง ป้องกันตัวและชะลอศัตรูใกล้ ๆ";
                case HeroType.Mage: return "Cottony  ·  โปรยกระสุนเวทมนตร์เป็นวงกว้าง";
                case HeroType.Sniper: return "Citra tart  ·  ตั้งสมาธิ เพิ่มพลังให้ทุกนัดชั่วคราว";
                default: return "Marlo  ·  ยิงลูกอมยักษ์ให้ระเบิดเมื่อโดนศัตรู";
            }
        }

        private void DrawHeroCard(float x, HeroType hero, string name, string weapon, string special, Sprite sprite)
        {
            GUI.Box(new Rect(x, 160, 224, 428), "", cardStyle);
            DrawAccent(new Rect(x + 23, 173, 178, previewHero == hero ? 6 : 3), hero == HeroType.Blueberry ? Blue :
                hero == HeroType.Sniper ? Yellow : hero == HeroType.Mage ? new Color32(177, 146, 227, 255) : Pink);
            if (GUI.Button(new Rect(x + 18, 184, 188, 189), GUIContent.none, GUIStyle.none)) previewHero = hero;
            if (sprite != null)
            {
                Rect source = sprite.rect;
                Texture2D texture = sprite.texture;
                Rect uv = new Rect(source.x / texture.width, source.y / texture.height,
                    source.width / texture.width, source.height / texture.height);
                float scale = Mathf.Min(178f / source.width, 180f / source.height);
                float width = source.width * scale;
                float height = source.height * scale;
                GUI.DrawTextureWithTexCoords(new Rect(x + 23 + (178f - width) * 0.5f,
                    189 + (180f - height) * 0.5f, width, height), texture, uv, true);
            }
            GUI.Label(new Rect(x + 8, 377, 208, 48), name, cardTitleStyle);
            GUI.Label(new Rect(x + 10, 426, 204, 29), "ยิงปกติ  " + weapon, hintStyle);
            GUI.Label(new Rect(x + 10, 462, 204, 29), "พิเศษ  " + special, hintStyle);
            if (SuccessButton(new Rect(x + 20, 518, 184, 52), "เลือก"))
            {
                selectedHero = hero;
                StartRun(pendingEndless);
            }
        }

        private void DrawShopCard(float x, float y, int type, string name, int rank, string bonus)
        {
            GUI.Box(new Rect(x, y, 270, 166), "", cardStyle);
            GUI.Label(new Rect(x + 14, y + 8, 242, 36), name + "  ระดับ " + rank + "/5", centeredStyle);
            DrawElementIcon(type == 0 ? UpgradeType.Health : type == 1 ? UpgradeType.Damage : UpgradeType.Speed,
                new Rect(x + 20, y + 48, 43, 43));
            GUI.Label(new Rect(x + 72, y + 47, 180, 36), "เพิ่มต่อระดับ  " + bonus, smallStyle);
            DrawBar(new Rect(x + 18, y + 84, 234, 10), rank / 5f,
                type == 0 ? Pink : type == 1 ? Yellow : Green);
            int cost = UpgradeCost(rank);
            string label = rank >= 5 ? "เต็มพลังแล้ว" : "ใช้  " + cost + " ลูกอม";
            if (rank < 5)
            {
                bool previousEnabled = GUI.enabled;
                GUI.enabled = wallet >= cost;
                bool bought = GoldButton(new Rect(x + 22, y + 105, 226, 46), label);
                GUI.enabled = previousEnabled;
                if (bought) BuyPermanent(type);
            }
            if (rank >= 5) GUI.Label(new Rect(x + 30, y + 105, 210, 46), label, centeredStyle);
        }

        private void DrawGameplay()
        {
            GUI.Box(new Rect(20, 14, 1240, 78), "", panelStyle);
            GUI.Label(new Rect(40, 24, 235, 35), "♥  " + Mathf.CeilToInt(Mathf.Max(0, hp)) + " / " + Mathf.CeilToInt(maxHp), bodyStyle);
            DrawBar(new Rect(42, 65, 224, 13), hp / maxHp, Pink);
            GUI.Label(new Rect(291, 24, 260, 35), bonusStage ? "✦  " + bonusCrystals + " / " + BonusGoal : "LV " + level + "   ✦ " + sugar + "/" + sugarGoal, bodyStyle);
            DrawBar(new Rect(293, 65, 230, 13), bonusStage ? bonusCrystals / (float)BonusGoal : sugar / (float)sugarGoal, Yellow);
            GUI.Label(new Rect(560, 18, 371, 41), bonusStage ? "ด่านพิเศษ · ล่าคริสตัล" : endless ? "ผจญภัยไม่รู้จบ " + (chapter + 1) : "ด่าน " + (chapter + 1) + "/3  " + ChapterNames[chapter % 3], centeredStyle);
            GUI.Label(new Rect(565, 55, 360, 25), bonusStage ? "เหลือ " + Mathf.CeilToInt(Mathf.Max(0, BonusTime - stageClock)) + " วิ  ·  คะแนน " + score : "เวลา " + Mathf.FloorToInt(stageClock) + " วิ  ·  คะแนน " + score, hintStyle);
            GUI.Box(new Rect(942, 23, 195, 51), "", cardStyle);
            DrawCoinIcon(new Rect(951, 31, 32, 32));
            GUI.Label(new Rect(986, 29, 143, 39), "ลูกอม  " + runCoins, centeredStyle);
            bool inputEnabled = mode == Mode.Playing;
            GUI.enabled = inputEnabled;
            bool pauseClicked = SoftButton(new Rect(1151, 27, 82, 51), "Ⅱ");
            GUI.enabled = true;
            if (pauseClicked) mode = Mode.Paused;
            string abilityLabel = abilityCooldown > 0 ? "รอพลัง  " + Mathf.CeilToInt(abilityCooldown) + " วิ" :
                                  Application.isMobilePlatform ? "✦  ใช้พลังพิเศษ" : "✦  ใช้พลัง  Space";
            GUI.enabled = inputEnabled && abilityCooldown <= 0;
            bool abilityClicked = GoldButton(new Rect(1004, 107, 230, 53), abilityLabel);
            GUI.enabled = true;
            if (abilityClicked) TriggerAbility();
            int iconIndex = 0;
            if (selectedHero == HeroType.Marshmallow || selectedHero == HeroType.Strawberry || selectedHero == HeroType.Blueberry ||
                iceShots || fireShots || poisonShots || ricochet > 0)
            {
                GUI.Box(new Rect(20, 104, 296, 58), "", panelStyle);
                if (selectedHero == HeroType.Marshmallow) DrawElementIcon(UpgradeType.BlastRadius, new Rect(32 + 52 * iconIndex++, 112, 42, 42));
                if (selectedHero == HeroType.Strawberry) DrawElementIcon(UpgradeType.JamDuration, new Rect(32 + 52 * iconIndex++, 112, 42, 42));
                if (selectedHero == HeroType.Blueberry) DrawElementIcon(UpgradeType.FrostPower, new Rect(32 + 52 * iconIndex++, 112, 42, 42));
                if (iceShots) DrawElementIcon(UpgradeType.Ice, new Rect(32 + 52 * iconIndex++, 112, 42, 42));
                if (fireShots) DrawElementIcon(UpgradeType.Fire, new Rect(32 + 52 * iconIndex++, 112, 42, 42));
                if (poisonShots) DrawElementIcon(UpgradeType.Poison, new Rect(32 + 52 * iconIndex++, 112, 42, 42));
                if (ricochet > 0) DrawElementIcon(UpgradeType.Ricochet, new Rect(32 + 52 * iconIndex++, 112, 42, 42));
            }
            Enemy boss = null;
            foreach (Enemy enemy in enemies) if (enemy.boss) { boss = enemy; break; }
            if (boss != null)
            {
                GUI.Box(new Rect(386, 104, 500, 60), "", panelStyle);
                GUI.Label(new Rect(408, 108, 456, 29), BossNames[chapter % 3], cardTitleStyle);
                DrawBar(new Rect(414, 143, 444, 10), boss.hp / boss.maxHp, new Color32(186, 139, 220, 255));
            }
            if (bannerClock > 0)
            {
                string message = bonusStage ? "เก็บคริสตัลให้ครบ 10 ชิ้น!" : bossSpawned ? "บอสมาแล้ว! " + BossNames[chapter % 3] :
                    !string.IsNullOrEmpty(eliteBanner) ? eliteBanner : "ยินดีต้อนรับสู่" + ChapterNames[chapter % 3];
                GUI.Box(new Rect(370, 192, 540, 65), "", panelStyle);
                DrawAccent(new Rect(391, 244, 498, 4), !string.IsNullOrEmpty(eliteBanner) && !bossSpawned ? Yellow : Pink);
                GUI.Label(new Rect(389, 198, 502, 46), message, cardTitleStyle);
            }
            if (Application.isMobilePlatform || Input.touchCount > 0) DrawTouchControls();
            else
            {
                GUI.Box(new Rect(20, 675, 795, 32), "", panelStyle);
                GUI.Label(new Rect(30, 675, 775, 30), "WASD/ลูกศร เดิน  •  คลิกค้างเพื่อเล็ง  •  ปล่อยเพื่อยิงอัตโนมัติ", hintStyle);
            }
        }

        private void DrawTouchControls()
        {
            float scale = Mathf.Min(Screen.width / 1280f, Screen.height / 720f);
            float offsetX = (Screen.width - 1280f * scale) * 0.5f;
            float offsetY = (Screen.height - 720f * scale) * 0.5f;
            Vector2 left = movingTouch ? new Vector2((leftAnchor.x - offsetX) / scale, (Screen.height - leftAnchor.y - offsetY) / scale) : new Vector2(140, 570);
            if (left.x < 55 || left.x > 400 || left.y < 350 || left.y > 700) left = new Vector2(140, 570);
            GUI.Box(new Rect(left.x - 70, left.y - 70, 140, 140), "", softButtonStyle);
            if (movingTouch)
            {
                Vector2 offset = Vector2.ClampMagnitude(moveInput, 1f) * 42f;
                GUI.Box(new Rect(left.x - 30 + offset.x, left.y - 30 - offset.y, 60, 60), "", cardStyle);
            }
            GUI.Box(new Rect(1070, 525, 150, 150), "", buttonStyle);
            if (manualAim && rightFinger >= 0)
                GUI.Box(new Rect(1115 + aimDirection.x * 42, 570 - aimDirection.y * 42, 60, 60), "", cardStyle);
            else GUI.Label(new Rect(1082, 560, 126, 75), "เล็ง\nยิง", centeredStyle);
            GUI.Box(new Rect(420, 670, 440, 32), "", panelStyle);
            GUI.Label(new Rect(425, 670, 430, 30), "แตะซ้ายเพื่อเดิน  •  แตะขวาเพื่อเล็งยิง", hintStyle);
        }

        private void DrawPause()
        {
            DrawPopupFrame(new Rect(365, 175, 550, 385), "เกมหยุดชั่วคราว", "พักหายใจสักครู่", new Color32(150, 212, 255, 255));
            GUI.Label(new Rect(425, 343, 430, 40), "พร้อมเมื่อไร ค่อยออกไปลุยกันต่อ!", centeredStyle);
            if (SuccessButton(new Rect(460, 397, 360, 62), "ลุยต่อ")) mode = Mode.Playing;
            if (SoftButton(new Rect(460, 477, 360, 62), "กลับหน้าหลัก"))
            {
                wallet += runCoins; runCoins = 0; bestScore = Mathf.Max(bestScore, score); Save();
                ClearEntities(); heroRenderer.gameObject.SetActive(false); mode = Mode.Title;
            }
        }

        private void DrawUpgrade()
        {
            DrawPopupFrame(new Rect(110, 88, 1060, 544), "เลเวล " + level + "  •  เลือกได้ 1 อย่าง", "เลือกพลังที่ถูกใจ", new Color32(255, 207, 115, 255));
            for (int i = 0; i < 3; i++)
            {
                float x = 145 + i * 335;
                GUI.Box(new Rect(x, 260, 310, 346), "", cardStyle);
                DrawAccent(new Rect(x + 24, 273, 262, 4), i == 0 ? Pink : i == 1 ? Yellow : Blue);
                DrawElementIcon(choices[i], new Rect(x + 103, 296, 104, 104));
                GUI.Label(new Rect(x + 17, 407, 276, 50), UpgradeName(choices[i]), cardTitleStyle);
                GUI.Label(new Rect(x + 22, 461, 266, 48), UpgradeDescription(choices[i]), hintStyle);
                if (SuccessButton(new Rect(x + 36, 528, 238, 54), "รับพลังนี้")) ApplyChoice(i);
            }
        }

        private static void DrawElementIcon(UpgradeType type, Rect rect)
        {
            Texture2D sheet = null;
            bool shotIcon = type == UpgradeType.DoubleShot || type == UpgradeType.Pierce ||
                            type == UpgradeType.Damage || type == UpgradeType.FireRate;
            bool heroIcon = type == UpgradeType.BlastRadius || type == UpgradeType.JamDuration ||
                            type == UpgradeType.FrostPower || type == UpgradeType.ProjectileSpeed;
            bool utilityIcon = type == UpgradeType.Magnet || type == UpgradeType.Speed ||
                               type == UpgradeType.Health || type == UpgradeType.Heal;
            if (shotIcon) sheet = CandyVisuals.ShotIconSheet;
            if (heroIcon) sheet = CandyVisuals.HeroUpgradeIconSheet;
            if (utilityIcon) sheet = CandyVisuals.UtilityIconSheet;
            if (type == UpgradeType.Ice || type == UpgradeType.Fire || type == UpgradeType.Poison || type == UpgradeType.Ricochet)
                sheet = CandyVisuals.IceIcon != null ? CandyVisuals.IceIcon.texture : null;
            if (sheet != null)
            {
                Rect uv = type == UpgradeType.Ice || type == UpgradeType.DoubleShot || type == UpgradeType.BlastRadius || type == UpgradeType.Magnet ? new Rect(0, 0.5f, 0.5f, 0.5f) :
                          type == UpgradeType.Fire || type == UpgradeType.Pierce || type == UpgradeType.JamDuration || type == UpgradeType.Speed ? new Rect(0.5f, 0.5f, 0.5f, 0.5f) :
                          type == UpgradeType.Poison || type == UpgradeType.Damage || type == UpgradeType.FrostPower || type == UpgradeType.Health ? new Rect(0, 0, 0.5f, 0.5f) :
                          new Rect(0.5f, 0, 0.5f, 0.5f);
                GUI.DrawTextureWithTexCoords(rect, sheet, uv, true);
            }
            else if (CandyVisuals.Star != null)
            {
                Color previous = GUI.color;
                GUI.color = new Color(1f, 0.86f, 0.4f);
                GUI.DrawTexture(rect, CandyVisuals.Star.texture, ScaleMode.ScaleToFit, true);
                GUI.color = previous;
            }
        }

        private static void DrawCoinIcon(Rect rect)
        {
            if (CandyVisuals.CandyCoin != null)
                GUI.DrawTexture(rect, CandyVisuals.CandyCoin.texture, ScaleMode.ScaleToFit, true);
        }

        private static string UpgradeName(UpgradeType type)
        {
            switch (type)
            {
                case UpgradeType.Damage: return "ลูกอมพลังแน่น";
                case UpgradeType.FireRate: return "รัวหวานทันใจ";
                case UpgradeType.Health: return "หัวใจมาร์ชเมลโลว์";
                case UpgradeType.Speed: return "รองเท้าติดสปีด";
                case UpgradeType.DoubleShot: return "ฝนลูกอม";
                case UpgradeType.Pierce: return "ลูกอมทะลวง";
                case UpgradeType.Magnet: return "แม่เหล็กดูดขนม";
                case UpgradeType.Ice: return "กระสุนน้ำแข็ง";
                case UpgradeType.Fire: return "กระสุนไฟ";
                case UpgradeType.Poison: return "กระสุนพิษ";
                case UpgradeType.Ricochet: return "ลูกอมเด้งดึ๋ง";
                case UpgradeType.BlastRadius: return "ระเบิดน้ำตาล";
                case UpgradeType.JamDuration: return "แยมเหนียวหนึบ";
                case UpgradeType.FrostPower: return "เย็นจับใจ";
                case UpgradeType.ProjectileSpeed: return "ลูกอมติดจรวด";
                default: return "เติมหัวใจ";
            }
        }

        private static string UpgradeDescription(UpgradeType type)
        {
            switch (type)
            {
                case UpgradeType.Damage: return "ลูกอมแรงขึ้น 23%";
                case UpgradeType.FireRate: return "ยิงถี่ขึ้น 17%";
                case UpgradeType.Health: return "เลือดสูงสุด +25 พร้อมฟื้นฟู";
                case UpgradeType.Speed: return "วิ่งคล่องขึ้นอีกนิด";
                case UpgradeType.DoubleShot: return "ยิงลูกอมเพิ่มในแต่ละชุด";
                case UpgradeType.Pierce: return "ทะลุศัตรูได้เพิ่มอีก 1 ตัว";
                case UpgradeType.Magnet: return "เก็บของจากระยะไกลขึ้น";
                case UpgradeType.Ice: return "ชะลอศัตรู 2.7 วินาที";
                case UpgradeType.Fire: return "เผาศัตรูอย่างต่อเนื่อง";
                case UpgradeType.Poison: return "พิษค่อย ๆ ลดพลังศัตรู";
                case UpgradeType.Ricochet: return "เด้งต่อไปหาเป้าหมายใกล้ ๆ";
                case UpgradeType.BlastRadius: return "ระเบิดกว้างขึ้น 22%";
                case UpgradeType.JamDuration: return "แยมอยู่นานและแผ่กว้างขึ้น";
                case UpgradeType.FrostPower: return "ชะลอศัตรูได้นานขึ้น";
                case UpgradeType.ProjectileSpeed: return "ลูกอมพุ่งไวขึ้น 18%";
                default: return "ฟื้นพลังชีวิต 55%";
            }
        }

        private void DrawChapterClear()
        {
            DrawPopupFrame(new Rect(310, 154, 660, 450), "ภารกิจสำเร็จ", "เยี่ยมเลย ผ่านด่านแล้ว!", new Color32(255, 216, 112, 255));
            GUI.Label(new Rect(355, 329, 570, 50), "ปราบปีศาจ " + kills + " ตัว   •   คะแนน " + score, centeredStyle);
            GUI.Label(new Rect(355, 386, 570, 50), "ลูกอมที่มี  " + wallet, centeredStyle);
            if (SuccessButton(new Rect(430, 460, 420, 72), endless ? "ลุยด่านถัดไป" : "ไปด่านพิเศษกัน!"))
            {
                if (endless) NextChapter(); else BeginBonus();
            }
        }

        private void DrawBonusResult()
        {
            DrawPopupFrame(new Rect(310, 148, 660, 462), "ด่านพิเศษ", bonusWon ? "เก็บคริสตัลครบแล้ว!" : "หมดเวลาแล้ว ไม่เป็นไรนะ",
                bonusWon ? new Color32(139, 237, 209, 255) : new Color32(255, 187, 149, 255));
            GUI.Label(new Rect(350, 329, 580, 55), "คริสตัลที่เก็บได้  " + bonusCrystals + "/" + BonusGoal, centeredStyle);
            GUI.Label(new Rect(350, 392, 580, 55), bonusWon ? "รับ 70 ลูกอม พร้อมฟื้นพลัง!" : "รับ 15 ลูกอมเป็นกำลังใจ", centeredStyle);
            if (SuccessButton(new Rect(430, 480, 420, 72), "ลุยด่านถัดไป")) NextChapter();
        }

        private void DrawEnding(bool win)
        {
            DrawPopupFrame(new Rect(310, 132, 660, 494), win ? "ภารกิจสำเร็จ" : "พักเติมพลังกันก่อน",
                win ? "เมืองขนมปลอดภัยแล้ว!" : "อีกนิดเดียว ลองใหม่กันนะ!", win ? new Color32(255, 218, 114, 255) : new Color32(255, 155, 180, 255));
            GUI.Label(new Rect(348, 316, 584, 58), "คะแนน " + score + "   •   ปราบปีศาจ " + kills + " ตัว", centeredStyle);
            GUI.Label(new Rect(348, 378, 584, 53), "ลูกอมที่มี  " + wallet, centeredStyle);
            bool showFreshRun = !win && checkpoint != null && (checkpoint.chapter > 0 || checkpoint.bonus);
            string retryLabel = checkpoint != null && checkpoint.bonus ? "ลองด่านพิเศษอีกครั้ง" :
                "ลองด่าน " + (chapter + 1) + " อีกครั้ง";
            if (SuccessButton(new Rect(425, 448, 430, showFreshRun ? 57 : 68), win ? "ผจญภัยไม่รู้จบ" : retryLabel))
            {
                if (win) StartRun(true); else RetryStage();
            }
            if (showFreshRun && SoftButton(new Rect(425, 516, 430, 50), "เริ่มใหม่ตั้งแต่ด่าน 1")) StartRun(false);
            if (SoftButton(new Rect(425, showFreshRun ? 578 : 540, 430, showFreshRun ? 43 : 56), "กลับหน้าหลัก"))
            {
                ClearEntities(); heroRenderer.gameObject.SetActive(false); mode = Mode.Title;
            }
        }

        private void DrawBar(Rect rect, float amount, Color color)
        {
            GUI.Box(rect, "", barTrackStyle);
            float fill = (rect.width - 4f) * Mathf.Clamp01(amount);
            if (fill < 1f) return;
            Color previous = GUI.color;
            GUI.color = color;
            GUI.Box(new Rect(rect.x + 2f, rect.y + 2f, fill, Mathf.Max(2f, rect.height - 4f)), "", barFillStyle);
            GUI.color = previous;
        }

        private void DrawPopupFrame(Rect rect, string eyebrow, string title, Color accent)
        {
            Overlay();
            GUI.color = new Color(0.36f, 0.24f, 0.45f, 0.26f);
            GUI.Box(new Rect(rect.x + 5, rect.y + 7, rect.width, rect.height), "", panelStyle);
            GUI.color = Color.white;
            GUI.Box(rect, "", panelStyle);
            DrawAccent(new Rect(rect.x + 35, rect.y + 23, rect.width - 70, 6), accent);
            Color previous = GUI.color;
            GUI.color = Yellow;
            GUI.Label(new Rect(rect.x + rect.width - 83, rect.y + 39, 48, 48), "✦", sparkleStyle);
            GUI.color = previous;
            GUI.Label(new Rect(rect.x + 30, rect.y + 34, rect.width - 60, 34), eyebrow, hintStyle);
            GUI.Label(new Rect(rect.x + 30, rect.y + 73, rect.width - 60, 77), title, headingStyle);
        }

        private void Overlay()
        {
            GUI.color = new Color(0.13f, 0.03f, 0.24f, 0.68f);
            GUI.DrawTexture(new Rect(0, 0, 1280, 720), barTexture);
            GUI.color = Color.white;
        }

        private void DrawAccent(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, barTexture);
            GUI.color = Color.white;
        }

        private bool ActionButton(Rect rect, string label)
        {
            return DecoratedButton(rect, label, buttonStyle, Pink);
        }

        private bool SoftButton(Rect rect, string label)
        {
            return DecoratedButton(rect, label, softButtonStyle, Blue);
        }

        private bool SuccessButton(Rect rect, string label)
        {
            return DecoratedButton(rect, label, successButtonStyle, Green);
        }

        private bool GoldButton(Rect rect, string label)
        {
            return DecoratedButton(rect, label, goldButtonStyle, Yellow);
        }

        private bool DecoratedButton(Rect rect, string label, GUIStyle style, Color accent)
        {
            bool hovered = GUI.enabled && PointerOver(rect);
            int key = Mathf.RoundToInt(rect.x) * 1024 + Mathf.RoundToInt(rect.y);
            float glow;
            buttonGlow.TryGetValue(key, out glow);
            if (Event.current.type == EventType.Repaint)
            {
                glow = Mathf.MoveTowards(glow, hovered ? 1f : 0f, Time.unscaledDeltaTime * 9f);
                buttonGlow[key] = glow;
            }

            if (GUI.enabled && glow > 0.01f)
            {
                bool pressed = hovered && Input.GetMouseButton(0);
                float spread = 3f + glow * 3f;
                Color previous = GUI.color;
                GUI.color = new Color(accent.r, accent.g, accent.b, glow * (pressed ? 0.35f : 0.58f));
                GUI.Box(new Rect(rect.x - spread, rect.y - spread, rect.width + spread * 2f,
                    rect.height + spread * 2f), "", buttonGlowStyle);
                GUI.color = previous;
            }

            Color shadowColor = GUI.color;
            GUI.color = GUI.enabled ? Color.white : new Color(1f, 1f, 1f, 0.58f);
            GUI.Box(new Rect(rect.x, rect.y + 5f, rect.width, rect.height), "", buttonShadowStyle);
            GUI.color = shadowColor;
            bool clicked = GUI.Button(rect, label, style);
            if (GUI.enabled && glow > 0.05f && rect.width >= 150f)
            {
                Color previous = GUI.color;
                GUI.color = new Color(1f, 1f, 1f, glow * 0.76f);
                GUI.Label(new Rect(rect.x + rect.width - 34f,
                    rect.y - 7f + Mathf.Sin(Time.unscaledTime * 7f) * 2f, 23f, 23f), "✦", sparkleStyle);
                GUI.color = previous;
            }
            return clicked;
        }

        private static bool PointerOver(Rect rect)
        {
            float scale = Mathf.Min(Screen.width / 1280f, Screen.height / 720f);
            float offsetX = (Screen.width - 1280f * scale) * 0.5f;
            float offsetY = (Screen.height - 720f * scale) * 0.5f;
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) continue;
                    Vector2 at = new Vector2((touch.position.x - offsetX) / scale,
                        (Screen.height - touch.position.y - offsetY) / scale);
                    if (rect.Contains(at)) return true;
                }
                return false;
            }
            if (Application.isMobilePlatform || !Input.mousePresent) return false;
            Vector3 mouse = Input.mousePosition;
            return rect.Contains(new Vector2((mouse.x - offsetX) / scale,
                (Screen.height - mouse.y - offsetY) / scale));
        }
    }
}
