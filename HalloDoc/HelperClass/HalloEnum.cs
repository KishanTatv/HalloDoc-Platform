namespace HalloDoc.HelperClass
{
    public class HalloEnum
    {
        public enum Status
        {
            Unassigned = 1,
            Accepted,
            Cancelled,
            MDEnRoute,
            MDOnSite,
            Conclude,
            CancelledByPatient,
            Closed,
            Unpaid,
            Clear,
            Block,
            CancelledByProvider, CCUploadedByClient, CCApprovedByAdmin
        }

        public enum repestType
        {
            Bussiness = 1, Patient, Family, Conciegre
        }

        public enum reqTypebg
        {
            bgBussiness = 1, bgPatient, bgFamily, bgConcierge
        }

        public enum DashName
        {
            New = 1, Pending, Active, Conclude, Toclose, Unpaid
        }
    }
}
