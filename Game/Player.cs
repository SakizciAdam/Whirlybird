using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;

namespace Whirlybird.Game
{
    public class Player
    {
        public float x = 32, y = 32;
        public float motionX, motionY;

        public readonly float FALL_SPEED = 3;

        public readonly WhirlybirdGame game;

        public readonly int PLATFORM_WIDTH = 50;
        public readonly int PLATFORM_HEIGHT = 10;

        public int deathTick = 0;

        public Player(WhirlybirdGame game)
        {
            this.game = game;
        }

        public void render(Texture2D tex)
        {
            int frame = motionY < 0 ? 0 : 1;
            bool left=motionX < 0;
            Raylib.DrawTexturePro(tex, new Raylib_cs.Rectangle(0, frame * tex.height/2, left ? -tex.width : tex.width, tex.height/2), new Raylib_cs.Rectangle(x,256,tex.width,tex.height/2), new System.Numerics.Vector2(0,0), 0, Raylib_cs.Color.WHITE);
            

         

        }

        public Platform getPlatform()
        {
            Platform plat = null;

            foreach (Platform platform in game.platforms)
            {
                float yDiff = platform.y - y;
                System.Drawing.Rectangle r1 = new System.Drawing.Rectangle((int)x, 0, 16, 32);
                System.Drawing.Rectangle r2 =new System.Drawing.Rectangle((int)platform.x, 0, PLATFORM_WIDTH, PLATFORM_HEIGHT);

                if (yDiff < 270 && yDiff > 250 && motionY > 0 && r1.IntersectsWith(r2))
                {
                    plat = platform;
                    break;
                }
            }

            return plat;
        }

        public void update()
        {
            if (game.died)
            {
                return;
            }

            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
            {
                motionX = -3f;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
            {
                motionX = 3;
            }
    
            
            motionY += 0.2f;

            if (motionY > 6)
            {
                motionY = 6;
            }
            Platform platform = getPlatform();
            if (platform != null)
            {
                motionY = Platform.PlatformType.JUMP == platform.type ? -26 : -16;
                if (platform.type == Platform.PlatformType.SPIKY)
                {
                    Raylib.PlaySound(game.deathSound);
                    game.died = true;
                    return;
                }
                Raylib.PlaySound(game.jumpSound);

            }
            bool lowest = true;
            foreach (Platform platform1 in game.platforms)
            {
                if (y < platform1.y)
                {
                    lowest = false;
                }
            }
            if (lowest)
            {
                deathTick++;
                if (deathTick >= 10)
                {
                    deathTick = 0;
                    Raylib.PlaySound(game.deathSound);
                    game.died = true;
                }
            }



            x += motionX;
            y += motionY;

            x = Math.Max(Math.Min(x, 256 - 16), 0);

            motionX *= 0.95f;
            motionY *= 0.99f;
        }
    }
}
