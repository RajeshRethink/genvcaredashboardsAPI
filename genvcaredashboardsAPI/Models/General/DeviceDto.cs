using System;
using System.Collections.Generic;

namespace genvcaredashboardsAPI.Models.General
{
    public class DeviceDto
    {
        /// <summary>
        /// Specifies id of the Patient
        /// </summary>
        /// <example>6</example>
        public string PatientId { get; set; }

        /// <summary>
        /// Specifies name of the Patient
        /// </summary>
        /// <example></example>
        public string PatientName { get; set; }

        /// <summary>
        /// Specifies id of the event
        /// </summary>
        /// <example>1</example>
        public string EventId { get; set; }

        /// <summary>
        /// Specifies id of the examination
        /// </summary>
        /// <example>1</example>
        public string ExamId { get; set; }

        /// <summary>
        /// Specifies exam info details
        /// </summary>
        /// <example></example>
        public string ExamInfoDetails { get; set; }

        /// <summary>
        /// Specifies id of the Test
        /// </summary>
        /// <example>1</example>
        public string TestId { get; set; }

        /// <summary>
        /// Specifies id of the device
        /// </summary>
        /// <example>1</example>
        public string DeviceId { get; set; }

        /// <summary>
        /// Specifies vendor name of the device
        /// </summary>
        /// <example>1</example>
        public string DeviceVendor { get; set; }

        /// <summary>
        /// Specifies device expiry date
        /// </summary>
        /// <example>2021-12-16 00:00:00.000</example>
        public DateTime DeviceExpiryDate { get; set; }

        /// <summary>
        /// Specifies device activation date
        /// </summary>
        /// <example>2021-12-16 00:00:00.000</example>
        public DateTime DeviceActivationDate { get; set; }

        /// <summary>
        /// Specifies device AccountId
        /// </summary>
        /// <example>1</example>
        public string DeviceAccountId { get; set; }

        /// <summary>
        /// Specifies device status
        /// </summary>
        /// <example>1</example>
        public string DeviceStatus { get; set; }
        /// <summary>
        /// Specifies additional config data
        /// </summary>
        /// <example>1</example>
        public string AdditionalConfigData { get; set; }
        /// <summary>
        /// Specifies Patietn MRN
        /// </summary>
        /// <example></example>
        public string PatientMRN { get; set; }
        /// <summary>
        /// Specifies Patient Mobile No
        /// </summary>
        /// <example></example>
        public string PatientMobileNo { get; set; }



    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class Device
    {
        public string vendorId { get; set; }
        public string deviceId { get; set; }
        public string deviceName { get; set; }
    }

    public class DeviceRegistered
    {
        public int accountId { get; set; }
        public List<Test> tests { get; set; }
        public List<UserAccessDevice> userAccessDevices { get; set; }
    }

    public class Test
    {
        public string testName { get; set; }
        public int testId { get; set; }
    }

    public class UserAccessDevice
    {
        public int userId { get; set; }
        public List<Device> devices { get; set; }
    }


}
