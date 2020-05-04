
// ReSharper disable once CheckNamespace
namespace LpmGames.Utils.ControlFlow
{
    public class WaitForContinue : WaitForResult<bool>
    {
        public WaitForContinue(float timeout = 0) : base(timeout){}
        public void Continue() => Continue(true);
    }
}