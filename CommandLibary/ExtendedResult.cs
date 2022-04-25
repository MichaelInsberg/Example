using System;
using System.Collections.Generic;
using System.Linq;

namespace Example.CommandLibrary
{
    /// <summary>
    /// The extended result class
    /// </summary>
    public class ExtendedResult : IEquatable<ExtendedResult>
    {
        private bool isResultValid;
        private List<string> resultMessages;

        /// <summary>
        /// Returns true if ... result is valid.
        /// </summary>
        public bool IsResultValid
        {
            get => isResultValid;
            private set => isResultValid = value;
        }

        /// <summary>
        /// Gets the result messages.
        /// </summary>
        public IReadOnlyList<string> ResultMessages
        {
            get => resultMessages.AsReadOnly();
            private set => resultMessages = new List<string>(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedResult"/> class.
        /// </summary>
        public ExtendedResult()
        {
            ResultMessages = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedResult" /> class.
        /// </summary>
        /// <param name="isValid">if set to <c>true</c> [is valid].</param>
        /// <param name="message">The message.</param>
        public ExtendedResult(bool isValid, string message = null)
            : this()
        {
            if (isValid && !string.IsNullOrEmpty(message))
            {
                throw new ArgumentException(
                    $"[{nameof(isValid)} = true] and [{nameof(message)} not empty] is not allowed");
            }
            SetResultValid(isValid);
            AddMessage(message);
        }

        /// <summary>
        /// Resets the extended result.
        /// </summary>
        /// <param name="isValid">if set to <c>true</c> [is valid].</param>
        public void Reset(bool isValid)
        {
            SetResultValid(isValid);
            ClearMessages();
        }

        /// <summary>
        /// Gets the result messages string.
        /// </summary>
        /// <param name="separator">The separator to separate the result messages.</param>
        /// <returns>The result messages as string separated by separator</returns>
        public virtual string GetResultMessagesString(string separator = "")
        {
            if (IsResultValid)
            {
                return string.Empty;
            }

            var messagesString = string.Join(separator,
                ResultMessages.Where(message => !string.IsNullOrWhiteSpace(message)));

            return messagesString;
        }

        /// <summary>
        /// Updates the validation result state
        /// </summary>
        /// <param name="isValid">if set to <c>true</c> [is valid].</param>
        /// <param name="validationMessages">The validation messages.</param>
        public virtual void Update(bool isValid, params string[] validationMessages)
        {
            if (isValid && validationMessages != null && validationMessages.Length > 0)
            {
                throw new ArgumentException(
                        $"Update with [{nameof(isValid)} = true] and [{nameof(validationMessages)} not empty] is not allowed");
            }
            SetResultValid(isValid);

            if (IsResultValid)
            {
                ClearMessages();
                return;
            }

            if (validationMessages == null)
            {
                return;
            }

            var newMessages = validationMessages.Where(message => !string.IsNullOrWhiteSpace(message)).ToArray();
            AddMessages(newMessages);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public virtual bool Equals(ExtendedResult other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return IsResultValid == other.IsResultValid && Equals(ResultMessages, other.ResultMessages);
        }

        /// <summary>
        /// Equals the specified result a.
        /// </summary>
        /// <param name="resultA">The result a.</param>
        /// <param name="resultB">The result b.</param>
        /// <returns></returns>
        public bool Equals(ExtendedResult resultA, ExtendedResult resultB)
        {
            if (resultA == null && resultB == null)
            {
                return true;
            }

            if (resultA == null || resultB == null)
            {
                return false;
            }

            return resultA.Equals(resultB);
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

            return obj.GetType() == this.GetType() && Equals((ExtendedResult)obj);
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
        public virtual int GetHashCode(ExtendedResult obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return obj.GetHashCode();
        }

        /// <summary>
        /// Deeps the clone.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T DeepClone<T>() where T : ExtendedResult, new()
        {
            var clone = new T();
            clone.SetResultValid(IsResultValid);
            clone.ClearMessages();
            clone.AddMessages(ResultMessages.ToArray());
            return clone;
        }

        /// <summary>
        /// Sets the result valid.
        /// </summary>
        /// <param name="isValid">if set to <c>true</c> [is valid].</param>
        protected void SetResultValid(bool isValid)
        {
            isResultValid = isValid;
        }

        /// <summary>
        /// Clears the messages.
        /// </summary>
        private void ClearMessages()
        {
            resultMessages.Clear();
        }

        /// <summary>
        /// Adds the message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void AddMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            resultMessages.Add(message);
        }

        /// <summary>
        /// Adds the messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        private void AddMessages(string[] messages)
        {
            var count = ResultMessages.Count;
            resultMessages.AddRange(messages);
        }
    }
}
