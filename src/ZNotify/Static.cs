using System;

namespace ZNotify;

internal static class Static
{
    public const string DefaultEndpoint = "https://push.learningman.top";

    /// <summary>
    /// Detect if we are running as part of a nUnit unit test.
    /// This is DIRTY and should only be used if absolutely necessary 
    /// as its usually a sign of bad design.
    /// </summary>    
    static class UnitTestDetector
    {
        private static readonly bool RunningFromNUnit = false;

        static UnitTestDetector()
        {
            foreach (var assem in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assem.FullName.ToLowerInvariant().StartsWith("nunit.framework")) continue;
                RunningFromNUnit = true;
                break;
            }
        }

        public static bool IsRunningFromNUnit
        {
            get { return RunningFromNUnit; }
        }
    }
}