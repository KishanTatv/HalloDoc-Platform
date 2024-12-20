﻿namespace HalloDoc.HelperClass
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
            Bussiness = 1, Patient, Family, Concierge
        }

        //public enum reqTypebg
        //{
        //    bgBussiness = 1, bgPatient, bgFamily, bgConcierge
        //}

        public enum DashName
        {
            New = 1, Pending, Active, Conclude, Toclose, Unpaid
        }

        public enum regName
        {
            Gujrat = 1, Maharashtra, Delhi, Bihar, Goa
        }

        public enum HalloMainRole
        {
            Admin = 1, Provider, Patient
        }
    }
}
