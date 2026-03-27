namespace Client;

public class Worldcity
{
	public long?   OgcFid     { get; set; } // INTEGER
	public byte[]? Geometry   { get; set; } // BLOB
	public string? City       { get; set; } // VARCHAR
	public string? CityAscii  { get; set; } // VARCHAR
	public double Lat        { get; set; } // FLOAT
	public double Lng        { get; set; } // FLOAT
	public string? Country    { get; set; } // VARCHAR
	public string? Iso2       { get; set; } // VARCHAR
	public string? Iso3       { get; set; } // VARCHAR
	public string? AdminName  { get; set; } // VARCHAR
	public string? Capital    { get; set; } // VARCHAR
	public string? Population { get; set; } // VARCHAR
	public string? Id         { get; set; } // VARCHAR
}
