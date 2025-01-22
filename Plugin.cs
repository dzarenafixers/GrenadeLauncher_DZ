using System;
using Exiled.API.Features;
using Exiled.CustomItems.API;
using GrenadeLauncher_DZ.Items;

namespace GrenadeLauncher_DZ
{
    public class Plugin : Plugin<Config>
    {
        public override string Author { get; } = "MONCEF50G";
        public override string Name { get; } = "Super Grenade Launcher";
        public override string Prefix { get; } = "mGrenadeLauncher";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(9, 4, 0);
        public static Plugin Instance;

        public GrenadeLauncher GrenadeLauncher { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;

            GrenadeLauncher = new GrenadeLauncher();
            GrenadeLauncher.Register();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            GrenadeLauncher?.Unregister();

            base.OnDisabled();
        }
    }
}