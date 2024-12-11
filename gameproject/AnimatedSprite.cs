using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameproject
{
    internal class AnimatedSprite //this class represents data related to a animated sprite
    {
        private int currentFrame = 0;
        private int frameCount;
        private int frameRate;
        private Point position;
        private Image[] frames;

        public AnimatedSprite(Point position, Image[] frames, int frameRate) //once created, it begin playback from the main loop
        {
            this.position = position;
            this.frames = frames;
            this.frameRate = frameRate;
            this.frameCount = frames.Length;
        }
    }
}
