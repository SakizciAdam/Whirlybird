using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Whirlybird.Game
{
    public class Platform
    {
        public int x, y;
        public bool remove;

        public PlatformType type = PlatformType.PLAIN;
        private readonly WhirlybirdGame game;

        public readonly Random random = new Random();

        public Platform(WhirlybirdGame game, int x, int y)
        {
            this.game = game;
            this.x = x;
            this.y = y;

            double r = random.NextDouble();

            if (r <= 0.1 && game.maxHeight > 5000)
            {
                this.type = PlatformType.SPIKY;
            }
            else if (r <= 0.15)
            {
                this.type = PlatformType.MOVINGL;
            }
            else if (r <= 0.2)
            {
                this.type = PlatformType.MOVINGR;
            }
            else if (r <= 0.3)
            {
                this.type = PlatformType.JUMP;
            }
        }

        public void update()
        {
            if ((int)(y - game.getCameraY) > 512)
            {
                remove = true;
            }
            switch (type)
            {
                case PlatformType.PLAIN:
                    break;
                case PlatformType.SPIKY:
                    break;
                case PlatformType.MOVINGL:

                    x -= 2;
                    if (x < 0)
                    {
                        x = 0;
                        type = PlatformType.MOVINGR;
                    }

                    break;
                case PlatformType.MOVINGR:
                    x += 2;
                    if (x > 256)
                    {
                        x = 256;
                        type = PlatformType.MOVINGL;
                    }
                    break;
            }
        }


        public void render(Texture2D tex,float cameraY)
        {
            Color color= type==PlatformType.SPIKY ? Color.RED : (type == PlatformType.JUMP ? Color.YELLOW : Color.LIGHTGRAY);



            Raylib.DrawTexture(tex, x, (int)(y - cameraY), color);






        }

        public enum PlatformType
        {
            PLAIN, SPIKY, MOVINGL, MOVINGR, JUMP
        }
    }
}
