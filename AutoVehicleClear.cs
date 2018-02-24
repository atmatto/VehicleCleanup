using Rocket.Core.Plugins;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System.Linq;
using Rocket.API.Collections;
using UnityEngine;

namespace PhaserArray.AutoVehicleClear
{
	public class AutoVehicleClear : RocketPlugin<AutoVehicleClearConfiguration>
	{
		public const string version = "v1.1";

		private AutoVehicleClearConfiguration config;

		protected override void Load()
		{
			config = Configuration.Instance;

			Logger.Log("Starting AutoVehicleClear " + version + "!");
			InvokeRepeating("ClearVehicles", config.ClearInterval, config.ClearInterval);
		}

		protected override void Unload()
		{
			CancelInvoke("ClearVehicles");
		}

		public void ClearVehicles()
		{
			Logger.Log("Clearing vehicles!");

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

			Logger.Log($"Cleared {cleared} vehicles!");
			if (config.SendClearMessage && cleared > 0)
			{
				UnturnedChat.Say(Translate("autovehicleclear_cleared_vehicles", cleared), Color.green);
			}
		}

		public bool CanClearVehicle(InteractableVehicle vehicle)
		{
			if (vehicle.passengers.Any(p => p.player != null) || vehicle.asset.engine == EEngine.TRAIN)
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
					{"autovehicleclear_cleared_vehicles", "Cleared {0} vehicles!"}
				};
			}
		}
	}
}
