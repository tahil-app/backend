﻿namespace Tahil.Application.Models;

public class UpdatePasswordParams
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}