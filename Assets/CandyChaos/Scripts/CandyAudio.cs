using UnityEngine;

namespace CandyChaos
{
    internal sealed class CandyAudio
    {
        private readonly AudioSource effects;
        private readonly AudioSource music;
        private readonly AudioClip[] shots;
        private readonly AudioClip[] abilities;
        private readonly AudioClip[] elements;
        private readonly AudioClip hit;
        private readonly AudioClip pickup;
        private readonly AudioClip hurt;
        private readonly AudioClip upgrade;
        private readonly AudioClip crystal;
        private readonly AudioClip bossAlert;
        private readonly AudioClip bonusStart;
        private readonly AudioClip stageClear;
        private readonly AudioClip explosion;
        private readonly AudioClip splat;
        private readonly AudioClip jellySplit;
        private readonly AudioClip cupcakeBlast;
        private readonly AudioClip eliteAlert;
        private readonly AudioClip enemyVolley;
        private readonly AudioClip enemyFrost;
        private float lastShot = -1f;
        private float lastSplat = -1f;
        private float lastElement = -1f;
        private float lastRicochet = -1f;
        private float lastEnemyVolley = -1f;
        private int elementCycle;

        public CandyAudio(GameObject root)
        {
            effects = root.AddComponent<AudioSource>();
            effects.playOnAwake = false;
            effects.volume = 0.28f;
            music = root.AddComponent<AudioSource>();
            music.playOnAwake = false;
            music.loop = true;
            music.volume = 0.10f;
            shots = new[] { Effect("Marshmallow cannon", 430, 130, 0.15f, 0.29f, 0.12f, 210f),
                            Effect("Strawberry jam shot", 690, 190, 0.16f, 0.25f, 0.19f, 160f),
                            Effect("Blueberry ice shot", 710, 1320, 0.20f, 0.21f, 0.03f, 130f),
                            Arpeggio("Sugar magic shot", new[] { 740f, 988f, 1320f }, 0.22f, 0.22f),
                            Effect("Licorice sniper", 230, 95, 0.25f, 0.33f, 0.42f, 420f) };
            abilities = new[] { Effect("Sugar bomb launch", 330, 75, 0.52f, 0.31f, 0.22f, 100f),
                                Effect("Jam storm", 920, 115, 0.54f, 0.28f, 0.27f, 90f),
                                Arpeggio("Blizzard shield", new[] { 660f, 880f, 1175f, 1568f }, 0.65f, 0.27f),
                                Arpeggio("Magic bloom", new[] { 392f, 523f, 784f, 1047f, 1568f }, 0.72f, 0.27f),
                                Effect("Golden focus", 220, 1180, 0.64f, 0.30f, 0.08f, 18f) };
            elements = new[] { Effect("Ice sparkle", 1100, 1680, 0.17f, 0.15f, 0.02f, 160f),
                               Effect("Fire flicker", 410, 850, 0.16f, 0.16f, 0.31f, 180f),
                               Effect("Poison bubble", 220, 320, 0.20f, 0.16f, 0.12f, 120f),
                               Effect("Candy ricochet", 1250, 720, 0.14f, 0.18f, 0.03f, 240f) };
            hit = Tone("Jelly squish", 280, 120, 0.11f, 0.25f);
            pickup = Tone("Candy pickup", 520, 960, 0.15f, 0.22f);
            hurt = Tone("Hero hurt", 350, 105, 0.21f, 0.29f);
            upgrade = Tone("Upgrade", 460, 980, 0.35f, 0.23f);
            crystal = Tone("Crystal chime", 780, 1450, 0.32f, 0.25f);
            bossAlert = Tone("Boss alert", 390, 95, 0.75f, 0.32f);
            bonusStart = Tone("Portal opens", 260, 880, 0.70f, 0.23f);
            stageClear = Tone("Stage clear", 550, 1320, 0.85f, 0.24f);
            explosion = Effect("Marshmallow burst", 300, 70, 0.30f, 0.33f, 0.30f, 260f);
            splat = Effect("Strawberry jam splat", 480, 130, 0.22f, 0.23f, 0.23f, 180f);
            jellySplit = Effect("Jelly split", 540, 190, 0.28f, 0.25f, 0.19f, 210f);
            cupcakeBlast = Effect("Cupcake blast", 210, 70, 0.43f, 0.33f, 0.38f, 300f);
            eliteAlert = Arpeggio("Elite candy alert", new[] { 440f, 554f, 659f }, 0.43f, 0.20f);
            enemyVolley = Effect("Candy enemy volley", 440, 690, 0.20f, 0.19f, 0.09f, 170f);
            enemyFrost = Effect("Enemy frost volley", 780, 1370, 0.27f, 0.20f, 0.03f, 120f);
            music.clip = Music();
            music.Play();
        }

        public void Shot(int hero, bool ice, bool fire, bool poison)
        {
            if (Time.unscaledTime - lastShot < 0.07f) return;
            lastShot = Time.unscaledTime;
            effects.PlayOneShot(shots[Mathf.Clamp(hero, 0, shots.Length - 1)], hero == 4 ? 0.87f : 0.72f);
            int count = (ice ? 1 : 0) + (fire ? 1 : 0) + (poison ? 1 : 0);
            if (count == 0 || Time.unscaledTime - lastElement < 0.2f) return;
            int choice = elementCycle++ % count;
            int element = ice && choice-- == 0 ? 0 : fire && choice-- == 0 ? 1 : 2;
            effects.PlayOneShot(elements[element], 0.38f);
            lastElement = Time.unscaledTime;
        }

        public void Hit() { effects.PlayOneShot(hit, 0.42f); }
        public void Pickup() { effects.PlayOneShot(pickup, 0.65f); }
        public void Hurt() { effects.PlayOneShot(hurt); }
        public void Upgrade() { effects.PlayOneShot(upgrade); }
        public void Ability(int hero) { effects.PlayOneShot(abilities[Mathf.Clamp(hero, 0, abilities.Length - 1)], 0.86f); }
        public void Ricochet()
        {
            if (Time.unscaledTime - lastRicochet < 0.13f) return;
            lastRicochet = Time.unscaledTime;
            effects.PlayOneShot(elements[3], 0.46f);
        }
        public void EnemyVolley(bool frost)
        {
            if (Time.unscaledTime - lastEnemyVolley < 0.5f) return;
            lastEnemyVolley = Time.unscaledTime;
            effects.PlayOneShot(frost ? enemyFrost : enemyVolley, 0.42f);
        }
        public void JellySplit() { effects.PlayOneShot(jellySplit, 0.72f); }
        public void CupcakeBlast() { effects.PlayOneShot(cupcakeBlast, 0.8f); }
        public void EliteAlert() { effects.PlayOneShot(eliteAlert, 0.62f); }
        public void Crystal() { effects.PlayOneShot(crystal); }
        public void BossAlert() { effects.PlayOneShot(bossAlert); }
        public void BonusStart() { effects.PlayOneShot(bonusStart); }
        public void StageClear() { effects.PlayOneShot(stageClear); }
        public void Explosion() { effects.PlayOneShot(explosion, 0.75f); }
        public void Splat()
        {
            if (Time.unscaledTime - lastSplat < 0.12f) return;
            lastSplat = Time.unscaledTime;
            effects.PlayOneShot(splat, 0.55f);
        }

        private static AudioClip Tone(string name, float start, float end, float seconds, float gain)
        {
            const int rate = 22050;
            int count = Mathf.CeilToInt(rate * seconds);
            float[] samples = new float[count];
            float phase = 0;
            for (int i = 0; i < count; i++)
            {
                float t = i / (float)count;
                float frequency = Mathf.Lerp(start, end, t);
                phase += 2f * Mathf.PI * frequency / rate;
                float envelope = Mathf.Sin(Mathf.PI * t) * (1f - t * 0.35f);
                samples[i] = (Mathf.Sin(phase) + 0.28f * Mathf.Sin(phase * 2f)) * envelope * gain;
            }
            AudioClip clip = AudioClip.Create(name, count, 1, rate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        private static AudioClip Effect(string name, float start, float end, float seconds, float gain, float noiseMix, float attack)
        {
            const int rate = 22050;
            int count = Mathf.CeilToInt(rate * seconds);
            float[] samples = new float[count];
            float phase = 0;
            uint noiseState = 0xA53127u;
            for (int i = 0; i < count; i++)
            {
                float t = i / (float)count;
                float frequency = Mathf.Lerp(start, end, t);
                phase += 2f * Mathf.PI * frequency / rate;
                noiseState = noiseState * 1664525u + 1013904223u;
                float noise = ((noiseState >> 8) & 0xffff) / 32768f - 1f;
                float envelope = Mathf.Clamp01(t * attack) * Mathf.Pow(1f - t, 1.6f);
                float wave = Mathf.Sin(phase) + 0.24f * Mathf.Sin(phase * 2f) + 0.11f * Mathf.Sin(phase * 0.5f);
                samples[i] = Mathf.Clamp((wave * (1f - noiseMix) + noise * noiseMix) * envelope * gain, -1f, 1f);
            }
            AudioClip clip = AudioClip.Create(name, count, 1, rate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        private static AudioClip Arpeggio(string name, float[] notes, float seconds, float gain)
        {
            const int rate = 22050;
            int count = Mathf.CeilToInt(rate * seconds);
            float[] samples = new float[count];
            float phase = 0;
            for (int i = 0; i < count; i++)
            {
                float t = i / (float)count;
                int note = Mathf.Min(notes.Length - 1, Mathf.FloorToInt(t * notes.Length));
                phase += 2f * Mathf.PI * notes[note] / rate;
                float beat = t * notes.Length - note;
                float envelope = Mathf.Clamp01(beat * 24f) * Mathf.Exp(-beat * 3.5f) * (1f - t * 0.25f);
                samples[i] = (Mathf.Sin(phase) + 0.35f * Mathf.Sin(phase * 2f)) * envelope * gain;
            }
            AudioClip clip = AudioClip.Create(name, count, 1, rate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        private static AudioClip Music()
        {
            const int rate = 22050;
            const float duration = 16f;
            int count = (int)(rate * duration);
            float[] samples = new float[count];
            int[] notes = { 523, 659, 784, 659, 587, 698, 880, 698, 523, 659, 784, 988, 880, 698, 659, 587,
                            523, 659, 784, 659, 587, 698, 880, 698, 784, 659, 587, 523, 587, 659, 523, 392 };
            for (int i = 0; i < count; i++)
            {
                float t = i / (float)rate;
                int beat = Mathf.FloorToInt(t * 2f) % notes.Length;
                float within = (t * 2f) % 1f;
                float envelope = Mathf.Exp(-within * 4.5f);
                float melody = Mathf.Sin(2f * Mathf.PI * notes[beat] * t) * envelope * 0.23f;
                float bass = Mathf.Sin(2f * Mathf.PI * (notes[beat] * 0.25f) * t) * 0.11f;
                samples[i] = melody + bass;
            }
            AudioClip clip = AudioClip.Create("Candy waltz", count, 1, rate, false);
            clip.SetData(samples, 0);
            return clip;
        }
    }
}
