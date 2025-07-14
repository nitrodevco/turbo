using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace Turbo.Core.Database.Attributes;

/// <summary>
/// Specifies the default value for a property.
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class DefaultValueSqlAttribute : Attribute
{
    /// <summary>
    /// This is the default value.
    /// </summary>
    private object? _value;

    // Delegate ad hoc created 'TypeDescriptor.ConvertFromInvariantString' reflection object cache
    private static object? s_convertFromInvariantString;

    /// <summary>
    /// Initializes a new instance of the <see cref='DefaultValueSqlAttribute'/>
    /// class, converting the specified value to the specified type, and using the U.S. English
    /// culture as the translation context.
    /// </summary>
    [RequiresUnreferencedCode(
        "Generic TypeConverters may require the generic types to be annotated. For example, NullableConverter requires the underlying type to be DynamicallyAccessedMembers All.")]
    public DefaultValueSqlAttribute(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
        Type type,
        string? value)
    {
        // The null check and try/catch here are because attributes should never throw exceptions.
        // We would fail to load an otherwise normal class.

        if (type == null)
        {
            return;
        }

        try
        {
            if (TryConvertFromInvariantString(type, value, out object? convertedValue))
            {
                _value = convertedValue;
            }
            else if (type.IsSubclassOf(typeof(Enum)) && value != null)
            {
                _value = Enum.Parse(type, value, true);
                _value = Convert.ChangeType(convertedValue, Enum.GetUnderlyingType(type), CultureInfo.InvariantCulture);
            }
            else if (type == typeof(TimeSpan) && value != null)
            {
                _value = TimeSpan.Parse(value);
            }
            else
            {
                _value = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
            }

            [RequiresUnreferencedCode(
                "Generic TypeConverters may require the generic types to be annotated. For example, NullableConverter requires the underlying type to be DynamicallyAccessedMembers All.")]
            // Looking for ad hoc created TypeDescriptor.ConvertFromInvariantString(Type, string)
            static bool TryConvertFromInvariantString(
                [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
                Type typeToConvert,
                string? stringValue,
                out object? conversionResult)
            {
                conversionResult = null;

                // lazy init reflection objects
                if (s_convertFromInvariantString == null)
                {
                    var typeDescriptorType =
                        Type.GetType("System.ComponentModel.TypeDescriptor, System.ComponentModel.TypeConverter",
                            throwOnError: false);
                    var mi = typeDescriptorType?.GetMethod("ConvertFromInvariantString",
                        BindingFlags.NonPublic | BindingFlags.Static);
                    Volatile.Write(ref s_convertFromInvariantString,
                        mi == null ? new object() : mi.CreateDelegate(typeof(Func<Type, string, object>)));
                }

                if (!(s_convertFromInvariantString is Func<Type, string?, object> convertFromInvariantString))
                    return false;

                try
                {
                    conversionResult = convertFromInvariantString(typeToConvert, stringValue);
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref='DefaultValueSqlAttribute'/>
    /// class using a Unicode character.
    /// </summary>
    public DefaultValueSqlAttribute(char value)
    {
        _value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref='DefaultValueSqlAttribute'/>
    /// class using an 8-bit unsigned integer.
    /// </summary>
    public DefaultValueSqlAttribute(byte value)
    {
        _value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref='DefaultValueSqlAttribute'/>
    /// class using a 16-bit signed integer.
    /// </summary>
    public DefaultValueSqlAttribute(short value)
    {
        _value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref='DefaultValueSqlAttribute'/>
    /// class using a 32-bit signed integer.
    /// </summary>
    public DefaultValueSqlAttribute(int value)
    {
        _value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref='DefaultValueSqlAttribute'/>
    /// class using a 64-bit signed integer.
    /// </summary>
    public DefaultValueSqlAttribute(long value)
    {
        _value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref='DefaultValueSqlAttribute'/>
    /// class using a single-precision floating point number.
    /// </summary>
    public DefaultValueSqlAttribute(float value)
    {
        _value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref='DefaultValueSqlAttribute'/>
    /// class using a double-precision floating point number.
    /// </summary>
    public DefaultValueSqlAttribute(double value)
    {
        _value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref='DefaultValueSqlAttribute'/>
    /// class using a <see cref='bool'/> value.
    /// </summary>
    public DefaultValueSqlAttribute(bool value)
    {
        _value = value ? 1 : 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref='DefaultValueSqlAttribute'/>
    /// class using a <see cref='string'/>.
    /// </summary>
    public DefaultValueSqlAttribute(string? value)
    {
        _value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref='DefaultValueSqlAttribute'/>
    /// class.
    /// </summary>
    public DefaultValueSqlAttribute(object? value)
    {
        var type = value?.GetType();

        if (type == null)
        {
            _value = "NULL"; // Handle null case appropriately
            return;
        }

        if (type.IsEnum)
        {
            // Convert the enum name to its underlying numeric value
            var enumValue = Enum.Parse(type, value.ToString(), true);
            var underlyingValue = Convert.ChangeType(enumValue, Enum.GetUnderlyingType(type), CultureInfo.InvariantCulture);
            _value = underlyingValue.ToString();
        }
        else if (type == typeof(string) || type == typeof(char))
        {
            _value = $"'{value}'";
        }
        else if (type == typeof(bool))
        {
            _value = (bool)value ? "1" : "0";
        }
        else
        {
            _value = value.ToString();
        }
    }


    /// <summary>
    /// Initializes a new instance of the <see cref='DefaultValueSqlAttribute'/>
    /// class using a <see cref='sbyte'/> value.
    /// </summary>
    [CLSCompliant(false)]
    public DefaultValueSqlAttribute(sbyte value)
    {
        _value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref='DefaultValueSqlAttribute'/>
    /// class using a <see cref='ushort'/> value.
    /// </summary>
    [CLSCompliant(false)]
    public DefaultValueSqlAttribute(ushort value)
    {
        _value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref='DefaultValueSqlAttribute'/>
    /// class using a <see cref='uint'/> value.
    /// </summary>
    [CLSCompliant(false)]
    public DefaultValueSqlAttribute(uint value)
    {
        _value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref='DefaultValueSqlAttribute'/>
    /// class using a <see cref='ulong'/> value.
    /// </summary>
    [CLSCompliant(false)]
    public DefaultValueSqlAttribute(ulong value)
    {
        _value = value;
    }

    /// <summary>
    /// Gets the default value of the property this attribute is bound to.
    /// </summary>
    public virtual object? Value => _value;

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj == this)
        {
            return true;
        }

        if (!(obj is DefaultValueSqlAttribute other))
        {
            return false;
        }

        if (Value == null)
        {
            return other.Value == null;
        }

        return Value.Equals(other.Value);
    }

    public override int GetHashCode() => base.GetHashCode();

    protected void SetValue(object? value) => _value = value;
}