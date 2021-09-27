using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace app.service.Models
{
    [DataContract]
    public class ServiceResponseBase
    {
        [DataMember]
        public bool IsSuccessfull { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }

    [DataContract]
    public class GenericServiceResponse<T> : ServiceResponseBase where T : class
    {
        [DataMember]
        public T Model { get; set; }
    }

    [DataContract]
    public class IntServiceResponse : ServiceResponseBase
    {
        [DataMember]
        public int Model { get; set; }
    }

    [DataContract]
    public class VoidServiceResponse : ServiceResponseBase
    {

    }

    [DataContract]
    public class BoolServiceResponse : ServiceResponseBase
    {
        [DataMember]
        public bool Model { get; set; }
    }
}