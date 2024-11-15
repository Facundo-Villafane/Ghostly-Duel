using System;
using Tao.Sdl;
using NAudio.Wave;


namespace MyGame
{
    
    class Program
    {
        static Image background;
        
        static bool gameStarted = false;
        static Image winScreen;
        static Image loseScreen;
        static bool gameOver = false;

        //Variables para la plataforma
        static Image platformInd;
        static int platformX = 0;
        static int platformY = 670;

        // Variables para el menú
        static Image startScreen;
        static Image controlsScreen; // Imagen para la pantalla de controles
        static bool showingControls = false; // Flag para mostrar la pantalla de controles

        // Struct para los botones
        struct Button
        {
            public int x;
            public int y;
            public int width;
            public int height;
            public string text;
            public bool isHovered;
            public Image normal;
            public Image hover;
        }

        // Botones del menú
        static Button playButton;
        static Button controlsButton;
        static Button exitButton;
        static Button backButton; // Para volver del menú de controles

        // Variables de sonido
        static WaveOutEvent startScreenMusic;
        static WaveOutEvent gameMusic;
        static WaveOutEvent playerAttackSound;
        static WaveOutEvent enemyAttackSound;
        static WaveOutEvent playerDamageSound;
        static WaveOutEvent enemyDamageSound;
        static WaveOutEvent winSound;
        static WaveOutEvent loseSound;
        static bool isMusicPlaying = false;

        // Variables de estado del juego
        static string currentAnimation = "idle";
        static int frameDelay = 2;
        
        // Instancias de Player y Enemy
        static Player player = new Player(10, 1, 5.0f); // Salud, daño, velocidad
        static Enemy enemy = new Enemy(10, 1, 3.0f);  // Salud, daño, velocidad

        //PLAYER 1
        public struct Player
        {
            // Variables de estado del jugador
            public int health;
            public int maxHealth;
            public int attackDamage;
            public float moveSpeed;
            public bool facingRight;
            public bool isJumping;
            public int currentJumpHeight;
            public bool isFacingRight;
            public bool isAttacking;
            public bool isDamaged;
            public bool isDead;

            // Posición y animación del jugador
            public int x;
            public int y;
            public int currentFrame;
            public Image[] idleFramesRight;
            public Image[] idleFramesLeft;
            public Image[] moveFramesRight;
            public Image[] moveFramesLeft;
            public Image[] attackFramesRight;
            public Image[] attackFramesLeft;
            public Image[] jumpFramesRight;
            public Image[] jumpFramesLeft;
            public Image[] dieFramesRight;
            public Image[] dieFramesLeft;

            // Variables para la barra de vida y temporizadores de animación
            public Image[] hpPlayer; // Barra de vida del jugador
            public int hpBarX;
            public int hpBarY;
            public int currentHpFrame;
            public int damageFrameCounter;
            public int damageFrameDelay;

            // Variables de control para animación y salto
            public int frameDelay;
            public int frameCounter;
            public int attackFrameDelay;
            public int attackFrameCounter;
            public int jumpFrame;
            public int jumpSpeed;
            public int jumpMaxHeight;
            public bool jumpingUp;
            public int initialY;

            // Variables para la animación de muerte
            public int currentDeadFrame;
            public int frameCounterDead;

            // Constructor para inicializar todos los valores del jugador
            public Player(int maxHealth, int attackDamage, float moveSpeed)
            {
                this.health = maxHealth;
                this.maxHealth = maxHealth;
                this.attackDamage = attackDamage;
                this.moveSpeed = moveSpeed;
                this.facingRight = true;
                this.isJumping = false;
                this.isFacingRight = true;
                this.isAttacking = false;
                this.isDamaged = false;
                this.isDead = false;
                this.x = 100;
                this.y = 472;
                this.currentFrame = 0;

                // Inicialización de los frames de animación
                this.idleFramesRight = new Image[25];
                this.idleFramesLeft = new Image[25];
                this.moveFramesRight = new Image[25];
                this.moveFramesLeft = new Image[25];
                this.attackFramesRight = new Image[16];
                this.attackFramesLeft = new Image[16];
                this.jumpFramesRight = new Image[16];
                this.jumpFramesLeft = new Image[16];
                this.dieFramesRight = new Image[25];
                this.dieFramesLeft = new Image[25];

                // Inicialización de la barra de vida y sus variables
                this.hpPlayer = new Image[11];
                this.hpBarX = 10;
                this.hpBarY = 10;
                this.currentHpFrame = 0;
                this.damageFrameCounter = 0;
                this.damageFrameDelay = 0;

                // Inicialización de las variables de animación
                this.frameDelay = 2;
                this.frameCounter = 0;
                this.attackFrameDelay = 2;
                this.attackFrameCounter = 0;

                // Inicialización de variables de salto
                this.jumpFrame = 3;
                this.jumpSpeed = 18;
                this.jumpMaxHeight = 180;
                this.jumpingUp = true;
                this.initialY = 472;
                this.currentJumpHeight = 0;

                // Inicialización de las variables de animación de muerte
                this.currentDeadFrame = 0;
                this.frameCounterDead = 0;
            }
        }

        //ENEMIGO 1
        public struct Enemy
        {
            // Variables de estado del enemigo
            public int health;
            public int maxHealth;
            public int attackDamage;
            public float moveSpeed;
            public bool isFacingRight;
            public bool isAttacking;
            public bool isDead;

            // Posición y animación del enemigo
            public int x;
            public int y;
            public int currentFrame;
            public int frameDelay;
            public int frameCounter;

            // Animación
            public Image[] idleFramesLeft;
            public Image[] idleFramesRight;
            public Image[] attackFramesLeft;
            public Image[] attackFramesRight;
            public Image[] dieFramesLeft;
            public Image[] dieFramesRight;

            // Variables para la barra de vida del enemigo
            public Image[] hpEnemy; // Barra de vida del enemigo
            public int hpBarX;
            public int hpBarY;
            public int currentHpFrame;

            // Variables de ataque y muerte
            public int attackRange;
            public int currentDeadFrame;
            public int frameCounterDead;

            // Constructor para inicializar los valores del enemigo
            public Enemy(int maxHealth, int attackDamage, float moveSpeed)
            {
                // Estado inicial
                this.health = maxHealth;
                this.maxHealth = maxHealth;
                this.attackDamage = attackDamage;
                this.moveSpeed = moveSpeed;
                this.isFacingRight = false;
                this.isAttacking = false;
                this.isDead = false;

                // Posición y fotogramas de animación
                this.x = 600;
                this.y = 560;
                this.currentFrame = 0;
                this.frameDelay = 3;
                this.frameCounter = 0;

                // Inicialización de los arreglos de animación
                this.idleFramesLeft = new Image[10];
                this.idleFramesRight = new Image[10];
                this.attackFramesLeft = new Image[8];
                this.attackFramesRight = new Image[8];
                this.dieFramesLeft = new Image[19];
                this.dieFramesRight = new Image[19];

                // Barra de vida del enemigo
                this.hpEnemy = new Image[11];
                this.hpBarX = 700;
                this.hpBarY = 10;
                this.currentHpFrame = 0;

                // Control de ataque y muerte
                this.attackRange = 50;
                this.currentDeadFrame = 0;
                this.frameCounterDead = 0;
            }
        }

        // Método para inicializar los botones
        static void InitializeButtons()
        {
            // Inicializar botón de play
            playButton = new Button
            {
                x = 130,
                y = 290,
                width = 200,
                height = 50,
                text = "Jugar",
                isHovered = false
            };
            playButton.normal = Engine.LoadImage("assets/buttons/PLAY.png");
            playButton.hover = Engine.LoadImage("assets/buttons/PLAY-export.png");

            // Inicializar botón de controls
            controlsButton = new Button
            {
                x = 130,
                y = 390,
                width = 200,
                height = 50,
                text = "Controles",
                isHovered = false
            };
            controlsButton.normal = Engine.LoadImage("assets/buttons/CONTROLS.png");
            controlsButton.hover = Engine.LoadImage("assets/buttons/CONTROLS-export.png");

            // Inicializar botón de exit
            exitButton = new Button
            {
                x = 130,
                y = 490,
                width = 200,
                height = 50,
                text = "Salir",
                isHovered = false
            };
            exitButton.normal = Engine.LoadImage("assets/buttons/EXIT.png");
            exitButton.hover = Engine.LoadImage("assets/buttons/EXIT-export.png");

            // Inicializar botón de volver
            backButton = new Button
            {
                x = 150,
                y = 60,
                width = 200,
                height = 50,
                text = "Volver",
                isHovered = false
            };
            backButton.normal = Engine.LoadImage("assets/buttons/BACK.png");
            backButton.hover = Engine.LoadImage("assets/buttons/BACK-export.png");
        }

        // Método para verificar si el mouse está sobre un botón
        static bool IsMouseOverButton(Button button)
        {
            int mouseX, mouseY;
            Sdl.SDL_GetMouseState(out mouseX, out mouseY);
            return mouseX >= button.x && mouseX <= button.x + button.width &&
                   mouseY >= button.y && mouseY <= button.y + button.height;
        }

        //Funcion de rango de ataque player
        static bool IsPlayerInAttackRange()
        {
            int attackRange = 50; // Alcance deseado del ataque del jugador
            return Math.Abs(player.x - enemy.x) <= attackRange;
        }

        // Función para manejar la animación de salto para el jugador
        static void UpdateJump(ref Player player)
        {
            if (player.isJumping)
            {
                // Actualizar el frame de animación
                player.frameCounter++;
                if (player.frameCounter >= player.frameDelay)
                {
                    player.frameCounter = 0;

                    // Actualizar frame de animación
                    if (player.jumpFrame < 15)  // 16 frames en total (0-15)
                    {
                        player.jumpFrame++;
                    }

                    // Manejar el movimiento vertical
                    if (player.jumpingUp)
                    {
                        player.y -= player.jumpSpeed;
                        player.currentJumpHeight += player.jumpSpeed;

                        // Verificar si alcanzamos la altura máxima
                        if (player.currentJumpHeight >= player.jumpMaxHeight)
                        {
                            player.jumpingUp = false;
                        }
                    }
                    else  // Descendiendo
                    {
                        player.y += player.jumpSpeed;

                        // Verificar si hemos llegado al suelo
                        if (player.y >= player.initialY)
                        {
                            // Reiniciar el salto
                            player.y = player.initialY;
                            player.isJumping = false;
                            player.jumpFrame = 0;
                            player.jumpingUp = false;
                            player.currentJumpHeight = 0;
                            currentAnimation = "idle";  // Volver a la animación idle
                        }
                    }
                }
            }
        }

        // Función para mover y atacar del enemigo
        static void UpdateEnemy()
        {
            // Calcular la distancia entre el enemigo y el jugador
            int distanceToPlayer = Math.Abs(player.x - enemy.x);

            // Mover al enemigo hacia el jugador si no está atacando
            if (!enemy.isAttacking)
            {
                if (distanceToPlayer > enemy.attackRange)
                {
                    // Si el enemigo está lejos del jugador, se mueve hacia él
                    if (player.x > enemy.x)
                    {
                        enemy.x += (int)(enemy.moveSpeed);  // Mover hacia la derecha
                        enemy.isFacingRight = true;
                    }
                    else
                    {
                        enemy.x -= (int)(enemy.moveSpeed);  // Mover hacia la izquierda
                        enemy.isFacingRight = false;
                    }
                }
                else
                {
                    // Si el enemigo está cerca del jugador, comienza el ataque
                    enemy.isAttacking = true;
                    enemy.currentFrame = 0;  // Reiniciar la animación de ataque
                }
            }

            // Determinar la cantidad de fotogramas según la animación actual
            int maxFrames = enemy.isAttacking ? enemy.attackFramesRight.Length : enemy.idleFramesRight.Length;

            // Actualizar la animación del enemigo
            enemy.frameCounter++;
            if (enemy.frameCounter >= enemy.frameDelay)
            {
                enemy.currentFrame++;
                enemy.currentFrame %= maxFrames;  // Ciclar entre los fotogramas de la animación actual
                enemy.frameCounter = 0;
            }

            // Si está atacando, aplicar daño al jugador
            if (enemy.isAttacking)
            {
                if (distanceToPlayer <= enemy.attackRange && enemy.currentFrame == 7)  // Aplicar daño en el fotograma 7 del ataque
                {
                    Console.WriteLine("Enemy attacking player");

                    try
                    {
                        var enemyAttackSound = new WaveOutEvent();
                        enemyAttackSound.Init(new AudioFileReader("assets/sounds/enemyAttack.wav"));
                        enemyAttackSound.Play();//Sonido de ataque Enemy
                    }
                    catch (Exception ex)
                    {
                        Log($"Error playing enemy attack sound: {ex.Message}");
                    }

                    if (!player.isDamaged) // Solo aplicar daño si el jugador no está en estado de daño
                    {
                        TakeDamage(enemy.attackDamage);  // Aplica daño al jugador
                    }
                }

                if (enemy.currentFrame == 7)  // Fin del ataque (fotograma 7 porque los fotogramas van de 0 a 7)
                {
                    enemy.isAttacking = false;
                }
            }
        }

        // Método para recibir daño al enemigo
        static void EnemyTakeDamage(int damage)
        {
            if (enemy.isDead) return; // No hay daño si ya está muerto

            Console.WriteLine($"Enemy taking damage. Current health: {enemy.health}, Damage: {damage}");
            enemy.health -= damage;
            if (enemy.health < 0) enemy.health = 0;

            try
            {
                var enemyDamageSound = new WaveOutEvent();
                enemyDamageSound.Init(new AudioFileReader("assets/sounds/enemyDamage.wav"));
                enemyDamageSound.Play(); //Sonido recibir daño enemy
            }
            catch (Exception ex)
            {
                Log($"Error playing enemy damage sound: {ex.Message}");
            }

            UpdateEnemyHpFrame();

            if (enemy.health == 0 && !enemy.isDead)
            {
                enemy.isDead = true;
                enemy.currentDeadFrame = 0;
                Console.WriteLine("Enemy has died. Starting death animation.");
            }
        }

        // Método para recibir daño al jugador
        static void TakeDamage(int damage)
        {
            if (player.isDead) return; // No hay daño si ya estás muerto

            Console.WriteLine($"Taking damage. Current health: {player.health}, Damage: {damage}");
            player.health -= damage;
            if (player.health < 0) player.health = 0;

            try
            {
                var playerDamage = new WaveOutEvent();
                playerDamage.Init(new AudioFileReader("assets/sounds/playerDamage.wav"));
                playerDamageSound.Play(); //Sonido recibir daño Player
            }
            catch (Exception ex)
            {
                Log($"Error playing damage sound: {ex.Message}");
            }

            UpdateHpFrame();

            if (player.health == 0 && !player.isDead)
            {
                player.isDead = true;
                player.currentDeadFrame = 0;
                Console.WriteLine("Player has died. Starting death animation.");
            }
        }

        // Método para actualizar la barra de salud del enemigo
        static void UpdateEnemyHpFrame()
        {
            Console.WriteLine($"UpdateEnemyHpFrame called. Current health: {enemy.health}");
            enemy.currentHpFrame = 10 - enemy.health;  // Supone que la barra tiene 10 frames
            if (enemy.currentHpFrame < 0) enemy.currentHpFrame = 0;
            if (enemy.currentHpFrame > 10) enemy.currentHpFrame = 10;
            Console.WriteLine($"New currentHpFrameE: {enemy.currentHpFrame}");
        }

        // Método para actualizar la barra de salud del jugador
        static void UpdateHpFrame()
        {
            Console.WriteLine($"UpdateHpFrame called. Current health: {player.health}");
            player.currentHpFrame = 10 - player.health;  // Supone que la barra tiene 10 frames
            if (player.currentHpFrame < 0) player.currentHpFrame = 0;
            if (player.currentHpFrame > 10) player.currentHpFrame = 10;
            Console.WriteLine($"New currentHpFrameP: {player.currentHpFrame}");
        }

        static void Main(string[] args)
        {
            try
            {
                Log("Initializing Engine...");
                Engine.Initialize();
                Log("Engine initialized successfully.");
                LoadAssets();
                LoadSoundAssets(); // Carga de los activos de sonido

                Log("Starting game loop...");
                bool programRunning = true;
                while (programRunning)
                {
                    if (!gameStarted)
                    {
                        RenderStartScreen();
                        CheckStartInput();
                    }
                    else if (!gameOver)
                    {
                        CheckInputs();
                        Update();
                        Render();
                    }
                    else
                    {
                        RenderGameOverScreen();
                        CheckGameOverInput(ref programRunning);
                    }

                    Sdl.SDL_Delay(20);
                }
            }
            catch (Exception ex)
            {
                Log($"An error occurred: {ex.Message}");
                Log($"Stack Trace: {ex.StackTrace}");
            }
            Log($"Game Over.");
            Console.ReadKey();
        }

        //Método para carga de Assets imagenes
        static void LoadAssets()
        {
            Log("Loading assets...");
            try
            {
                background = Engine.LoadImage("assets/fondoInd.png");
                Log("Background loaded successfully.");

                platformInd = Engine.LoadImage("assets/tiles/platformInd.png");
                Log("Platform loaded successfully.");

                // Cargar las animaciones del jugador
                LoadPlayerAssets();

                // Cargar las animaciones del enemigo
                LoadEnemyAssets();

                // Cargar barras de vida
                LoadHealthBars();

                // Carga la imagen de la pantalla de inicio
                startScreen = Engine.LoadImage("assets/Start-Screen2.png");
                Log("Start screen loaded successfully.");

                // Cargar nueva pantalla de controles
                controlsScreen = Engine.LoadImage("assets/Controls-Screen.png");
                Log("Controls screen loaded successfully.");

                // Inicializar botones
                InitializeButtons();

                // Carga la imagen de victoria y derrota
                winScreen = Engine.LoadImage("assets/Win-Screen.png");
                loseScreen = Engine.LoadImage("assets/Lose-Screen.png");
                Log("Win and lose screens loaded successfully.");

                // Cargar recursos de sonido
                LoadSoundAssets();
                Log("Sound assets loaded successfully.");

                Log("All assets loaded successfully.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading assets: " + ex.Message, ex);
            }
        }

        // Método para cargar activos del jugador
        static void LoadPlayerAssets()
        {
            for (int i = 0; i < 25; i++)
            {
                player.idleFramesLeft[i] = Engine.LoadImage($"assets/player/idleLeft/idleLeft{i + 1}.png");
                player.idleFramesRight[i] = Engine.LoadImage($"assets/player/idleRight/idleRight{i + 1}.png");
                Log($"Idle frames {i + 1} loaded successfully.");
            }

            for (int m = 0; m < 25; m++)
            {
                player.moveFramesLeft[m] = Engine.LoadImage($"assets/player/moveLeft/moveLeft{m + 1}.png");
                player.moveFramesRight[m] = Engine.LoadImage($"assets/player/moveRight/moveRight{m + 1}.png");
                Log($"Move frames {m + 1} loaded successfully.");
            }

            for (int j = 0; j < 16; j++)
            {
                player.jumpFramesRight[j] = Engine.LoadImage($"assets/player/jumpRight/jumpRight{j + 1}.png");
                player.jumpFramesLeft[j] = Engine.LoadImage($"assets/player/jumpLeft/jumpLeft{j + 1}.png");
                Log($"Jump frames {j + 1} loaded successfully.");
            }

            for (int a = 0; a < 16; a++)
            {
                player.attackFramesRight[a] = Engine.LoadImage($"assets/player/atkRight/atkRight{a + 1}.png");
                player.attackFramesLeft[a] = Engine.LoadImage($"assets/player/atkLeft/atkLeft{a + 1}.png");
                Log($"Attack frames {a + 1} loaded successfully.");
            }

            for (int d = 0; d < 25; d++)
            {
                player.dieFramesRight[d] = Engine.LoadImage($"assets/player/diePRight/diePRight{d + 1}.png");
                player.dieFramesLeft[d] = Engine.LoadImage($"assets/player/diePLeft/diePLeft{d + 1}.png");
                Log($"Die frames {d + 1} loaded successfully.");
            }
        }

        // Método para cargar activos del enemigo
        static void LoadEnemyAssets()
        {
            for (int e = 0; e < 10; e++)
            {
                enemy.idleFramesLeft[e] = Engine.LoadImage($"assets/enemy/enemIdleL/enemIdleL{e + 1}.png");
                enemy.idleFramesRight[e] = Engine.LoadImage($"assets/enemy/enemIdleR/enemIdleR{e + 1}.png");
                Log($"Enemy idle frames {e + 1} loaded successfully.");
            }

            for (int t = 0; t < 8; t++)
            {
                enemy.attackFramesLeft[t] = Engine.LoadImage($"assets/enemy/enemAtkL/enemAtkL{t + 1}.png");
                enemy.attackFramesRight[t] = Engine.LoadImage($"assets/enemy/enemAtkR/enemAtkR{t + 1}.png");
                Log($"Enemy attack frames {t + 1} loaded successfully.");
            }

            for (int k = 0; k < 19; k++)
            {
                enemy.dieFramesLeft[k] = Engine.LoadImage($"assets/enemy/enemDieL/enemDieL{k + 1}.png");
                enemy.dieFramesRight[k] = Engine.LoadImage($"assets/enemy/enemDieR/enemDieR{k + 1}.png");
                Log($"Enemy death frames {k + 1} loaded successfully.");
            }
        }

        // Método para cargar barras de vida
        static void LoadHealthBars()
        {
            for (int h = 0; h < 11; h++)
            {
                player.hpPlayer[h] = Engine.LoadImage($"assets/hpPlayer/hpPlayer{h + 1}.png");
                Log($"Player HP frames {h + 1} loaded successfully.");
            }

            for (int y = 0; y < 11; y++)
            {
                enemy.hpEnemy[y] = Engine.LoadImage($"assets/hpEnemy/hpEnemy{y + 1}.png");
                Log($"Enemy HP frames {y + 1} loaded successfully.");
            }
        }

        //Método para carga de sonidos
        static void LoadSoundAssets()
        {
            try
            {
                startScreenMusic = new WaveOutEvent();
                startScreenMusic.Init(new AudioFileReader("assets/sounds/startScreen.wav"));

                gameMusic = new WaveOutEvent();
                gameMusic.Init(new AudioFileReader("assets/sounds/gameMusic.wav"));

                playerAttackSound = new WaveOutEvent();
                playerAttackSound.Init(new AudioFileReader("assets/sounds/playerAttack.wav"));

                enemyAttackSound = new WaveOutEvent();
                enemyAttackSound.Init(new AudioFileReader("assets/sounds/enemyAttack.wav"));

                playerDamageSound = new WaveOutEvent();
                playerDamageSound.Init(new AudioFileReader("assets/sounds/playerDamage.wav"));

                enemyDamageSound = new WaveOutEvent();
                enemyDamageSound.Init(new AudioFileReader("assets/sounds/enemyDamage.wav"));

                winSound = new WaveOutEvent();
                winSound.Init(new AudioFileReader("assets/sounds/win.wav"));

                loseSound = new WaveOutEvent();
                loseSound.Init(new AudioFileReader("assets/sounds/lose.wav"));
            }
            catch (Exception ex)
            {
                Log($"Error loading sound assets: {ex.Message}");
            }
        }

        // Método para dibujar la start screen
        static void RenderStartScreen()
        {
            Engine.Clear();

            if (showingControls)
            {
                // Renderizar pantalla de controles
                Engine.Draw(controlsScreen, 0, 0);

                // Dibujar botón de volver
                Engine.Draw(backButton.isHovered ? backButton.hover : backButton.normal,
                          backButton.x, backButton.y);
            }
            else
            {
                // Renderizar pantalla de inicio normal
                Engine.Draw(startScreen, 0, 0);

                // Dibujar botones
                Engine.Draw(playButton.isHovered ? playButton.hover : playButton.normal,
                          playButton.x, playButton.y);
                Engine.Draw(controlsButton.isHovered ? controlsButton.hover : controlsButton.normal,
                          controlsButton.x, controlsButton.y);
                Engine.Draw(exitButton.isHovered ? exitButton.hover : exitButton.normal,
                          exitButton.x, exitButton.y);
            }

            Engine.Show();

            if (!isMusicPlaying)
            {
                try
                {
                    startScreenMusic.Play();
                    isMusicPlaying = true;
                }
                catch (Exception ex)
                {
                    Log($"Error playing start screen music: {ex.Message}");
                }
            }
        }

        // Metodo para el imput en la start screen
        static void CheckStartInput()
        {
            // Obtener estado del mouse
            int mouseX, mouseY;
            uint mouseState = (uint)Sdl.SDL_GetMouseState(out mouseX, out mouseY);
            bool mouseClicked = (mouseState & Sdl.SDL_BUTTON(1)) != 0;

            if (showingControls)
            {
                // Actualizar estado hover del botón volver
                backButton.isHovered = IsMouseOverButton(backButton);

                if (mouseClicked && backButton.isHovered)
                {
                    showingControls = false;
                }
            }
            else
            {
                // Actualizar estado hover de los botones
                playButton.isHovered = IsMouseOverButton(playButton);
                controlsButton.isHovered = IsMouseOverButton(controlsButton);
                exitButton.isHovered = IsMouseOverButton(exitButton);

                if (mouseClicked)
                {
                    if (playButton.isHovered)
                    {
                        gameStarted = true;
                        if (isMusicPlaying)
                        {
                            try
                            {
                                startScreenMusic.Stop();
                                gameMusic.Play();
                            }
                            catch (Exception ex)
                            {
                                Log($"Error switching to game music: {ex.Message}");
                            }
                        }
                    }
                    else if (controlsButton.isHovered)
                    {
                        showingControls = true;
                    }
                    else if (exitButton.isHovered)
                    {
                        Environment.Exit(0);
                    }
                }
            }

            // Se puede iniciar el juego con SPACE tambien ✨accesibilidad✨
            if (Engine.KeyPress(Engine.KEY_ESP))
            {
                gameStarted = true;
                if (isMusicPlaying)
                {
                    try
                    {
                        startScreenMusic.Stop();
                        gameMusic.Play();
                    }
                    catch (Exception ex)
                    {
                        Log($"Error switching to game music: {ex.Message}");
                    }
                }
            }
        }

        // Método para resetear el juego
        static void ResetGame()
        {
            // Reinicia todas las variables necesarias a sus valores iniciales
            player.health = 10;
            
            player.currentHpFrame = 0;
            player.x = 100;
            player.y = 472;
            player.isDead = false;
            player.currentDeadFrame = 0;

            enemy.health = 10;
            
            enemy.currentHpFrame = 0;
            enemy.x = 600;
            enemy.y = 560;
            enemy.isDead = false;
            enemy.currentDeadFrame = 0;

            gameOver = false;
            currentAnimation = "idle";

            // Reiniciar música
            try
            {
                if (!isMusicPlaying)
                {
                    winSound?.Stop();
                    loseSound?.Stop();
                    gameMusic.Play();
                    isMusicPlaying = true;
                }
            }
            catch (Exception ex)
            {
                Log($"Error resetting game music: {ex.Message}");
            }

        }

        //Método para imputs de teclado
        static void CheckInputs()
        {
            if (player.isDead) return; // No procesar inputs si el jugador está muerto

            if (Engine.KeyPress(Engine.KEY_LEFT))
            {
                player.x -= (int)player.moveSpeed;
                player.isFacingRight = false;
                if (!player.isJumping && !player.isAttacking)
                {
                    currentAnimation = "moveLeft";
                }
            }
            else if (Engine.KeyPress(Engine.KEY_RIGHT))
            {
                player.x += (int)player.moveSpeed;
                player.isFacingRight = true;
                if (!player.isJumping && !player.isAttacking)
                {
                    currentAnimation = "moveRight";
                }
            }
            else if (!player.isAttacking && !player.isJumping)
            {
                currentAnimation = "idle";
            }

            if (Engine.KeyPress(Engine.KEY_A) && !player.isAttacking && !player.isJumping)
            {
                currentAnimation = "attack";
                player.isAttacking = true;
                player.currentFrame = 0;
                player.attackFrameCounter = 0;

                try
                {
                    var playerAttackSound = new WaveOutEvent();
                    playerAttackSound.Init(new AudioFileReader("assets/sounds/playerAttack.wav"));
                    playerAttackSound.Play(); //Sonido ataque player
                }
                catch (Exception ex)
                {
                    Log($"Error playing player attack sound: {ex.Message}");
                }

                if (IsPlayerInAttackRange() && !enemy.isDead)
                {
                    EnemyTakeDamage(player.attackDamage);
                }
            }

            if (Engine.KeyPress(Engine.KEY_UP) && !player.isJumping && !player.isAttacking)
            {
                player.isJumping = true;
                player.jumpingUp = true;
                player.jumpFrame = 0;
                player.currentJumpHeight = 0;
                currentAnimation = "jump";
            }

            if (Engine.KeyPress(Engine.KEY_ESC))
            {
                Environment.Exit(0);
            }
        }

        //Manejo de errores en consola
        static void Log(string message)
        {
            Console.WriteLine(message);
        }

        //Manejo de animacion de muerte player/enemy
        static void UpdateDeathAnimation(ref bool isDead, ref int frameCounterDead, ref int currentDeadFrame, int maxFrames)
        {
            if (isDead)
            {
                frameCounterDead++;
                if (frameCounterDead >= frameDelay)
                {
                    currentDeadFrame++;
                    if (currentDeadFrame >= maxFrames)
                    {
                        currentDeadFrame = maxFrames - 1; // Mantener el último frame
                        gameOver = true; // Activa la pantalla de fin de juego
                    }
                    frameCounterDead = 0;
                    Console.WriteLine($"Death animation frame updated to: {currentDeadFrame}");
                }
            }
        }

        static void Update()
        {
            // Actualizar animaciones de muerte para jugador y enemigo
            UpdateDeathAnimation(ref player.isDead, ref player.frameCounterDead, ref player.currentDeadFrame, 25);
            UpdateDeathAnimation(ref enemy.isDead, ref enemy.frameCounterDead, ref enemy.currentDeadFrame, 19);

            if (!player.isDead && !enemy.isDead)
            {
                // Control de las animaciones del jugador
                if (player.isJumping)
                {
                    UpdateJump(ref player);
                }
                else if (player.isAttacking)
                {
                    // Control de la animación de ataque
                    player.attackFrameCounter++;
                    if (player.attackFrameCounter >= player.attackFrameDelay)
                    {
                        player.currentFrame++;
                        player.attackFrameCounter = 0;

                        // Aplica daño en un frame específico de la animación de ataque 
                        if (player.currentFrame == 8 && IsPlayerInAttackRange() && !enemy.isDead)
                        {
                            EnemyTakeDamage(player.attackDamage);
                        }

                        // Si la animación de ataque ha terminado
                        if (player.currentFrame >= 15)  // 16 frames, del 0 al 15
                        {
                            player.isAttacking = false;
                            player.currentFrame = 0;
                            currentAnimation = "idle";
                        }
                    }
                }
                else
                {
                    // Actualizar frame según la animación actual
                    player.frameCounter++;
                    if (player.frameCounter >= player.frameDelay)
                    {
                        player.frameCounter = 0;
                        player.currentFrame++;

                        // Determinar el máximo de frames según la animación actual
                        int maxFrames;
                        switch (currentAnimation)
                        {
                            case "idle":
                                maxFrames = 24;  // 25 frames, del 0 al 24
                                break;
                            case "moveLeft":
                                maxFrames = 24;  // 25 frames, del 0 al 24
                                break;
                            case "moveRight":
                                maxFrames = 24;  // 25 frames, del 0 al 24
                                break;
                            case "attack":
                                maxFrames = 15;  // 16 frames, del 0 al 15
                                break;
                            case "jump":
                                maxFrames = 15;  // 16 frames, del 0 al 15
                                break;
                            default:
                                maxFrames = 24;
                                break;
                        }

                        // Resetear el frame si llegamos al final de la animación
                        if (player.currentFrame > maxFrames)
                        {
                            player.currentFrame = 0;
                        }
                    }
                }

                // Actualizar la animación de daño
                if (player.isDamaged)
                {
                    player.damageFrameCounter++;
                    if (player.damageFrameCounter >= player.damageFrameDelay)
                    {
                        player.isDamaged = false;
                        player.damageFrameCounter = 0;
                    }
                }

                // Actualizar enemigo
                UpdateEnemy();

                // Comprobación de muerte del jugador
                if (player.health <= 0 && !player.isDead)
                {
                    player.isDead = true;
                    player.currentDeadFrame = 0;
                    currentAnimation = "die";
                    Console.WriteLine("Player has died. Starting death animation.");
                }

                // Comprobación de muerte del enemigo
                if (enemy.health <= 0 && !enemy.isDead)
                {
                    enemy.isDead = true;
                    enemy.currentDeadFrame = 0;
                    Console.WriteLine("Enemy has died. Starting death animation.");
                }

                // Comprobar si el juego ha terminado
                if (player.isDead || enemy.isDead)
                {
                    if ((player.isDead && player.currentDeadFrame >= 24) ||
                        (enemy.isDead && enemy.currentDeadFrame >= 18))
                    {
                        gameOver = true;
                    }
                }
            }
        }

        static void RenderGameOverScreen()
        {
            Engine.Clear();
            if (player.isDead)
            {
                
                Engine.Draw(loseScreen, 0, 0);
                
                try
                {
                    if (isMusicPlaying)
                    {
                        gameMusic.Stop();
                        
                        isMusicPlaying = false;
                    }
                    loseSound.Play(); //Sonido pantalla de perdida
                }
                catch (Exception ex)
                {
                    Log($"Error playing lose sound: {ex.Message}");
                }
            }
            else if (enemy.isDead)
            {
                
                Engine.Draw(winScreen, 0, 0);
                try
                {
                    if (isMusicPlaying)
                    {
                        gameMusic.Stop();
                        
                        isMusicPlaying = false;
                    }
                    winSound.Play(); //Sonido pantalla ganar
                }
                catch (Exception ex)
                {
                    Log($"Error playing win sound: {ex.Message}");
                }
            }
            Engine.Show();
        }

        static void CheckGameOverInput(ref bool programRunning)
        {
            if (Engine.KeyPress(Engine.KEY_R))
            {
                ResetGame();
                Log($"All player and enemy parameters were reset");
            }
            else if (Engine.KeyPress(Engine.KEY_M))
            {
                ResetGame();
                Log($"All player and enemy parameters were reset");
                gameMusic.Stop(); // para la musica de juego del Reset
                startScreenMusic.Play(); // inicia la musica del start screen
                Log($"Returning to the Start Screen...");
                gameStarted = false;
                Log($"Start Screen rendered.");
            }
            else if (Engine.KeyPress(Engine.KEY_ESC))
            {
                programRunning = false;
            }
        }

        static void Render()
        {
            Engine.Clear();
            Engine.Draw(background, 0, 0);
            Engine.Draw(platformInd, platformX, platformY);

            // Dibujar barra de vida del jugador
            if (player.currentHpFrame >= 0 && player.currentHpFrame < player.hpPlayer.Length)
            {
                Engine.Draw(player.hpPlayer[player.currentHpFrame], player.hpBarX, player.hpBarY);
            }

            // Dibujar jugador
            if (player.isDead)
            {
                Image[] deathFrames = player.isFacingRight ? player.dieFramesRight : player.dieFramesLeft;
                if (player.currentDeadFrame >= 0 && player.currentDeadFrame < deathFrames.Length)
                {
                    Engine.Draw(deathFrames[player.currentDeadFrame], player.x, player.y);
                }
            }
            else
            {
                Image[] currentFrames;
                int frameIndex = player.currentFrame;

                // Seleccionar el array de frames correcto según la animación actual
                switch (currentAnimation)
                {
                    case "idle":
                        currentFrames = player.isFacingRight ? player.idleFramesRight : player.idleFramesLeft;
                        frameIndex %= 25;  // Para asegurar que el índice esté dentro del rango
                        break;
                    case "moveRight":
                    case "moveLeft":
                        currentFrames = player.isFacingRight ? player.moveFramesRight : player.moveFramesLeft;
                        frameIndex %= 25;
                        break;
                    case "attack":
                        currentFrames = player.isFacingRight ? player.attackFramesRight : player.attackFramesLeft;
                        frameIndex %= 16;
                        break;
                    case "jump":
                        currentFrames = player.isFacingRight ? player.jumpFramesRight : player.jumpFramesLeft;
                        frameIndex = player.jumpFrame;
                        if (frameIndex >= currentFrames.Length) frameIndex = currentFrames.Length - 1;
                        break;
                    default:
                        currentFrames = player.idleFramesRight;
                        frameIndex %= 25;
                        break;
                }

                // Verificar que el frame existe antes de dibujarlo
                if (frameIndex >= 0 && frameIndex < currentFrames.Length && currentFrames[frameIndex] != null)
                {
                    Engine.Draw(currentFrames[frameIndex], player.x, player.y);
                }
                else
                {
                    // Si hay algún problema, dibuja el primer frame como fallback
                    Engine.Draw(currentFrames[0], player.x, player.y);
                }
            }

            // Dibujar barra de vida del enemigo
            if (enemy.currentHpFrame >= 0 && enemy.currentHpFrame < enemy.hpEnemy.Length)
            {
                Engine.Draw(enemy.hpEnemy[enemy.currentHpFrame], enemy.hpBarX, enemy.hpBarY);
            }

            // Dibujar enemigo
            if (enemy.isDead)
            {
                Image[] enemyDeathFrames = enemy.isFacingRight ? enemy.dieFramesRight : enemy.dieFramesLeft;
                if (enemy.currentDeadFrame >= 0 && enemy.currentDeadFrame < enemyDeathFrames.Length)
                {
                    Engine.Draw(enemyDeathFrames[enemy.currentDeadFrame], enemy.x, enemy.y);
                }
            }
            else
            {
                Image[] enemyFrames;
                if (enemy.isAttacking)
                {
                    enemyFrames = enemy.isFacingRight ? enemy.attackFramesRight : enemy.attackFramesLeft;
                }
                else
                {
                    enemyFrames = enemy.isFacingRight ? enemy.idleFramesRight : enemy.idleFramesLeft;
                }

                if (enemy.currentFrame >= 0 && enemy.currentFrame < enemyFrames.Length)
                {
                    Engine.Draw(enemyFrames[enemy.currentFrame], enemy.x, enemy.y);
                }
            }

            Engine.Show();
        }
    }
}