using Rocket.Core.Plugins;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Core.Commands;
using Rocket.API;
using SDG.Unturned;
using System.Linq;
using Rocket.API.Collections;
using UnityEngine;

namespace Matto.VehicleCleanup
{
	public class VehicleCleanup : RocketPlugin<VehicleCleanupConfiguration>
	{
		public const string version = "v2.0.0";

		private VehicleCleanupConfiguration config;

		protected override void Load()
		{
			config = Configuration.Instance;

			Rocket.Core.Logging.Logger.Log("Starting VehicleCleanup " + version + "!");
			if (config.Automatic) {
				InvokeRepeating("SendWarning", config.ClearInterval, config.ClearInterval);
			}
		}

		protected override void Unload()
		{
			CancelInvoke("SendWarning");
			CancelInvoke("ClearVehicles");
		}

		[RocketCommand("clearvehicles", "Clear vehicles, exact behaviour depends on plugin's config.")]
		[RocketCommandAlias("cv")]
		public void ClearCommand(IRocketPlayer caller, string[] command)
		{
			if (config.SendWarningMessage) {
				SendWarning();
			} else {
				ClearVehicles();
			}
		}
		public void SendWarning()
		{
			if (config.SendWarningMessage) {
				UnturnedChat.Say(Translate("VehicleCleanup_warning", config.WarningTime), Color.yellow);
			}
			Invoke("ClearVehicles", config.WarningTime);
		}

		public void ClearVehicles()
		{
			Rocket.Core.Logging.Logger.Log("Clearing vehicles!");

			var cleared = 0;
			var vehicles = VehicleManager.vehicles;
			for (int i = vehicles.Count - 1; i >= 0; i--)
			{
				var vehicle = vehicles[i];
				if (CanClearVehicle(vehicle))
				{
					VehicleManager.instance.channel.send("tellVehicleDestroy", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, vehicle.instanceID);
					cleared++;
				}
			}

			Rocket.Core.Logging.Logger.Log($"Cleared {cleared} vehicles!");
			if (config.SendClearMessage && cleared > 0)
			{
				UnturnedChat.Say(Translate("VehicleCleanup_cleared_vehicles", cleared), Color.green);
			}
		}

		public bool CanClearVehicle(InteractableVehicle vehicle)
		{
			if (vehicle.passengers.Any(p => p.player != null) || vehicle.asset.engine == EEngine.TRAIN)
			{
				return false;
			}
			if (config.ProtectLocked && vehicle.isLocked)
			{
				return false;
			}
			if (config.ClearAll)
			{
				return true;
			}
			if (config.ClearExploded && vehicle.isExploded)
			{
				return true;
			}
			if (config.ClearDrowned)
			{
				if (vehicle.isDrowned && vehicle.transform.FindChild("Buoyancy") == null)
				{
					return true;
				}
			}
			if (config.ClearNoTires)
			{
				var tires = vehicle.transform.FindChild("Tires");
				if (tires != null)
				{
					var totalTires = vehicle.transform.FindChild("Tires").childCount;
					if (totalTires == 0)
					{
						return false;
					}

					var aliveTires = 0;
					for (var i = 0; i < totalTires; i++)
					{
						if (tires.GetChild(i).gameObject.activeSelf)
						{
							aliveTires++;
						}
					}
					if (aliveTires == 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public override TranslationList DefaultTranslations
		{
			get
			{
				return new TranslationList()
				{
					{"VehicleCleanup_cleared_vehicles", "Cleared {0} vehicles!"},
					{"VehicleCleanup_warning", "Clearing vehicles in {0} seconds!"}
				};
			}
		}
	}
}
