# Original art for Candy Chaos

## Noto Sans Thai

The Thai UI uses `NotoSansThai-Regular.ttf` from the Noto Project Authors. It is distributed under the SIL Open Font License 1.1. The license text is stored at `Assets/CandyChaos/Fonts/NotoSansThai-OFL.txt`.

Source: https://github.com/notofonts/thai/releases/tag/NotoSansThai-v2.002

The PNG files in `Assets/CandyChaos/Resources/Art/` were generated with the built-in imagegen tool on 2026-10-27 and 2026-10-28. They are original game assets, using the user's pastel candy and soft jelly references as high-level direction. No images from the search-results screenshot were copied.

## Opening story illustrations

`StoryCity.png`, `StoryStorm.png`, `StoryMonsters.png`, `StoryGuardians.png`, and `StoryJourney.png` were generated on 2026-10-28 as a five-page panoramic cutscene. The images depict the peaceful crystal city, the jam storm shattering the crystal, the candy monsters, the five guardians, and the garden/desert/snow journey. Prompts requested original hand-painted pastel storybook manga scenes without lettering or speech bubbles. The local `MenuArt.png`, character sprite sheets, elite sprite sheet, stage props, and earlier story images were supplied as style and character references. The game displays each panorama as one full-width image and renders Thai narration separately.

## Collectible coin and enemy projectiles

`CandyCoin.png`, `EnemyJamBolt.png`, `EnemyCaramelBolt.png`, `EnemyFrostBolt.png`, and `EnemyMintBolt.png` were generated on 2026-10-28 as separate transparent sprites. Prompts asked for a gold star candy coin and four readable enemy projectiles with distinct jam, caramel, ice, and mint silhouettes. The local `UtilityIcons.png`, `HeroCombat.png`, and `ElementIcons.png` were supplied as style references. The coin is used for arena drops and currency counters; the bolts are used by the corresponding enemy attacks.

The user-cropped individual PNGs (`GummyMonster`, `MiniJelly`, `EliteJelly`, `EliteCupcake`, `StrawberryHero`, `BlueberryHero`, `MintMonster`, `LollipopMonster`, `MageHero`, `SniperHero`, `GardenMushroom`, and `DesertCactus`) now replace the corresponding sprite-sheet cells at runtime. The original sheets remain as art sources.

## MenuArt.png

Prompt:

> Use case: illustration-story. Asset type: original main menu background for a Unity 2D top-down candy shooter game called Candy Chaos. A whimsical candy bakery arena seen from a slightly elevated top-down angle, with rounded soft-lined adorable jelly monsters, a cheerful cupcake monster, a small brave candy hero holding a toy-like candy blaster, frosting swirls, gumdrops and sprinkles. Polished hand-painted 2D game illustration, clean expressive shapes, soft outlines, glossy translucent jelly, pastel lavender, lemon yellow, bubblegum pink and sky blue. Landscape 16:9 composition; keep the central upper-middle area calm and uncluttered so UI title and buttons can be overlaid by code. No text, no logo, no watermark, no existing copyrighted characters. Full opaque background.

## Characters.png

Prompt:

> Use case: stylized-concept. Asset type: original game sprite sheet source for Candy Chaos. On a genuine transparent background, arrange four completely separate full-body cute 2D top-down-ish character sprites in a precise 2 by 2 grid with generous transparent gutters: top left brave small marshmallow hero with pastel blue candy blaster and yellow frosting tuft; top right round pink jelly monster; bottom left lavender jelly monster; bottom right cheerful but mischievous yellow cupcake monster with pink frosting. Rounded soft dark-purple outlines, hand-painted flat pastel colors, subtle glossy highlights, highly readable at 96 pixels tall, all same scale, no overlap between cells, no shadows outside each sprite, no text, no watermark, original characters.

The sheet is split at runtime into four sprites; the source remains as a single PNG to simplify importing.

## ExtraCharacters.png

Prompt:

> Use case: stylized-concept. Asset type: original transparent character sprite sheet for Candy Chaos Unity 2D game, matching a polished soft-outline pastel jelly style. EXACT 2 by 2 grid of four isolated full-body characters with generous transparent gutters and no overlap: top left playable strawberry cream ranger, small heroic pink-and-cream candy person with berry leaf hood and tiny pink candy blaster; top right playable blueberry bubble guardian, small heroic sky-blue round candy person with lavender cape and blue bubble wand; bottom left mischievous mint marshmallow monster, cute square fluffy mint-green body, angry expressive face; bottom right mischievous rainbow lollipop monster, colorful spiral candy head with little arms and legs, angry expressive face. All figures front-facing with slight top-down game sprite readability, same approximate visual scale. Hand-painted 2D game art, rounded soft dark-purple outlines, glossy highlights, pastel pink blue mint lavender yellow. Truly transparent background, no ground, no text, no watermark, no recognizable existing characters.

## BonusProps.png

Prompt:

> Use case: stylized-concept. Asset type: original transparent prop sprite sheet for a polished pastel 2D top-down candy shooter game. EXACT 2 by 2 grid of four separate game props with large transparent gutters: top left glowing gold sugar crystal cluster collectible; top right purple frosting portal gate with sparkling center for bonus stages; bottom left heart-shaped strawberry health candy with glossy shine; bottom right giant golden cupcake treasure chest with a few candy coins. Each isolated object centered in its own quadrant, similar scale and consistent 2D hand-painted game sprite style, soft dark-purple outlines, glossy jelly-like highlights, palette pastel pink lavender sky blue and lemon yellow, transparent background, no ground shadow, no text, no watermark.

The sheet has genuine transparent alpha and is split at runtime into four props for the bonus stages and pickups.

## AdvancedCharacters.png

Prompt (built-in imagegen):

> Use case: stylized-concept. Asset type: original transparent 2D character sprite sheet for Candy Chaos Unity top-down shooter, matching soft dark-purple outlines, hand-painted pastel jelly style and readability of existing small game sprites. EXACT 2 by 2 grid, four fully isolated figures centered in quadrants with generous transparent gutters: top left playable lavender sugar mage with pointed frosting hat, star-tipped candy wand and flowing lilac cape; top right playable lemon-cream candy sniper with a long toy-like licorice rifle, small goggles and golden scarf; bottom left whimsical mushroom-like pink jelly garden decoration; bottom right small frosted cactus candy decoration. Front facing slight top-down perspective, similar scale, clean silhouettes, cute original designs, pastel purple yellow pink blue. Genuinely transparent background, no floor, no text, no watermark.

## ElementIcons.png

Prompt (built-in imagegen):

> Use case: stylized-concept. Asset type: original transparent 2D game icon sprite sheet for Candy Chaos upgrade selection UI. EXACT 2 by 2 grid with four large distinct icons isolated in quadrants: top left icy blue snowflake candy bullet encased in crystal with frost sparkles; top right orange-red flaming candy bullet with clear fire swirl; bottom left vivid purple poisoned candy bullet with tiny green bubbling droplets; bottom right golden ricochet candy bullet with curved motion arrow and sparkles. Polished hand-painted pastel mobile game item icons, soft dark-purple outlines, glossy candy materials, high readability at 64 px, generous transparent gutters, no background, no text, no watermark.

## StageProps.png

Prompt (built-in imagegen):

> Use case: stylized-concept. Asset type: transparent 2D environment prop sheet for Candy Chaos pastel top-down candy shooter. EXACT 2 by 2 grid with four independent scenery props centered in generous transparent quadrants: top left leafy mint-green jelly tree with pink gumdrop fruit for candy grass garden; top right golden sand dune candy rock with caramel stripes and tiny sprinkles for dessert desert; bottom left small snow-covered lavender fir tree made of frosting with blue icicles for winter stage; bottom right rounded snowdrift with sugar crystal shards and blue shadows. Cute polished hand-painted game assets, soft dark-purple outlines, gently glossy candy look, consistent scale, slight top-down view, readable at 80 px. Genuinely transparent background, no lettering, no watermark, no ground plane.

## ShotIcons.png

Prompt (built-in imagegen):

> Use case: stylized-concept. Asset type: second transparent 2D game icon sheet for Candy Chaos upgrade selection UI, visually matching polished pastel glossy candy bullets with dark-purple soft outlines. EXACT 2 by 2 grid with four fully isolated icons centered in generous transparent quadrants: top left three small pink candy bullets spreading outward in a fan to represent multishot; top right one sky-blue candy bullet piercing through two stacked jelly targets to represent penetration; bottom left one oversized red-pink candy bullet with a bright impact star to represent increased damage; bottom right a circular ring of four tiny lemon-yellow candy bullets with speed trails to represent faster fire rate. Strong distinct silhouettes, readable at 64 pixels, no text, no watermark, genuinely transparent background.

## HeroCombat.png

Prompt (built-in imagegen):

> Use case: stylized-concept. Asset type: original transparent 2D combat sprite sheet for Candy Chaos pastel candy top-down shooter. EXACT 2 by 2 grid with four isolated objects centered in spacious transparent quadrants: top left oversized round toasted marshmallow candy cannonball with warm golden crust and bright sugar center; top right sticky strawberry jam projectile, glossy deep pink-red blob with small berry seeds; bottom left blueberry ice bullet, bright blue frosted berry with tiny snowflake facets; bottom right flattened strawberry jam puddle viewed from above, translucent glossy raspberry pink with seeds and rippling rim. Distinct readable shapes at small gameplay scale, soft dark-purple outlines, polished hand-painted cute candy style, no characters, no floor, genuine transparent background, no text, no watermark.

## HeroUpgradeIcons.png

Prompt (built-in imagegen):

> Use case: stylized-concept. Asset type: original transparent 2D upgrade icon sheet for Candy Chaos pastel mobile game, matching glossy candy bullets and soft dark-purple outlines. EXACT 2 by 2 grid, four separate centered icons with generous transparent gutters: top left a marshmallow cannonball bursting into a circular sugar explosion, representing larger splash radius; top right a pink strawberry jam puddle with a small hourglass-shaped sparkle, representing longer sticky puddle duration; bottom left a frosty blueberry bullet with layered snowflake halo, representing longer freeze and chill effect; bottom right a rainbow candy bullet with extra shiny star trails, representing faster projectile travel. Each a compact vivid readable game UI icon, consistent hand-painted polished candy art, genuine transparent background, no labels, no text, no watermark.

## SweetEnemies.png

Prompt (built-in imagegen):

> Use case: stylized-concept. Asset type: transparent 2D enemy sprite sheet for Candy Chaos pastel top-down candy shooter, matching hand-painted glossy jelly characters with soft dark-purple outlines. EXACT 2 by 2 grid, four independent full-body cute but mischievous monster sprites centered in quadrants with generous transparent gutters: top left ring-shaped pink-frosted DONUT monster with sprinkle spikes and tiny legs; top right golden chocolate-chip COOKIE monster with angry brows and little crumb fists; bottom left two-layer lavender and lemon MACARON monster with tiny feet and sideways mischievous eyes; bottom right melting sky-blue ICE CREAM cone monster with frosty spikes and expressive face. Front-facing slight top-down game sprite readability, similar scale, polished soft outlines, pastel pink purple yellow blue, original designs, truly transparent background, no ground, no letters, no watermark.

## SweetElites.png

Prompt (built-in imagegen):

> Use case: stylized-concept. Asset type: transparent 2D elite enemy and minion sprite sheet for Candy Chaos pastel top-down candy shooter, matching soft dark-purple outlines and polished glossy hand-painted jelly art. EXACT 2 by 2 grid, four isolated full-body characters centered in generous transparent quadrants: top left lime-green GUMMY BEAR monster with translucent candy body and grumpy face; top right tiny pink jelly SLIME minion with fierce brows, small enough to read as a split-off child; bottom left giant crowned pink JELLY monster with cracks in its translucent body and an expressive angry face, clearly an elite; bottom right giant frosted CUPCAKE monster with bright cherry crown, threatening eyes and cream bursting from wrapper, clearly an elite. Similar perspective front-facing slight top-down, strong readable silhouettes and original designs, pastel pink green purple yellow blue, genuinely transparent background, no ground, no text, no watermark.

Crop correction (built-in imagegen edit, using the prior SweetElites.png as the edit target; the prior sheet is retained at `ArtSources/SweetElites-original.png`):

> Edit target: the attached Candy Chaos 2D sprite sheet. Keep exactly the same four original candy characters, their expressions, colors, outlines, shading, and transparent background. Fix ONLY their placement and scale for a usable 2-by-2 sprite sheet. Place the green gummy bear fully inside the TOP LEFT quarter; tiny pink jelly fully inside TOP RIGHT; crowned giant pink jelly fully inside BOTTOM LEFT; giant cherry cupcake fully inside BOTTOM RIGHT. Each character must fit entirely within its own quadrant, including crown, ears, arms, droplets, frosting, and shadows. Leave a clear fully transparent gutter of at least 8% of the full canvas width around the central vertical dividing line, and at least 8% of canvas height around the central horizontal dividing line. No visible pixels from any character may cross either center line. Preserve relative visual hierarchy: bear and crowned jelly large, tiny jelly small, cupcake large. No new characters, no added decorations, no background, no text, no watermark.

## UtilityIcons.png

Prompt (built-in imagegen):

> Use case: stylized-concept. Asset type: transparent 2D upgrade icon sheet for Candy Chaos pastel mobile game, consistent with existing glossy candy projectile icons and soft dark-purple outlines. EXACT 2 by 2 grid with four separate centered readable icons and generous transparent gutters: top left candy MAGNET made from striped pink and blue sugar pulling small sweets; top right SPEED BOOTS made of lemon-yellow candy with wing-shaped frosting and dynamic motion sparkle; bottom left glossy strawberry HEART candy with creamy white highlight for maximum health; bottom right glowing mint-green HEALING potion candy drop with a small plus-shaped sparkle. Distinct silhouettes for a 64-pixel UI card, polished hand-painted candy style, genuine transparent background, no words, no logo, no watermark.
