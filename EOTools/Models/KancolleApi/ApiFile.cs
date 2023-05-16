﻿using System;

namespace EOTools.Models.KancolleApi;

public class ApiFile
{
    public int Id { get; set; }
    public ApiFileType ApiFileType { get; set; }
    public string Name { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime TimeStamp { get; set; }
    public int Version { get; set; }
    public int? SortieRecordId { get; set; }
}
