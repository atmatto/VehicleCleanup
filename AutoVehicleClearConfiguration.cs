using Rocket.API;

namespace PhaserArray.AutoVehicleClear
{
	public class AutoVehicleClearConfiguration : IRocketPluginConfiguration
	{
		public bool SendClearMessage;
		public bool ClearAll;
		public bool ClearExploded;
		public bool ClearDrowned;
		public bool ClearNoTires;
		
		public float ClearInterval;

		public void LoadDefaults()
		{
			SendClearMessage = true;
			ClearAll = false;
			ClearExploded = true;
			ClearDrowned = true;
			ClearNoTires = true;

			ClearInterval = 600f;
		}
	}
}
