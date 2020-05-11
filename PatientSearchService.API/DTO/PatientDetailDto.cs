using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientSearchService.API
{
    public class PatientDetailDto
    {

        [JsonProperty("AccountNumber")]
        public string AccountNumber { get; set; }  //message.Hl7Message.Pid.Pid18.Pid181

        [JsonProperty("AdmitDate")]
        public DateTimeOffset AdmitDate { get; set; }  //message.Hl7Message.Pv1.Pv144.Pv1441

        [JsonProperty("PayerCode")]
        public string PayerCode { get; set; }

        [JsonProperty("SSN")]
        public string SSN { get; set; }  //message.Hl7Message.Pid.Pid19.Pid191

        [JsonProperty("FinancialClass")]
        public string FinancialClass { get; set; }  //message.Hl7Message.Pv1.Pv120.Pv1201

        [JsonProperty("PatientType")]
        public string PatientType { get; set; }  //message.Hl7Message.Pv1.Pv118

        [JsonProperty("MRN")]
        public string MRN { get; set; }  //message.Hl7Message.Pid.Pid3.Pid31

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }  //message.Hl7Message.Pid.Pid5.Pid51

        [JsonProperty("LastName")]
        public string LastName { get; set; }   //message.Hl7Message.Pid.Pid5.Pid52

        [JsonProperty("AdmitType")]
        public string AdmitType { get; set; }  //message.Hl7Message.Pv1.Pv14.Pv141

        [JsonProperty("Status")]
        public string Status { get; set; }   // message.Hl7Message.Pv2.Pv224.Pv2241

        [JsonProperty("HAR")]
        public string HAR { get; set; }

        [JsonProperty("ClientID")]
        public string ClientID { get; set; }

        [JsonProperty("PatientVisitID")]
        public string PatientVisitID { get; set; }

        [JsonProperty("Registrar")]
        public string Registrar { get; set; }

        [JsonProperty("DOS")]
        public string DOS { get; set; }

        [JsonProperty("Gender")]
        public string Gender { get; set; }  //message.Hl7Message.Pid.Pid8.Pid81
    }
}
