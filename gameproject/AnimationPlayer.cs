using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameproject
{
    internal static class AnimationPlayer //this class will be called every frame and will update the animation according to its framerate
    {
        private static List<KeyValuePair<DateTime, AnimatedSprite>> animationList = new(); //time refers to when it was started

        public static void updateAnimations(DateTime now, Graphics g)
        {
            foreach (var animation in animationList)
            { //compare current time to animation start time to see what frame should be drawn
                var time = animation.Key;
                var anim = animation.Value;
                var delta = now - time;
                int frameNo = (int)delta.TotalMilliseconds / anim.frameTime;
                if (delta.TotalMilliseconds > anim.animationLength)
                {
                    animationList.Remove(animation);
                    return;
                }
                //Debug.WriteLine($"delta: {delta.TotalMilliseconds}, length: {anim.animationLength}, frameNo: {frameNo}");
                g.DrawImage(animation.Value.frames[frameNo], animation.Value.position.X, animation.Value.position.Y);
            }
        }

        public static void addAnimation(AnimatedSprite animatedSprite) { 
            animationList.Add(new(DateTime.Now, animatedSprite)); //begins immediately
        }
    }
}
