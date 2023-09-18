﻿using AutoMapper;

namespace Products.Application.Common.Mapping;

public interface IMapWith<T>
{
    void Mapping(Profile profile) =>
        profile.CreateMap(typeof(T), GetType());
}