using System;
using System.Collections.Generic;
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
            { //compare current time, started time, and using framerate, choose a frame to draw to the screen
                
            }
        }

        public static void addAnimation(AnimatedSprite animatedSprite) { 
            animationList.Add(new(DateTime.Now, animatedSprite)); //begins immediately
        }
    }
}
