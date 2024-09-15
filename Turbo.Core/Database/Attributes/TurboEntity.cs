using System;

namespace Turbo.Database.Attributes;

/// <summary>
///     This attribute is needed to find entities provided by plugins
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class TurboEntity : Attribute
{
}