# Neon Kolobok: Hell Tower

Hardcore 2D arcade platformer для Unity (Windows 11 target), вдохновлённый идеей опасной вертикальной башни, но реализованный как самостоятельная игра с оригинальной структурой секций, ловушками и процедурным неоновым адским стилем.

## Рекомендуемая версия Unity
- **Unity 2022.3.20f1 (LTS)**

## Что реализовано
- Полная структура Unity-проекта (`Assets`, `ProjectSettings`, `Packages`).
- Сцены:
  - `Boot`
  - `MainMenu`
  - `Game`
  - `Results`
- Игровые режимы:
  - `Normal`
  - `Hardcore` (без чекпоинтов)
  - `Practice` (с чекпоинтами)
- Геймплей:
  - управление шариком-колобком (A/D, стрелки, Space)
  - платформы
  - шипы/смертельные зоны
  - лазеры по таймингу
  - вращающиеся опасности
  - исчезающие платформы
  - движущиеся платформы
  - зона лавы
  - boost-зоны (скрипт готов, можно ставить в сцене)
- Системы:
  - `PlayerController`
  - `CameraFollow`
  - `Hazard`
  - `RespawnSystem` + чекпоинты
  - `SaveLoadSystem` (JSON в `Application.persistentDataPath`)
  - `SettingsManager`
  - `AudioManager` (минималистичный синт-звук)
  - `TimerSystem`
  - `UIManager`
  - `GameBootstrap` (инициализация сцен и runtime-сборка vertical slice)
- HUD и UI:
  - меню (Start, Mode, Fullscreen toggle, Quit)
  - пауза (Esc)
  - HUD (режим, таймер, смерти)
  - экран результатов (время, смерти, best time, total deaths)
- Локальные рекорды:
  - best time
  - total deaths
  - last selected mode
  - audio/settings

## Управление
- `A/D` или `←/→` — движение
- `Space` — прыжок
- `R` — рестарт ранa
- `Esc` — пауза
- `F11` — fullscreen toggle

## Логические зоны башни
Сцена `Game` процедурно строит 5 зон:
1. `Entry Rift`
2. `Ember Shafts`
3. `Laser Pits`
4. `Infernal Machinery`
5. `Crown of Hell`

## Архитектура скриптов
- `Assets/Scripts/Core`
- `Assets/Scripts/Player`
- `Assets/Scripts/UI`
- `Assets/Scripts/Environment`
- `Assets/Scripts/Systems`
- `Assets/Scripts/Data`

## Сборка под Windows 11
1. Открыть проект в Unity Hub (версия 2022.3.20f1).
2. Дождаться импорта.
3. Проверить, что сцены в Build Settings уже добавлены:
   - `Boot`, `MainMenu`, `Game`, `Results`
4. `File -> Build Settings -> PC, Mac & Linux Standalone`.
5. Target Platform: `Windows`, Architecture: `x86_64`.
6. Нажать `Build` или `Build and Run`.

## Что протестировано в текущем окружении
> Ограничение: в этом контейнере нет Unity Editor, поэтому нельзя выполнить реальный playtest и сборку `.exe` автоматически.

Проверено:
- структура проекта и папок;
- наличие сцен и их подключение в `EditorBuildSettings.asset`;
- полнота C# исходников по основным системам;
- наличие README и инструкций по сборке.

## Known issues / ограничения
- Визуальные эффекты типа настоящего bloom зависят от post-processing в Unity (в проекте сделан neon look через цвет/контраст/трейлы/пульсации, без URP-профиля).
- Звуки синтезируются runtime для placeholder-эффекта; при желании легко заменить на wav/ogg клипы.
- Под `Practice checkpoints` сохраняется режим и прогресс рекордов, а не промежуточное положение игрока между сессиями.
