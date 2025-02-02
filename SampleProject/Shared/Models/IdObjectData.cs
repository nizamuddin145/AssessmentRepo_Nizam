﻿using System;
using BusinessEntities;

namespace Shared.Models
{
    public abstract class IdObjectData
    {
        public IdObjectData(string id)
        {
            Id = id;
        }

        public IdObjectData(IdObject entity) : this(entity.Id)
        {
        }

        public string Id { get; set; }
    }
}