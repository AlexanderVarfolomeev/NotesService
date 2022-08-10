﻿using System.Threading.Tasks;
using Notes.WPF.Models.Auth;

namespace Notes.WPF.Services.Auth;

public interface IAuthService
{

    Task<LoginResult> Login(LoginModel loginModel);
    Task Logout();
    Task Register(LoginModel registerModel);
}