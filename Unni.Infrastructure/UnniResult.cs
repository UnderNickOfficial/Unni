#pragma warning disable CS8618
using System;
using System.Collections.Generic;

namespace Unni.Infrastructure
{
    /// <summary>
    /// Basic Result from method
    /// </summary>
    public partial class UnniResult
    {
        /// <summary>
        /// Indicates if result was successful
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// List with errors occured
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
        /// <summary>
        /// Inner exception occured
        /// </summary>
        public Exception? InnerException { get; set; } = null;
        /// <summary>
        /// Sets Success status as true
        /// </summary>
        public UnniResult()
        {
            Success = true;
        }
        /// <summary>
        /// Sets Success status
        /// </summary>
        /// <param name="success">Success</param>
        public UnniResult(bool success)
        {
            Success = success;
        }
        /// <summary>
        /// Sets Success status as false and adds error to the Errors list
        /// </summary>
        /// <param name="error">Error</param>
        public UnniResult(string error)
        {
            Success = false;
            Errors.Add(error);
        }
        /// <summary>
        /// Sets Success status as false and adds errors to the Errors list
        /// </summary>
        /// <param name="errors">List with errors</param>
        public UnniResult(List<string> errors)
        {
            Success = false;
            Errors = errors;
        }
        /// <summary>
        /// Sets Success status as false and sets InnerException
        /// </summary>
        /// <param name="innerException">Exception</param>
        public UnniResult(Exception innerException)
        {
            Success = false;
            InnerException = innerException;
        }
        /// <summary>
        /// Sets Success status as false, adds error to the Errors list and sets InnerException
        /// </summary>
        /// <param name="error">Error</param>
        /// <param name="innerException">Exception</param>
        public UnniResult(string error, Exception innerException)
        {
            Success = false;
            Errors = new List<string>() { error };
            InnerException = innerException;
        }
        /// <summary>
        /// Sets Success status and sets InnerException
        /// </summary>
        /// <param name="success">Success</param>
        /// <param name="innerException">Exception</param>
        public UnniResult(bool success, Exception innerException) : this(success)
        {
            InnerException = innerException;
        }
        /// <summary>
        /// Sets Success status as false, sets InnerException and adds errors to the Errors list
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="innerException"></param>
        public UnniResult(List<string> errors, Exception innerException) : this(errors)
        {
            InnerException = innerException;
        }
        /// <summary>
        /// Sets Success status, adds errors to the Errors list and sets InnerException
        /// </summary>
        /// <param name="success">Success</param>
        /// <param name="errors">Errors</param>
        /// <param name="innerException">Exception</param>
        public UnniResult(bool success, List<string> errors, Exception innerException) : this(success, innerException)
        {
            Errors = errors;
        }
        /// <summary>
        /// Copies all data from result
        /// </summary>
        /// <param name="result">Result to copy from</param>
        public virtual UnniResult FromResult<T>(UnniResult<T> result)
        {
            Success = result.Success;
            Errors = result.Errors;
            InnerException = result.InnerException;
            return this;
        }
        /// <summary>
        /// Copies all data from result
        /// </summary>
        /// <param name="result">Result to copy from</param>
        public virtual UnniResult FromResult(UnniResult result)
        {
            Success = result.Success;
            Errors = result.Errors;
            InnerException = result.InnerException;
            return this;
        }
    }

    /// <summary>
    /// Result from method with data
    /// </summary>
    public partial class UnniResult<T> : UnniResult
    {
        /// <summary>
        /// Data to transfer
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// Sets Success status as true
        /// </summary>
        public UnniResult() : base()
        {

        }
        /// <summary>
        /// Sets Success status as true and sets Data
        /// </summary>
        /// <param name="data"></param>
        public UnniResult(T data) : base()
        {
            Data = data;
        }
        /// <summary>
        /// Sets Success status
        /// </summary>
        /// <param name="success"></param>
        public UnniResult(bool success) : base(success)
        {

        }
        /// <summary>
        /// Sets Success status as false and adds error to the Errors list
        /// </summary>
        /// <param name="error"></param>
        public UnniResult(string error) : base(error)
        {

        }
        /// <summary>
        /// Sets Success status as false and adds errors to the Errors list
        /// </summary>
        /// <param name="errors"></param>
        public UnniResult(List<string> errors) : base(errors)
        {

        }
        /// <summary>
        /// Sets Success status as false, sets Data and adds error to the Errors list
        /// </summary>
        /// <param name="data"></param>
        /// <param name="error"></param>
        public UnniResult(T data, string error) : base(error)
        {
            Data = data;
        }
        /// <summary>
        /// Sets Success status as false and sets InnerException
        /// </summary>
        /// <param name="innerException"></param>
        public UnniResult(Exception innerException) : base(innerException)
        {

        }
        /// <summary>
        /// Sets Success status and sets InnerException
        /// </summary>
        /// <param name="success">Success</param>
        /// <param name="innerException">Exception</param>
        public UnniResult(bool success, Exception innerException) : base(success, innerException)
        {

        }
        /// <summary>
        /// Sets Success status as true, sets Data and sets InnerException
        /// </summary>
        /// <param name="data"></param>
        /// <param name="innerException"></param>
        public UnniResult(T data, Exception innerException)
        {
            Success = true;
            Data = data;
            InnerException = innerException;
        }
        /// <summary>
        /// Sets Success status as false, adds error to the Errors list and sets InnerException
        /// </summary>
        /// <param name="error">Error</param>
        /// <param name="innerException">Exception</param>
        public UnniResult(string error, Exception innerException) : base(error, innerException)
        {

        }

        /// <summary>
        /// Sets Success status as false, sets InnerException and adds errors to the Errors list
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="innerException"></param>
        public UnniResult(List<string> errors, Exception innerException) : base(errors, innerException)
        {

        }
        /// <summary>
        /// Sets Success status, adds errors to the Errors list, sets InnerException and sets Data
        /// </summary>
        /// <param name="success">Success</param>
        /// <param name="errors">Errors</param>
        /// <param name="innerException">Exception</param>
        /// <param name="data">Data</param>
        public UnniResult(bool success, List<string> errors, Exception innerException, T data) : base(success, errors, innerException)
        {
            Data = data;
        }
        /// <summary>
        /// Copies all data from result
        /// </summary>
        /// <param name="result">Result to copy from</param>
        public new virtual UnniResult<T> FromResult<T2>(UnniResult<T2> result)
        {
            base.FromResult(result);
            InnerException = result.InnerException;
            return this;
        }
        /// <summary>
        /// Copies all data from result
        /// </summary>
        /// <param name="result">Result to copy from</param>
        public new virtual UnniResult<T> FromResult(UnniResult result)
        {
            base.FromResult(result);
            return this;
        }
    }
}