namespace MDD4All.FMC4SE.Plugin.DataModels
{
	public class TypeDataModel
	{
		public string Name { get; set; }

		public int ElementID { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
