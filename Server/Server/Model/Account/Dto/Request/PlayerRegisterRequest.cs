﻿namespace Server.Model.Account.Dto.Request;

public class PlayerRegisterRequest
{
    public string PlayerId { get; set; } = null!;
    public string PlayerName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
}