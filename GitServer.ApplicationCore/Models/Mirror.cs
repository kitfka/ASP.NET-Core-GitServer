using System;

namespace GitServer.ApplicationCore.Models;

public class Mirror : BaseEntity
{
    public long RepositoryID { get; set; }
    /// <summary>
    /// 小时
    /// </summary>
    public int Interval { get; set; }
    public DateTime Update { get; set; }
    public DateTime NextUpdate { get; set; }
    public string Address { get; set; }
}
