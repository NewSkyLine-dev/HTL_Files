namespace Server
{
	public class CityInfo
	{
		public long?   InfoId      { get; set; } // INTEGER
		public long    CityId      { get; set; } // INTEGER
		public long?   FoundedYear { get; set; } // INTEGER
		public double? AreaKm2     { get; set; } // REAL
		public long?   ElevationM  { get; set; } // INTEGER
		public string? Timezone    { get; set; } // TEXT
		public string? PostalCode  { get; set; } // TEXT
		public string? Website     { get; set; } // TEXT
		public string? Description { get; set; } // TEXT
		[System.Xml.Serialization.XmlIgnore]
		public City City { get; set; } = null!;
	}
}
