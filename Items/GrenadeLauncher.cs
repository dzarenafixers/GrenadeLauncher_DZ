using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.API.Interfaces;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using UnityEngine;

namespace GrenadeLauncher_DZ.Items
{
    public class GrenadeLauncher : CustomItem, IConfig
    {
        public override uint Id { get; set; } = 125488; // Unique ID for the item
        public override string Name { get; set; } = "Grenade Launcher";
        public override string Description { get; set; } =
            "A weapon that fires grenades that explode upon impact.";
        public override ItemType Type { get; set; } = ItemType.GunLogicer; // E11SR as base gun
        public override float Weight { get; set; } = 1.5f;
        public override SpawnProperties SpawnProperties { get; set; }

        // Properties loaded from the config
        private float ExplosionDamage => Plugin.Instance.Config.GrenadeDamage;
        private float ExplosionRadius => Plugin.Instance.Config.ExplosionRadius;
        private float LaunchForce => Plugin.Instance.Config.LaunchForce;

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem += UsedItem;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem -= UsedItem;
            base.UnsubscribeEvents();
        }

        private void UsedItem(UsedItemEventArgs ev)
        {
            if (!Check(ev.Item))
                return;

            FireGrenade(ev.Player);
        }

        private void FireGrenade(Player player)
        {
            // Starting position and direction
            Vector3 startPosition = player.CameraTransform.position + player.CameraTransform.forward;
            Vector3 direction = player.CameraTransform.forward;

            // Create grenade object
            GameObject grenade = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            grenade.transform.position = startPosition;
            grenade.transform.localScale = Vector3.one * 0.3f;

            Rigidbody rb = grenade.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.AddForce(direction * LaunchForce, ForceMode.Impulse);

            // Add explosion behavior
            GrenadeBehavior behavior = grenade.AddComponent<GrenadeBehavior>();
            behavior.Initialize(ExplosionDamage, ExplosionRadius, player);
        }

        public bool IsEnabled { get; set; }
        public bool Debug { get; set; }
    }

    public class GrenadeBehavior : MonoBehaviour
    {
        private float damage;
        private float radius;
        private Player owner;

        public void Initialize(float explosionDamage, float explosionRadius, Player player)
        {
            damage = explosionDamage;
            radius = explosionRadius;
            owner = player;

            // Auto-destroy after 5 seconds
            Invoke(nameof(Explode), 5f);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Explode();
        }

        private void Explode()
        {
            if (this == null) return;

            Vector3 position = transform.position;

            // Deal damage to nearby players
            foreach (Player player in Player.List)
            {
                if (Vector3.Distance(player.Position, position) <= radius)
                {
                    player.Hurt(damage, DamageType.Explosion, owner?.Nickname ?? "Unknown");
                }
            }

            // Optional: Display an explosion effect
            Exiled.API.Features.Map.Broadcast(2, "<color=red>Explosion occurred!</color>");
            
            // Destroy the grenade object
            Destroy(gameObject);
        }
    }
}
