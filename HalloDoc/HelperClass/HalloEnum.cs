namespace HalloDoc.HelperClass
{
    public class HalloEnum
    {
        public enum Status
        {
            Unassigned = 1, Accepted, Cancelled, Reserving, MDEnRoute, MDOnSite, FollowUp, Closed, Locked, Declined, Consult, Clear, CancelledByProvider, CCUploadedByClient, CCApprovedByAdmin
        }

        public enum repestType
        {
            Bussiness = 1, Patient, Family, Conciegre
        }

        public enum reqTypebg
        {
            bgBussiness = 1, bgPatient, bgFamily, bgConcierge
        }
    }
}
