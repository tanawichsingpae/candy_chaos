using UnityEngine;

namespace CandyChaos
{
    public sealed partial class CandyGame
    {
        private GUIStyle titleStyle, headingStyle, bodyStyle, smallStyle, buttonStyle, cardStyle, panelStyle, centeredStyle;
        private Texture2D panelTexture, buttonTexture, goldTexture, barTexture;

        private void PrepareGui()
        {
            Font font = Font.CreateDynamicFontFromOSFont(new[] { "Tahoma", "Noto Sans Thai", "Arial" }, 28);
            panelTexture = RoundedTexture(new Color(0.20f, 0.12f, 0.31f, 0.89f));
            buttonTexture = RoundedTexture(new Color(0.39f, 0.25f, 0.57f, 1f));
            goldTexture = RoundedTexture(new Color(0.98f, 0.58f, 0.46f, 1f));
            barTexture = Texture2D.whiteTexture;
            titleStyle = Style(font, 74, FontStyle.Bold, TextAnchor.MiddleCenter, Color.white);
            headingStyle = Style(font, 38, FontStyle.Bold, TextAnchor.MiddleCenter, Color.white);
            bodyStyle = Style(font, 26, FontStyle.Normal, TextAnchor.MiddleLeft, Color.white);
            smallStyle = Style(font, 20, FontStyle.Normal, TextAnchor.MiddleLeft, new Color(1f, 0.92f, 1f));
            centeredStyle = Style(font, 25, FontStyle.Normal, TextAnchor.MiddleCenter, Color.white);
            panelStyle = new GUIStyle(GUI.skin.box) { normal = { background = panelTexture }, border = new RectOffset(12, 12, 12, 12) };
            cardStyle = new GUIStyle(GUI.skin.box) { normal = { background = buttonTexture }, border = new RectOffset(12, 12, 12, 12) };
            buttonStyle = Style(font, 27, FontStyle.Bold, TextAnchor.MiddleCenter, Color.white);
            buttonStyle.normal.background = goldTexture;
            buttonStyle.hover.background = goldTexture;
            buttonStyle.active.background = goldTexture;
            buttonStyle.border = new RectOffset(12, 12, 12, 12);
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

        private void OnGUI()
        {
            if (titleStyle == null) PrepareGui();
            if ((mode == Mode.Title || mode == Mode.CharacterSelect) && CandyVisuals.MenuArt != null)
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), CandyVisuals.MenuArt, ScaleMode.ScaleAndCrop);
            float scale = Mathf.Min(Screen.width / 1280f, Screen.height / 720f);
            float x = (Screen.width - 1280f * scale) * 0.5f;
            float y = (Screen.height - 720f * scale) * 0.5f;
            Matrix4x4 previous = GUI.matrix;
            GUI.matrix = Matrix4x4.TRS(new Vector3(x, y, 0), Quaternion.identity, new Vector3(scale, scale, 1));
            switch (mode)
            {
                case Mode.Title: DrawTitle(); break;
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
            GUI.Box(new Rect(315, 30, 650, 235), "", panelStyle);
            GUI.Label(new Rect(335, 39, 610, 100), "CANDY CHAOS", titleStyle);
            GUI.Label(new Rect(348, 135, 584, 76), "ปกป้องอาณาจักรขนมจากเหล่าปีศาจเยลลี่!", centeredStyle);
            if (ActionButton(new Rect(455, 230, 370, 74), "เริ่มผจญภัย")) { pendingEndless = false; mode = Mode.CharacterSelect; }
            if (PlayerPrefs.GetInt("cc_won", 0) == 1 && ActionButton(new Rect(455, 314, 370, 63), "โหมดไม่สิ้นสุด"))
            { pendingEndless = true; mode = Mode.CharacterSelect; }
            GUI.Label(new Rect(430, 381, 420, 28), "สถิติสูงสุด  " + bestScore, centeredStyle);
            GUI.Box(new Rect(190, 407, 900, 270), "", panelStyle);
            GUI.Label(new Rect(225, 418, 830, 52), "อัปเกรดถาวร    ลูกอม " + wallet, headingStyle);
            DrawShopCard(220, 485, 0, "พลังชีวิต", permanentHealth, "+18 HP");
            DrawShopCard(505, 485, 1, "พลังโจมตี", permanentDamage, "+2.5");
            DrawShopCard(790, 485, 2, "ความเร็ว", permanentSpeed, "+0.25");
            GUI.Label(new Rect(220, 673, 840, 34), "PC: WASD / ลูกศร เพื่อเดิน • คลิกเมาส์เพื่อเล็งยิง • ปล่อยเมาส์เพื่อยิงอัตโนมัติ", smallStyle);
        }

        private void DrawCharacterSelect()
        {
            Overlay();
            GUI.Box(new Rect(20, 48, 1240, 626), "", panelStyle);
            GUI.Label(new Rect(80, 61, 1120, 73), "เลือกผู้พิทักษ์ขนม", headingStyle);
            DrawHeroCard(36, HeroType.Marshmallow, "มาร์ชเมลโลว์", "กระสุนระเบิดวงเล็ก\nSugar Bomb", CandyVisuals.CharacterSheet);
            DrawHeroCard(282, HeroType.Strawberry, "สตรอว์เบอร์รี", "แยมชะลอศัตรู\nJam Storm", CandyVisuals.ExtraCharacterSheet);
            DrawHeroCard(528, HeroType.Blueberry, "บลูเบอร์รี", "กระสุนเยือกแข็ง\nBlizzard Shield", CandyVisuals.ExtraCharacterSheet);
            DrawHeroCard(774, HeroType.Mage, "จอมเวทน้ำตาล", "ยิงวงกว้าง\nMagic Bloom", CandyVisuals.AdvancedCharacterSheet);
            DrawHeroCard(1020, HeroType.Sniper, "มือสไนเปอร์", "ยิงแรงแต่ช้า\nGolden Focus", CandyVisuals.AdvancedCharacterSheet);
            if (ActionButton(new Rect(507, 608, 266, 53), "กลับหน้าแรก")) mode = Mode.Title;
        }

        private void DrawHeroCard(float x, HeroType hero, string name, string description, Texture2D sheet)
        {
            GUI.Box(new Rect(x, 154, 224, 436), "", cardStyle);
            if (sheet != null)
            {
                Rect uv = hero == HeroType.Blueberry || hero == HeroType.Sniper ? new Rect(0.5f, 0.5f, 0.5f, 0.5f) : new Rect(0, 0.5f, 0.5f, 0.5f);
                GUI.DrawTextureWithTexCoords(new Rect(x + 23, 169, 178, 190), sheet, uv, true);
            }
            GUI.Label(new Rect(x + 8, 359, 208, 55), name, centeredStyle);
            GUI.Label(new Rect(x + 10, 416, 204, 75), description, smallStyle);
            if (ActionButton(new Rect(x + 20, 510, 184, 58), "เลือกตัวนี้"))
            {
                selectedHero = hero;
                StartRun(pendingEndless);
            }
        }

        private void DrawShopCard(float x, float y, int type, string name, int rank, string bonus)
        {
            GUI.Box(new Rect(x, y, 270, 166), "", cardStyle);
            GUI.Label(new Rect(x + 14, y + 8, 242, 36), name + "  " + rank + "/5", centeredStyle);
            GUI.Label(new Rect(x + 18, y + 48, 235, 30), "แต่ละระดับ " + bonus, smallStyle);
            string label = rank >= 5 ? "เต็มแล้ว" : "ซื้อ  " + UpgradeCost(rank) + " ลูกอม";
            if (rank < 5 && ActionButton(new Rect(x + 22, y + 94, 226, 56), label)) BuyPermanent(type);
            if (rank >= 5) GUI.Label(new Rect(x + 30, y + 96, 210, 50), label, centeredStyle);
        }

        private void DrawGameplay()
        {
            GUI.Box(new Rect(20, 14, 1240, 94), "", panelStyle);
            GUI.Label(new Rect(44, 22, 440, 34), "♥  " + Mathf.CeilToInt(Mathf.Max(0, hp)) + " / " + Mathf.CeilToInt(maxHp), bodyStyle);
            DrawBar(new Rect(46, 64, 294, 16), hp / maxHp, new Color32(255, 117, 157, 255));
            GUI.Label(new Rect(365, 23, 245, 34), bonusStage ? "คริสตัล " + bonusCrystals + "/" + BonusGoal : "LV " + level + "   ✦ " + sugar + "/" + sugarGoal, bodyStyle);
            DrawBar(new Rect(365, 64, 233, 16), bonusStage ? bonusCrystals / (float)BonusGoal : sugar / (float)sugarGoal, new Color32(255, 218, 102, 255));
            GUI.Label(new Rect(625, 21, 330, 38), bonusStage ? "ด่านพิเศษ: ล่าคริสตัล" : endless ? "ไม่สิ้นสุด " + (chapter + 1) : "ด่าน " + (chapter + 1) + "/3  " + ChapterNames[chapter % 3], bodyStyle);
            GUI.Label(new Rect(627, 61, 300, 29), bonusStage ? "เหลือ " + Mathf.CeilToInt(Mathf.Max(0, BonusTime - stageClock)) + " วิ  •  แต้ม " + score : "เวลา " + Mathf.FloorToInt(stageClock) + " วิ  •  แต้ม " + score, smallStyle);
            GUI.Label(new Rect(963, 30, 165, 43), "ลูกอม " + runCoins, bodyStyle);
            if (ActionButton(new Rect(1140, 26, 92, 60), "Ⅱ")) mode = mode == Mode.Paused ? Mode.Playing : Mode.Paused;
            string abilityLabel = abilityCooldown > 0 ? "พลัง " + Mathf.CeilToInt(abilityCooldown) + " วิ" :
                                  Application.isMobilePlatform ? "พลังพิเศษ" : "พลังพิเศษ  Space";
            if (ActionButton(new Rect(970, 116, 258, 58), abilityLabel)) TriggerAbility();
            int iconIndex = 0;
            if (selectedHero == HeroType.Marshmallow || selectedHero == HeroType.Strawberry || selectedHero == HeroType.Blueberry ||
                iceShots || fireShots || poisonShots || ricochet > 0)
            {
                GUI.Box(new Rect(20, 118, 304, 60), "", panelStyle);
                if (selectedHero == HeroType.Marshmallow) DrawElementIcon(UpgradeType.BlastRadius, new Rect(31 + 55 * iconIndex++, 125, 48, 48));
                if (selectedHero == HeroType.Strawberry) DrawElementIcon(UpgradeType.JamDuration, new Rect(31 + 55 * iconIndex++, 125, 48, 48));
                if (selectedHero == HeroType.Blueberry) DrawElementIcon(UpgradeType.FrostPower, new Rect(31 + 55 * iconIndex++, 125, 48, 48));
                if (iceShots) DrawElementIcon(UpgradeType.Ice, new Rect(31 + 55 * iconIndex++, 125, 48, 48));
                if (fireShots) DrawElementIcon(UpgradeType.Fire, new Rect(31 + 55 * iconIndex++, 125, 48, 48));
                if (poisonShots) DrawElementIcon(UpgradeType.Poison, new Rect(31 + 55 * iconIndex++, 125, 48, 48));
                if (ricochet > 0) DrawElementIcon(UpgradeType.Ricochet, new Rect(31 + 55 * iconIndex++, 125, 48, 48));
            }
            Enemy boss = null;
            foreach (Enemy enemy in enemies) if (enemy.boss) { boss = enemy; break; }
            if (boss != null)
            {
                GUI.Box(new Rect(360, 116, 560, 72), "", panelStyle);
                GUI.Label(new Rect(377, 119, 526, 36), BossNames[chapter % 3], centeredStyle);
                DrawBar(new Rect(395, 160, 490, 14), boss.hp / boss.maxHp, new Color32(202, 123, 239, 255));
            }
            if (bannerClock > 0)
            {
                string message = bonusStage ? "ด่านพิเศษ: เก็บคริสตัล 10 ชิ้น!" : bossSpawned ? "บอสปรากฏตัว: " + BossNames[chapter % 3] :
                    !string.IsNullOrEmpty(eliteBanner) ? eliteBanner : "ด่าน " + (chapter + 1) + " — " + ChapterNames[chapter % 3];
                GUI.Box(new Rect(360, 216, 560, 74), "", panelStyle);
                Color old = GUI.color;
                GUI.color = !string.IsNullOrEmpty(eliteBanner) && !bossSpawned ? new Color32(255, 212, 111, 255) : new Color32(255, 164, 201, 255);
                GUI.DrawTexture(new Rect(380, 278, 520, 4), barTexture);
                GUI.color = old;
                GUI.Label(new Rect(375, 223, 530, 60), message, headingStyle);
            }
            if (Application.isMobilePlatform || Input.touchCount > 0) DrawTouchControls();
            else GUI.Label(new Rect(27, 673, 870, 31), "WASD / ลูกศร: เดิน  •  คลิกค้าง: เล็งยิง  •  ปล่อย: ยิงอัตโนมัติ  •  Space: พลังพิเศษ", smallStyle);
        }

        private void DrawTouchControls()
        {
            float scale = Mathf.Min(Screen.width / 1280f, Screen.height / 720f);
            float offsetX = (Screen.width - 1280f * scale) * 0.5f;
            float offsetY = (Screen.height - 720f * scale) * 0.5f;
            Vector2 left = movingTouch ? new Vector2((leftAnchor.x - offsetX) / scale, (Screen.height - leftAnchor.y - offsetY) / scale) : new Vector2(140, 570);
            if (left.x < 55 || left.x > 400 || left.y < 350 || left.y > 700) left = new Vector2(140, 570);
            GUI.Box(new Rect(left.x - 70, left.y - 70, 140, 140), "", cardStyle);
            if (movingTouch)
            {
                Vector2 offset = Vector2.ClampMagnitude(moveInput, 1f) * 42f;
                GUI.Box(new Rect(left.x - 30 + offset.x, left.y - 30 - offset.y, 60, 60), "", panelStyle);
            }
            GUI.Box(new Rect(1070, 525, 150, 150), "", cardStyle);
            if (manualAim && rightFinger >= 0)
                GUI.Box(new Rect(1115 + aimDirection.x * 42, 570 - aimDirection.y * 42, 60, 60), "", panelStyle);
            else GUI.Label(new Rect(1082, 560, 126, 75), "เล็ง\nยิง", centeredStyle);
            GUI.Label(new Rect(422, 669, 436, 30), "แตะซ้ายเพื่อเดิน • แตะขวาเพื่อเล็งยิง", smallStyle);
        }

        private void DrawPause()
        {
            DrawPopupFrame(new Rect(365, 175, 550, 385), "หยุดพักสักครู่", "หยุดเกม", new Color32(150, 212, 255, 255));
            GUI.Label(new Rect(425, 343, 430, 40), "พร้อมแล้วกลับไปปกป้องอาณาจักรขนม", centeredStyle);
            if (ActionButton(new Rect(460, 397, 360, 62), "เล่นต่อ")) mode = Mode.Playing;
            if (ActionButton(new Rect(460, 477, 360, 62), "กลับหน้าแรก"))
            {
                wallet += runCoins; runCoins = 0; bestScore = Mathf.Max(bestScore, score); Save();
                ClearEntities(); heroRenderer.gameObject.SetActive(false); mode = Mode.Title;
            }
        }

        private void DrawUpgrade()
        {
            DrawPopupFrame(new Rect(110, 88, 1060, 544), "เลเวล " + level + "  •  เลือก 1 จาก 3", "เลือกพลังใหม่!", new Color32(255, 207, 115, 255));
            for (int i = 0; i < 3; i++)
            {
                float x = 145 + i * 335;
                GUI.Box(new Rect(x, 260, 310, 346), "", cardStyle);
                Color previous = GUI.color;
                GUI.color = i == 0 ? new Color32(255, 167, 191, 255) : i == 1 ? new Color32(255, 220, 118, 255) : new Color32(150, 213, 255, 255);
                GUI.DrawTexture(new Rect(x + 24, 272, 262, 5), barTexture);
                GUI.color = previous;
                DrawElementIcon(choices[i], new Rect(x + 103, 282, 96, 96));
                GUI.Label(new Rect(x + 17, 379, 276, 52), UpgradeName(choices[i]), centeredStyle);
                GUI.Label(new Rect(x + 22, 432, 266, 65), UpgradeDescription(choices[i]), centeredStyle);
                if (ActionButton(new Rect(x + 36, 526, 238, 57), "เลือกพลังนี้")) ApplyChoice(i);
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

        private static string UpgradeName(UpgradeType type)
        {
            switch (type)
            {
                case UpgradeType.Damage: return "กระสุนหวานจัด";
                case UpgradeType.FireRate: return "รัวลูกกวาด";
                case UpgradeType.Health: return "หัวใจมาร์ชเมลโลว์";
                case UpgradeType.Speed: return "รองเท้าน้ำตาล";
                case UpgradeType.DoubleShot: return "ยิงหลายเม็ด";
                case UpgradeType.Pierce: return "ลูกอมทะลุ";
                case UpgradeType.Magnet: return "แม่เหล็กขนม";
                case UpgradeType.Ice: return "กระสุนน้ำแข็ง";
                case UpgradeType.Fire: return "กระสุนไฟ";
                case UpgradeType.Poison: return "กระสุนพิษ";
                case UpgradeType.Ricochet: return "กระสุนเด้ง";
                case UpgradeType.BlastRadius: return "ระเบิดน้ำตาล";
                case UpgradeType.JamDuration: return "แยมเข้มข้น";
                case UpgradeType.FrostPower: return "น้ำแข็งจัด";
                case UpgradeType.ProjectileSpeed: return "ลูกอมพุ่งไว";
                default: return "เติมพลัง";
            }
        }

        private static string UpgradeDescription(UpgradeType type)
        {
            switch (type)
            {
                case UpgradeType.Damage: return "พลังโจมตี +23%";
                case UpgradeType.FireRate: return "ยิงเร็วขึ้น 17%";
                case UpgradeType.Health: return "เลือดสูงสุด +25 และฟื้นเลือด";
                case UpgradeType.Speed: return "เคลื่อนที่เร็วขึ้น";
                case UpgradeType.DoubleShot: return "เพิ่มจำนวนกระสุนต่อชุด";
                case UpgradeType.Pierce: return "กระสุนทะลุศัตรูอีก 1 ตัว";
                case UpgradeType.Magnet: return "ดูดของที่ตกได้ไกลขึ้น";
                case UpgradeType.Ice: return "ศัตรูช้าลง 2.7 วินาที";
                case UpgradeType.Fire: return "เผาไหม้ศัตรูต่อเนื่อง";
                case UpgradeType.Poison: return "พิษสะสมสร้างความเสียหาย";
                case UpgradeType.Ricochet: return "เด้งไปหาเป้าหมายใกล้เคียง";
                case UpgradeType.BlastRadius: return "รัศมีระเบิดเพิ่ม 22%";
                case UpgradeType.JamDuration: return "แยมอยู่ได้นานและกว้างขึ้น";
                case UpgradeType.FrostPower: return "แช่เย็นได้นานและช้าลง";
                case UpgradeType.ProjectileSpeed: return "กระสุนเดินทางเร็วขึ้น 18%";
                default: return "ฟื้นเลือด 55%";
            }
        }

        private void DrawChapterClear()
        {
            DrawPopupFrame(new Rect(310, 154, 660, 450), "ภารกิจสำเร็จ", "ผ่านด่านแล้ว!", new Color32(255, 216, 112, 255));
            GUI.Label(new Rect(355, 329, 570, 50), "กำจัด " + kills + " ตัว   •   คะแนน " + score, centeredStyle);
            GUI.Label(new Rect(355, 386, 570, 50), "ลูกอมสะสม  " + wallet, centeredStyle);
            if (ActionButton(new Rect(430, 460, 420, 72), endless ? "ไปด่านถัดไป" : "เข้าสู่ด่านพิเศษ"))
            {
                if (endless) NextChapter(); else BeginBonus();
            }
        }

        private void DrawBonusResult()
        {
            DrawPopupFrame(new Rect(310, 148, 660, 462), "ผลการล่าคริสตัล", bonusWon ? "ผ่านด่านพิเศษ!" : "หมดเวลาด่านพิเศษ",
                bonusWon ? new Color32(139, 237, 209, 255) : new Color32(255, 187, 149, 255));
            GUI.Label(new Rect(350, 329, 580, 55), "เก็บคริสตัล " + bonusCrystals + "/" + BonusGoal, centeredStyle);
            GUI.Label(new Rect(350, 392, 580, 55), bonusWon ? "โบนัส 70 ลูกอม + ฟื้นพลังชีวิต" : "รางวัลเข้าร่วม 15 ลูกอม", centeredStyle);
            if (ActionButton(new Rect(430, 480, 420, 72), "ไปด่านถัดไป")) NextChapter();
        }

        private void DrawEnding(bool win)
        {
            DrawPopupFrame(new Rect(310, 132, 660, 494), win ? "ชัยชนะของเหล่าผู้พิทักษ์" : "ภารกิจสิ้นสุด",
                win ? "อาณาจักรขนมปลอดภัย!" : "ขนมบุกเมืองแล้ว!", win ? new Color32(255, 218, 114, 255) : new Color32(255, 155, 180, 255));
            GUI.Label(new Rect(348, 316, 584, 58), "คะแนน " + score + "   •   กำจัด " + kills + " ตัว", centeredStyle);
            GUI.Label(new Rect(348, 378, 584, 53), "ลูกอมสะสม  " + wallet, centeredStyle);
            bool showFreshRun = !win && checkpoint != null && (checkpoint.chapter > 0 || checkpoint.bonus);
            string retryLabel = checkpoint != null && checkpoint.bonus ? "ลองด่านพิเศษอีกครั้ง" :
                "ลองด่าน " + (chapter + 1) + " อีกครั้ง";
            if (ActionButton(new Rect(425, 448, 430, showFreshRun ? 57 : 68), win ? "เล่นโหมดไม่สิ้นสุด" : retryLabel))
            {
                if (win) StartRun(true); else RetryStage();
            }
            if (showFreshRun && ActionButton(new Rect(425, 516, 430, 50), "เริ่มใหม่ตั้งแต่ด่าน 1")) StartRun(false);
            if (ActionButton(new Rect(425, showFreshRun ? 578 : 540, 430, showFreshRun ? 43 : 56), "กลับหน้าแรก"))
            {
                ClearEntities(); heroRenderer.gameObject.SetActive(false); mode = Mode.Title;
            }
        }

        private void DrawBar(Rect rect, float amount, Color color)
        {
            GUI.color = new Color(0.12f, 0.08f, 0.19f, 0.8f);
            GUI.DrawTexture(rect, barTexture);
            GUI.color = color;
            GUI.DrawTexture(new Rect(rect.x + 3, rect.y + 3, Mathf.Max(0, rect.width - 6) * Mathf.Clamp01(amount), rect.height - 6), barTexture);
            GUI.color = Color.white;
        }

        private void DrawPopupFrame(Rect rect, string eyebrow, string title, Color accent)
        {
            Overlay();
            GUI.color = new Color(0.04f, 0.02f, 0.09f, 0.5f);
            GUI.Box(new Rect(rect.x + 8, rect.y + 12, rect.width, rect.height), "", panelStyle);
            GUI.color = Color.white;
            GUI.Box(rect, "", panelStyle);
            GUI.color = accent;
            GUI.DrawTexture(new Rect(rect.x + 35, rect.y + 23, rect.width - 70, 5), barTexture);
            GUI.color = Color.white;
            GUI.Label(new Rect(rect.x + 30, rect.y + 34, rect.width - 60, 34), eyebrow, centeredStyle);
            GUI.Label(new Rect(rect.x + 30, rect.y + 73, rect.width - 60, 77), title, headingStyle);
            GUI.color = accent;
            GUI.DrawTexture(new Rect(rect.x + 95, rect.y + 158, rect.width - 190, 2), barTexture);
            GUI.color = Color.white;
        }

        private void Overlay()
        {
            GUI.color = new Color(0.12f, 0.07f, 0.21f, 0.78f);
            GUI.DrawTexture(new Rect(0, 0, 1280, 720), barTexture);
            GUI.color = Color.white;
        }

        private bool ActionButton(Rect rect, string label)
        {
            return GUI.Button(rect, label, buttonStyle);
        }
    }
}
