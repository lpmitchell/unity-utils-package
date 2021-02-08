using System;

namespace LpmGames.Utils.Easing
{
    public enum Ease
    {
        Linear,
        ExpoEaseOut, ExpoEaseIn, ExpoEaseInOut, ExpoEaseOutIn,
        CircEaseOut, CircEaseIn, CircEaseInOut, CircEaseOutIn,
        QuadEaseOut, QuadEaseIn, QuadEaseInOut, QuadEaseOutIn,
        SineEaseOut, SineEaseIn, SineEaseInOut, SineEaseOutIn,
        CubicEaseOut, CubicEaseIn, CubicEaseInOut, CubicEaseOutIn,
        QuartEaseOut, QuartEaseIn, QuartEaseInOut, QuartEaseOutIn,
        QuintEaseOut, QuintEaseIn, QuintEaseInOut, QuintEaseOutIn,
        ElasticEaseOut, ElasticEaseIn, ElasticEaseInOut, ElasticEaseOutIn,
        BounceEaseOut, BounceEaseIn, BounceEaseInOut, BounceEaseOutIn,
        BackEaseOut, BackEaseIn, BackEaseInOut, BackEaseOutIn
    }

    public static class EaseFactory
    {
        public static Func<float, float> GetEquation(Ease ease)
        {
            switch (ease)
            {
                case Ease.Linear: return f => PennerEasingEquations.Linear(f);
                case Ease.ExpoEaseOut: return f => PennerEasingEquations.ExpoEaseOut(f);
                case Ease.ExpoEaseIn: return f => PennerEasingEquations.ExpoEaseIn(f);
                case Ease.ExpoEaseInOut: return f => PennerEasingEquations.ExpoEaseInOut(f);
                case Ease.ExpoEaseOutIn: return f => PennerEasingEquations.ExpoEaseOutIn(f);
                case Ease.CircEaseOut: return f => PennerEasingEquations.CircEaseOut(f);
                case Ease.CircEaseIn: return f => PennerEasingEquations.CircEaseIn(f);
                case Ease.CircEaseInOut: return f => PennerEasingEquations.CircEaseInOut(f);
                case Ease.CircEaseOutIn: return f => PennerEasingEquations.CircEaseOutIn(f);
                case Ease.QuadEaseOut: return f => PennerEasingEquations.QuadEaseOut(f);
                case Ease.QuadEaseIn: return f => PennerEasingEquations.QuadEaseIn(f);
                case Ease.QuadEaseInOut: return f => PennerEasingEquations.QuadEaseInOut(f);
                case Ease.QuadEaseOutIn: return f => PennerEasingEquations.QuadEaseOutIn(f);
                case Ease.SineEaseOut: return f => PennerEasingEquations.SineEaseOut(f);
                case Ease.SineEaseIn: return f => PennerEasingEquations.SineEaseIn(f);
                case Ease.SineEaseInOut: return f => PennerEasingEquations.SineEaseInOut(f);
                case Ease.SineEaseOutIn: return f => PennerEasingEquations.SineEaseOutIn(f);
                case Ease.CubicEaseOut: return f => PennerEasingEquations.CubicEaseOut(f);
                case Ease.CubicEaseIn: return f => PennerEasingEquations.CubicEaseIn(f);
                case Ease.CubicEaseInOut: return f => PennerEasingEquations.CubicEaseInOut(f);
                case Ease.CubicEaseOutIn: return f => PennerEasingEquations.CubicEaseOutIn(f);
                case Ease.QuartEaseOut: return f => PennerEasingEquations.QuartEaseOut(f);
                case Ease.QuartEaseIn: return f => PennerEasingEquations.QuartEaseIn(f);
                case Ease.QuartEaseInOut: return f => PennerEasingEquations.QuartEaseInOut(f);
                case Ease.QuartEaseOutIn: return f => PennerEasingEquations.QuartEaseOutIn(f);
                case Ease.QuintEaseOut: return f => PennerEasingEquations.QuintEaseOut(f);
                case Ease.QuintEaseIn: return f => PennerEasingEquations.QuintEaseIn(f);
                case Ease.QuintEaseInOut: return f => PennerEasingEquations.QuintEaseInOut(f);
                case Ease.QuintEaseOutIn: return f => PennerEasingEquations.QuintEaseOutIn(f);
                case Ease.ElasticEaseOut: return f => PennerEasingEquations.ElasticEaseOut(f);
                case Ease.ElasticEaseIn: return f => PennerEasingEquations.ElasticEaseIn(f);
                case Ease.ElasticEaseInOut: return f => PennerEasingEquations.ElasticEaseInOut(f);
                case Ease.ElasticEaseOutIn: return f => PennerEasingEquations.ElasticEaseOutIn(f);
                case Ease.BounceEaseOut: return f => PennerEasingEquations.BounceEaseOut(f);
                case Ease.BounceEaseIn: return f => PennerEasingEquations.BounceEaseIn(f);
                case Ease.BounceEaseInOut: return f => PennerEasingEquations.BounceEaseInOut(f);
                case Ease.BounceEaseOutIn: return f => PennerEasingEquations.BounceEaseOutIn(f);
                case Ease.BackEaseOut: return f => PennerEasingEquations.BackEaseOut(f);
                case Ease.BackEaseIn: return f => PennerEasingEquations.BackEaseIn(f);
                case Ease.BackEaseInOut: return f => PennerEasingEquations.BackEaseInOut(f);
                case Ease.BackEaseOutIn: return f => PennerEasingEquations.BackEaseOutIn(f);
                default:
                    throw new ArgumentOutOfRangeException(nameof(ease), ease, null);
            }
        }
    }
}