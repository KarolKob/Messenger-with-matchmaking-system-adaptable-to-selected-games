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
    
    public partial class GroupChatUsers
    {
        public long GroupChatJoinID { get; set; }
        public long UserInChatID { get; set; }
        public long JoinedGroupChatID { get; set; }
        public System.DateTime JoinTime { get; set; }
    
        public virtual GroupChat GroupChat { get; set; }
        public virtual Users Users { get; set; }
    }
}
