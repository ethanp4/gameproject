using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameproject
{
    internal class AnimatedSprite //this class represents data related to a animated sprite
    {
        public int frameCount;
        public int frameTime; //length of one frame in ms
        public Point position;
        public Image[] frames;
        public int animationLength { get { return frameCount * frameTime; } }

        public AnimatedSprite(Point position, Image[] frames, int frameTime, int size) //once created, it begin playback from the main loop
        {
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = new Bitmap(frames[i], new Size(size, size));
            }
            this.position = position;
            this.frames = frames;
            this.frameTime = frameTime;
            this.frameCount = frames.Length;
        }
    }
}
