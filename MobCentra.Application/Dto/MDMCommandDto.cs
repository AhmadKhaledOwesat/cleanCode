namespace MobCentra.Application.Dto
{
    public class MDMCommandDto : BaseDto<Guid>
    {
        public string Name { get; set; }
        public string NameEn { get; set; }

        public string Code { get; set; }
        public string Icon { get; set; }
        public int Active { get; set; }
        public int Sort { get; set; }
        public string AlertMessage { get; set; }
        public string AlertMessageOt { get; set; }

        public string AlertIcon { get; set; }
        public int? ShowConfirmatrionAlert { get; set; }
        public Guid? CommandGroupId { get; set; }
        public int? ShowInSetupForm { get; set; }
        public string SettingName { get; set; }


    }
}
