using System.Xml.Linq;

namespace RoboterWPF.Models;

public class LevelParser
{
    public int Width { get; private set; } = 10;
    public int Height { get; private set; } = 10;

    public List<RobotElement> ParseLevel(string filename)
    {
        var elements = new List<RobotElement>();

        try
        {
            var doc = XDocument.Load(filename);
            var root = doc.Root;

            if (root == null)
                return elements;

            // Parse Width and Height
            var widthElement = root.Element("Width");
            var heightElement = root.Element("Height");

            if (widthElement != null)
                Width = int.Parse(widthElement.Value);

            if (heightElement != null)
                Height = int.Parse(heightElement.Value);

            // Parse Fields
            var fieldsElement = root.Element("Fields");
            if (fieldsElement != null)
            {
                // Handle both "Field" and "XML_Cell" element names
                foreach (var field in fieldsElement.Elements())
                {
                    if (field.Name != "Field" && field.Name != "XML_Cell")
                        continue;

                    var element = new RobotElement();

                    var xElement = field.Element("X");
                    var yElement = field.Element("Y");
                    var typeElement = field.Element("Type");

                    if (xElement != null)
                        element.X = int.Parse(xElement.Value);

                    if (yElement != null)
                        element.Y = int.Parse(yElement.Value);

                    if (typeElement != null)
                    {
                        string typeText = typeElement.Value.ToLower();

                        if (typeText == "stone")
                        {
                            element.Type = ElementType.Obstacle;
                        }
                        else if (typeText == "robot")
                        {
                            element.Type = ElementType.Robot;
                        }
                        else if (typeText == "wall")
                        {
                            element.Type = ElementType.Wall;
                        }
                        else if (IsLetter(typeText))
                        {
                            element.Type = ElementType.Letter;
                            element.Letter = typeElement.Value.ToUpper();
                        }
                        else
                        {
                            // Default to wall for unknown types
                            element.Type = ElementType.Wall;
                        }
                    }

                    elements.Add(element);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error parsing level: {ex.Message}");
        }

        return elements;
    }

    private static bool IsLetter(string str)
    {
        return str.Length == 1 && char.IsLetter(str[0]);
    }
}
