using System;
using System.Collections.Generic;

namespace Example.CommandLibrary
{
    /// <summary>
    /// The extended value result class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ExtendedResult" />
    public sealed class ExtendedValueResult<T> : ExtendedResult, IEquatable<ExtendedValueResult<T>>
    {
        private T value;

        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value => value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedValueResult{T}"/> class.
        /// </summary>
        public ExtendedValueResult()
        {
            SetValue(default);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedValueResult{T}"/> class.
        /// </summary>
        /// <param name="isValid">if set to <c>true</c> [is valid].</param>
        public ExtendedValueResult(bool isValid) : base(isValid)
        {
            SetResultValid(isValid);
            SetValue(default);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedValueResult{T}"/> class.
        /// </summary>
        /// <param name="isValid">if set to <c>true</c> [is valid].</param>
        /// <param name="objectValue">The value.</param>
        public ExtendedValueResult(bool isValid, T objectValue) : base(isValid)
        {
            SetResultValid(isValid);
            SetValue(objectValue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedValueResult{T}"/> class.
        /// </summary>
        /// <param name="isValid">if set to <c>true</c> [is valid].</param>
        /// <param name="objectValue">The object value</param>
        /// <param name="message">The message.</param>
        public ExtendedValueResult(bool isValid, T objectValue, string message)
            : base(isValid, message)
        {
            SetValue(objectValue);
        }

        /// <summary>
        /// Updates the specified is valid.
        /// </summary>
        /// <param name="isValid">if set to <c>true</c> [is valid].</param>
        /// <param name="objectValue">The object value.</param>
        /// <param name="validationMessages">The validation messages.</param>
        public void Update(bool isValid, T objectValue = default, params string[] validationMessages)
        {
            Update(isValid, validationMessages);
            SetValue(objectValue);
        }

        /// <summary>
        /// Performs a deep clone.
        /// </summary>
        /// <returns>
        /// The deeply cloned object.
        /// </returns>
        public new ExtendedValueResult<T> DeepClone()
        {
            var clone = DeepClone<ExtendedValueResult<T>>();

            clone.SetValue(Value);

            return clone;
        }

        /// <inheritdoc />
        public override string GetResultMessagesString(string separator = "")
        {
            var result = base.GetResultMessagesString(separator);
            if (!string.IsNullOrWhiteSpace(result) && value is string valueString)
            {
                if (!string.IsNullOrWhiteSpace(valueString))
                {
                    throw new InvalidOperationException(
                        $"The state that {nameof(ResultMessages)} is not empty and {nameof(Value)} is also not empty is invalid");
                }
            }

            return result;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(ExtendedValueResult<T> other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other) && EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((ExtendedValueResult<T>)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode() => 0;

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        /// <exception cref="ArgumentNullException">obj - Delivered { nameof(obj) } is null!</exception>
        public int GetHashCode(ExtendedValueResult<T> obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return obj.GetHashCode();
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="objectValue">The object value.</param>
        private void SetValue(T objectValue)
        {
            value = objectValue;
        }
    }
}
