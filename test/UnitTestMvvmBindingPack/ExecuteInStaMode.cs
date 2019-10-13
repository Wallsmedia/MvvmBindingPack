using System.Threading;

namespace UnitTestMvvmBindingPack
{
    public static class ExecuteInStaMode
    {
        public static void Invoke(ThreadStart action)
        {
            Thread t = new Thread(action);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}
