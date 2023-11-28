using UnityEngine;

namespace jp.co.jetman.utils
{
    public class Easing
    {

        /*
         * t: the rate overall 0 <= t <= 1
         * start: at the start of the value
         * change: the difference between the end of the value
         * duration: the whole time
         */

        #region static class members
        // Linear           : no easing, no acceleration
        static public float linear(float t, float start, float change, float duration)
        {
            return change * t / duration + start;
        }
        // Quadratic
        // easing in        : accelerating from zero velocity
        // easing out       : decelerating to zero velocity
        // easing in/out
        static public float easeInQuad(float t, float start, float change, float duration)
        {
            t /= duration;
            return change * t * t + start;
        }
        static public float easeOutQuad(float t, float start, float change, float duration)
        {
            t /= duration;
            return -change * t * (t - 2) + start;
        }
        static public float easeInOutQuad(float t, float start, float change, float duration)
        {
            t /= duration / 2;
            if (t < 1)
            {
                return change / 2 * t * t + start;
            }
            t--;
            return -change / 2 * (t * (t - 2) - 1) + start;
        }
        // Cubic
        //  easing in       : accelerating from zero velocity
        //  easing out      : decelerating to zero velocity
        //  easing in/out
        static public float easeInCubic(float t, float start, float change, float duration)
        {
            t /= duration;
            return change * t * t * t + start;
        }
        static public float easeOutCubic(float t, float start, float change, float duration)
        {
            t /= duration;
            t--;
            return change * (t * t * t + 1) + start;
        }
        static public float easeInOutCubic(float t, float start, float change, float duration)
        {
            t /= duration / 2;
            if (t < 1)
            {
                return change / 2 * t * t * t + start;
            }
            t -= 2;
            return change / 2 * (t * t * t + 2) + start;
        }
        // Quartic
        static public float easeInQuart(float t, float start, float change, float duration)
        {
            t /= duration;
            return change * t * t * t * t + start;
        }
        static public float easeOutQuart(float t, float start, float change, float duration)
        {
            t /= duration;
            t--;
            return -change * (t * t * t * t - 1) + start;
        }
        static public float easeInOutQuart(float t, float start, float change, float duration)
        {
            t /= duration / 2;
            if (t < 1)
            {
                return change / 2 * t * t * t * t + start;
            }
            t -= 2;
            return -change / 2 * (t * t * t * t - 2) + start;
        }
        // Quintic
        static public float easeInQuint(float t, float start, float change, float duration)
        {
            t /= duration;
            return change * t * t * t * t * t + start;
        }
        static public float easeOutQuint(float t, float start, float change, float duration)
        {
            t /= duration;
            t--;
            return change * (t * t * t * t * t + 1) + start;
        }
        static public float easeInOutQuint(float t, float start, float change, float duration)
        {
            t /= duration / 2;
            if (t < 1)
            {
                return change / 2 * t * t * t * t * t + start;
            }
            t -= 2;
            return change / 2 * (t * t * t * t * t + 2) + start;
        }
        // Sinusoidal
        static public float easeInSine(float t, float start, float change, float duration)
        {
            return -change * Mathf.Cos(t / duration * (Mathf.PI / 2)) + change + start;
        }
        static public float easeOutSine(float t, float start, float change, float duration)
        {
            return change * Mathf.Sin(t / duration * (Mathf.PI / 2)) + start;
        }
        static public float easeInOutSine(float t, float start, float change, float duration)
        {
            return -change / 2 * (Mathf.Cos(Mathf.PI * t / duration) - 1) + start;
        }
        // Exponential
        static public float easeInExpo(float t, float start, float change, float duration)
        {
            return change * Mathf.Pow(2, 10 * (t / duration - 1)) + start;
        }
        static public float easeOutExpo(float t, float start, float change, float duration)
        {
            return change * (-Mathf.Pow(2, -10 * t / duration) + 1) + start;
        }
        static public float easeInOutExpo(float t, float start, float change, float duration)
        {
            t /= duration / 2;
            if (t < 1)
            {
                return change / 2 * Mathf.Pow(2, 10 * (t - 1)) + start;
            }
            t--;
            return change / 2 * (-Mathf.Pow(2, -10 * t) + 2) + start;
        }

        // Circular
        static public float easeInCircular(float t, float start, float change, float duration)
        {
            t /= duration;
            return -change * (Mathf.Sqrt(1 - t * t) - 1) + start;
        }
        static public float easeOutCircular(float t, float start, float change, float duration)
        {
            t /= duration;
            t -= 1;
            return change * Mathf.Sqrt(1 - t * t) + start;
        }
        static public float easeInOutCirc(float t, float start, float change, float duration)
        {
            t /= duration / 2;
            if (t < 1)
            {
                return -change / 2 * (Mathf.Sqrt(1 - t * t) - 1) + start;
            }
            t -= 2;
            return change / 2 * (Mathf.Sqrt(1 - t * t) + 1) + start;
        }
        // Bounce
        static public float easeInBounce(float t, float start, float change, float duration)
        {
            return change - easeOutBounce(duration - t, 0, change, duration) + start;
        }
        static public float easeOutBounce(float t, float start, float change, float duration)
        {
            if ((t /= duration) < (1 / 2.75f))
            {
                return change * (7.5625f * t * t) + start;
            }
            else if (t < (2 / 2.75f))
            {
                return change * (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f) + start;
            }
            else if (t < (2.5 / 2.75))
            {
                return change * (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f) + start;
            }
            else
            {
                return change * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + start;
            }
        }
        static public float easeInOutBounce(float t, float start, float change, float duration)
        {
            if (t < duration / 2)
                return easeInBounce(t * 2, 0, change, duration) * 0.5f + start;
            else
                return easeOutBounce(t * 2 - duration, 0, change, duration) * .5f + change * 0.5f + start;
        }
        // Back
        public static float  easeInBack(float t,float b , float c, float d) {
		    float s = 1.70158f;
		    return c*(t/=d)*t*((s+1)*t - s) + b;
	    }
	
	    public static float easeInBack(float t,float b , float c, float d, float s) {
		    return c*(t/=d)*t*((s+1)*t - s) + b;
	    }
	
	    public static float easeOutBack(float t,float b , float c, float d) {
		    float s = 1.70158f;
		    return c*((t=t/d-1)*t*((s+1)*t + s) + 1) + b;
	    }
	
	    public static float easeOutBack(float t,float b , float c, float d, float s) {
		    return c*((t=t/d-1)*t*((s+1)*t + s) + 1) + b;
	    }
	
	    public static float easeInOutBack(float t,float b , float c, float d) {
		    float s = 1.70158f;
		    if ((t/=d/2) < 1) return c/2*(t*t*(((s*=(1.525f))+1)*t - s)) + b;
		    return c/2*((t-=2)*t*(((s*=(1.525f))+1)*t + s) + 2) + b;
	    }
	
	    public static float easeInOutBack(float t,float b , float c, float d, float s) {	
		    if ((t/=d/2) < 1) return c/2*(t*t*(((s*=(1.525f))+1)*t - s)) + b;
		    return c/2*((t-=2)*t*(((s*=(1.525f))+1)*t + s) + 2) + b;
	    }

        /*
        // Elastic
        public static float  easeIn(float t,float b , float c, float d ) {
		if (t==0) return b;  if ((t/=d)==1) return b+c;  
		float p=d*.3f;
		float a=c; 
		float s=p/4;
		return -(a*(float)Math.pow(2,10*(t-=1)) * (float)Math.sin( (t*d-s)*(2*(float)Math.PI)/p )) + b;
	}

	public static float  easeIn(float t,float b , float c, float d, float a, float p) {
		float s;
		if (t==0) return b;  if ((t/=d)==1) return b+c;  
		if (a < Math.abs(c)) { a=c;  s=p/4; }
		else { s = p/(2*(float)Math.PI) * (float)Math.asin (c/a);}
		return -(a*(float)Math.pow(2,10*(t-=1)) * (float)Math.sin( (t*d-s)*(2*Math.PI)/p )) + b;
	}

	public static float  easeOut(float t,float b , float c, float d) {
		if (t==0) return b;  if ((t/=d)==1) return b+c;  
		float p=d*.3f;
		float a=c; 
		float s=p/4;
		return (a*(float)Math.pow(2,-10*t) * (float)Math.sin( (t*d-s)*(2*(float)Math.PI)/p ) + c + b);	
	}
	
	public static float  easeOut(float t,float b , float c, float d, float a, float p) {
		float s;
		if (t==0) return b;  if ((t/=d)==1) return b+c;  
		if (a < Math.abs(c)) { a=c;  s=p/4; }
		else { s = p/(2*(float)Math.PI) * (float)Math.asin (c/a);}
		return (a*(float)Math.pow(2,-10*t) * (float)Math.sin( (t*d-s)*(2*(float)Math.PI)/p ) + c + b);	
	}
	
	public static float  easeInOut(float t,float b , float c, float d) {
		if (t==0) return b;  if ((t/=d/2)==2) return b+c; 
		float p=d*(.3f*1.5f);
		float a=c; 
		float s=p/4;
		if (t < 1) return -.5f*(a*(float)Math.pow(2,10*(t-=1)) * (float)Math.sin( (t*d-s)*(2*(float)Math.PI)/p )) + b;
		return a*(float)Math.pow(2,-10*(t-=1)) * (float)Math.sin( (t*d-s)*(2*(float)Math.PI)/p )*.5f + c + b;
	}
	
	public static float  easeInOut(float t,float b , float c, float d, float a, float p) {
		float s;
		if (t==0) return b;  if ((t/=d/2)==2) return b+c;  
		if (a < Math.abs(c)) { a=c; s=p/4; }
		else { s = p/(2*(float)Math.PI) * (float)Math.asin (c/a);}
		if (t < 1) return -.5f*(a*(float)Math.pow(2,10*(t-=1)) * (float)Math.sin( (t*d-s)*(2*(float)Math.PI)/p )) + b;
		return a*(float)Math.pow(2,-10*(t-=1)) * (float)Math.sin( (t*d-s)*(2*(float)Math.PI)/p )*.5f + c + b;
	}
        */
        #endregion

        #region constructor method
        #endregion

        #region private methods
        #endregion

        #region public methods, messages
        #endregion

    }

}