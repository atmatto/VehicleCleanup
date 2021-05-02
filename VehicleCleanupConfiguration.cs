using Rocket.API;

namespace Matto.VehicleCleanup
{
	public class VehicleCleanupConfiguration : IRocketPluginConfiguration
	{
		public bool Automatic;
		public bool SendClearMessage;
		public bool SendWarningMessage;
		public bool ClearAll;
		public bool ProtectLocked;
		public bool ClearExploded;
		public bool ClearDrowned;
		public bool ClearNoTires;
		public float WarningTime;
		public float ClearInterval;

		public void LoadDefaults()
		{
			Automatic = false;
			SendClearMessage = true;
			SendWarningMessage = true;
			ClearAll = false;
			ProtectLocked = true;
			ClearExploded = true;
			ClearDrowned = true;
			ClearNoTires = true;

			WarningTime = 30f;
			ClearInterval = 600f;
		}
	}
}
