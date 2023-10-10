using System;
using System.Collections.Generic;

namespace Service.ManagerVPS.DTO.FileManagement
{
    public class ListObjectsDto
    {
        public string Key { get; set; }
        public string LastModified { get; set; }
        public string ETag { get; set; }
        public long Size { get; set; }
        public bool IsDir { get; set; }
        public string VersionId { get; set; }
        public bool IsLastest { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
    }
    public class GetOneDto
    {
        public string ObjectName { get; private set; }
        public long Size { get; private set; }
        public DateTime LastModified { get; private set; }
        public string ETag { get; private set; }
        public string ContentType { get; private set; }
        public Dictionary<string, string> MetaData { get; }
        public string VersionId { get; private set; }
        public bool DeleteMarker { get; private set; }
        public Dictionary<string, string> ExtraHeaders { get; }
        public uint? TaggingCount { get; private set; }
        public string ArchiveStatus { get; private set; }
        public DateTime? Expires { get; private set; }
        public string ReplicationStatus { get; }
 //       public RetentionMode? ObjectLockMode { get; private set; }
        public DateTime? ObjectLockRetainUntilDate { get; private set; }
        public bool? LegalHoldEnabled { get; private set; }
    }
}