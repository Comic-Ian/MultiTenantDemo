namespace MultiTenantDemo2.Infrastructure
{
    /// <summary>
    /// 规定StoreDbContext中，必须可以返回TenantInfo。
    /// </summary>
    public interface ITenantDbContext
    {
        TenantInfo TenantInfo { get; }
    }
}
