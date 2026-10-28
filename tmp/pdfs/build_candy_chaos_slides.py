from pathlib import Path
from PIL import Image
from html import escape

ROOT = Path(r"D:\candy_chaos")
ART = ROOT / "Assets" / "CandyChaos" / "Resources" / "Art"
TMP = ROOT / "tmp" / "pdfs"
ASSETS = TMP / "slides_assets"
ASSETS.mkdir(parents=True, exist_ok=True)


def uri(path):
    return path.resolve().as_uri()


def jpg(name, width=1600):
    src = ART / name
    dst = ASSETS / name.replace(".png", ".jpg")
    image = Image.open(src).convert("RGB")
    if image.width > width:
        image = image.resize((width, round(image.height * width / image.width)), Image.Resampling.LANCZOS)
    image.save(dst, "JPEG", quality=88, optimize=True)
    return uri(dst)


def crop(name, quadrant, out, inset=0):
    image = Image.open(ART / name).convert("RGBA")
    w, h = image.size
    x, y = quadrant
    box = (x * w // 2 + inset, y * h // 2 + inset,
           (x + 1) * w // 2 - inset, (y + 1) * h // 2 - inset)
    part = image.crop(box)
    bbox = part.getbbox()
    if bbox:
        part = part.crop(bbox)
    dst = ASSETS / out
    part.thumbnail((620, 620), Image.Resampling.LANCZOS)
    part.save(dst)
    return uri(dst)


def single(name, out=None):
    src = ART / name
    if out is None:
        return uri(src)
    image = Image.open(src).convert("RGBA")
    bbox = image.getbbox()
    if bbox:
        image = image.crop(bbox)
    image.thumbnail((620, 620), Image.Resampling.LANCZOS)
    dst = ASSETS / out
    image.save(dst)
    return uri(dst)


menu = jpg("MenuArt.png")
city = jpg("StoryCity.png")
storm = jpg("StoryStorm.png")
journey = jpg("StoryJourney.png")
monsters = jpg("StoryMonsters.png")
guardians = jpg("StoryGuardians.png")
hero_combat = jpg("HeroCombat.png", 1200)
stage_props = jpg("StageProps.png", 1100)

heroes = [
    ("มาร์ชเมลโลว์", crop("Characters.png", (0, 0), "marshmallow.png"), "กระสุนใหญ่ ระเบิดวงเล็ก", "Sugar Bomb"),
    ("สตรอว์เบอร์รี", single("StrawberryHero.png", "strawberry.png"), "แยมหนืด ชะลอศัตรู", "Jam Storm"),
    ("บลูเบอร์รี", single("BlueberryHero.png", "blueberry.png"), "กระสุนน้ำแข็ง", "Blizzard Shield"),
    ("จอมเวทน้ำตาล", single("MageHero.png", "mage.png"), "ยิงกระจายวงกว้าง", "Magic Bloom"),
    ("มือสไนเปอร์", single("SniperHero.png", "sniper.png"), "ยิงแรง ช้า และทะลุ", "Golden Focus"),
]

elites = [
    ("เยลลี่", single("EliteJelly.png", "elite_jelly.png"), "แตกตัวเมื่อถูกกำจัด"),
    ("คัพเค้ก", single("EliteCupcake.png", "elite_cupcake.png"), "ระเบิดรอบตัวเมื่อแพ้"),
    ("กัมมี", single("GummyMonster.png", "elite_gummy.png"), "เรียกลูกน้องมาช่วย"),
    ("โดนัท", crop("SweetEnemies.png", (0, 0), "enemy_donut.png"), "ยิงกระสุนรอบทิศ"),
]

upgrades = [
    ("น้ำแข็ง", crop("ElementIcons.png", (0, 0), "icon_ice.png"), "ลดความเร็วศัตรู"),
    ("ไฟ", crop("ElementIcons.png", (1, 0), "icon_fire.png"), "เผาต่อเนื่อง"),
    ("พิษ", crop("ElementIcons.png", (0, 1), "icon_poison.png", 30), "สะสมความเสียหาย"),
    ("กระสุนเด้ง", crop("ElementIcons.png", (1, 1), "icon_bounce.png", 30), "เปลี่ยนไปหาเป้าหมายใกล้เคียง"),
]


def bg(url, overlay=""):
    return f'<img class="bg" src="{url}"><div class="shade {overlay}"></div>'


def page(num, body, classes=""):
    return f'<section id="slide-{num}" class="slide {classes}">{body}<div class="page-num">{num:02d} / 11</div></section>'


slides = []
slides.append(page(1, bg(menu, "cover-shade") + '''
    <div class="cover-copy">
      <div class="kicker light">GAME PRESENTATION</div>
      <h1>CANDY<br>CHAOS</h1>
      <p>เกมยิง 2D ในอาณาจักรขนมที่ความหวานกำลังปั่นป่วน</p>
    </div>''', "dark-page"))

slides.append(page(2, bg(city, "bottom-shade") + '''
    <div class="story-bottom">
      <div class="kicker light">เนื้อเรื่อง</div>
      <h2>เมืองขนมแสนหวาน</h2>
      <p>คริสตัลสายรุ้งคอยรักษาความหวานให้พอดี<br>ทุกชีวิตในเมืองจึงอยู่ร่วมกันอย่างสดใส</p>
    </div>''', "dark-page"))

slides.append(page(3, bg(storm, "bottom-shade") + '''
    <div class="story-bottom">
      <div class="kicker light">จุดเริ่มต้นของความวุ่นวาย</div>
      <h2>คืนที่พายุแยมพัดมา</h2>
      <p>คริสตัลแตกกระจาย ขนมหลายชนิดถูกพลังป่วนครอบงำ<br>ผู้พิทักษ์ทั้งห้าจึงออกตามหาเศษคริสตัลเพื่อช่วยบ้านของตน</p>
    </div>''', "dark-page"))

slides.append(page(4, bg(journey, "bottom-shade") + '''
    <div class="story-bottom journey-copy">
      <div class="kicker light">โครงสร้างแคมเปญ</div>
      <h2>สามดินแดน หนึ่งภารกิจ</h2>
      <p><b>สวนเยลลี่</b> → <b>ทะเลทรายคาราเมล</b> → <b>ทุ่งหิมะน้ำตาล</b></p>
      <small>มีด่านพิเศษตามล่าคริสตัลหลังผ่านด่านหลัก 1 และ 2</small>
    </div>''', "dark-page"))

slides.append(page(5, f'''
    <div class="plain-left">
      <div class="kicker">การออกแบบเกม</div>
      <h2>เล่นง่าย แต่เลือกวิธีสู้ได้</h2>
      <div class="steps">
        <p><span>01</span> เดินหลบและเลือกตำแหน่งในสนาม</p>
        <p><span>02</span> ยิงอัตโนมัติ หรือเล็งยิงเอง</p>
        <p><span>03</span> เก็บน้ำตาลเพื่อเลือกอัปเกรด</p>
        <p><span>04</span> ใช้พลังพิเศษรับมือศัตรูตัวใหญ่</p>
      </div>
      <div class="small-note">รองรับเมาส์และคีย์บอร์ด รวมถึงจอสัมผัสแนวนอน</div>
    </div>
    <img class="combat-art" src="{hero_combat}">
    ''', "cream-page"))

hero_html = "".join(f'''<div class="hero"><img src="{u}"><h3>{n}</h3><p>{normal}</p><small>พิเศษ: {special}</small></div>''' for n, u, normal, special in heroes)
slides.append(page(6, f'''
    <div class="slide-title"><div class="kicker">คาแรกเตอร์</div><h2>ผู้พิทักษ์ขนมทั้งห้า</h2></div>
    <div class="hero-row">{hero_html}</div>
    <div class="bottom-line">อาวุธและท่าเฉพาะตัวทำให้แต่ละรอบมีจังหวะการเล่นต่างกัน</div>
    ''', "lavender-page"))

elite_html = "".join(f'''<div class="enemy"><img src="{u}"><h3>{n}</h3><p>{ability}</p></div>''' for n, u, ability in elites)
slides.append(page(7, f'''
    <div class="slide-title"><div class="kicker">คาแรกเตอร์ศัตรู</div><h2>ขนมตัวแสบมีมากกว่าหน้าตา</h2></div>
    <div class="enemy-row">{elite_html}</div>
    <div class="bottom-line">ศัตรูปกติ 11 แบบ · ตัวใหญ่พิเศษ 7 แบบ · บอสประจำด่าน 3 ตัว</div>
    ''', "cream-page"))

upgrade_html = "".join(f'''<div class="upgrade"><img src="{u}"><h3>{n}</h3><p>{effect}</p></div>''' for n, u, effect in upgrades)
slides.append(page(8, f'''
    <div class="slide-title"><div class="kicker">ทางเลือกในแต่ละรอบ</div><h2>อัปเกรดเปลี่ยนวิธียิง</h2></div>
    <div class="upgrade-row">{upgrade_html}</div>
    <div class="bottom-line">ยังมีลูกกระสุนหลายทาง กระสุนทะลุ ความเร็ว แม่เหล็ก และอัปเกรดเฉพาะฮีโร่</div>
    ''', "lavender-page"))

slides.append(page(9, f'''
    <div class="slide-title"><div class="kicker">ภาพและเสียง</div><h2>โลกขนมที่จำได้ตั้งแต่แรกเห็น</h2></div>
    <div class="art-layout">
      <img src="{stage_props}">
      <div class="art-copy">
        <p><b>ภาพ:</b> เส้นนุ่ม รูปทรงกลม และผิวเยลลี่เงาใส</p>
        <p><b>สี:</b> ม่วง ชมพู เหลือง และฟ้าในโทนพาสเทล</p>
        <p><b>ฉาก:</b> สวน คาราเมล และหิมะมีบรรยากาศต่างกัน</p>
        <p><b>เสียง:</b> เสียงยิงและพลังพิเศษแยกตามตัวละคร</p>
      </div>
    </div>
    ''', "cream-page"))

slides.append(page(10, bg(monsters, "left-shade") + '''
    <div class="tech-copy">
      <div class="kicker light">เครื่องมือที่ใช้พัฒนา</div>
      <h2>สร้างด้วย Unity 6.6</h2>
      <p><b>C#</b> ควบคุมการเล่น ศัตรู อัปเกรด และ UI</p>
      <p><b>2D sprites</b> สำหรับตัวละคร กระสุน และฉาก</p>
      <p><b>เสียงสังเคราะห์ในโค้ด</b> สำหรับการยิงและสกิล</p>
      <p><b>เป้าหมาย:</b> PC และมือถือ/แท็บเล็ตแนวนอน</p>
    </div>
    ''', "dark-page"))

slides.append(page(11, bg(guardians, "top-shade") + '''
    <div class="closing-copy">
      <div class="kicker light">CANDY CHAOS</div>
      <h2>ช่วยเมืองขนม<br>ให้กลับมาหวานพอดี</h2>
      <p>เลือกผู้พิทักษ์ วางแผนอัปเกรด และฝ่าศัตรูขนมทั้งสามดินแดน</p>
    </div>
    ''', "dark-page"))

style = r'''
@page { size: 13.333333in 7.5in; margin: 0; }
* { box-sizing: border-box; }
html, body { margin:0; padding:0; background:#eee; font-family:Tahoma,Arial,sans-serif; }
.slide { position:relative; width:1280px; height:720px; overflow:hidden; page-break-after:always; break-after:page; }
.slide:last-child { page-break-after:auto; break-after:auto; }
.slide img { display:block; }
.bg { position:absolute; inset:0; width:100%; height:100%; object-fit:cover; }
.shade { position:absolute; inset:0; }
.cover-shade { background:linear-gradient(90deg,rgba(39,20,66,.84) 0%,rgba(44,23,68,.62) 43%,rgba(44,23,68,.06) 76%); }
.bottom-shade { background:linear-gradient(0deg,rgba(33,18,57,.94) 0%,rgba(41,22,60,.7) 24%,rgba(41,22,60,0) 62%); }
.left-shade { background:linear-gradient(90deg,rgba(33,18,57,.95) 0%,rgba(40,20,62,.83) 47%,rgba(40,20,62,.05) 86%); }
.top-shade { background:linear-gradient(180deg,rgba(31,16,58,.88) 0%,rgba(31,16,58,.48) 33%,rgba(31,16,58,0) 65%); }
.cream-page { background:#fff8f2; color:#3e2856; }
.lavender-page { background:#f2eafc; color:#3e2856; }
.page-num { position:absolute; right:42px; bottom:25px; font-size:15px; letter-spacing:.08em; color:#7b6794; }
.dark-page .page-num { color:rgba(255,255,255,.85); }
.kicker { font-size:20px; font-weight:700; color:#da5c91; letter-spacing:.07em; }
.kicker.light { color:#ffe4a4; }
h1,h2,h3,p { margin:0; }
h1 { font-family:Tahoma,Arial,sans-serif; font-weight:900; font-size:100px; line-height:.95; letter-spacing:-.055em; }
h2 { font-size:46px; line-height:1.18; font-weight:700; }
h3 { font-size:21px; line-height:1.25; font-weight:700; }
p { font-size:23px; line-height:1.55; }
.cover-copy { position:absolute; left:82px; top:153px; color:#fff; width:720px; }
.cover-copy h1 { margin:18px 0 24px; text-shadow:0 5px 20px rgba(23,7,39,.3); }
.cover-copy p { width:570px; font-size:25px; }
.story-bottom { position:absolute; left:78px; right:70px; bottom:74px; color:white; }
.story-bottom h2 { margin:13px 0 13px; }
.story-bottom p { font-size:25px; }
.journey-copy { bottom:72px; }
.journey-copy small { display:block; font-size:20px; margin-top:7px; color:#ffe9bd; }
.plain-left { position:absolute; left:74px; top:80px; width:650px; }
.plain-left h2 { margin:16px 0 37px; width:640px; }
.steps p { font-size:24px; margin-bottom:19px; }
.steps span { display:inline-block; width:52px; font-weight:700; color:#db5c91; }
.small-note { margin-top:32px; font-size:19px; color:#806988; }
.combat-art { position:absolute; right:0; top:85px; width:510px; height:520px; object-fit:contain; border-radius:24px 0 0 24px; }
.slide-title { position:absolute; left:66px; top:50px; right:60px; }
.slide-title h2 { margin-top:10px; }
.hero-row { position:absolute; top:210px; left:54px; right:54px; display:flex; justify-content:space-between; gap:10px; }
.hero { width:224px; text-align:center; }
.hero img { width:188px; height:188px; object-fit:contain; margin:0 auto 13px; }
.hero h3 { margin-bottom:6px; }
.hero p { font-size:17px; line-height:1.3; min-height:45px; }
.hero small { display:block; font-size:16px; color:#a5447d; margin-top:8px; }
.bottom-line { position:absolute; left:70px; right:70px; bottom:76px; border-top:2px solid rgba(85,52,111,.22); padding-top:19px; font-size:21px; color:#614d72; }
.enemy-row { position:absolute; top:205px; left:65px; right:65px; display:flex; justify-content:space-between; gap:25px; }
.enemy { width:260px; text-align:center; }
.enemy img { width:210px; height:205px; object-fit:contain; margin:0 auto 10px; }
.enemy h3 { margin-bottom:7px; }
.enemy p { font-size:18px; line-height:1.3; }
.upgrade-row { position:absolute; top:205px; left:65px; right:65px; display:flex; justify-content:space-between; gap:25px; }
.upgrade { width:260px; text-align:center; }
.upgrade img { width:215px; height:210px; object-fit:contain; margin:0 auto 9px; }
.upgrade h3 { margin-bottom:7px; }
.upgrade p { font-size:18px; line-height:1.3; }
.art-layout { position:absolute; left:70px; right:70px; top:193px; display:flex; gap:68px; align-items:center; }
.art-layout img { width:440px; height:430px; object-fit:contain; }
.art-copy { flex:1; }
.art-copy p { font-size:23px; line-height:1.42; margin-bottom:22px; }
.art-copy b { color:#ce4e86; }
.tech-copy { position:absolute; left:76px; top:94px; width:760px; color:#fff; }
.tech-copy h2 { margin:15px 0 33px; }
.tech-copy p { margin:0 0 17px; font-size:23px; }
.tech-copy b { color:#ffe2a4; }
.closing-copy { position:absolute; left:83px; top:70px; color:#fff; }
.closing-copy h2 { font-size:56px; margin:15px 0 13px; text-shadow:0 4px 20px #311746; }
.closing-copy p { font-size:23px; max-width:830px; }
@media print { html, body { background:white; } .slide { print-color-adjust:exact; -webkit-print-color-adjust:exact; } }
'''

html = f'''<!doctype html><html lang="th"><head><meta charset="utf-8"><title>Candy Chaos - Presentation</title><style>{style}</style></head><body>{''.join(slides)}</body></html>'''
target = TMP / "candy_chaos_slides.html"
target.write_text(html, encoding="utf-8")
for index, slide in enumerate(slides, 1):
    preview = f'''<!doctype html><html lang="th"><head><meta charset="utf-8"><style>{style}</style></head><body>{slide}</body></html>'''
    (TMP / f"slide-{index:02d}.html").write_text(preview, encoding="utf-8")
print(target)
