public class Hinweise {
    private String [] images = new String[] {"Chips", "Cola", "Donut", "Kaese", "Milch", "Pizza", "Pommes", "Schoko"};

    static boolean isValidDate(int d, int m, int y)
    {
        if (m < 1 || m > 12)
            return false;
        if (d < 1 || d > 31)
            return false;
        if (m == 2)
        {
            if (((y % 4 == 0) && (y % 100 != 0)) || (y % 400 == 0))
                return (d <= 29);
            else
                return (d <= 28);
        }
        if (m == 4 || m == 6 || m == 9 || m == 11)
            return (d <= 30);
        return true;
    }
}
