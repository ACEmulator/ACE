using System;

namespace ACE.Server.Mods
{
    /// <summary>
    /// Defines interactions between mods and their ACE.Server host
    /// </summary>
    public interface IHarmonyMod : IDisposable //, IAsyncDisposable //Todo: Decide on async support
    {
        //https://github.com/natemcmaster/DotNetCorePlugins#what-is-a-shared-type

        void Initialize();
    }
}
