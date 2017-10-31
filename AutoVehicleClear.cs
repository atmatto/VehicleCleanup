using Rocket.Core.Plugins;
using UnityEngine;
using Rocket.Core.Logging;
using SDG.Unturned;
using Rocket.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoVehicleClear
{
    public class AutoVehicleClear : RocketPlugin<AutoVehicleClearConfiguration>
	{
		public const string version = "v1.0";

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
			var vehicles = VehicleManager.vehicles;
			for (int i = vehicles.Count - 1; i >= 0; i--)
			{
				var vehicle = vehicles[i];
				if (CanClearVehicle(vehicle))
				{
					VehicleManager.instance.channel.send("tellVehicleDestroy", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, vehicle.instanceID);
				}
			}
		}

		public bool CanClearVehicle(InteractableVehicle vehicle)
		{
			if (!vehicle.passengers.All(p => p.player == null))
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
					for (int i = 0; i < totalTires; i++)
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
	}
}
