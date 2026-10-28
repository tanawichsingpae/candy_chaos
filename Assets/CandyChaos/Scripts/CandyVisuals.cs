using UnityEngine;

namespace CandyChaos
{
    internal static class CandyVisuals
    {
        public static Sprite Hero { get; private set; }
        public static Sprite StrawberryHero { get; private set; }
        public static Sprite BlueberryHero { get; private set; }
        public static Sprite MageHero { get; private set; }
        public static Sprite SniperHero { get; private set; }
        public static Sprite PinkJelly { get; private set; }
        public static Sprite PurpleJelly { get; private set; }
        public static Sprite Cupcake { get; private set; }
        public static Sprite MintMonster { get; private set; }
        public static Sprite LollipopMonster { get; private set; }
        public static Sprite DonutMonster { get; private set; }
        public static Sprite CookieMonster { get; private set; }
        public static Sprite MacaronMonster { get; private set; }
        public static Sprite IceCreamMonster { get; private set; }
        public static Sprite GummyMonster { get; private set; }
        public static Sprite MiniJelly { get; private set; }
        public static Sprite EliteJelly { get; private set; }
        public static Sprite EliteCupcake { get; private set; }
        public static Sprite Dot { get; private set; }
        public static Sprite Star { get; private set; }
        public static Sprite Ring { get; private set; }
        private static Sprite[] floors;
        public static Sprite Floor { get { return floors[0]; } }
        public static Sprite FloorForChapter(int chapter) { return floors[chapter % floors.Length]; }
        public static Texture2D MenuArt { get; private set; }
        public static Texture2D CharacterSheet { get; private set; }
        public static Texture2D ExtraCharacterSheet { get; private set; }
        public static Texture2D AdvancedCharacterSheet { get; private set; }
        public static Texture2D ShotIconSheet { get; private set; }
        public static Texture2D HeroUpgradeIconSheet { get; private set; }
        public static Texture2D UtilityIconSheet { get; private set; }
        public static Sprite MarshmallowShot { get; private set; }
        public static Sprite JamShot { get; private set; }
        public static Sprite BlueberryShot { get; private set; }
        public static Sprite JamPuddle { get; private set; }
        public static Sprite IceIcon { get; private set; }
        public static Sprite FireIcon { get; private set; }
        public static Sprite PoisonIcon { get; private set; }
        public static Sprite RicochetIcon { get; private set; }
        public static Sprite GardenTree { get; private set; }
        public static Sprite DesertRock { get; private set; }
        public static Sprite SnowTree { get; private set; }
        public static Sprite SnowDrift { get; private set; }
        public static Sprite GardenMushroom { get; private set; }
        public static Sprite DesertCactus { get; private set; }
        public static Sprite CrystalProp { get; private set; }
        public static Sprite PortalProp { get; private set; }
        public static Sprite HeartProp { get; private set; }
        public static Sprite ChestProp { get; private set; }

        public static void Load()
        {
            MenuArt = Resources.Load<Texture2D>("Art/MenuArt");
            Texture2D sheet = Resources.Load<Texture2D>("Art/Characters");
            CharacterSheet = sheet;
            if (sheet != null)
            {
                float w = sheet.width * 0.5f;
                float h = sheet.height * 0.5f;
                Hero = Make(sheet, new Rect(0, h, w, h), 460);
                PinkJelly = Make(sheet, new Rect(w, h, w, h), 460);
                PurpleJelly = Make(sheet, new Rect(0, 0, w, h), 460);
                Cupcake = Make(sheet, new Rect(w, 0, w, h), 460);
            }
            Texture2D extra = Resources.Load<Texture2D>("Art/ExtraCharacters");
            ExtraCharacterSheet = extra;
            if (extra != null)
            {
                float w = extra.width * 0.5f;
                float h = extra.height * 0.5f;
                StrawberryHero = Make(extra, new Rect(0, h, w, h), 460);
                BlueberryHero = Make(extra, new Rect(w, h, w, h), 460);
                MintMonster = Make(extra, new Rect(0, 0, w, h), 460);
                LollipopMonster = Make(extra, new Rect(w, 0, w, h), 460);
            }
            Texture2D advanced = Resources.Load<Texture2D>("Art/AdvancedCharacters");
            AdvancedCharacterSheet = advanced;
            if (advanced != null)
            {
                float w = advanced.width * 0.5f;
                float h = advanced.height * 0.5f;
                MageHero = Make(advanced, new Rect(0, h, w, h), 460);
                SniperHero = Make(advanced, new Rect(w, h, w, h), 460);
                GardenMushroom = Make(advanced, new Rect(0, 0, w, h), 460);
                DesertCactus = Make(advanced, new Rect(w, 0, w, h), 460);
            }
            Texture2D icons = Resources.Load<Texture2D>("Art/ElementIcons");
            ShotIconSheet = Resources.Load<Texture2D>("Art/ShotIcons");
            HeroUpgradeIconSheet = Resources.Load<Texture2D>("Art/HeroUpgradeIcons");
            UtilityIconSheet = Resources.Load<Texture2D>("Art/UtilityIcons");
            Texture2D sweets = Resources.Load<Texture2D>("Art/SweetEnemies");
            if (sweets != null)
            {
                float w = sweets.width * 0.5f;
                float h = sweets.height * 0.5f;
                DonutMonster = Make(sweets, new Rect(0, h, w, h), 460);
                CookieMonster = Make(sweets, new Rect(w, h, w, h), 460);
                MacaronMonster = Make(sweets, new Rect(0, 0, w, h), 460);
                IceCreamMonster = Make(sweets, new Rect(w, 0, w, h), 460);
            }
            Texture2D elites = Resources.Load<Texture2D>("Art/SweetElites");
            if (elites != null)
            {
                // The painted figures do not share a geometric center. Slice at the
                // transparent gutters so crowns, frosting and arms stay with their sprite.
                const float splitX = 690f;
                const float splitY = 636f;
                GummyMonster = Make(elites, new Rect(0, splitY, splitX, elites.height - splitY), 460);
                MiniJelly = Make(elites, new Rect(splitX, splitY, elites.width - splitX, elites.height - splitY), 460);
                EliteJelly = Make(elites, new Rect(0, 0, splitX, splitY), 460);
                EliteCupcake = Make(elites, new Rect(splitX, 0, elites.width - splitX, splitY), 460);
            }
            Texture2D combat = Resources.Load<Texture2D>("Art/HeroCombat");
            if (combat != null)
            {
                float w = combat.width * 0.5f;
                float h = combat.height * 0.5f;
                MarshmallowShot = Make(combat, new Rect(0, h, w, h), 460);
                JamShot = Make(combat, new Rect(w, h, w, h), 460);
                BlueberryShot = Make(combat, new Rect(0, 0, w, h), 460);
                JamPuddle = Make(combat, new Rect(w, 0, w, h), 460);
            }
            if (icons != null)
            {
                float w = icons.width * 0.5f;
                float h = icons.height * 0.5f;
                IceIcon = Make(icons, new Rect(0, h, w, h), 460);
                FireIcon = Make(icons, new Rect(w, h, w, h), 460);
                PoisonIcon = Make(icons, new Rect(0, 0, w, h), 460);
                RicochetIcon = Make(icons, new Rect(w, 0, w, h), 460);
            }
            Texture2D stage = Resources.Load<Texture2D>("Art/StageProps");
            if (stage != null)
            {
                float w = stage.width * 0.5f;
                float h = stage.height * 0.5f;
                GardenTree = Make(stage, new Rect(0, h, w, h), 460);
                DesertRock = Make(stage, new Rect(w, h, w, h), 460);
                SnowTree = Make(stage, new Rect(0, 0, w, h), 460);
                SnowDrift = Make(stage, new Rect(w, 0, w, h), 460);
            }
            Texture2D props = Resources.Load<Texture2D>("Art/BonusProps");
            if (props != null)
            {
                float w = props.width * 0.5f;
                float h = props.height * 0.5f;
                CrystalProp = Make(props, new Rect(0, h, w, h), 460);
                PortalProp = Make(props, new Rect(w, h, w, h), 460);
                HeartProp = Make(props, new Rect(0, 0, w, h), 460);
                ChestProp = Make(props, new Rect(w, 0, w, h), 460);
            }
            Dot = CreateDot();
            Star = CreateStar();
            Ring = CreateRing();
            floors = new[] { CreateFloor(0), CreateFloor(1), CreateFloor(2) };
        }

        private static Sprite Make(Texture2D texture, Rect rect, float pixelsPerUnit)
        {
            return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), pixelsPerUnit);
        }

        public static SpriteRenderer Create(string name, Sprite sprite, Vector2 position, float scale, int order, Transform parent = null)
        {
            GameObject obj = new GameObject(name);
            if (parent != null) obj.transform.SetParent(parent);
            obj.transform.position = new Vector3(position.x, position.y, 0);
            obj.transform.localScale = new Vector3(scale, scale, 1);
            SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = order;
            return renderer;
        }

        private static Sprite CreateDot()
        {
            const int size = 64;
            Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            Color[] pixels = new Color[size * size];
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float dx = (x + 0.5f - size * 0.5f) / (size * 0.5f);
                float dy = (y + 0.5f - size * 0.5f) / (size * 0.5f);
                float d = Mathf.Sqrt(dx * dx + dy * dy);
                float alpha = Mathf.Clamp01((1f - d) * 9f);
                float light = Mathf.Clamp01(1.06f - d * 0.25f + Mathf.Max(0, 0.25f - Vector2.Distance(new Vector2(dx, dy), new Vector2(-0.3f, 0.35f))));
                pixels[y * size + x] = new Color(light, light, light, alpha);
            }
            texture.SetPixels(pixels);
            texture.Apply();
            return Make(texture, new Rect(0, 0, size, size), size);
        }

        private static Sprite CreateStar()
        {
            const int size = 64;
            Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            Color[] pixels = new Color[size * size];
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float dx = (x + 0.5f - 32) / 32f;
                float dy = (y + 0.5f - 32) / 32f;
                float angle = Mathf.Atan2(dy, dx);
                float r = Mathf.Sqrt(dx * dx + dy * dy);
                float edge = 0.46f + 0.24f * Mathf.Cos(5f * (angle - Mathf.PI * 0.5f));
                float a = Mathf.Clamp01((edge - r) * 16f);
                pixels[y * size + x] = new Color(1, 1, 1, a);
            }
            texture.SetPixels(pixels);
            texture.Apply();
            return Make(texture, new Rect(0, 0, size, size), size);
        }

        private static Sprite CreateRing()
        {
            const int size = 96;
            Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            Color[] pixels = new Color[size * size];
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float dx = (x + 0.5f - size * 0.5f) / (size * 0.5f);
                float dy = (y + 0.5f - size * 0.5f) / (size * 0.5f);
                float radius = Mathf.Sqrt(dx * dx + dy * dy);
                float alpha = Mathf.Clamp01((radius - 0.66f) * 24f) * Mathf.Clamp01((0.84f - radius) * 24f);
                pixels[y * size + x] = new Color(1, 1, 1, alpha);
            }
            texture.SetPixels(pixels);
            texture.Apply();
            return Make(texture, new Rect(0, 0, size, size), size);
        }

        private static Sprite CreateFloor(int chapter)
        {
            const int width = 512;
            const int height = 288;
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            Color baseColor = chapter == 0 ? new Color32(199, 239, 204, 255) : chapter == 1 ? new Color32(255, 228, 173, 255) : new Color32(225, 244, 255, 255);
            Color accent = chapter == 0 ? new Color32(147, 213, 169, 255) : chapter == 1 ? new Color32(237, 195, 139, 255) : new Color32(186, 222, 245, 255);
            Color[] pixels = new Color[width * height];
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                float wave = Mathf.Sin(x * 0.043f + Mathf.Sin(y * 0.018f) * 2f) + Mathf.Cos(y * 0.051f - x * 0.009f);
                float patch = Mathf.Sin(x * 0.075f) * Mathf.Cos(y * 0.062f);
                float blend = Mathf.Clamp01((wave + patch * 0.7f - 0.25f) * 0.26f);
                Color c = Color.Lerp(baseColor, accent, blend);
                int n = (x * 73856093 ^ y * 19349663) & 4095;
                if (n < 4) c = chapter == 0 ? new Color32(255, 183, 212, 255) : chapter == 1 ? new Color32(255, 247, 200, 255) : new Color32(255, 255, 255, 255);
                else if (n < 8) c = chapter == 0 ? new Color32(119, 199, 168, 255) : chapter == 1 ? new Color32(230, 175, 123, 255) : new Color32(170, 214, 245, 255);
                pixels[y * width + x] = c;
            }
            texture.SetPixels(pixels);
            texture.Apply();
            return Make(texture, new Rect(0, 0, width, height), 24);
        }
    }
}
