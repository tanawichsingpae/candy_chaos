using UnityEngine;

namespace CandyChaos
{
    public sealed partial class CandyGame
    {
        private static readonly string[] StoryTitles =
        {
            "เมืองขนมแสนหวาน", "คืนที่พายุแยมมา", "เหล่าขนมตัวแสบ", "ผู้พิทักษ์ทั้งห้า", "เส้นทางตามหาคริสตัล"
        };

        private static readonly string[] StoryCaptions =
        {
            "เมืองขนมสดใสเสมอ เพราะคริสตัลสายรุ้งคอยรักษาความหวานให้พอดี",
            "คืนหนึ่ง พายุแยมปริศนาพัดมา คริสตัลแตกกระจายไปทั่วอาณาจักร",
            "เหล่าขนมเริ่มอาละวาด แม้แต่ราชาเยลลี่กับราชินีคัพเค้กก็ถูกพลังป่วนครอบงำ",
            "ผู้พิทักษ์ขนมทั้งห้าจึงรวมตัวกัน เพื่อช่วยเพื่อน ๆ และปกป้องบ้านของพวกเขา",
            "ตามหาเศษคริสตัลผ่านสามดินแดน แล้วหยุดจักรพรรดิลูกกวาดให้ได้!"
        };

        private int storyPage;
        private int storyPreviousPage;
        private int storyDirection = 1;
        private int storySwipeFinger = -1;
        private float storySlide = 1f;
        private Vector2 storySwipeStart;
        private Texture2D[] storyArtwork;
        private Texture2D endingArtwork;

        private void BeginStory()
        {
            if (storyArtwork == null)
            {
                storyArtwork = new[]
                {
                    Resources.Load<Texture2D>("Art/StoryCity"),
                    Resources.Load<Texture2D>("Art/StoryStorm"),
                    Resources.Load<Texture2D>("Art/StoryMonsters"),
                    Resources.Load<Texture2D>("Art/StoryGuardians"),
                    Resources.Load<Texture2D>("Art/StoryJourney")
                };
            }
            storyPage = 0;
            storyPreviousPage = 0;
            storySlide = 1f;
            storySwipeFinger = -1;
            mode = Mode.Story;
        }

        private void BeginEndingStory()
        {
            if (endingArtwork == null)
            {
                endingArtwork = Resources.Load<Texture2D>("Art/StoryEnding");
            }
            mode = Mode.EndingStory;
        }

        private void FinishStory()
        {
            storySwipeFinger = -1;
            mode = Mode.CharacterSelect;
        }

        private void StepStory(int direction)
        {
            if (storySlide < 1f) return;
            int next = storyPage + direction;
            if (next < 0) return;
            if (next >= StoryTitles.Length)
            {
                FinishStory();
                return;
            }
            storyPreviousPage = storyPage;
            storyPage = next;
            storyDirection = direction;
            storySlide = 0f;
        }

        private void UpdateStory()
        {
            storySlide = Mathf.Min(1f, storySlide + Time.unscaledDeltaTime / 0.42f);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                FinishStory();
                return;
            }
            if (storySlide < 1f) return;
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                StepStory(1);
                return;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                StepStory(-1);
                return;
            }

            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector2 at = StoryGuiPoint(touch.position);
                if (touch.phase == TouchPhase.Began && storySwipeFinger < 0 &&
                    new Rect(36, 112, 1208, 436).Contains(at))
                {
                    storySwipeFinger = touch.fingerId;
                    storySwipeStart = at;
                }
                if (touch.fingerId != storySwipeFinger) continue;
                if (touch.phase == TouchPhase.Ended)
                {
                    Vector2 swipe = at - storySwipeStart;
                    storySwipeFinger = -1;
                    if (Mathf.Abs(swipe.x) > 75f && Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y) * 1.2f)
                        StepStory(swipe.x < 0f ? 1 : -1);
                }
                else if (touch.phase == TouchPhase.Canceled) storySwipeFinger = -1;
            }
        }

        private static Vector2 StoryGuiPoint(Vector2 position)
        {
            float scale = Mathf.Min(Screen.width / 1280f, Screen.height / 720f);
            float offsetX = (Screen.width - 1280f * scale) * 0.5f;
            float offsetY = (Screen.height - 720f * scale) * 0.5f;
            return new Vector2((position.x - offsetX) / scale,
                (Screen.height - position.y - offsetY) / scale);
        }

        private void DrawStory()
        {
            GUI.Box(new Rect(18, 15, 1244, 690), "", panelStyle);
            GUI.Label(new Rect(54, 31, 880, 55), "บทนำ  ·  " + StoryTitles[storyPage], storyHeadingStyle);
            GUI.Label(new Rect(1080, 37, 145, 43), "หน้า " + (storyPage + 1) + "/" + StoryTitles.Length, centeredStyle);
            for (int i = 0; i < StoryTitles.Length; i++)
                DrawAccent(new Rect(1068 + i * 32, 87, 23, 4), i <= storyPage ? Pink : new Color32(220, 211, 225, 255));

            GUI.BeginGroup(new Rect(36, 112, 1208, 436));
            if (storySlide < 1f)
            {
                float t = Mathf.SmoothStep(0f, 1f, storySlide);
                DrawSlidingStoryPage(storyPreviousPage, -storyDirection * 1208f * t);
                DrawSlidingStoryPage(storyPage, storyDirection * 1208f * (1f - t));
            }
            else DrawSlidingStoryPage(storyPage, 0f);
            GUI.EndGroup();

            GUI.Box(new Rect(36, 560, 1208, 67), "", cardStyle);
            GUI.Label(new Rect(62, 565, 1156, 55), StoryCaptions[storyPage], centeredStyle);

            bool previousEnabled = GUI.enabled;
            GUI.enabled = storyPage > 0 && storySlide >= 1f;
            bool back = SoftButton(new Rect(43, 645, 177, 53), "หน้าก่อน");
            GUI.enabled = previousEnabled;
            if (back) StepStory(-1);
            if (SoftButton(new Rect(790, 645, 190, 53), "ข้ามเรื่องราว")) FinishStory();
            GUI.enabled = previousEnabled && storySlide >= 1f;
            bool next = ActionButton(new Rect(996, 645, 242, 53),
                storyPage == StoryTitles.Length - 1 ? "เลือกผู้พิทักษ์" : "หน้าถัดไป");
            GUI.enabled = previousEnabled;
            if (next) StepStory(1);
        }

        private void DrawSlidingStoryPage(int page, float offsetX)
        {
            GUI.BeginGroup(new Rect(offsetX, 0, 1208, 436));
            GUI.Box(new Rect(0, 0, 1208, 436), "", storyFrameStyle);
            Texture2D artwork = storyArtwork != null ? storyArtwork[page] : null;
            if (artwork == null) artwork = CandyVisuals.MenuArt;
            if (artwork != null)
                GUI.DrawTexture(new Rect(5, 4, 1198, 428), artwork, ScaleMode.ScaleToFit);
            GUI.EndGroup();
        }
        private void DrawEndingStory()
        {
            GUI.Box(new Rect(18, 15, 1244, 690), "", panelStyle);
            GUI.Label(new Rect(54, 31, 880, 55), "บทส่งท้าย  ·  คริสตัลสายรุ้งหวนคืน", storyHeadingStyle);
            GUI.Label(new Rect(1080, 37, 145, 43), "ชัยชนะ!", centeredStyle);
            DrawAccent(new Rect(1068, 87, 150, 4), Yellow);

            GUI.BeginGroup(new Rect(36, 102, 1208, 446));
            GUI.Box(new Rect(0, 0, 1208, 446), "", storyFrameStyle);
            Texture2D art = endingArtwork != null ? endingArtwork : CandyVisuals.MenuArt;
            if (art != null)
                GUI.DrawTexture(new Rect(5, 4, 1198, 438), art, ScaleMode.ScaleToFit);
            GUI.EndGroup();

            GUI.Box(new Rect(36, 560, 1208, 67), "", cardStyle);
            GUI.Label(new Rect(62, 565, 1156, 55),
                "ด้วยความกล้าหาญของเหล่าผู้พิทักษ์ คริสตัลสายรุ้งได้ถูกประกอบกลับคืน ความสงบสุขและความหวานละมุนได้กลับคืนสู่อาณาจักรขนมอีกครั้ง!",
                centeredStyle);

            if (ActionButton(new Rect(480, 645, 320, 53), "ดูผลคะแนน"))
            {
                EndRun(true);
            }
        }
    }
}
