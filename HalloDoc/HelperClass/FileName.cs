namespace HalloDoc.HelperClass
{
    public class FileName
    {
        public int YearDiff(DateTime dob)
        {
            DateTime currentDate = DateTime.Now;
            int yearsDifference = currentDate.Year - dob.Year;

            return yearsDifference;
        }
    }
}
