﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public DoLoginUseCase(
        IUserReadOnlyRepository repository,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _repository = repository;
        _accessTokenGenerator = accessTokenGenerator;
        _passwordEncripter = passwordEncripter;
    }
    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await _repository.GetUserByEmail(request.Email);

        if (user is null) 
        {
            throw new InvalidLoginException();
        }

        var passwordMatch = _passwordEncripter.Verify(request.Password, user.Password);

        if(passwordMatch == false)
        {
            throw new InvalidLoginException();
        }

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Token = _accessTokenGenerator.Generate(user)
        };
    }

}
