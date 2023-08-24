using Raylib_cs;
using System;

namespace Whirlybird.Game
{
    public class WhirlybirdGame
    {

        public bool died;
        public int maxHeight = -999;

        public Player player;

        public float getCameraY => player.y;
        public List<Platform> platforms= new List<Platform>();
        public readonly Random random = new Random();

        public WhirlybirdGame()
        {
            reset();
            init();
        }

        public void reset()
        {
            player = new Player(this);
            platforms.Clear();
            maxHeight = -999;
            platforms.Add(new Platform(this, 128, 512));
            died = false;
        }

        public Platform getLowestPlatform()
        {
            int y = Int32.MinValue;
            Platform p = null;
            foreach (Platform platform in platforms)
            {
                if (platform.y > y)
                {
                    p = platform;
                }
            }
            return p;
        }

        public Platform getHighestPlatform()
        {
            int y = Int32.MaxValue;
            Platform p = null;
            foreach(Platform platform in platforms)
            {
                if (platform.y < y)
                {
                    p = platform;
                }
            }
            return p;
        }

        public void update()
        {
            if (died && Raylib.IsKeyDown(KeyboardKey.KEY_ENTER))
            {
    
                reset();
            }
            for(int i=0;i<platforms.Count;i++)
            {
                platforms[i].update();

                if (platforms[i].remove)
                {
                    platforms.RemoveAt(i);
                }
            }
    
            if (-player.y > maxHeight)
            {
                maxHeight = (int)-player.y;
            }
            if (-getHighestPlatform().y < maxHeight)
            {
                platforms.Add(new Platform(this, (int)(random.NextDouble() * 256), -64 + (int)(random.NextDouble() * -64) + getHighestPlatform().y));
            }
            player.update();
        }

        public void render()
        {
            if (died)
            {
                Raylib.DrawText( "YOU DIED PRESS ENTER TO RESTART", 128-Raylib.MeasureText("YOU DIED PRESS ENTER TO RESTART",12)/2,256, 12, Color.BLACK);
                return;
            }
            Raylib.DrawText("SCORE "+Math.Max(maxHeight,0), 12 , 24, 16, Color.BLACK);
            Raylib.DrawText("FPS " + fps, 12, 12, 16, Color.BLACK);
            player.render(playerTex);
            foreach(Platform platform in platforms)
            {
                platform.render(platformTex,getCameraY);
            }
        }

        public int fps=0;

        public Texture2D platformTex,playerTex;
        public Sound deathSound, jumpSound;

        public void init()
        {
            Raylib.SetConfigFlags(ConfigFlags.FLAG_VSYNC_HINT);
            
            Raylib.InitWindow(256, 512, "Whirlybird Clone");
            Raylib.InitAudioDevice();
            long start = 0L;
            long fpsCounter = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            int frames=0;
            unsafe
            {
                platformTex = Raylib.LoadTexture((new string(Raylib.GetWorkingDirectory()) + "\\Assets\\Platform.png"));
                playerTex = Raylib.LoadTexture((new string(Raylib.GetWorkingDirectory()) + "\\Assets\\Player.png"));
                deathSound = Raylib.LoadSound((new string(Raylib.GetWorkingDirectory()) + "\\Assets\\hitHurt.wav"));
                jumpSound = Raylib.LoadSound((new string(Raylib.GetWorkingDirectory()) + "\\Assets\\jump.wav"));
            }
            



            while (!Raylib.WindowShouldClose())
            {


                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.WHITE);
                long current = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                if (current - start >= 10l)
                {
                    update();
                    start = current;
                }
                if (current - fpsCounter >= 1000l)
                {
                    fpsCounter = current;
                    fps = frames;
                    frames = 0;
                }

                render();
                frames++;

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();

        }
    }
}
