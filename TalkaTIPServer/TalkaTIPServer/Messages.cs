//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TalkaTIPServer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Messages
    {
        public long ChatID { get; set; }
        public long UserSenderID { get; set; }
        public long UserReceiverID { get; set; }
        public System.DateTime SendTime { get; set; }
        public string Message { get; set; }
    
        public virtual Users Users { get; set; }
        public virtual Users Users1 { get; set; }
    }
}
