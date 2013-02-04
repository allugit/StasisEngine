using System;
using System.Collections.Generic;

namespace StasisEditor.Models
{
    public enum ActorComponentType
    {
        Point,
        Box,
        Circle,
        Polygon
    };

    public interface IActorComponent
    {
        ActorComponentType componentType { get; }
    }
}
