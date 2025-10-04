﻿using System.ComponentModel.DataAnnotations.Schema;

namespace DcpTracker.Domain.Entities
{
    public class Setting : BaseEntity<Guid>
    {
        public string SettingName { get; set; }
        public string DisplayName { get; set; }
        public string SettingValue { get; set; }
        public string SettingValueOt { get; set; }
        public int IsMedia { get; set; }
        public int EnableEditor { get; set; }
        public int SendToMobileApp { get; set; }
        public Guid? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; }
        public int IsSystem { get; set; }   
    }
}
