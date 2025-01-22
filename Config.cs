using System.ComponentModel;
using Exiled.API.Interfaces;

namespace GrenadeLauncher_DZ
{
    public class Config : IConfig
    {
        public Config(float spawn)
        {
            Spawn = spawn;
        }

        public Config()
        {
            throw new System.NotImplementedException();
        }

        [Description("Whether the plugin is enabled or not.")]
        public bool IsEnabled { get; set; } = true;

        public bool Debug { get; set; }

        [Description("The damage dealt by the grenade explosion.")]
        public float GrenadeDamage { get; set; } = 250f;

        [Description("The explosion radius of the grenade.")]
        public float ExplosionRadius { get; set; } = 7f;

        [Description("The force applied to the grenade when fired.")]
        public float LaunchForce { get; set; } = 25f;
        
        [Description("maximum spawn grenadlauncher in the raound.")]
        public float Spawn { get; set; } = 1f;

    }
}