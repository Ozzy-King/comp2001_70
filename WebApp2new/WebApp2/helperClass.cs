namespace WebApp2
{
    public class helperClass
    {
        public static bool dateValidator(int day, int month, int year)
        {
            //simple check of day, month and year
            if (day < 1) { return true; }
            if (month < 1 || month > 12) { return true; }
            if (year < 1 || year > 9999) { return true; }

            //check day is valid for year and month
            bool isLeapYear = false;
            if ((float)year / 100 == (float)(int)(year / 100))
            {//end of the century
                isLeapYear = (year % 400 == 0 ? true : false);
            }
            else
            {
                isLeapYear = (year % 4 == 0 ? true : false);
            }
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    if (day > 31)
                    {
                        return true;
                    }
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    if (day > 30)
                    {
                        return true;
                    }
                    break;
                case 2:
                    if (day > (isLeapYear ? 29 : 28))
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

    }
}
