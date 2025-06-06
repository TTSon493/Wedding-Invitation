﻿using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;
public class ResponseDTO
{
    public object? Result { get; set; }
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
}
