using Rocket.API;

namespace AutoVehicleClear
{
	public class AutoVehicleClearConfiguration : IRocketPluginConfiguration
	{
		public bool ClearAll;
		public bool ClearExploded;
		public bool ClearDrowned;
		public bool ClearNoTires;
		
		public float ClearInterval;

		public void LoadDefaults()
		{
			ClearAll = false;
			ClearExploded = true;
			ClearDrowned = true;
			ClearNoTires = true;

			ClearInterval = 600f;
		}
	}
}
