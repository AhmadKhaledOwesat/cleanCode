namespace DcpTracker.Domain.Enum
{
    [Flags]
    public enum OperationType
    {
        User = 0,
        UserRole = 2,
        UserCommand = 4,
        UserGroup = 8,
        UserRoleAndCommand = UserRole | UserCommand,
        All = User | UserRoleAndCommand
    }
}
